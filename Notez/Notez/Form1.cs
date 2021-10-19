using Microsoft.Win32;
using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BetterNotePad
{
    public partial class Form1 : Form
    {
        #region PublicProperties
        public int Mode
        {
            get
            {
                return mode();
            }
        }
        public string TextBoxText
        {
            get
            {
                return MainTextBox.Text;
            }
            set
            {
                MainTextBox.Text = value;
            }
        }
        public static Form1 _Form1;
        #endregion

        #region PrivateProperties
        bool KeyPressHandled;
        Point Offset = new Point(3, 3);
        Point InnerOffset = new Point(3, 3);
        int TopOffset = 25;
        /// <summary>
        /// Returns 0 if only one app is opened 
        /// Returns 1 if more than one app is opened
        /// </summary>
        /// <returns></returns>
        int mode()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() == 1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        #endregion


        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
            Form1_Resize(null, null);
            FormClosing += Form1_FormClosing;
            new Server();
            if(Environment.GetCommandLineArgs().Length > 1)
            {
                TabManager.AddTab(2, Environment.GetCommandLineArgs()[1]);
            }
            else
            {
                TabManager.AddTab();
            }
        }



        #region Events

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                TabManager.CloseAllTabs();
            }
            
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = KeyPressHandled;
            if (!e.Handled)
            {
                TabManager.ActiveTab.MemoryText = MainTextBox.Text;
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            TabManager.UpdateVariables(ClientSize.Width, Offset.X);
            TabManager.Recenter();

            MainTextBox.Size = new Size(ClientSize.Width - (Offset.X + InnerOffset.X) * 2, ClientSize.Height - (Offset.Y + InnerOffset.Y) * 2 - TopOffset);
            MainTextBox.Location = new Point(Offset.X + InnerOffset.X, Offset.Y + InnerOffset.Y + TopOffset);

            panel1.Size = new Size(ClientSize.Width - Offset.X * 2, ClientSize.Height - Offset.Y * 2 - TopOffset);
            panel1.Location = new Point(Offset.X, Offset.Y + TopOffset);
        }
        private void MainTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region OverRide
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.S):
                    TabManager.ActiveTab.Save();
                    break;
                case (Keys.Control | Keys.O):
                    TabManager.AddTab(1);
                    KeyPressHandled = true;
                    break;
                case (Keys.Control | Keys.N):
                    TabManager.AddTab(0);
                    KeyPressHandled = true;
                    break;
                default:
                    KeyPressHandled = false;
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        private void MainTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TabManager.ActiveTab.MemoryText = TextBoxText;
        }
    }
}
