using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    //[System.Runtime.InteropServices.ComVisible(false)]
    [Serializable]
    public class BaseForm
    {
        public Int32 Top = 50;

        public Int32 Left = 50;
    }

    //[System.Runtime.InteropServices.ComVisible(false)]
    [Serializable]
    public class SizableForm : BaseForm
    {
        public Int32 Height = 500;

        public Int32 Width = 900;

        public FormWindowState WindowState = FormWindowState.Normal;

        public Rectangle RestoreBounds;
    }

    //[System.Runtime.InteropServices.ComVisible(false)]
    [Serializable]
    public class Settings
    {
        public SizableForm MainForm;

        public BaseForm DetailsForm;

        public DefaultValues DefaultValues;

        public String CurrentVersion;
    }
}