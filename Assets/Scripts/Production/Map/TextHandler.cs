using System.IO;
using System.Text;

namespace Tools
{
    public class TextHandler
    {
        private StringBuilder m_StringBuilder = new StringBuilder(); 
        private const string k_MapPath = "Assets/Resources/MapSettings/";
        private const string k_TextEnding = ".txt";

        public string ReadText(string name)
        {
            m_StringBuilder.Append(k_MapPath);
            m_StringBuilder.Append(name);
            m_StringBuilder.Append(k_TextEnding);
            string path = m_StringBuilder.ToString();
            m_StringBuilder.Clear();
            StreamReader reader = new StreamReader(path);
            string textContent = (reader.ReadToEnd());
            reader.Close();
            return textContent;
        }
        
        public void WriteText(string name, string textContent)
        {
            m_StringBuilder.Append(k_MapPath);
            m_StringBuilder.Append(name);
            m_StringBuilder.Append(k_TextEnding);
            string path = m_StringBuilder.ToString();
            m_StringBuilder.Clear();
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(textContent);
            writer.Close();
        }
    }
}