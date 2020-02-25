using System;

namespace Improved_Health
{
    static class WriteOnConsole
    {
        public static void DisplayCharacter(Character character)
        {
            Console.WriteLine("\n///////////////////////////////");
            Console.WriteLine("Here is your character!.");
            DisplayLine("\nName:", character.m_Name, ConsoleColor.DarkYellow);
            foreach (Character.ClassType l_Class in character.m_ClassesType)
            {
                DisplayLine("Class:", l_Class.m_Clase + " " + l_Class.m_Level + " (" + l_Class.HitDie.ToString() + ")", ConsoleColor.White);
            }
            DisplayLine("Total Level:", character.m_TotalLevel.ToString(), ConsoleColor.Blue);
            DisplayLine("Stats:", "", ConsoleColor.Blue);
            character.m_CharacterStats.DisplayStats();
            DisplayLine("\nArmor Class:", character.m_ArmorClass.ToString(), ConsoleColor.Yellow);
            DisplayLine("\nHealth Points:", Character.Stats.CalculateHealthPoints(character).ToString(), ConsoleColor.Green);
            DisplayLine((character.m_BuildType is Character.BuildType.Dexterity ? "Dodge" : "Armor") + " Points:", Character.Stats.CalculatePoints(character).ToString(), ConsoleColor.Green);
        }

        public static void DisplayLine(string text, string value, ConsoleColor color)
        {
            Console.Write(text);
            Console.ForegroundColor = color;
            Console.WriteLine(" " + value + " ");
            Console.ResetColor();
        }

        public static void WriteAt(string text, int topPos, int leftPos = 0)
        {
            Console.SetCursorPosition(leftPos, topPos);
            Console.Write(text);
        }

        public static void WriteLineAt(string text, int topPos, int leftPos = 0)
        {
            Console.SetCursorPosition(leftPos, topPos);
            Console.WriteLine(text);
            Console.SetCursorPosition(leftPos, topPos);
        }

        public static void ClearConsoleLineAt(int topPos = 0, int leftPosition = 0)
        {
            Console.SetCursorPosition(leftPosition, topPos);
            Console.Write(new string(' ', Console.WindowWidth - leftPosition));
            Console.SetCursorPosition(leftPosition, topPos);
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void DisplayOnBottom(string text, string secondText = "")
        {
            Console.SetCursorPosition(1, Console.WindowHeight - 2);
            WriteOnConsole.ClearCurrentConsoleLine();
            WriteOnConsole.DisplayLine(text, secondText, ConsoleColor.Blue);
        }

    }
}
