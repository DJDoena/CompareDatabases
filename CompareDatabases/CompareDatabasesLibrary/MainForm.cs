using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version390;
using Invelos.DVDProfilerPlugin;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    public partial class MainForm : Form
    {
        private Settings Settings;

        private IDVDProfilerAPI Api;

        private Boolean CanClose;

        private Collection Collection;

        private ProgressWindow ProgressWindow;

        private Boolean SkipVersionCheck;

        #region Delegates
        private delegate void ProgressBarDelegate();

        private delegate String GetProfileDataDelegate(String id);

        private delegate void ThreadFinishedDelegate(Collection collection);
        #endregion

        public MainForm(Settings settings, IDVDProfilerAPI api)
        {
            this.SkipVersionCheck = false;
            this.CanClose = true;
            this.Settings = settings;
            this.Api = api;
            InitializeComponent();
            this.LeftFileTextBox.Text = Texts.CurrentDatabase;
            this.LeftFileButton.Enabled = false;
        }

        public MainForm(Settings settings, Boolean skipVersionCheck)
        {
            this.SkipVersionCheck = skipVersionCheck;
            this.CanClose = true;
            this.Settings = settings;
            InitializeComponent();
        }

        private void OnMainFormClosing(Object sender, FormClosingEventArgs e)
        {
            if (this.CanClose == false)
            {
                e.Cancel = true;
                return;
            }
            this.Settings.MainForm.Left = this.Left;
            this.Settings.MainForm.Top = this.Top;
            this.Settings.MainForm.Width = this.Width;
            this.Settings.MainForm.Height = this.Height;
            this.Settings.MainForm.WindowState = this.WindowState;
            this.Settings.MainForm.RestoreBounds = this.RestoreBounds;
        }

        private void LayoutForm()
        {
            if (this.Settings.MainForm.WindowState == FormWindowState.Normal)
            {
                this.Left = this.Settings.MainForm.Left;
                this.Top = this.Settings.MainForm.Top;
                if (this.Settings.MainForm.Width > this.MinimumSize.Width)
                {
                    this.Width = this.Settings.MainForm.Width;
                }
                else
                {
                    this.Width = this.MinimumSize.Width;
                }
                if (this.Settings.MainForm.Height > this.MinimumSize.Height)
                {
                    this.Height = this.Settings.MainForm.Height;
                }
                else
                {
                    this.Height = this.MinimumSize.Height;
                }
            }
            else
            {
                this.Left = this.Settings.MainForm.RestoreBounds.X;
                this.Top = this.Settings.MainForm.RestoreBounds.Y;
                if (this.Settings.MainForm.RestoreBounds.Width > this.MinimumSize.Width)
                {
                    this.Width = this.Settings.MainForm.RestoreBounds.Width;
                }
                else
                {
                    this.Width = this.MinimumSize.Width;
                }
                if (this.Settings.MainForm.RestoreBounds.Height > this.MinimumSize.Height)
                {
                    this.Height = this.Settings.MainForm.RestoreBounds.Height;
                }
                else
                {
                    this.Height = this.MinimumSize.Height;
                }
            }
            if (this.Settings.MainForm.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = this.Settings.MainForm.WindowState;
            }
        }

        private void OnMainFormLoad(Object sender, EventArgs e)
        {
            this.LayoutForm();
            this.WinMergeTextBox.Text = this.Settings.DefaultValues.WinMergePath;
            this.CheckForNewVersion(true);
        }

        private void OnLeftFileButtonClick(Object sender, EventArgs e)
        {
            SelectCollectionFile(this.LeftFileTextBox);
        }

        private void OnRightFileButtonClick(Object sender, EventArgs e)
        {
            SelectCollectionFile(this.RightFileTextBox);
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
            if (this.Api == null)
            {
                if (CheckFile(this.LeftFileTextBox.Text) == false)
                {
                    MessageBox.Show(this, MessageBoxTexts.InvalidLeftFileSelected
                        , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (CheckFile(this.RightFileTextBox.Text) == false)
            {
                MessageBox.Show(this, MessageBoxTexts.InvalidRightFileSelected
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckFile(this.WinMergeTextBox.Text) == false)
            {
                MessageBox.Show(this, MessageBoxTexts.WinMergeMissingWarning
                 , MessageBoxTexts.WarningHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.CanClose = false;
            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;
            if (this.Api != null)
            {
                if (this.Collection == null)
                {
                    Thread thread;
                    Object[] allIds;

                    this.ProgressWindow = new ProgressWindow();
                    this.ProgressWindow.ProgressBar.Minimum = 0;
                    this.ProgressWindow.ProgressBar.Step = 1;
                    this.ProgressWindow.CanClose = false;
                    allIds = (Object[])(this.Api.GetAllProfileIDs());
                    this.ProgressWindow.ProgressBar.Maximum = allIds.Length;
                    this.ProgressWindow.Show();
                    if (TaskbarManager.IsPlatformSupported)
                    {
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(0, this.ProgressWindow.ProgressBar.Maximum);
                    }
                    thread = new Thread(new ParameterizedThreadStart(this.ThreadRun));
                    thread.IsBackground = false;
                    thread.Start(new Object[] { allIds });
                }
                else
                {
                    this.ThreadFinished(this.Collection);
                }
            }
            else
            {
                Collection leftCollection;

                try
                {
                    leftCollection = Serializer<Collection>.Deserialize(LeftFileTextBox.Text);
                }
                catch (Exception ex)
                {
                    this.FinishCompare();
                    MessageBox.Show(this, String.Format(MessageBoxTexts.FileCantBeRead, this.LeftFileTextBox.Text, ex.Message)
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
            }
            if (this.ProgressWindow != null)
            {
                this.ProgressWindow.CanClose = true;
                this.ProgressWindow.Close();
                this.ProgressWindow.Dispose();
                this.ProgressWindow = null;
            }
            this.DuplicateListView.Items.Clear();
            this.OnlyLeftiesListView.Items.Clear();
            this.OnlyRightiesListView.Items.Clear();
            try
            {
                rightCollection = Serializer<Collection>.Deserialize(RightFileTextBox.Text);
            }
            catch (Exception ex)
            {
                this.FinishCompare();
                MessageBox.Show(this, String.Format(MessageBoxTexts.FileCantBeRead, this.RightFileTextBox.Text, ex.Message)
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
                        String leftInfo = Serializer<DVD>.ToString(leftDvd, DVD.DefaultEncoding);

                        leftInfo = leftInfo.Replace("\r\n", "\n");

                        String rightInfo = Serializer<DVD>.ToString(rightDvd, DVD.DefaultEncoding);

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
                this.DuplicateListView.Items.Add(item);
            }
            foreach (KeyValuePair<String, DVD> kvp in fullListLeft)
            {
                ListViewItem item;
                DVD temp;

                item = new ListViewItem(new String[] { kvp.Value.SortTitle, kvp.Value.Title, kvp.Value.UPC, kvp.Value.ID_LocalityDesc });
                temp = CreateTempDvd();
                item.Tag = new KeyValuePair<String, DVD[]>(kvp.Key, new DVD[] { kvp.Value, temp });
                this.OnlyLeftiesListView.Items.Add(item);
            }
            foreach (KeyValuePair<String, DVD> kvp in onlyRighties)
            {
                ListViewItem item;
                DVD temp;

                item = new ListViewItem(new String[] { kvp.Value.SortTitle, kvp.Value.Title, kvp.Value.UPC, kvp.Value.ID_LocalityDesc });
                temp = CreateTempDvd();
                item.Tag = new KeyValuePair<String, DVD[]>(kvp.Key, new DVD[] { temp, kvp.Value });
                this.OnlyRightiesListView.Items.Add(item);
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
            this.CanClose = true;
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
                    String xml = (String)(Invoke(new GetProfileDataDelegate(GetProfileData), allIds[i]));

                    DVD dvd = Serializer<DVD>.FromString(xml, DVD.DefaultEncoding);

                    dvdList.Add(dvd);

                    Invoke(new ProgressBarDelegate(UpdateProgressBar));
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
                this.Collection = collection;
                this.Invoke(new ThreadFinishedDelegate(this.ThreadFinished), collection);
            }
        }

        private void UpdateProgressBar()
        {
            this.ProgressWindow.ProgressBar.PerformStep();
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressValue(this.ProgressWindow.ProgressBar.Value, this.ProgressWindow.ProgressBar.Maximum);
            }
        }

        private String GetProfileData(String id)
        {
            IDVDInfo dvdInfo;
            String xml;

            this.Api.DVDByProfileID(out dvdInfo, id, -1, -1);
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
                    = (new FileInfo(Environment.ExpandEnvironmentVariables(this.WinMergeTextBox.Text))).DirectoryName;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.WinMergeTextBox.Text = ofd.FileName;
                    this.Settings.DefaultValues.WinMergePath = this.WinMergeTextBox.Text;
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
                using (DetailsForm detailsForm = new DetailsForm(kvp.Value[0], kvp.Value[1], this.Settings))
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
                if (this.SkipVersionCheck == false)
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
            ExportFlagSet(DuplicateListView, "DifferentProfiles");
        }

        private void OnExportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItemClick(Object sender, EventArgs e)
        {
            ExportFlagSet(OnlyLeftiesListView, "LeftOnlyProfiles");
        }

        private void OnExportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItemClick(Object sender, EventArgs e)
        {
            ExportFlagSet(OnlyRightiesListView, "RightOnlyProfiles");
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