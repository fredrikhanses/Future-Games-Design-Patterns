﻿using System.IO;
using System.Text;

public interface IReadText
{
    string ReadText(string name);
}

public interface IWriteText
{
    void WriteText(string name, string content);
}

public interface ITextHandler : IReadText, IWriteText { }

namespace Tools
{
    public class TextHandler : ITextHandler
    {
        private StringBuilder m_StringBuilder = new StringBuilder(); 
        private const string k_MapPath = "Assets/Resources/MapSettings/";
        private const string k_TextEnding = ".txt";

        /// <summary> Reads and fetches data from a text file.</summary>
        /// <param name="name"> Name of the text file to read.</param>
        /// <returns> Map content as a string.</returns>
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

        /// <summary> Writes a string to a text file.</summary>
        /// <param name="name"> Name of the text file to write to.</param>
        /// <param name="textContent"> Content to write as a string.</param>
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