using Microsoft.Win32;
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
        //string newFilePath = @"d:\Projekty_trening\napisy\Wrong.Turn.UNRATED.2003.1080p.BRrip.x264.YIFY.txt";
        //string filePath = @"d:Projekty_trening\napisy\Wrong.Turn.UNRATED.2003.1080p.BRrip.x264.YIFY.srt";

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void GetSubtitles()
        {
            string newFilePath = "";
            SubtitleManager sm = new SubtitleManager();
            List<string> linesOut;

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    linesOut = sm.SubtitleConvert(filePath);
                    
                    if (filePath.EndsWith(".txt")) newFilePath = filePath.Replace(".txt", ".srt");
                    else if (filePath.EndsWith(".srt")) newFilePath = filePath.Replace(".srt", ".txt");

                    File.WriteAllLines(newFilePath, linesOut, Encoding.GetEncoding("Windows-1250"));
                    tb_textFile.Text = "Napisy gotowe!";
                }
            }
            catch (Exception exc)
            {
                tb_textFile.Text = exc.Message;                
            }
        }

        private void loadSubtitlesHandler(object sender, RoutedEventArgs e)
        {
            GetSubtitles();
        }      
    }
}
