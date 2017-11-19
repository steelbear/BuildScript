using System.IO;

using static BuildScript.Util.Checker;

namespace BuildScript.Util
{
    public class SourceText
    {
        private string filename;
        private char[] buffer;

        public SourceText(FileInfo file)
        {
            CheckNull(file, nameof(file));

            using (var reader = file.OpenText())
            {
                buffer = reader.ReadToEnd().ToCharArray();
            }

            filename = file.Name;
        }

        public SourceText(string source)
        {
            CheckNull(source, nameof(source));

            buffer = source.ToCharArray();
            filename = "<internal>";
        }

        public int Length
        {
            get => buffer.Length;
        }

        public char this[int index]
        {
            get => buffer[index];
        }

        public string FileName
        {
            get => filename;
        }

        public string GetString(int start, int length) => new string(buffer, start, length);
    }
}
