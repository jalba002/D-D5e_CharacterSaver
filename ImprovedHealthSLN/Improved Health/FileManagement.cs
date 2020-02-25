using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Improved_Health
{
    class FileManagement
    {
        #region Character Related
        public static void SaveToFile(Character l_Character)
        {
            JsonSerializer mySerializer = new JsonSerializer();
            mySerializer.Converters.Add(new StringEnumConverter());
            mySerializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(MainProgram.m_Settings.m_DefaultRoute + l_Character.m_Name + ".txt"))
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                mySerializer.Serialize(jsonWriter, l_Character);
            }
        }

        public static Character LoadFileAsCharacter(string characterName)
        {
            string input = File.ReadAllText(MainProgram.m_Settings.m_DefaultRoute + characterName + ".txt");
            Character l_Result = JsonConvert.DeserializeObject<Character>(input);
            return l_Result;
        }
        #endregion

        #region Settings
        public static DefaultSettings LoadDefaultSettings()
        {
            try
            {
                string l_File = File.ReadAllText(@BaseInfo.SettingsDir);
                DefaultSettings l_Result = JsonConvert.DeserializeObject<DefaultSettings>(l_File);
                return l_Result;
            }
            catch
            {
                return null;
            }
        }

        public static DefaultSettings CreateDefaultSettings()
        {
            DefaultSettings l_DefSet = new DefaultSettings();
            string temp = JsonConvert.SerializeObject(l_DefSet, Newtonsoft.Json.Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(BaseInfo.SettingsDir))
            {
                sw.Write(temp);
                sw.Close();
            }
            return l_DefSet;
        }
        #endregion

        public static string CharacterToString(Character l_Character)
        {
            return JsonConvert.SerializeObject(l_Character);
        }
    }

}
