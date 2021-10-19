using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BetterNotePad
{
    public partial class Tab : UserControl
    {
        #region Properties
        public int X 
        {
            get { return Location.X; }
            set{ Location = new Point(value, Location.Y); }
        }
        public int TabWidth
        {
            get { return Size.Width; }
            set 
            { 
                Size = new Size(value, Size.Height);
                button1.Location = new Point(TabWidth - button1.Size.Width, button1.Location.Y);
            }
        }
        public String FilePath 
        { 
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;

                if (File.Exists(value))
                {   
                    FileName = Path.GetFileNameWithoutExtension(value);
                }
            }
        }
        public String FileName
        {
            get
            {
                return fileName;
            }
            private set
            {
                label1.Text = value;
                fileName = value;
            }
        }
        public bool Active
        {
            get
            {
                return this == TabManager.ActiveTab;
            }
        }
        public String MemoryText 
        { 
            get
            {
                TabManager.RefreshTabs();
                Invalidate();
                return memoryText;
            }
            set
            {
                memoryText = value;
            }
             
        }
        private String SavedText
        {
            get
            {
                if (File.Exists(FilePath))
                {
                    return File.ReadAllText(FilePath);
                }
                else
                {
                    string s = "";
                    return s;
                }
            }
        }
        public bool Saved
        {
            get
            {
                if (SavedText == MemoryText)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region PrivateProperties
        
        private string filePath;
        private string fileName = "untitled";
        private string memoryText = "";
        private bool mouseOver;
        #endregion

        #region Colors
        Color defaultBackColor;
        Color UnderLineTabColor = Color.FromArgb(79, 104, 255);
        Color TabHighLight = Color.FromArgb(79, 128, 255);
        #endregion

        public Tab(int Mode = 0, string path = "")
        {
            InitializeComponent();

            if (Mode == 0)
            {
                FileName = "Untitled";
            }
            if (Mode == 1)
            {
                LoadTextFromFileOFP();
            }
            if (Mode == 2)
            {
                LoadTextFromPath(path);
            }
            TabManager.ActiveTab = this;
            Form1._Form1.Controls.Add(this);
            defaultBackColor = BackColor;
            label1.BackColor = System.Drawing.Color.Transparent;
            
        }

        #region FileManagment
        public void Save()
        {
            if (!File.Exists(filePath))
            {
                SaveFileDialog SFD = new SaveFileDialog();

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MyNotes\"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MyNotes\");
                }

                SFD.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MyNotes";
                SFD.RestoreDirectory = true;
                SFD.Filter = "Text (*.txt) |*txt";
                SFD.FileName = "Untitled.txt";
                SFD.DefaultExt = ".txt";
                SFD.ShowDialog();

                if (SFD.FileName != "")
                {
                    File.WriteAllText(SFD.FileName, MemoryText);
                    FilePath = SFD.FileName;
                }
                else
                {
                    MessageBox.Show("Please enter valid path");
                }
            }
            else
            {
                File.WriteAllText(FilePath, MemoryText);
            }
        }
        private void LoadTextFromFileOFP()
        {
            OpenFileDialog OFP = new OpenFileDialog();
            OFP.Filter = "Text (*.txt) |*txt";
            OFP.ShowDialog();

            if (File.Exists(OFP.FileName))
            {
                FilePath = OFP.FileName;
                MemoryText = File.ReadAllText(OFP.FileName);
            }
            else
            {
                MessageBox.Show("File doesn't exist");
            }
        }
        private void LoadTextFromPath(string path)
        {
            

            if (File.Exists(path))
            {
                FilePath = path;
                MemoryText = File.ReadAllText(path);
            }
            else
            {
                MessageBox.Show("File doesn't exist");
            }
        }
        #endregion

        #region Events
        private void button1_Click(object sender, EventArgs e)
        {
            TabManager.RemoveTab(this);
        }
        private void Tab_Click(object sender, EventArgs e)
        {
            TabManager.ActiveTab = this;
        }
        private void Tab_Paint(object sender, PaintEventArgs e)
        {
            if(mouseOver && Active)
            {
                e.Graphics.Clear(TabHighLight);
                e.Graphics.FillRectangle(new SolidBrush(UnderLineTabColor), 0, Height - 2, Width, 2);
            }
            else if(Active)
            {
                e.Graphics.Clear(defaultBackColor);
                e.Graphics.FillRectangle(new SolidBrush(UnderLineTabColor), 0, Height - 2, Width, 2);
            }
            else if(mouseOver)
            {
                e.Graphics.Clear(TabHighLight);
            }
            else
            {
                e.Graphics.Clear(defaultBackColor);
            }
        }
        private void Tab_MouseEnter(object sender, EventArgs e)
        {
            mouseOver = true;
            Invalidate();
        }
        private void Tab_MouseLeave(object sender, EventArgs e)
        {
            mouseOver = false;
            Invalidate();
        }
        public void OnClosing()
        {
            if (SavedText != memoryText)
            {
                if (MessageBox.Show(fileName + ".txt" + " isn't saved. Do you want to save it ?", "", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    Save();
                }
            }
            if (TabManager._tabList.Count == 0)
            {
                Application.Exit();
            }
            else
            {
                TabManager.ActiveTab = TabManager._tabList[0];
            }
        }
        #endregion




    }
}
