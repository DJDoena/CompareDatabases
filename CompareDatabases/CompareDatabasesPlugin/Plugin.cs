using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    [ComVisible(true)]
    [Guid("6FB8CBB1-02C9-4A73-9204-0135D90E6AFF")]
    public class Plugin : IDVDProfilerPlugin, IDVDProfilerPluginInfo
    {
        private IDVDProfilerAPI Api;

        private const Int32 MenuId = 1;

        private String MenuTokenISCP = "";

        private static Settings Settings;

        private readonly String SettingsFile;

        private readonly String ErrorFile;

        private readonly String ApplicationPath;

        public Plugin()
        {
            Debugger.Launch();

            this.ApplicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Doena Soft\CompareDatabases\";
            this.SettingsFile = this.ApplicationPath + "CompareDatabasesPluginSettings.xml";
            this.ErrorFile = Environment.GetEnvironmentVariable("TEMP") + @"\CompareDatabasesPluginCrash.xml";
        }

        #region IDVDProfilerPlugin Members
        public void Load(IDVDProfilerAPI api)
        {
            Debugger.Launch();

            this.Api = api;
            if (Directory.Exists(this.ApplicationPath) == false)
            {
                Directory.CreateDirectory(this.ApplicationPath);
            }
            if (File.Exists(this.SettingsFile))
            {
                try
                {
                    Settings = DVDProfilerSerializer<Settings>.Deserialize(SettingsFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeRead, this.SettingsFile, ex.Message)
                        , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            CreateSettings();
            MenuTokenISCP = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form
                , "Collection", Texts.PluginName, MenuId);
        }

        public void Unload()
        {
            this.Api.UnregisterMenuItem(MenuTokenISCP);
            try
            {
                DVDProfilerSerializer<Settings>.Serialize(SettingsFile, Settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, this.SettingsFile, ex.Message)
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Api = null;
        }

        public void HandleEvent(Int32 EventType, Object EventData)
        {
            if (EventType == PluginConstants.EVENTID_CustomMenuClick)
            {
                this.HandleMenuClick((Int32)EventData);
            }
        }
        #endregion

        #region IDVDProfilerPluginInfo Members
        public string GetName() => Texts.PluginName;

        public string GetDescription() => Texts.PluginDescription;

        public string GetAuthorName() => "Doena Soft.";

        public string GetAuthorWebsite() => Texts.PluginUrl;

        public int GetPluginAPIVersion() => PluginConstants.API_VERSION;

        public int GetVersionMajor()
        {
            Version version;

            version = Assembly.GetAssembly(this.GetType()).GetName().Version;
            return (version.Major);
        }

        public int GetVersionMinor()
        {
            Version version;

            version = Assembly.GetAssembly(this.GetType()).GetName().Version;
            return (version.Minor * 100 + version.Build * 10 + version.Revision);
        }
        #endregion

        private void HandleMenuClick(int MenuEventID)
        {
            if (MenuEventID == MenuId)
            {
                try
                {
                    using (MainForm mainForm = new MainForm(Settings, this.Api))
                    {
                        mainForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        ExceptionXml exceptionXml;

                        MessageBox.Show(String.Format(MessageBoxTexts.CriticalError, ex.Message, this.ErrorFile)
                            , MessageBoxTexts.CriticalErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        if (File.Exists(this.ErrorFile))
                        {
                            File.Delete(ErrorFile);
                        }
                        exceptionXml = new ExceptionXml(ex);

                        DVDProfilerSerializer<ExceptionXml>.Serialize(ErrorFile, exceptionXml);
                    }
                    catch (Exception inEx)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, this.ErrorFile, inEx.Message), MessageBoxTexts.ErrorHeader
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

        #region Plugin Registering
        [DllImport("user32.dll")]
        public extern static int SetParent(int child, int parent);

        [ComImport(), Guid("0002E005-0000-0000-C000-000000000046")]
        internal class StdComponentCategoriesMgr { }

        [ComRegisterFunction()]
        public static void RegisterServer(Type t)
        {
            CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();
            Guid clsidThis = new Guid("6FB8CBB1-02C9-4A73-9204-0135D90E6AFF");
            Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.RegisterClassImplCategories(ref clsidThis, 1,
                new Guid[] { catid });
        }

        [ComUnregisterFunction()]
        public static void UnregisterServer(Type t)
        {
            CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();
            Guid clsidThis = new Guid("6FB8CBB1-02C9-4A73-9204-0135D90E6AFF");
            Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.UnRegisterClassImplCategories(ref clsidThis, 1,
                new Guid[] { catid });
        }
        #endregion
    }
}