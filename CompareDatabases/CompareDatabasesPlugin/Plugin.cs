using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using Invelos.DVDProfilerPlugin;
using System.Diagnostics;
using System.Reflection;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    [ComVisible(true)]
    [Guid("01CD7B58-DD10-47D7-B15A-899431CF5157")]
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
            this.ApplicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Doena Soft\CompareDatabases\";
            this.SettingsFile = this.ApplicationPath + "CompareDatabasesPluginSettings.xml";
            this.ErrorFile = Environment.GetEnvironmentVariable("TEMP") + @"\CompareDatabasesPluginCrash.xml";
        }

        #region IDVDProfilerPlugin Members
        public void Load(IDVDProfilerAPI api)
        {            
            this.Api = api;
            if(Directory.Exists(this.ApplicationPath) == false)
            {
                Directory.CreateDirectory(this.ApplicationPath);
            }
            if(File.Exists(this.SettingsFile))
            {
                try
                {
                    Settings = Settings.Deserialize(this.SettingsFile);
                }
                catch(Exception ex)
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
                Settings.Serialize(this.SettingsFile);
            }
            catch(Exception ex)
            {
                MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, this.SettingsFile, ex.Message)
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Api = null;
        }

        public void HandleEvent(Int32 EventType, Object EventData)
        {
            if(EventType == PluginConstants.EVENTID_CustomMenuClick)
            {
                this.HandleMenuClick((Int32)EventData);
            }
        }
        #endregion

        #region IDVDProfilerPluginInfo Members
        public String GetName()
        {
            //if(Settings.DefaultValues.UiCulture!=0)
            //{
            //    return (Texts.ResourceManager.GetString("PluginName", CultureInfo.GetCultureInfo(Settings.DefaultValues.UiCulture)));
            //}
            return (Texts.PluginName);
        }

        public String GetDescription()
        {
            //if(Settings.DefaultValues.UiCulture != 0)
            //{
            //    return (Texts.ResourceManager.GetString("PluginDescription", CultureInfo.GetCultureInfo(Settings.DefaultValues.UiCulture)));
            //}
            return (Texts.PluginDescription);
        }

        public String GetAuthorName()
        {
            return ("Doena Soft.");
        }

        public String GetAuthorWebsite()
        {
            //if(Settings.DefaultValues.UiCulture != 0)
            //{
            //    return (Texts.ResourceManager.GetString("PluginUrl", CultureInfo.GetCultureInfo(Settings.DefaultValues.UiCulture)));
            //}
            return (Texts.PluginUrl);
        }

        public Int32 GetPluginAPIVersion()
        {
            return (PluginConstants.API_VERSION);
        }

        public Int32 GetVersionMajor()
        {
            Version version;

            version = Assembly.GetAssembly(this.GetType()).GetName().Version;
            return (version.Major);
        }

        public Int32 GetVersionMinor()
        {
            Version version;

            version = Assembly.GetAssembly(this.GetType()).GetName().Version;
            return (version.Minor * 100 + version.Build * 10 + version.Revision);
        }
        #endregion

        private void HandleMenuClick(Int32 MenuEventID)
        {
            if(MenuEventID == MenuId)
            {
                try
                {
                    using(MainForm mainForm = new MainForm(Settings, this.Api))
                    {
                        mainForm.ShowDialog();
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        ExceptionXml exceptionXml;

                        MessageBox.Show(String.Format(MessageBoxTexts.CriticalError, ex.Message, this.ErrorFile)
                            , MessageBoxTexts.CriticalErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        if(File.Exists(this.ErrorFile))
                        {
                            File.Delete(ErrorFile);
                        }
                        exceptionXml = new ExceptionXml(ex);

                        Serializer<ExceptionXml>.Serialize(ErrorFile, exceptionXml);
                    }
                    catch(Exception inEx)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, this.ErrorFile, inEx.Message), MessageBoxTexts.ErrorHeader
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private static void CreateSettings()
        {
            if(Settings == null)
            {
                Settings = new Settings();
            }
            if(Settings.MainForm == null)
            {
                Settings.MainForm = new SizableForm();
            }
            if(Settings.DetailsForm ==null)
            {
                Settings.DetailsForm = new BaseForm();
            }
            if(Settings.DefaultValues == null)
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
            Guid clsidThis = new Guid("01CD7B58-DD10-47D7-B15A-899431CF5157");
            Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.RegisterClassImplCategories(ref clsidThis, 1,
                new Guid[] { catid });
        }

        [ComUnregisterFunction()]
        public static void UnregisterServer(Type t)
        {
            CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();
            Guid clsidThis = new Guid("01CD7B58-DD10-47D7-B15A-899431CF5157");
            Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.UnRegisterClassImplCategories(ref clsidThis, 1,
                new Guid[] { catid });
        }
        #endregion
    }
}