using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.ToolBox.Generics;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    public static class Program
    {
        private static Settings Settings;

        private static readonly String SettingsFile;

        private static readonly WindowHandle WindowHandle;

        static Program()
        {
            String applicationPath;

            applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Doena Soft\CompareDatabases\";
            if (Directory.Exists(applicationPath) == false)
            {
                Directory.CreateDirectory(applicationPath);
            }
            SettingsFile = applicationPath + "CompareDatabasesSettings.xml";
            WindowHandle = new WindowHandle();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread()]
        private static void Main(String[] args)
        {
            Boolean skipVersionCheck;

            skipVersionCheck = false;
            if ((args != null) && (args.Length > 0))
            {
                foreach (String argIterator in args)
                {
                    String arg;

                    arg = argIterator.ToLower();
                    if (arg == "/lang=de")
                    {
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
                    }
                    else if (arg == "/lang=en")
                    {
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                    }
                    else if (arg == "/cult=de")
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
                    }
                    else if (arg == "/cult=en")
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                    }
                    else if (arg == "/skipversioncheck")
                    {
                        skipVersionCheck = true;
                    }
                }
            }
            if (File.Exists(SettingsFile))
            {
                try
                {
                    Settings = XmlSerializer<Settings>.Deserialize(SettingsFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(WindowHandle, String.Format(MessageBoxTexts.FileCantBeRead, SettingsFile, ex.Message)
                        , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            CreateSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(Settings, skipVersionCheck));
            try
            {
                XmlSerializer<Settings>.Serialize(SettingsFile, Settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(WindowHandle, String.Format(MessageBoxTexts.FileCantBeWritten, SettingsFile, ex.Message)
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CreateSettings()
        {
            if (Settings == null)
            {
                Settings = new Settings();
            }
            if (Settings.MainForm == null)
            {
                Settings.MainForm = new SizableForm();
            }
            if (Settings.DetailsForm == null)
            {
                Settings.DetailsForm = new BaseForm();
            }
            if (Settings.DefaultValues == null)
            {
                Settings.DefaultValues = new DefaultValues();
            }
        }
    }
}