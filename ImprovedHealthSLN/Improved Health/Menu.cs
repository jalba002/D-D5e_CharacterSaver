using System;
using System.Linq;

namespace Improved_Health
{
    public abstract class Menu
    {
        public abstract void ReadInputs(ConsoleKeyInfo keyInfo);
        public abstract void OnEnter();
        public abstract void OnExit();
    }

    public class MainMenu : Menu
    {
        public override void ReadInputs(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                MainProgram.m_Running = false;
            }
            else
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1: //Create your own character. This must pull up a new screen to create your character.
                        MainProgram.ChangeMenu(new CreateCharacter());
                        break;
                    case ConsoleKey.D2: //Delete a character. New screen showing all character, or atleast their names, and be able to delete them.
                        WriteOnConsole.DisplayOnBottom("Option not implemented.");
                        break;
                    case ConsoleKey.D3: //Display all saved characters.
                        MainProgram.ChangeMenu(new DisplayCharacters());
                        break;
                    case ConsoleKey.D4: //Save a character as a file.
                        foreach (var character in MainProgram.Characters)
                        {
                            FileManagement.SaveToFile(character);
                        }
                        break;
                    case ConsoleKey.D5: //Currenly displays JSON, TODO: Remove it.
                        /*Console.Clear();
                        Console.WriteLine(FileManagement.CharacterToString(m_Characters[1]));
                        Console.ReadKey();*/
                        break;
                    case ConsoleKey.D6: //Load a character from a file.
                        MainProgram.Characters.Add(FileManagement.LoadFileAsCharacter("Seraphina"));
                        break;
                    default: //Default wrong input feedback. No changes needed right now.
                        WriteOnConsole.DisplayOnBottom("Input not registered:", keyInfo.Key.ToString());
                        break;
                }
            }
        }

        public override void OnEnter()
        {
            Console.ResetColor();
            DisplayMenu(true);
        }

        public override void OnExit()
        {

        }

        /// <summary>
        /// End of abstract class.
        /// </summary>

        public void DisplayMenu(bool clearConsole)
        {
            if (clearConsole)
                Console.Clear();
            WriteOnConsole.DisplayLine("Welcome to the character calculator!", "", ConsoleColor.White);
            WriteOnConsole.DisplayLine("Press the button correspondant to the menu option to select it", "", ConsoleColor.White);
            Console.WriteLine("");
            WriteOnConsole.DisplayLine("1. Add a character", "", ConsoleColor.White);
            WriteOnConsole.DisplayLine("2. Delete a character", "", ConsoleColor.White);
            WriteOnConsole.DisplayLine("3. View all saved characters", "", ConsoleColor.White);
            WriteOnConsole.DisplayLine("4. Save a character", "", ConsoleColor.White);
            WriteOnConsole.DisplayLine("5. Display character as JSON", "", ConsoleColor.White);
            WriteOnConsole.DisplayLine("6. Load Character", "", ConsoleColor.White);
        }

    }

    public class DisplayCharacters : Menu
    {
        private void DisplayAllCharacters()
        {
            foreach (var l_Character in MainProgram.Characters)
            {
                WriteOnConsole.DisplayCharacter(l_Character);
            }
        }

        public override void ReadInputs(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                MainProgram.ChangeMenu(new MainMenu());
            }

        }

        public override void OnEnter()
        {
            Console.Clear();
            DisplayAllCharacters();
        }

        public override void OnExit()
        {
            Console.Clear();
        }


    }

    public class CreateCharacter : Menu
    {
        private int step;
        private int stepOffset = 2;
        private Character m_TempCharacter = new Character();
        private string m_UserInput = "";
        private int mouseXPosition = Console.CursorLeft;

        private readonly string[] m_CharacterStepsText =
        {
            "Character's name: ",
            "Race: ",
            "Class: ",
            "Level: ",
            "Armor Class: ",
            "Buildtype (STR or DEX): ",
        };

        public override void OnEnter()
        {
            mouseXPosition = Console.CursorLeft;
            m_UserInput = "";
            Console.Clear();
            WriteOnConsole.WriteLineAt("Welcome to character creation!", 0);
            UpdateStep(true);
        }

        public override void OnExit()
        {

        }

        public override void ReadInputs(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    MainProgram.ChangeMenu(new MainMenu());
                    break;
                case ConsoleKey.LeftArrow:
                    step--;
                    if (step < 0)
                        MainProgram.ChangeMenu(new MainMenu());
                    else
                        UpdateStep(false);
                    break;
                case ConsoleKey.Enter:
                    step++;
                    UpdateStep(true);
                    break;
                default:
                    CompositeString(keyInfo);
                    break;
            }
        }

        private void UpdateStep(bool add)
        {
            m_UserInput = null;
            WriteOnConsole.ClearConsoleLineAt(step + stepOffset - (add ? 0 : -1));
            WriteOnConsole.ClearConsoleLineAt(step + stepOffset);
            WriteOnConsole.WriteAt(m_CharacterStepsText[step], step + stepOffset);
            mouseXPosition = Console.CursorLeft;
        }

        private void CompositeString(ConsoleKeyInfo key)
        {
            if (key == null) return;
            if (key.Key == ConsoleKey.Backspace)
            {
                if (m_UserInput.Length > 0)
                    m_UserInput = m_UserInput.Remove(m_UserInput.Length - 1);
            }
            else
            {
                m_UserInput += GetGoodString(key.KeyChar.ToString());
            }
            WriteOnConsole.ClearConsoleLineAt(Console.CursorTop, mouseXPosition);
            Console.Write(m_UserInput);
        }

        public string GetGoodString(string input)
        {
            var allowedChars = Enumerable.Range('A', 26).Concat(Enumerable.Range('a', 26)).Concat(Enumerable.Range('-', 1));

            var goodChars = input.Where(c => allowedChars.Contains(c));
            return new string(goodChars.ToArray());
        }

        public void StartCreatingCharacter(ConsoleKeyInfo keyPressed)
        {
            /*
            switch (m_Step)
            {
                case 0:
                    break;

            }
            m_Step++;
            m_TempCharacter.m_ArmorClass = WriteOnConsole.WriteInAndGetInt("Please introduce your AC:", m_Step);
            Console.Clear();
            WriteOnConsole.DisplayCharacter(m_TempCharacter);*/
        }
    }
}
