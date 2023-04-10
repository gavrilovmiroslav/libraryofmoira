using LibraryOfMoira.DataManagement;
using System.Collections.Generic;

namespace LibraryOfMoira.Models
{
    public class Ability
    {
        public string Name;
        public HashSet<string> Tags;

        [QueryAs("Damage (low)")]
        public int LowDamage;
        [QueryAs("Damage (high)")]
        public int HighDamage;
    }
}
