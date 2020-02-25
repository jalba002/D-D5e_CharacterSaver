namespace Improved_Health
{
    struct BaseInfo
    {
        public static readonly string SettingsDir = "C:/Users/Crynano/AppData/Roaming/_CryOS/Settings.txt";
    }

    [System.Serializable]
    class DefaultSettings
    {
        public string m_DefaultRoute;

        public DefaultSettings()
        {
            m_DefaultRoute = "C:/Users/Crynano/Desktop/AppTestings/Characters/";
        }
    }

    class Settings
    {
        string CharactersSaveDir;
    }
}
