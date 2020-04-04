using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SubtitlesConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = @"d:\Projekty_trening\Wrong Turn UNRATED (2003) 1080p BrRip x264 - YIFY\Wrong.Turn.UNRATED.2003.1080p.BRrip.x264.YIFY.txt";
        string newFilePath = @"d:Projekty_trening\noweNapiski.srt";

        public MainWindow()
        {
            InitializeComponent();
            GetText();
        }

        private void GetText()
        {
            SubtitleManager sm = new SubtitleManager();
            List<string> text = sm.ReadTxtSubtitles(filePath);

            //tb_textFile.Text = "";
            //foreach (var str in text)
            //    tb_textFile.Text += str;

            File.WriteAllLines(newFilePath, text);
        }
    }
}
