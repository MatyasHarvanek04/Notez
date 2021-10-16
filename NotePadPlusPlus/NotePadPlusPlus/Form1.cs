using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BetterNotePad
{
    public partial class Form1 : Form
    {
        public static Form1 _Form1;
        public bool KeyPressHandled;
        public static String CurrentPath;
        public static String NameAddon;
        public static string SavedText;

        public Form1()
        {
            InitializeComponent();

            _Form1 = this;
            Form1_Resize(null, null);

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                TextBox.Text = Main.LoadTextFromFile(Environment.GetCommandLineArgs()[1]);
                CurrentPath = Environment.GetCommandLineArgs()[1];
                
                SavedText = TextBox.Text;
            }
            else
            {
                CurrentPath = "";
            }

            UpdateName();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            switch(keyData)
            {
                case (Keys.Control | Keys.S):
                    CurrentPath = Main.SaveTextFile(CurrentPath, TextBox.Text);
                    KeyPressHandled = true;
                    SavedText = TextBox.Text;
                    UpdateName();
                    break;
                case (Keys.Control | Keys.O):
                    TextBox.Text = Main.LoadTextFromFile();
                    SavedText = TextBox.Text;
                    KeyPressHandled = true;
                    UpdateName();
                    break;
                case (Keys.Control | Keys.N):
                    TextBox.Text = "";
                    SavedText = "asdfgedfhb";
                    CurrentPath = "";
                    UpdateName();
                    break;
                default:
                    KeyPressHandled = false;
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = KeyPressHandled;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Size = new Size(ClientSize.Width - 10, ClientSize.Height - 10);
            panel1.Location = new Point(5, 5);
            
            TextBox.Size = new Size(ClientSize.Width - 16, ClientSize.Height -16);
            TextBox.Location = new Point(8, 8);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateName();
        }

        void UpdateName()
        {
            if (CurrentPath == "")
            {
                Text = "Untitled* - Notez";
                return;
            }
            if (TextBox.Text == SavedText)
            {
                Text = Path.GetFileName(CurrentPath) + " - Notez";
            }
            else
            {
                Text = Path.GetFileName(CurrentPath) + "* - Notez";
                
            }
            
        }
    }
}
