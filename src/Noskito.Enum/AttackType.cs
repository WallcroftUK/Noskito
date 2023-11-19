using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noskito.Enum
{
    public enum AttackType : byte
    {
        Melee = 0,
        Ranged = 1,
        Magical = 2,
        Other = 3,
        Charge = 4,
        Dash = 5
    }
}
