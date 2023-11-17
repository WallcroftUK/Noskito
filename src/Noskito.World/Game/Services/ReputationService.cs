using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Noskito.Enum;

namespace Noskito.World.Game.Services
{
    public class ReputationService
    {
        private readonly Dictionary<int, ReputationIcon> icons = new()
        {
            [0] = ReputationIcon.GreenBeginner,
            [50] = ReputationIcon.BlueBeginner,
            [150] = ReputationIcon.RedBeginner,
            [250] = ReputationIcon.GreenTrainee,
            [500] = ReputationIcon.BlueTrainee,
            [750] = ReputationIcon.RedTrainee,
            [1000] = ReputationIcon.GreenExperienced,
            [2250] = ReputationIcon.BlueExperienced,
            [3500] = ReputationIcon.RedExperienced,
            [5000] = ReputationIcon.GreenSoldier,
            [9500] = ReputationIcon.BlueSoldier,
            [19000] = ReputationIcon.RedSoldier,
            [25000] = ReputationIcon.GreenExpert,
            [40000] = ReputationIcon.BlueExpert,
            [60000] = ReputationIcon.RedExpert,
            [85000] = ReputationIcon.GreenLeader,
            [115000] = ReputationIcon.BlueLeader,
            [150000] = ReputationIcon.RedLeader,
            [190000] = ReputationIcon.GreenMaster,
            [235000] = ReputationIcon.BlueMaster,
            [285000] = ReputationIcon.RedMaster,
            [350000] = ReputationIcon.GreenNos,
            [500000] = ReputationIcon.BlueNos,
            [1500000] = ReputationIcon.RedNos,
            [2500000] = ReputationIcon.GreenElite,
            [3750000] = ReputationIcon.BlueElite,
            [5000000] = ReputationIcon.RedElite
        };
        
        public ValueTask<ReputationIcon> GetIcon(long reputation)
        {
            var icon = icons.Last(x => reputation >= x.Key).Value;
            
            if (icon == ReputationIcon.RedElite)
            {
                // TODO : Check in database for potential legends/hero etc...
            }

            return ValueTask.FromResult(icon);
        }
    }
}