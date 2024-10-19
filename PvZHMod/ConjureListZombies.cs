using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PvZHMod
{
    public static class ConjureZombie
    {
        private static string GUID => PvZHModCards.Instance.GUID;

        // private static System.Random rnd = new System.Random();

        public enum ZombieTribes
        {
            Dancing,
            Mustache,
            Science,
            Clock,
            Pet,
            Party,
            Professional,
            Gourmet
        }

        public enum CardTypes
        {
            Zombies,
            Tricks,
            Environments,
            Gravestones
        }

        public static String GetRandomZombie()
        {

            return ConjureZombie.All.zombies.RandomItem();
            // return ConjureZombie.All.zombies[rnd.Next(ConjureZombie.All.zombies.Count)];
            /*
            // var randomTribe = rnd.Next(0,16) // 16 total zombie tribes
            var randomTribe = rnd.Next(0,14); // 14 currently implemented
            int randomElem;
            Debug.Log($"Value of randomTribe: [{randomTribe}]");
            switch (randomTribe)
            {
                case 0: // Dancing
                    randomElem = rnd.Next(0, ConjureZombie.Dancing.zombies.Count());
                    return ConjureZombie.Dancing.zombies[randomElem];
                case 1: // Mustache
                    randomElem = rnd.Next(0, ConjureZombie.Mustache.zombies.Count());
                    return ConjureZombie.Mustache.zombies[randomElem];
                case 2: // Science
                    randomElem = rnd.Next(0, ConjureZombie.Science.zombies.Count());
                    return ConjureZombie.Science.zombies[randomElem];
                case 3: // Clock
                    randomElem = rnd.Next(0, ConjureZombie.Clock.zombies.Count());
                    return ConjureZombie.Clock.zombies[randomElem];
                case 4: // Pet
                    randomElem = rnd.Next(0, ConjureZombie.Pet.zombies.Count());
                    return ConjureZombie.Pet.zombies[randomElem];
                case 5: // Party
                    randomElem = rnd.Next(0, ConjureZombie.Party.zombies.Count());
                    return ConjureZombie.Party.zombies[randomElem];
                case 6: // Professional
                    randomElem = rnd.Next(0, ConjureZombie.Professional.zombies.Count());
                    return ConjureZombie.Professional.zombies[randomElem];
                case 7: // Gourmet
                    randomElem = rnd.Next(0, ConjureZombie.Gourmet.zombies.Count());
                    return ConjureZombie.Gourmet.zombies[randomElem];
                case 8: // Sports
                    randomElem = rnd.Next(0, ConjureZombie.Sports.zombies.Count());
                    return ConjureZombie.Sports.zombies[randomElem];
                case 9: // Imp
                    randomElem = rnd.Next(0, ConjureZombie.Imp.zombies.Count());
                    return ConjureZombie.Gourmet.zombies[randomElem];
                default:
                    return null;
            }
            */ 
        }

        public static string GetRandomCardFromTribe(ZombieTribes tribe, CardTypes type)
        {
            return "";
        }


        public class Dancing
        {
            public static List<string> zombies = new List<String>()
            {
                $"{GUID}.DiscoZombie",
                $"{GUID}.BackupDancer",
                $"{GUID}.Discobot",
                $"{GUID}.JesterZombie",
                $"{GUID}.UnlifeParty",
                $"{GUID}.FlamencoZombie",
                $"{GUID}.CongaZombie",
                $"{GUID}.CosmicDancer",
                $"{GUID}.Aerobics",
            };

            public static List<String> tricks = new List<string>()
            {

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {
                $"{GUID}.GraveConga"
            };
        }

        public class Mustache
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.BackupDancer"
            };

            public static List<String> tricks = new List<String>()
            {
                $"{GUID}.BungeePlumber"
            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Science
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.Discobot",
            };

            public static List<String> tricks = new List<string>()
            {
                $"{GUID}.FinalMission",
            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Clock
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.CuckooZombie"
            };

            public static List<String> tricks = new List<string>()
            {

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Pet
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.CuckooZombie",
                $"{GUID}.GizzardLizard"
            };

            public static List<String> tricks = new List<string>()
            {
                $"{GUID}.ChickenAttack"
            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Party
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.JesterZombie",
                $"{GUID}.UnlifeParty"
            };

            public static List<String> tricks = new List<string>()
            {
                $"{GUID}.Cakesplosion"

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Professional
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.NewspaperZombie",
                $"{GUID}.FootSoldier"
            };

            public static List<String> tricks = new List<String>()
            {
                $"{GUID}.BungeePlumber"
            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {
                $"{GUID}.GraveNewspaper"
            };
        }

        public class Gourmet
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.MysteryEgg",
                $"{GUID}.CongaZombie"
            };

            public static List<String> tricks = new List<string>()
            {
                $"{GUID}.SugaryTreat"
            };

            public static List<String> environments = new List<string>()
            {
                $"{GUID}.EnvTrapper"
            };

            public static List<String> gravestones = new List<string>()
            {
                $"{GUID}.GraveConga"
            };
        }

        public class Sports
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.TennisChamp"
            };

            public static List<String> tricks = new List<string>()
            {

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Imp
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.ExplodingImp",
                $"{GUID}.LooseCannon"
            };

            public static List<String> tricks = new List<string>()
            {
                $"{GUID}.Cakesplosion"
            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {
                $"{GUID}.GraveExplodingImp"
            };
        }

        public class Monster
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.GizzardLizard"
            };

            public static List<String> tricks = new List<string>()
            {

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Gargantuar
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.ImpThrowGarg"
            };

            public static List<String> tricks = new List<string>()
            {

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Pirate
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.ImpThrowGarg"
            };

            public static List<String> tricks = new List<string>()
            {

            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class Barrel
        {
            public static List<String> zombies = new List<String>()
            {
                $"{GUID}.LooseCannon",
            };

            public static List<String> tricks = new List<string>()
            {
                $"{GUID}.FinalMission",
            };

            public static List<String> environments = new List<string>()
            {

            };

            public static List<String> gravestones = new List<string>()
            {

            };
        }

        public class All
        {
            public static List<String> zombies =
            [
                .. Dancing.zombies,
                .. Mustache.zombies,
                .. Science.zombies,
                .. Clock.zombies,
                .. Pet.zombies,
                .. Party.zombies,
                .. Professional.zombies,
                .. Gourmet.zombies,
                .. Sports.zombies,
                .. Imp.zombies,
                .. Monster.zombies,
                .. Pirate.zombies,
                .. Gargantuar.zombies,
            ];

            public static List<String> tricks =
            [
                .. Dancing.tricks,
                .. Mustache.tricks,
                .. Science.tricks,
                .. Clock.tricks,
                .. Pet.tricks,
                .. Party.tricks,
                .. Professional.tricks,
                .. Gourmet.tricks,
                .. Sports.tricks,
                .. Imp.tricks,
                .. Monster.tricks,
                .. Pirate.tricks,
                .. Gargantuar.tricks,
            ];

            public static List<String> environments =
            [
                .. Dancing.environments,
                .. Mustache.environments,
                .. Science.environments,
                .. Clock.environments,
                .. Pet.environments,
                .. Party.environments,
                .. Professional.environments,
                .. Gourmet.environments,
                .. Sports.environments,
                .. Imp.environments,
                .. Monster.environments,
                .. Pirate.environments,
                .. Gargantuar.environments,
                $"{GUID}.MeteorZ"
            ];

            public static List<String> gravestones =
            [
                .. Dancing.gravestones,
                .. Mustache.gravestones,
                .. Science.gravestones,
                .. Clock.gravestones,
                .. Pet.gravestones,
                .. Party.gravestones,
                .. Professional.gravestones,
                .. Gourmet.gravestones,
                .. Sports.gravestones,
                .. Imp.gravestones,
                .. Monster.gravestones,
                .. Pirate.gravestones,
                .. Gargantuar.gravestones,
            ];
        }
    }
}
