﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400;
using DoenaSoft.ToolBox.Generics;
using Invelos.DVDProfilerPlugin;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    //[System.Runtime.InteropServices.ComVisible(false)]
    public partial class MainForm : Form
    {
        private readonly Settings Settings;

        private readonly IDVDProfilerAPI Api;

        private Boolean CanClose;

        private Collection Collection;

        private ProgressWindow ProgressWindow;

        private readonly Boolean SkipVersionCheck;

        #region Delegates
        private delegate void ProgressBarDelegate();

        private delegate String GetProfileDataDelegate(String id);

        private delegate void ThreadFinishedDelegate(Collection collection);
        #endregion

        public MainForm(Settings settings, IDVDProfilerAPI api)
        {
            SkipVersionCheck = false;
            CanClose = true;
            Settings = settings;
            Api = api;
            this.InitializeComponent();
            LeftFileTextBox.Text = Texts.CurrentDatabase;
            LeftFileButton.Enabled = false;
        }

        public MainForm(Settings settings, Boolean skipVersionCheck)
        {
            SkipVersionCheck = skipVersionCheck;
            CanClose = true;
            Settings = settings;
            this.InitializeComponent();
        }

        private void OnMainFormClosing(Object sender, FormClosingEventArgs e)
        {
            if (CanClose == false)
            {
                e.Cancel = true;
                return;
            }
            Settings.MainForm.Left = this.Left;
            Settings.MainForm.Top = this.Top;
            Settings.MainForm.Width = this.Width;
            Settings.MainForm.Height = this.Height;
            Settings.MainForm.WindowState = this.WindowState;
            Settings.MainForm.RestoreBounds = this.RestoreBounds;
        }

        private void LayoutForm()
        {
            if (Settings.MainForm.WindowState == FormWindowState.Normal)
            {
                this.Left = Settings.MainForm.Left;
                this.Top = Settings.MainForm.Top;
                if (Settings.MainForm.Width > this.MinimumSize.Width)
                {
                    this.Width = Settings.MainForm.Width;
                }
                else
                {
                    this.Width = this.MinimumSize.Width;
                }
                if (Settings.MainForm.Height > this.MinimumSize.Height)
                {
                    this.Height = Settings.MainForm.Height;
                }
                else
                {
                    this.Height = this.MinimumSize.Height;
                }
            }
            else
            {
                this.Left = Settings.MainForm.RestoreBounds.X;
                this.Top = Settings.MainForm.RestoreBounds.Y;
                if (Settings.MainForm.RestoreBounds.Width > this.MinimumSize.Width)
                {
                    this.Width = Settings.MainForm.RestoreBounds.Width;
                }
                else
                {
                    this.Width = this.MinimumSize.Width;
                }
                if (Settings.MainForm.RestoreBounds.Height > this.MinimumSize.Height)
                {
                    this.Height = Settings.MainForm.RestoreBounds.Height;
                }
                else
                {
                    this.Height = this.MinimumSize.Height;
                }
            }
            if (Settings.MainForm.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = Settings.MainForm.WindowState;
            }
        }

        private void OnMainFormLoad(Object sender, EventArgs e)
        {
            this.LayoutForm();
            WinMergeTextBox.Text = Settings.DefaultValues.WinMergePath;
            this.CheckForNewVersion(true);
        }

        private void OnLeftFileButtonClick(Object sender, EventArgs e)
        {
            SelectCollectionFile(LeftFileTextBox);
        }

        private void OnRightFileButtonClick(Object sender, EventArgs e)
        {
            SelectCollectionFile(RightFileTextBox);
        }

        private static void SelectCollectionFile(TextBox textBox)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "Collection Export File|*.xml";
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Title = "Select the Collection Export file.";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = ofd.FileName;
                }
            }
        }

        private void OnWinMergeLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            String process;

            process = "http://winmerge.org/";
            if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith("de"))
            {
                process += "?lang=de";
            }
            Process.Start(process);
        }

        private void OnCompareDatabasesButtonClick(Object sender, EventArgs e)
        {
            if (Api == null)
            {
                if (CheckFile(LeftFileTextBox.Text) == false)
                {
                    MessageBox.Show(this, MessageBoxTexts.InvalidLeftFileSelected
                        , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (CheckFile(RightFileTextBox.Text) == false)
            {
                MessageBox.Show(this, MessageBoxTexts.InvalidRightFileSelected
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckFile(WinMergeTextBox.Text) == false)
            {
                MessageBox.Show(this, MessageBoxTexts.WinMergeMissingWarning
                 , MessageBoxTexts.WarningHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            CanClose = false;
            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;
            if (Api != null)
            {
                if (Collection == null)
                {
                    Thread thread;
                    Object[] allIds;

                    ProgressWindow = new ProgressWindow();
                    ProgressWindow.ProgressBar.Minimum = 0;
                    ProgressWindow.ProgressBar.Step = 1;
                    ProgressWindow.CanClose = false;
                    allIds = (Object[])(Api.GetAllProfileIDs());
                    ProgressWindow.ProgressBar.Maximum = allIds.Length;
                    ProgressWindow.Show();
                    if (TaskbarManager.IsPlatformSupported)
                    {
                        TaskbarManager.Instance.OwnerHandle = this.Handle;
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(0, ProgressWindow.ProgressBar.Maximum);
                    }
                    thread = new Thread(new ParameterizedThreadStart(this.ThreadRun));
                    thread.IsBackground = false;
                    thread.Start(new Object[] { allIds });
                }
                else
                {
                    this.ThreadFinished(Collection);
                }
            }
            else
            {
                Collection leftCollection;

                try
                {
                    leftCollection = XmlSerializer<Collection>.Deserialize(LeftFileTextBox.Text);
                }
                catch (Exception ex)
                {
                    this.FinishCompare();
                    MessageBox.Show(this, String.Format(MessageBoxTexts.FileCantBeRead, LeftFileTextBox.Text, ex.Message)
                        , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.ThreadFinished(leftCollection);
            }
        }

        private static Boolean CheckFile(String fileName)
        {
            if (File.Exists(Environment.ExpandEnvironmentVariables(fileName)))
            {
                return (true);
            }
            return (false);
        }

        private void ThreadFinished(Collection leftCollection)
        {
            Collection rightCollection;
            Dictionary<String, DVD> fullListLeft;
            Dictionary<String, DVD[]> duplicates;
            Dictionary<String, DVD> onlyRighties;

            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                TaskbarManager.Instance.OwnerHandle = IntPtr.Zero;
            }
            if (ProgressWindow != null)
            {
                ProgressWindow.CanClose = true;
                ProgressWindow.Close();
                ProgressWindow.Dispose();
                ProgressWindow = null;
            }
            DuplicateListView.Items.Clear();
            OnlyLeftiesListView.Items.Clear();
            OnlyRightiesListView.Items.Clear();
            try
            {
                rightCollection = XmlSerializer<Collection>.Deserialize(RightFileTextBox.Text);
            }
            catch (Exception ex)
            {
                this.FinishCompare();
                MessageBox.Show(this, String.Format(MessageBoxTexts.FileCantBeRead, RightFileTextBox.Text, ex.Message)
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((leftCollection != null) && (leftCollection.DVDList != null))
            {
                fullListLeft = new Dictionary<String, DVD>(leftCollection.DVDList.Length);
                foreach (DVD dvd in leftCollection.DVDList)
                {
                    fullListLeft.Add(dvd.ID, dvd);
                }
            }
            else
            {
                fullListLeft = new Dictionary<String, DVD>(0);
            }
            duplicates = new Dictionary<String, DVD[]>(fullListLeft.Count);
            onlyRighties = new Dictionary<String, DVD>();
            if ((rightCollection != null) && (rightCollection.DVDList != null))
            {
                foreach (DVD rightDvd in rightCollection.DVDList)
                {
                    DVD leftDvd;

                    if (fullListLeft.TryGetValue(rightDvd.ID, out leftDvd))
                    {
                        String leftInfo = XmlSerializer<DVD>.ToString(leftDvd, DVD.DefaultEncoding);

                        leftInfo = leftInfo.Replace("\r\n", "\n");

                        String rightInfo = XmlSerializer<DVD>.ToString(rightDvd, DVD.DefaultEncoding);

                        rightInfo = rightInfo.Replace("\r\n", "\n");

                        if ((leftInfo.Length != rightInfo.Length) || (leftInfo != rightInfo))
                        {
                            duplicates.Add(rightDvd.ID, new[] { leftDvd, rightDvd });
                        }

                        fullListLeft.Remove(rightDvd.ID);
                    }
                    else
                    {
                        onlyRighties.Add(rightDvd.ID, rightDvd);
                    }
                }
            }
            if ((duplicates.Count == 0) && (fullListLeft.Count == 0) && (onlyRighties.Count == 0))
            {
                this.FinishCompare();
                MessageBox.Show(this, MessageBoxTexts.DatabasesAreDifferent, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
                return;
            }
            foreach (KeyValuePair<String, DVD[]> kvp in duplicates)
            {
                ListViewItem item;

                item = new ListViewItem(new String[] { kvp.Value[0].SortTitle, kvp.Value[0].Title, kvp.Value[0].UPC, kvp.Value[0].ID_LocalityDesc });
                item.Tag = kvp;
                DuplicateListView.Items.Add(item);
            }
            foreach (KeyValuePair<String, DVD> kvp in fullListLeft)
            {
                ListViewItem item;
                DVD temp;

                item = new ListViewItem(new String[] { kvp.Value.SortTitle, kvp.Value.Title, kvp.Value.UPC, kvp.Value.ID_LocalityDesc });
                temp = CreateTempDvd();
                item.Tag = new KeyValuePair<String, DVD[]>(kvp.Key, new DVD[] { kvp.Value, temp });
                OnlyLeftiesListView.Items.Add(item);
            }
            foreach (KeyValuePair<String, DVD> kvp in onlyRighties)
            {
                ListViewItem item;
                DVD temp;

                item = new ListViewItem(new String[] { kvp.Value.SortTitle, kvp.Value.Title, kvp.Value.UPC, kvp.Value.ID_LocalityDesc });
                temp = CreateTempDvd();
                item.Tag = new KeyValuePair<String, DVD[]>(kvp.Key, new DVD[] { temp, kvp.Value });
                OnlyRightiesListView.Items.Add(item);
            }
            this.FinishCompare();
            MessageBox.Show(this, MessageBoxTexts.Done, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK
                  , MessageBoxIcon.Information);
        }

        private static DVD CreateTempDvd()
        {
            DVD dvd;

            dvd = new DVD();
            dvd.ProfileTimestamp = new DateTime(1900, 1, 1, 0, 0, 0);
            dvd.LastEdited = dvd.ProfileTimestamp;
            dvd.LastEditedSpecified = true;
            dvd.PurchaseInfo.Date = dvd.ProfileTimestamp;
            dvd.PurchaseInfo.DateSpecified = true;
            return (dvd);
        }

        private void FinishCompare()
        {
            this.Enabled = true;
            this.Cursor = Cursors.Default;
            this.UseWaitCursor = false;
            CanClose = true;
        }

        private void ThreadRun(Object param)
        {
            Collection collection;

            collection = null;
            try
            {
                Object[] allIds;
                List<DVD> dvdList;

                allIds = (Object[])(((Object[])param)[0]);
                dvdList = new List<DVD>(allIds.Length);
                for (Int32 i = 0; i < allIds.Length; i++)
                {
                    String xml = (String)(this.Invoke(new GetProfileDataDelegate(this.GetProfileData), allIds[i]));

                    DVD dvd = XmlSerializer<DVD>.FromString(xml, DVD.DefaultEncoding);

                    dvdList.Add(dvd);

                    this.Invoke(new ProgressBarDelegate(this.UpdateProgressBar));
                }
                collection = new Collection();
                collection.DVDList = dvdList.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Collection = collection;
                this.Invoke(new ThreadFinishedDelegate(this.ThreadFinished), collection);
            }
        }

        private void UpdateProgressBar()
        {
            ProgressWindow.ProgressBar.PerformStep();
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressValue(ProgressWindow.ProgressBar.Value, ProgressWindow.ProgressBar.Maximum);
            }
        }

        private String GetProfileData(String id)
        {
            IDVDInfo dvdInfo;
            String xml;

            Api.DVDByProfileID(out dvdInfo, id, -1, -1);
            xml = dvdInfo.GetXML(true);
            return (xml);
        }

        private void OnWinMergeButtonClick(Object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "WinMergeU.exe|WinMergeU.exe";
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Title = "Select the WinMerge executable.";
                ofd.InitialDirectory
                    = (new FileInfo(Environment.ExpandEnvironmentVariables(WinMergeTextBox.Text))).DirectoryName;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    WinMergeTextBox.Text = ofd.FileName;
                    Settings.DefaultValues.WinMergePath = WinMergeTextBox.Text;
                }
            }
        }

        private void OnListViewDoubleClick(Object sender, EventArgs e)
        {
            ListView listView;

            listView = (ListView)sender;
            if (listView.SelectedIndices.Count == 1)
            {
                KeyValuePair<String, DVD[]> kvp;
                Int32 selectedIndex;

                selectedIndex = listView.SelectedIndices[0];
                kvp = (KeyValuePair<String, DVD[]>)(listView.Items[selectedIndex].Tag);
                using (DetailsForm detailsForm = new DetailsForm(kvp.Value[0], kvp.Value[1], Settings))
                {
                    detailsForm.ShowDialog();
                }
            }
        }

        private void OnCheckForUpdatesToolStripMenuItemClick(Object sender, EventArgs e)
        {
            this.CheckForNewVersion(false);
        }

        private void OnReadMeToolStripMenuItemClick(Object sender, EventArgs e)
        {
            this.OpenReadme();
        }

        private void OpenReadme()
        {
            String helpFile;

            helpFile = (new FileInfo(this.GetType().Assembly.Location)).DirectoryName + @"\ReadMe\readme.html";
            if (File.Exists(helpFile))
            {
                using (HelpForm helpForm = new HelpForm(helpFile))
                {
                    helpForm.Text = "Read Me";
                    helpForm.ShowDialog(this);
                }
            }
        }

        private void OnAboutToolStripMenuItemClick(Object sender, EventArgs e)
        {
            using (AboutBox aboutBox = new AboutBox(this.GetType().Assembly))
            {
                aboutBox.ShowDialog();
            }
        }

        private void CheckForNewVersion(Boolean silently)
        {
            OnlineAccess.Init("Doena Soft.", "CompareDatabases");
            if (silently)
            {
                if (SkipVersionCheck == false)
                {
                    OnlineAccess.CheckForNewVersion("http://doena-soft.de/dvdprofiler/3.9.0/versions.xml", this, "CompareDatabases"
                        , this.GetType().Assembly, silently);
                }
            }
            else
            {
                OnlineAccess.CheckForNewVersion("http://doena-soft.de/dvdprofiler/3.9.0/versions.xml", this, "CompareDatabases"
                        , this.GetType().Assembly, silently);
            }
        }

        private void OnExportDifferentProfilesFlagSetToolStripMenuItemClick(Object sender, EventArgs e)
        {
            this.ExportFlagSet(DuplicateListView, "DifferentProfiles");
        }

        private void OnExportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItemClick(Object sender, EventArgs e)
        {
            this.ExportFlagSet(OnlyLeftiesListView, "LeftOnlyProfiles");
        }

        private void OnExportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItemClick(Object sender, EventArgs e)
        {
            this.ExportFlagSet(OnlyRightiesListView, "RightOnlyProfiles");
        }

        private void ExportFlagSet(ListView listView
            , String fileName)
        {
            if (listView.Items.Count == 0)
            {
                MessageBox.Show(MessageBoxTexts.NoEntriesSelected, MessageBoxTexts.WarningHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.DefaultExt = ".lst";
                sfd.FileName = fileName + sfd.DefaultExt;
                sfd.Filter = "Flag Set List files|*.lst";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.ValidateNames = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding(1252)))
                        {
                            foreach (ListViewItem item in listView.Items)
                            {
                                KeyValuePair<String, DVD[]> kvp;

                                kvp = (KeyValuePair<String, DVD[]>)(item.Tag);
                                sw.WriteLine(kvp.Key);
                            }
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, sfd.FileName, ex.Message), MessageBoxTexts.ErrorHeader
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}