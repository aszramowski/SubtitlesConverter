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
        public List<string> ReadTxtSubtitles(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
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

                    tsStart = TimeSpan.FromSeconds(double.Parse(sbStartTimeSecond.ToString())  / 23.976);
                    tsEnd = TimeSpan.FromSeconds(double.Parse(sbEndTimeSecond.ToString())  / 23.976);

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
