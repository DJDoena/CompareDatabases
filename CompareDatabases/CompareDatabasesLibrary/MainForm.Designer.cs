namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LeftFileButton = new System.Windows.Forms.Button();
            this.RightFileButton = new System.Windows.Forms.Button();
            this.LeftFileTextBox = new System.Windows.Forms.TextBox();
            this.RightFileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WinMergeTextBox = new System.Windows.Forms.TextBox();
            this.WinMergeButton = new System.Windows.Forms.Button();
            this.WinMergeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.CompareDatabasesButton = new System.Windows.Forms.Button();
            this.DuplicateListView = new System.Windows.Forms.ListView();
            this.SortTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UPC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Locality = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDifferentProfilesFlagSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.DifferentProfilesTab = new System.Windows.Forms.TabPage();
            this.LeftDatabaseTab = new System.Windows.Forms.TabPage();
            this.OnlyLeftiesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RightDatabaseTab = new System.Windows.Forms.TabPage();
            this.OnlyRightiesListView = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MainMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.DifferentProfilesTab.SuspendLayout();
            this.LeftDatabaseTab.SuspendLayout();
            this.RightDatabaseTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftFileButton
            // 
            resources.ApplyResources(this.LeftFileButton, "LeftFileButton");
            this.LeftFileButton.Name = "LeftFileButton";
            this.LeftFileButton.UseVisualStyleBackColor = true;
            this.LeftFileButton.Click += new System.EventHandler(this.OnLeftFileButtonClick);
            // 
            // RightFileButton
            // 
            resources.ApplyResources(this.RightFileButton, "RightFileButton");
            this.RightFileButton.Name = "RightFileButton";
            this.RightFileButton.UseVisualStyleBackColor = true;
            this.RightFileButton.Click += new System.EventHandler(this.OnRightFileButtonClick);
            // 
            // LeftFileTextBox
            // 
            resources.ApplyResources(this.LeftFileTextBox, "LeftFileTextBox");
            this.LeftFileTextBox.Name = "LeftFileTextBox";
            this.LeftFileTextBox.ReadOnly = true;
            // 
            // RightFileTextBox
            // 
            resources.ApplyResources(this.RightFileTextBox, "RightFileTextBox");
            this.RightFileTextBox.Name = "RightFileTextBox";
            this.RightFileTextBox.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // WinMergeTextBox
            // 
            resources.ApplyResources(this.WinMergeTextBox, "WinMergeTextBox");
            this.WinMergeTextBox.Name = "WinMergeTextBox";
            this.WinMergeTextBox.ReadOnly = true;
            // 
            // WinMergeButton
            // 
            resources.ApplyResources(this.WinMergeButton, "WinMergeButton");
            this.WinMergeButton.Name = "WinMergeButton";
            this.WinMergeButton.UseVisualStyleBackColor = true;
            this.WinMergeButton.Click += new System.EventHandler(this.OnWinMergeButtonClick);
            // 
            // WinMergeLinkLabel
            // 
            resources.ApplyResources(this.WinMergeLinkLabel, "WinMergeLinkLabel");
            this.WinMergeLinkLabel.Name = "WinMergeLinkLabel";
            this.WinMergeLinkLabel.TabStop = true;
            this.WinMergeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnWinMergeLinkLabelLinkClicked);
            // 
            // CompareDatabasesButton
            // 
            resources.ApplyResources(this.CompareDatabasesButton, "CompareDatabasesButton");
            this.CompareDatabasesButton.Name = "CompareDatabasesButton";
            this.CompareDatabasesButton.UseVisualStyleBackColor = true;
            this.CompareDatabasesButton.Click += new System.EventHandler(this.OnCompareDatabasesButtonClick);
            // 
            // DuplicateListView
            // 
            resources.ApplyResources(this.DuplicateListView, "DuplicateListView");
            this.DuplicateListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SortTitle,
            this.Title,
            this.UPC,
            this.Locality});
            this.DuplicateListView.FullRowSelect = true;
            this.DuplicateListView.MultiSelect = false;
            this.DuplicateListView.Name = "DuplicateListView";
            this.DuplicateListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.DuplicateListView.UseCompatibleStateImageBehavior = false;
            this.DuplicateListView.View = System.Windows.Forms.View.Details;
            this.DuplicateListView.DoubleClick += new System.EventHandler(this.OnListViewDoubleClick);
            // 
            // SortTitle
            // 
            resources.ApplyResources(this.SortTitle, "SortTitle");
            // 
            // Title
            // 
            resources.ApplyResources(this.Title, "Title");
            // 
            // UPC
            // 
            resources.ApplyResources(this.UPC, "UPC");
            // 
            // Locality
            // 
            resources.ApplyResources(this.Locality, "Locality");
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // MainMenu
            // 
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainMenu.Name = "MainMenu";
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportDifferentProfilesFlagSetToolStripMenuItem,
            this.exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem,
            this.exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // exportDifferentProfilesFlagSetToolStripMenuItem
            // 
            resources.ApplyResources(this.exportDifferentProfilesFlagSetToolStripMenuItem, "exportDifferentProfilesFlagSetToolStripMenuItem");
            this.exportDifferentProfilesFlagSetToolStripMenuItem.Name = "exportDifferentProfilesFlagSetToolStripMenuItem";
            this.exportDifferentProfilesFlagSetToolStripMenuItem.Click += new System.EventHandler(this.OnExportDifferentProfilesFlagSetToolStripMenuItemClick);
            // 
            // exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem
            // 
            resources.ApplyResources(this.exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem, "exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem");
            this.exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem.Name = "exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem";
            this.exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem.Click += new System.EventHandler(this.OnExportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItemClick);
            // 
            // exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem
            // 
            resources.ApplyResources(this.exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem, "exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem");
            this.exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem.Name = "exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem";
            this.exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem.Click += new System.EventHandler(this.OnExportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readMeToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // readMeToolStripMenuItem
            // 
            resources.ApplyResources(this.readMeToolStripMenuItem, "readMeToolStripMenuItem");
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Click += new System.EventHandler(this.OnReadMeToolStripMenuItemClick);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            resources.ApplyResources(this.checkForUpdatesToolStripMenuItem, "checkForUpdatesToolStripMenuItem");
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.OnCheckForUpdatesToolStripMenuItemClick);
            // 
            // aboutToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutToolStripMenuItemClick);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.DifferentProfilesTab);
            this.tabControl1.Controls.Add(this.LeftDatabaseTab);
            this.tabControl1.Controls.Add(this.RightDatabaseTab);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // DifferentProfilesTab
            // 
            resources.ApplyResources(this.DifferentProfilesTab, "DifferentProfilesTab");
            this.DifferentProfilesTab.Controls.Add(this.DuplicateListView);
            this.DifferentProfilesTab.Name = "DifferentProfilesTab";
            this.DifferentProfilesTab.UseVisualStyleBackColor = true;
            // 
            // LeftDatabaseTab
            // 
            resources.ApplyResources(this.LeftDatabaseTab, "LeftDatabaseTab");
            this.LeftDatabaseTab.Controls.Add(this.OnlyLeftiesListView);
            this.LeftDatabaseTab.Name = "LeftDatabaseTab";
            this.LeftDatabaseTab.UseVisualStyleBackColor = true;
            // 
            // OnlyLeftiesListView
            // 
            resources.ApplyResources(this.OnlyLeftiesListView, "OnlyLeftiesListView");
            this.OnlyLeftiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.OnlyLeftiesListView.FullRowSelect = true;
            this.OnlyLeftiesListView.MultiSelect = false;
            this.OnlyLeftiesListView.Name = "OnlyLeftiesListView";
            this.OnlyLeftiesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.OnlyLeftiesListView.UseCompatibleStateImageBehavior = false;
            this.OnlyLeftiesListView.View = System.Windows.Forms.View.Details;
            this.OnlyLeftiesListView.DoubleClick += new System.EventHandler(this.OnListViewDoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // RightDatabaseTab
            // 
            resources.ApplyResources(this.RightDatabaseTab, "RightDatabaseTab");
            this.RightDatabaseTab.Controls.Add(this.OnlyRightiesListView);
            this.RightDatabaseTab.Name = "RightDatabaseTab";
            this.RightDatabaseTab.UseVisualStyleBackColor = true;
            // 
            // OnlyRightiesListView
            // 
            resources.ApplyResources(this.OnlyRightiesListView, "OnlyRightiesListView");
            this.OnlyRightiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.OnlyRightiesListView.FullRowSelect = true;
            this.OnlyRightiesListView.MultiSelect = false;
            this.OnlyRightiesListView.Name = "OnlyRightiesListView";
            this.OnlyRightiesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.OnlyRightiesListView.UseCompatibleStateImageBehavior = false;
            this.OnlyRightiesListView.View = System.Windows.Forms.View.Details;
            this.OnlyRightiesListView.DoubleClick += new System.EventHandler(this.OnListViewDoubleClick);
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CompareDatabasesButton);
            this.Controls.Add(this.WinMergeLinkLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WinMergeTextBox);
            this.Controls.Add(this.WinMergeButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RightFileTextBox);
            this.Controls.Add(this.LeftFileTextBox);
            this.Controls.Add(this.RightFileButton);
            this.Controls.Add(this.LeftFileButton);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMainFormClosing);
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.DifferentProfilesTab.ResumeLayout(false);
            this.LeftDatabaseTab.ResumeLayout(false);
            this.RightDatabaseTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LeftFileButton;
        private System.Windows.Forms.Button RightFileButton;
        private System.Windows.Forms.TextBox LeftFileTextBox;
        private System.Windows.Forms.TextBox RightFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox WinMergeTextBox;
        private System.Windows.Forms.Button WinMergeButton;
        private System.Windows.Forms.LinkLabel WinMergeLinkLabel;
        private System.Windows.Forms.Button CompareDatabasesButton;
        private System.Windows.Forms.ListView DuplicateListView;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader UPC;
        private System.Windows.Forms.ColumnHeader Locality;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader SortTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage DifferentProfilesTab;
        private System.Windows.Forms.TabPage LeftDatabaseTab;
        private System.Windows.Forms.TabPage RightDatabaseTab;
        private System.Windows.Forms.ListView OnlyLeftiesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView OnlyRightiesListView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDifferentProfilesFlagSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportProfilesOnlyInLeftDatabaseFlagSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportProfilesOnlyInRightDatabaseFlagSetToolStripMenuItem;
    }
}