using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitlesConverter
{
    public class SubtitleManager
    {
        private readonly double framePerSeconds = 23.976;
        private List<string> subtitleLinesOut;
        public List<string> SubtitleConvert(string filePath)
        {
            subtitleLinesOut = null;

            if (filePath.EndsWith(".txt"))
                subtitleLinesOut = ReadTxtSubtitles(filePath);
            else if (filePath.EndsWith(".srt"))
                subtitleLinesOut = ReadSrtSubtitles(filePath);
            else
                throw new Exception("Niepoprawna ścieżka pliku z napisami");

            return subtitleLinesOut;
        }

        private List<string> ReadSrtSubtitles(string filePath)
        {
            string line = File.ReadAllText(filePath, Encoding.GetEncoding("Windows-1250"));
            List<string> linesOut = new List<string>();

            StringBuilder sbStartTimeSecond = new StringBuilder();
            StringBuilder sbEndTimeSecond = new StringBuilder();
            StringBuilder sbText = new StringBuilder();
            StringBuilder sbTxtLine = new StringBuilder();

            int timeStart;
            int timeEnd;

            string pattern = @"((?<rowNumber>\d+\r\n)(?<startTime>\d\d:\d\d:\d\d,\d\d\d)( --> )(?<endTime>\d\d:\d\d:\d\d,\d\d\d\r\n))(?<text>(.+\r\n){1,})\r\n";
            string patternTime = @"\d{2}:\d{2}:\d{2},\d{3}";
            Regex regex = new Regex(pattern);
            Regex regexTime = new Regex(patternTime);
            MatchCollection matchCollection = regex.Matches(line);

            for (int i = 0; i < matchCollection.Count; i++)
            {
                sbStartTimeSecond.Clear();
                sbEndTimeSecond.Clear();
                sbText.Clear();

                sbStartTimeSecond.Append(matchCollection[i].Groups["startTime"].Value);
                sbEndTimeSecond.Append(matchCollection[i].Groups["endTime"].Value);
                sbText.Append(matchCollection[i].Groups["text"].Value);
                
                timeStart = Convert.ToInt32(TimeSpan.Parse(sbStartTimeSecond.ToString()).TotalSeconds * framePerSeconds);
                timeEnd = Convert.ToInt32(TimeSpan.Parse(sbEndTimeSecond.ToString()).TotalSeconds * framePerSeconds);

                sbTxtLine.Clear();
                sbTxtLine.Append("{");
                sbTxtLine.Append(timeStart);
                sbTxtLine.Append("}");
                sbTxtLine.Append("{");
                sbTxtLine.Append(timeEnd);
                sbTxtLine.Append("}");
                sbTxtLine.Append(sbText.ToString().Replace("\r\n", "|"));
                sbTxtLine.Remove(sbTxtLine.Length - 1, 1);                
                linesOut.Add(sbTxtLine.ToString());
            }
            return linesOut;
        }
                    
        private List<string> ReadTxtSubtitles(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("Windows-1250"));            
            List<string> linesOut = new List<string>();            

            StringBuilder sbStartTimeSecond = new StringBuilder();
            StringBuilder sbEndTimeSecond = new StringBuilder();
            StringBuilder sbText = new StringBuilder();
            StringBuilder sbSrtLine = new StringBuilder();

            TimeSpan tsStart;
            TimeSpan tsEnd;

            string pattern = @"(\{)(?<startTime>\d+)(\})(\{)(?<endTime>\d+)(\})(?<text>.+)";
            Regex regex = new Regex(pattern);

            for (int i = 0; i < lines.Length; i++)
            {
                Match match = regex.Match(lines[i]);
                if (match.Success)
                {
                    sbStartTimeSecond.Clear();
                    sbEndTimeSecond.Clear();
                    sbText.Clear();

                    sbStartTimeSecond.Append(match.Groups["startTime"].Value);
                    sbEndTimeSecond.Append(match.Groups["endTime"].Value);
                    sbText.Append(match.Groups["text"].Value);

                    tsStart = TimeSpan.FromSeconds(double.Parse(sbStartTimeSecond.ToString())  / framePerSeconds);
                    tsEnd = TimeSpan.FromSeconds(double.Parse(sbEndTimeSecond.ToString())  / framePerSeconds);

                    sbSrtLine.Clear();
                    sbSrtLine.Append(i+1);
                    sbSrtLine.AppendLine();
                    sbSrtLine.Append(tsStart.ToString(@"hh\:mm\:ss\,fff"));
                    sbSrtLine.Append(" --> ");
                    sbSrtLine.Append(tsEnd.ToString(@"hh\:mm\:ss\,fff"));
                    sbSrtLine.AppendLine();
                    sbSrtLine.Append(sbText.ToString().Replace("|","\r\n"));
                    sbSrtLine.AppendLine();
                    linesOut.Add(sbSrtLine.ToString());
                } 
            }
            return linesOut;
        }        
    }
}
