using Noskito.Database.Dto.BCards;
using Noskito.Database.Extension;
using Noskito.Enum;
using Noskito.Logging;
using System.Text;

namespace DataLoader.Module.Services
{
    public class CardRsrcFileLoaderService : RsrcProv<CardDTO>
    {
        private readonly RsrcConfigLoader _config;

        public CardRsrcFileLoaderService(RsrcConfigLoader config)
        {
            _config = config;
        }

        public async Task<IReadOnlyList<CardDTO>> FetchAsync()
        {
            string filePath = Path.Combine(_config.DataPath, "Card.dat");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{filePath} should be present");
            }

            var cards = new List<CardDTO>();
            using var npcIdStream = new StreamReader(filePath, Encoding.GetEncoding(1252));

            CardDTO card = null;
            bool itemAreaBegin = false;

            string line;
            while ((line = await npcIdStream.ReadLineAsync()) != null)
            {
                string[] currentLine = line.Split('\t');

                switch (currentLine.Length)
                {
                    case > 2 when currentLine[1] == "VNUM":
                        card = InitializeCard(currentLine);
                        itemAreaBegin = true;
                        break;

                    case > 2 when currentLine[1] == "NAME":
                        card.Name = currentLine[2];
                        break;

                    case > 3 when currentLine[1] == "GROUP":
                        ProcessGroup(currentLine, card, itemAreaBegin);
                        break;

                    case > 3 when currentLine[1] == "STYLE":
                        ProcessStyle(currentLine, card);
                        break;

                    case > 3 when currentLine[1] == "EFFECT":
                        card.EffectId = Convert.ToInt32(currentLine[2]);
                        break;

                    case > 3 when currentLine[1] == "TIME":
                        card.Duration = Convert.ToInt32(currentLine[2]);
                        card.SecondBCardsDelay = Convert.ToInt32(currentLine[3]);
                        break;

                    default:
                        ProcessBCards(currentLine, card);
                        break;
                }
            }

            Log.Info($"[DATA_LOADER] {cards.Count.ToString()} cards loaded");
            return cards;
        }

        private static CardDTO InitializeCard(string[] currentLine)
        {
            return new CardDTO
            {
                Id = Convert.ToInt16(currentLine[2])
            };
        }

        private static void ProcessGroup(string[] currentLine, CardDTO card, bool itemAreaBegin)
        {
            if (!itemAreaBegin)
            {
                return;
            }

            card.GroupId = Convert.ToInt32(currentLine[2]);
            card.Level = Convert.ToByte(currentLine[3]);
        }

        private static void ProcessStyle(string[] currentLine, CardDTO card)
        {
            card.BuffCategory = (BuffCategoryType)byte.Parse(currentLine[2]);
            card.BuffType = Convert.ToByte(currentLine[3]);
            card.ElementType = Convert.ToByte(currentLine[4]);
            card.IsConstEffect = currentLine[5] == "1";
            card.BuffPartnerLevel = Convert.ToByte(currentLine[6]);
        }

        private static void ProcessBCards(string[] currentLine, CardDTO card)
        {
            BCardDTO bCard;

            switch (currentLine.Length)
            {
                case > 3 when currentLine[1] == "1ST":
                    ProcessBCardSet(currentLine, card, 3, 6, 0, 3, 3);
                    break;

                case > 3 when currentLine[1] == "2ST":
                    ProcessBCardSet(currentLine, card, 2, 6, 1, 3, 2);
                    break;

                case > 3 when currentLine[1] == "LAST":
                    card.TimeoutBuff = short.Parse(currentLine[2]);
                    card.TimeoutBuffChance = byte.Parse(currentLine[3]);
                    break;
            }
        }

        private static void ProcessBCardSet(string[] currentLine, CardDTO card, int iterations, int startIdx, int baseIdx, int modIdx1, int modIdx2)
        {
            BCardDTO bCard;

            for (int i = 0; i < iterations; i++)
            {
                if (currentLine[startIdx + i * baseIdx] == "-1" || currentLine[startIdx + i * baseIdx] == "0")
                {
                    continue;
                }

                int first = int.Parse(currentLine[startIdx + i * baseIdx + modIdx1]);
                int second = int.Parse(currentLine[startIdx + i * baseIdx + modIdx2]);

                ProcessBCard(card, first, second, currentLine, 2 + i * baseIdx, 3 + i * baseIdx, 4 + i * baseIdx, 5 + i * baseIdx);
            }
        }

        private static void ProcessBCard(CardDTO card, int first, int second, string[] currentLine, int typeIdx, int subTypeIdx, int procChanceIdx, int tickPeriodIdx)
        {
            int firstModulo = GetModulo(first);
            int secondModulo = GetModulo(second);

            byte tickPeriod = byte.Parse(currentLine[tickPeriodIdx]);
            var bCard = new BCardDTO
            {
                CardId = card.Id,
                Type = byte.Parse(currentLine[typeIdx]),
                SubType = (byte)((Convert.ToByte(currentLine[subTypeIdx]) + 1) * 10 + 1 + (first < 0 ? 1 : 0)),
                FirstData = (int)Math.Abs(Math.Floor(first / 4.0)),
                SecondData = (int)Math.Abs(Math.Floor(second / 4.0)),
                ProcChance = int.Parse(currentLine[procChanceIdx]),
                TickPeriod = tickPeriod == 0 ? null : (byte?)(tickPeriod * 2),
                FirstDataScalingType = (BCardScalingType)firstModulo,
                SecondDataScalingType = (BCardScalingType)secondModulo,
                IsSecondBCardExecution = false
            };

            card.Bcards.Add(bCard);
        }

        private static int GetModulo(int value)
        {
            int modulo = value % 4;
            return modulo switch
            {
                -1 => 1,
                -2 => 2,
                -3 => 1,
                _ => modulo
            };
        }
    }
}
