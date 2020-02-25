using System;
using System.Collections.Generic;

namespace Improved_Health
{
    static class MainProgram
    {
        public static bool m_Running = true;
        public static readonly List<Character> Characters = new List<Character>();
        public static DefaultSettings m_Settings;
        private static Menu _currentMenu;

        private static void Main(string[] args)
        {
            LoadDefaultSettings();
            ChangeMenu(new MainMenu());
            AddDemoCharacters();
            while (m_Running)
            {
                _currentMenu.ReadInputs(Console.ReadKey(true));
            }
        }

        private static void AddDemoCharacters()
        {
            foreach (var cha in Character.AddDemoCharacters())
            {
                Characters.Add(cha);
            }
        }

        public static void ChangeMenu(Menu newMenu)
        {
            _currentMenu?.OnExit();
            _currentMenu = newMenu;
            _currentMenu.OnEnter();
        }

        private static void LoadDefaultSettings()
        {
            var l_Settings = FileManagement.LoadDefaultSettings();
            m_Settings = l_Settings ?? FileManagement.CreateDefaultSettings();
        }
    }
}
