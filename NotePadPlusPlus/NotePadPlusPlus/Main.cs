using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetterNotePad
{
    public static class Main
    {
        public static string SaveTextFile(string Path, string Text)
        {
            if (!File.Exists(Path))
            {
                SaveFileDialog SFD = new SaveFileDialog();

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MyNotes\"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MyNotes\");
                }

                SFD.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MyNotes";
                SFD.RestoreDirectory = true;
                SFD.Filter = "Text (*.txt) |*txt";
                SFD.DefaultExt = ".txt";
                SFD.ShowDialog();

                if (SFD.FileName != "")
                {
                    File.WriteAllText(SFD.FileName, Text);
                    Form1.CurrentPath = SFD.FileName;
                    
                    return Path;
                }
                else
                {
                    MessageBox.Show("Please enter valid path");
                    return "";
                }
            }
            else
            {
                File.WriteAllText(Path, Text);
                return Path;
            }
        }
        public static string LoadTextFromFile(string path = "")
        {
            if(path == "")
            {
                OpenFileDialog OFP = new OpenFileDialog();
                OFP.Filter = "Text (*.txt) |*txt";
                OFP.ShowDialog();

                if(File.Exists(OFP.FileName))
                {
                    Form1.CurrentPath = OFP.FileName;
                    Form1.SavedText = File.ReadAllText(OFP.FileName);
                    return File.ReadAllText(OFP.FileName);
                }
                else
                {
                    MessageBox.Show("File doesn't exist");
                    return "";
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    Form1.CurrentPath = path;
                    return File.ReadAllText(path);
                }
                else
                {
                    MessageBox.Show("File doesn't exist");
                    return "";
                }

            }
        }

        public static void NewText()
        {
            Form1._Form1.TextBox.Text = "";
            Form1.SavedText = "Saved";
            Form1.CurrentPath = "";
        }
    }
}
