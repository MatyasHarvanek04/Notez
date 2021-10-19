using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetterNotePad
{
    public static class TabManager
    {
        #region PublicProperties
        public static Tab ActiveTab
        {
            get
            {
                return activeTab;
            } 
            set
            {
                Form1._Form1.TextBoxText = value.MemoryText;
                Form1._Form1.MainTextBox.Focus();
                activeTab = value;
            }
        }
        public static int NubmerOfTabs { get; private set; }
        public static List<Tab> _tabList = new List<Tab>();
        #endregion

        #region PrivateProperties
        private static Tab activeTab;
        private static int Width;
        private static int OffsetX;
        #endregion

        #region PublicMethods
        public static void RefreshTabs()
        {
            foreach (Tab tab in _tabList)
            {
                tab.Invalidate();
            }
        }

        public static void CloseAllTabs()
        {
            RemoveTab(_tabList[0]);
            if(_tabList.Count > 1)
            {
                CloseAllTabs();
            }
            else if(_tabList.Count != 0)
            {
                RemoveTab(_tabList[0]);
                Application.Exit();
            }
        }

        public static Tab GetTabByPath(string Path)
        {
            for (int i = 0; i < _tabList.Count; i++)
            {
                if(Path == _tabList[i].FilePath)
                {
                    return _tabList[i];
                }
            }
            return null;
        }

        public static void UpdateVariables (int Width, int OffsetX)
        {
            TabManager.Width = Width;
            TabManager.OffsetX = OffsetX;
        }
        public static void Recenter(bool CheckPath = false)
        {
            

            if(_tabList.Count > 0)
            {
                int StepX = (Width - OffsetX * 2) / _tabList.Count;

                for (int i = 0; i < _tabList.Count; i++)
                {
                    _tabList[i].X = StepX * i + OffsetX;
                    _tabList[i].TabWidth = StepX;
                    if (StepX * _tabList.Count != Width - OffsetX * 2 && i == _tabList.Count - 1)
                    {
                        _tabList[i].TabWidth = StepX + (Width - OffsetX * 2) % _tabList.Count;
                    }
                }
            }
        }
        #endregion

        #region PublicModifiers
        /// <summary>
        /// Adds Tab to the _Path list
        /// </summary>
        /// <param name="Mode"> 0 = Blank Tab; 1 = OFP; 2 = Direct Path</param>
        /// <param name="path"> Direct path to file</param>
        public static void AddTab(int Mode = 0, string path = "")
        {
            Tab NewTab = new Tab(Mode, path);

            if (Mode == 1 || Mode == 2)
            {
                if (GetTabByPath(NewTab.FilePath) != null)
                {
                    activeTab = GetTabByPath(NewTab.FilePath);
                    return;
                }
            
            }
            _tabList.Add(NewTab);
            Recenter();
        }
        public static void RemoveTab(Tab t)
        {
            Form1.ActiveForm.Controls.Remove(t);
            _tabList.Remove(t);
            t.OnClosing();
            
            Recenter();
        }
        #endregion
    }
}
