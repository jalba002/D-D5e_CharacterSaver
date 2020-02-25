using System;

namespace Improved_Health
{
    [Serializable]
    public class Character
    {
        public enum HitDice
        {
            D4, D6, D8, D10, D12, D20
        }

        public enum BuildType
        {
            Dexterity = 1, Strength
        }

        public enum Race
        {
            Aasimar,
            Dragonborn,
            Dwarf,
            Elf,
            Halfling,
            Human,
            Kalashtar,
            Goliath,
            Gnome,
            HalfElf,
            HalfOrc,
            Tiefling
        }

        public enum Clase
        {
            Barbarian,
            Bard,
            Cleric,
            Druid,
            Fighter,
            Monk,
            Paladin,
            Ranger,
            Rogue,
            Sorcerer,
            Warlock,
            Wizard
        }

        [Serializable]
        public class Stats
        {
            public int m_Strength;
            public int m_Dexterity;
            public int m_Constitution;
            public int m_Intelligence;
            public int m_Wisdom;
            public int m_Charisma;

            private const int MaxStat = 30;
            private const int MinStat = 0;

            public Stats()
            {
                m_Strength = 10;
                m_Dexterity = 10;
                m_Constitution = 10;
                m_Intelligence = 10;
                m_Wisdom = 10;
                m_Charisma = 10;
            }

            public Stats(int[] stats)
            {
                if (stats.Length != 6) return;
                m_Strength = CheckMax(stats[0]);
                m_Dexterity = CheckMax(stats[1]);
                m_Constitution = CheckMax(stats[2]);
                m_Intelligence = CheckMax(stats[3]);
                m_Wisdom = CheckMax(stats[4]);
                m_Charisma = CheckMax(stats[5]);
            }

            private int CheckMax(int value)
            {
                int l_NewValue = Math.Min(MaxStat, value);
                l_NewValue = Math.Max(MinStat, l_NewValue);
                return l_NewValue;
            }

            public void DisplayStats()
            {
                DisplayStatLine("\tStrength", new [] { "\t" + m_Strength.ToString(), " (", StatToBonus(m_Strength).ToString(), ")" }, new [] { ConsoleColor.DarkBlue, ConsoleColor.White, (StatToBonus(m_Strength) >= 0 ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed), ConsoleColor.White });
                DisplayStatLine("\tDexterity", new [] { "\t" + m_Dexterity.ToString(), " (", StatToBonus(m_Dexterity).ToString(), ")" }, new [] { ConsoleColor.DarkBlue, ConsoleColor.White, (StatToBonus(m_Dexterity) >= 0 ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed), ConsoleColor.White });
                DisplayStatLine("\tConstitution", new [] { "\t" + m_Constitution.ToString(), " (", StatToBonus(m_Constitution).ToString(), ")" }, new [] { ConsoleColor.DarkBlue, ConsoleColor.White, (StatToBonus(m_Constitution) >= 0 ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed), ConsoleColor.White });
                DisplayStatLine("\tIntelligence", new [] { "\t" + m_Intelligence.ToString(), " (", StatToBonus(m_Intelligence).ToString(), ")" }, new [] { ConsoleColor.DarkBlue, ConsoleColor.White, (StatToBonus(m_Wisdom) >= 0 ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed), ConsoleColor.White });
                DisplayStatLine("\tWisdom", new [] { "\t\t" + m_Wisdom.ToString(), " (", StatToBonus(m_Wisdom).ToString(), ")" }, new [] { ConsoleColor.DarkBlue, ConsoleColor.White, (StatToBonus(m_Intelligence) >= 0 ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed), ConsoleColor.White });
                DisplayStatLine("\tCharisma", new [] { "\t" + m_Charisma.ToString(), " (", StatToBonus(m_Charisma).ToString(), ")" }, new [] { ConsoleColor.DarkBlue, ConsoleColor.White, (StatToBonus(m_Charisma) >= 0 ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed), ConsoleColor.White });
            }

            public void DisplayStatLine(string text, string[] values, ConsoleColor[] colors)
            {
                Console.Write(text);
                for (int i = 0; i < values.Length; i++)
                {
                    Console.ForegroundColor = colors[i];
                    Console.Write(values[i]);
                    Console.ResetColor();
                }
                Console.WriteLine("");
            }

            public static int CalculateHealthPoints(Character character) //Roll max + (Level - 1 * Hit Dice/2 + 1) + CON Bonus * Level.
            {
                int l_TotalHp = 0;
                for (int i = 0; i < character.m_ClassesType.Length; i++)
                {
                    l_TotalHp += (int)Math.Ceiling((GetHitDiceNumber(character.m_ClassesType[i].HitDie) * 0.5f + 1) * character.m_ClassesType[i].m_Level);
                }
                return l_TotalHp;
            }

