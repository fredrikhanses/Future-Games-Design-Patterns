using System.IO;
using System.Text;

namespace Tools
{
    public class TextHandler
    {
        private StringBuilder stringBuilder = new StringBuilder();

        private readonly string mapPath = "Assets/Resources/MapSettings/";
        private readonly string textEnding = ".txt";
        private string path;

        public string ReadText(string name)
        {
            stringBuilder.Append(mapPath);
            stringBuilder.Append(name);
            stringBuilder.Append(textEnding);
            path = stringBuilder.ToString();
            stringBuilder.Clear();
            StreamReader reader = new StreamReader(path);
            string textContent = (reader.ReadToEnd());
            reader.Close();
            return textContent;
        }
        
        public void WriteText(string name, string textContent)
        {
            stringBuilder.Append(mapPath);
            stringBuilder.Append(name);
            stringBuilder.Append(textEnding);
            path = stringBuilder.ToString();
            stringBuilder.Clear();
            StreamWriter writer = new StreamWriter(path, true);
            writer.Write(textContent);
            writer.Close();
        }
    }
}