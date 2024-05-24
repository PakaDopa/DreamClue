using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class CSVReader
{
    private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private static char[] TRIM_CHARS = { '\"' };

    public List<Dictionary<string, object>> Read(string file)
    {
        var resultList = new List<Dictionary<string, object>>();
        string[] lines;

        //string fileFullPath = CustomPath.Path.CSV_Path + file;
        string fileFullPath = "";
        if (File.Exists(fileFullPath))
        {
            string source;
            StreamReader sr = new StreamReader(fileFullPath, System.Text.Encoding.Default);
            source = sr.ReadToEnd();
            sr.Close();

            lines = Regex.Split(source, LINE_SPLIT_RE);
        }
        else
            return null;

        if (lines.Length <= 1)
            return resultList;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                value = value.Replace("<br>", "\n");
                value = value.Replace("<c>", ",");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            resultList.Add(entry);
        }

        return resultList;
    }
}