            private static int GetHitDiceNumber(HitDice value)
            {
                var l_NewHitDie = "";
                foreach (var str in value.ToString().Split(new char[]{'D','d'}))
                {
                    l_NewHitDie += str;
                }
                int.TryParse(l_NewHitDie, out var l_HitDie);
                return l_HitDie;
            }

            public static int GetCharacterLevel(ClassType[] clase)
            {
                int totalLevel = 0;
                foreach (ClassType classType in clase)
                {
                    totalLevel += classType.m_Level;
                }
                return totalLevel;
            }

            public static int CalculatePoints(Character character)
            {
                int l_BuildBonus = character.m_BuildType == BuildType.Dexterity ? StatToBonus(character.m_CharacterStats.m_Dexterity) : StatToBonus(character.m_CharacterStats.m_Strength);
                int l_ConBonus = StatToBonus(character.m_CharacterStats.m_Constitution);

                return character.m_TotalLevel + Math.Max((l_BuildBonus >= l_ConBonus ? l_BuildBonus : l_ConBonus), 1) * character.m_ArmorClass; //Base AC + Level + (Best Stat * AC)
            }

            public static int StatToBonus(int value)
            {
                return (int)Math.Floor((value - 10) / 2f);
            }
        }
        [Serializable]
        public class ClassType
        {
            public Clase m_Clase;
            public int m_Level;
            public HitDice HitDie { get; private set; }

            public ClassType()
            {
                this.m_Clase = Clase.Fighter;
                this.m_Level = 10;
                HitDie = SetHitDie(m_Clase);
            }

            public ClassType(Clase clase, int level)
            {
                this.m_Clase = clase;
                this.m_Level = level;
                HitDie = SetHitDie(this.m_Clase);
            }

            private HitDice SetHitDie(Clase clase)
            {
                HitDice l_HitDice;

                switch (clase)
                {
                    case Clase.Sorcerer:
                    case Clase.Wizard:
                        l_HitDice = HitDice.D6;
                        break;
                    case Clase.Monk:
                    case Clase.Warlock:
                    case Clase.Cleric:
                    case Clase.Druid:
                    case Clase.Rogue:
                    case Clase.Bard:
                        l_HitDice = HitDice.D8;
                        break;
                    case Clase.Fighter:
                    case Clase.Ranger:
                    case Clase.Paladin:
                        l_HitDice = HitDice.D10;
                        break;
                    case Clase.Barbarian:
                        l_HitDice = HitDice.D12;
                        break;
                    default:
                        l_HitDice = HitDice.D20;
                        break;
                }

                return l_HitDice;
            }
        }

        #region Variables
        public string m_Name;
        public Race m_Race;
        public ClassType[] m_ClassesType;
        public Stats m_CharacterStats;
        public int m_ArmorClass;
        public int m_TotalLevel;
        public BuildType m_BuildType;
        #endregion

        public Character()
        {
            m_Name = "Default";
            m_Race = Race.Human;
            m_ClassesType = new [] { new ClassType() };
            m_TotalLevel = Stats.GetCharacterLevel(m_ClassesType);
            m_CharacterStats = new Stats();
            m_ArmorClass = 10;
            m_BuildType = BuildType.Strength;
        }

        public Character(string name, Race race, ClassType[] classType, Stats characterStats, int armorClass, BuildType buildType)
        {
            m_Name = name;
            m_Race = race;
            m_ClassesType = classType;
            m_TotalLevel = Stats.GetCharacterLevel(m_ClassesType);
            m_CharacterStats = characterStats;
            m_ArmorClass = armorClass;
            m_BuildType = buildType;
        }

        public static Character[] AddDemoCharacters()
        {
            Character[] l_List =
            {
                new Character(),
                new Character("Kora",Race.Kalashtar,new []{new ClassType(Clase.Wizard,9)},new Stats(new []{6,16,15,20,16,10}),15,BuildType.Dexterity), 
                new Character("Seraphina", Race.Human, new []{ new ClassType(Clase.Fighter,8)}, new Stats(new [] { 18, 10, 18, 10, 8, 10 }), 18, BuildType.Strength),
                new Character("David", Race.Gnome, new []{ new ClassType(Clase.Wizard, 7)}, new Stats(new [] {8,12,16,14,18,10 }),14,BuildType.Dexterity),
                new Character("Goliad", Race.HalfOrc, new []{new ClassType(Clase.Barbarian,4), new ClassType(Clase.Bard,3) }, new Stats(new [] {20, 14, 16, 6, 8, 16 }),15,BuildType.Dexterity)
            };
            return l_List;
        }
    }
}
