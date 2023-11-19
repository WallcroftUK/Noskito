using Noskito.Database.Extension;
using Noskito.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noskito.Database.Dto.BCards
{
    public class CardDTO : IIntDtoExtension
    {
        public int Duration { get; set; }

        public int EffectId { get; set; }

        public int GroupId { get; set; }

        public byte Level { get; set; }

        public string Name { get; set; }

        public short TimeoutBuff { get; set; }

        public int BuffType { get; set; }

        public byte TimeoutBuffChance { get; set; }

        public int SecondBCardsDelay { get; set; }

        public BuffCategoryType BuffCategory { get; set; }

        public byte BuffPartnerLevel { get; set; }

        public bool IsConstEffect { get; set; }

        public byte ElementType { get; set; }
        public List<BCardDTO> Bcards { get; set; } = new();
        public int Id { get; set; }
    }
}
