using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noskito.Enum
{
    public enum BCardNpcMonsterTriggerType
    {
        NONE = 0, // should always trigger
        ON_FIRST_ATTACK = 1, // triggers only on first hit
        ON_DEATH = 2 // triggers on npcMonster's death
    }
}
