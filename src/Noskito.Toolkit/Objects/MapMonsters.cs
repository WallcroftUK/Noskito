using System.Collections.Generic;

namespace Noskito.Toolkit.Objects
{
    public class MapMonsters
    {
        public int MapId { get; init; }
        public IEnumerable<Monster> Monsters { get; init; }
    }
}