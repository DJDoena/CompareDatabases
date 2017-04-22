using DateTimePickerWithBackColor;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version390;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    public partial class DetailsForm : Form
    {
        private DVD m_LeftDvd;

        private DVD m_RightDvd;

        private static XmlSerializer s_MyLinksSerializer;

        private static XmlSerializer s_BoxSetSerializer;

        private Settings m_Settings;

        private Settings Settings
        {
            [DebuggerStepThrough()]
            get
            {
                return (this.m_Settings);
            }
        }

        private static XmlSerializer MyLinksSerializer
        {
            get
            {
                if (s_MyLinksSerializer == null)
                {
                    s_MyLinksSerializer = new XmlSerializer(typeof(MyLinks));
                }
                return (s_MyLinksSerializer);
            }
        }

        private static XmlSerializer BoxSetSerializer
        {
            get
            {
                if (s_BoxSetSerializer == null)
                {
                    s_BoxSetSerializer = new XmlSerializer(typeof(BoxSet));
                }
                return (s_BoxSetSerializer);
            }
        }

        public DetailsForm(DVD leftDvd, DVD rightDvd, Settings settings)
        {
            CultureInfo ci;
            String format;
            Boolean leftIsNewer;
            DateTime newestLeftTimeStamp;

            this.m_Settings = settings;
            InitializeComponent();
            ci = CultureInfo.CurrentCulture;
            format = ci.DateTimeFormat.ShortDatePattern + "\t" + ci.DateTimeFormat.LongTimePattern;
            this.LeftProfileTimestampPicker.CustomFormat = format;
            this.LeftLastEditedPicker.CustomFormat = format;
            this.RightProfileTimestampPicker.CustomFormat = format;
            this.RightLastEditedPicker.CustomFormat = format;
            this.m_LeftDvd = leftDvd;
            this.m_RightDvd = rightDvd;
            newestLeftTimeStamp = leftDvd.ProfileTimestamp;
            if (leftDvd.LastEdited > newestLeftTimeStamp)
            {
                newestLeftTimeStamp = leftDvd.LastEdited;
            }
            leftIsNewer = true;
            if ((rightDvd.ProfileTimestamp > newestLeftTimeStamp)
                || (rightDvd.LastEdited > newestLeftTimeStamp))
            {
                leftIsNewer = false;
            }

            this.LeftUpcTextBox.Text = leftDvd.UPC;
            this.RightUpcTextBox.Text = rightDvd.UPC;

            this.LeftTitleTextBox.Text = leftDvd.Title;
            this.RightTitleTextBox.Text = rightDvd.Title;
            ColourizeTextBox(leftIsNewer, this.LeftTitleTextBox, this.RightTitleTextBox);

            this.LeftEditionTextBox.Text = leftDvd.Edition;
            this.RightEditionTextBox.Text = rightDvd.Edition;
            ColourizeTextBox(leftIsNewer, this.LeftEditionTextBox, this.RightEditionTextBox);

            this.LeftOriginalTitleTextBox.Text = leftDvd.OriginalTitle;
            this.RightOriginalTitleTextBox.Text = rightDvd.OriginalTitle;
            ColourizeTextBox(leftIsNewer, this.LeftOriginalTitleTextBox, this.RightOriginalTitleTextBox);

            this.LeftLocalityTextBox.Text = leftDvd.ID_LocalityDesc;
            this.RightLocalityTextBox.Text = rightDvd.ID_LocalityDesc;

            this.LeftProfileTimestampPicker.Value = leftDvd.ProfileTimestamp;
            this.RightProfileTimestampPicker.Value = rightDvd.ProfileTimestamp;
            ColourizePicker(leftIsNewer, this.LeftProfileTimestampPicker, this.RightProfileTimestampPicker);

            if (leftDvd.LastEditedSpecified)
            {
                this.LeftLastEditedPicker.Value = leftDvd.LastEdited;
            }
            else
            {
                this.LeftLastEditedPicker.Value = this.LeftLastEditedPicker.MinDate;
            }
            if (rightDvd.LastEditedSpecified)
            {
                this.RightLastEditedPicker.Value = rightDvd.LastEdited;
            }
            else
            {
                this.RightLastEditedPicker.Value = this.LeftLastEditedPicker.MinDate;
            }
            ColourizePicker(leftIsNewer, this.LeftLastEditedPicker, this.RightLastEditedPicker);

            if ((leftDvd.PurchaseInfo != null) && (leftDvd.PurchaseInfo.DateSpecified))
            {
                this.LeftPurchaseDatePicker.Value = leftDvd.PurchaseInfo.Date;
            }
            else
            {
                this.LeftPurchaseDatePicker.Value = this.LeftLastEditedPicker.MinDate;
            }
            if ((rightDvd.PurchaseInfo != null) && (rightDvd.PurchaseInfo.DateSpecified))
            {
                this.RightPurchaseDatePicker.Value = rightDvd.PurchaseInfo.Date;
            }
            else
            {
                this.RightPurchaseDatePicker.Value = this.LeftLastEditedPicker.MinDate;
            }
            ColourizePicker(leftIsNewer, this.LeftPurchaseDatePicker, this.RightPurchaseDatePicker);

            if (leftDvd.PurchaseInfo != null)
            {
                this.LeftPurchasePlaceTextBox.Text = leftDvd.PurchaseInfo.Place;
            }
            if (rightDvd.PurchaseInfo != null)
            {
                this.RightPurchasePlaceTextBox.Text = rightDvd.PurchaseInfo.Place;
            }
            ColourizeTextBox(leftIsNewer, this.LeftPurchasePlaceTextBox, this.RightPurchasePlaceTextBox);

            if ((leftDvd.PurchaseInfo != null) && (leftDvd.PurchaseInfo.Price != null))
            {
                this.LeftPurchasePriceTextBox.Text = leftDvd.PurchaseInfo.Price.FormattedValue;
            }
            if ((rightDvd.PurchaseInfo != null) && (rightDvd.PurchaseInfo.Price != null))
            {
                this.RightPurchasePriceTextBox.Text = rightDvd.PurchaseInfo.Price.FormattedValue;
            }
            ColourizeTextBox(leftIsNewer, this.LeftPurchasePriceTextBox, this.RightPurchasePriceTextBox);

            FillCastTextBox(leftIsNewer, leftDvd.CastList, rightDvd.CastList, this.LeftCastTextBox, this.RightCastTextBox);

            FillCrewTextBox(leftIsNewer, leftDvd.CrewList, rightDvd.CrewList, this.LeftCrewTextBox, this.RightCrewTextBox);

            FillEventsTextBox(leftIsNewer, leftDvd, rightDvd, this.LeftEventsTextBox, this.RightEventsTextBox);

            FillTagsTextBox(leftIsNewer, leftDvd, rightDvd, this.LeftTagsTextBox, this.RightTagsTextBox);

            FillMyLinksTextBox(leftIsNewer, leftDvd, rightDvd, this.LeftMyLinksTextBox, this.RightMyLinksTextBox);

            FillBoxSetTextBox(leftIsNewer, leftDvd, rightDvd, this.LeftBoxSetTextBox, this.RightBoxSetTextBox);
        }

        private void FillBoxSetTextBox(Boolean leftIsNewer, DVD leftDvd, DVD rightDvd, TextBox leftTextBox, TextBox rightTextBox)
        {
            String leftBoxSetParent;
            String rightBoxSetParent;
            Int32 leftBoxSetParentCount;
            Int32 rightBoxSetParentCount;
            String[] leftBoxSetContents;
            String[] rightBoxSetContents;
            Int32 leftBoxSetContentsLength;
            Int32 rightBoxSetContentsLength;

            leftBoxSetParent = null;
            leftBoxSetParentCount = 0;
            leftBoxSetContents = null;
            leftBoxSetContentsLength = 0;
            if (leftDvd.BoxSet != null)
            {
                if (String.IsNullOrEmpty(leftDvd.BoxSet.Parent) == false)
                {
                    leftBoxSetParent = leftDvd.BoxSet.Parent;
                    leftBoxSetParentCount = 1;
                }
                leftBoxSetContents = leftDvd.BoxSet.ContentList;
                if (leftBoxSetContents != null)
                {
                    leftBoxSetContentsLength = leftBoxSetContents.Length;
                }
            }
            rightBoxSetParent = null;
            rightBoxSetParentCount = 0;
            rightBoxSetContents = null;
            rightBoxSetContentsLength = 0;
            if (rightDvd.BoxSet != null)
            {
                if (String.IsNullOrEmpty(rightDvd.BoxSet.Parent) == false)
                {
                    rightBoxSetParent = rightDvd.BoxSet.Parent;
                    rightBoxSetParentCount = 1;
                }
                rightBoxSetContents = rightDvd.BoxSet.ContentList;

                if (rightBoxSetContents != null)
                {
                    rightBoxSetContentsLength = rightBoxSetContents.Length;
                }
            }
            WriteBoxSetEntry(leftBoxSetParentCount, leftBoxSetContentsLength, leftTextBox);
            WriteBoxSetEntry(rightBoxSetParentCount, rightBoxSetContentsLength, rightTextBox);
            ColourizeTextBox(leftIsNewer, leftTextBox, rightTextBox);
            if ((leftBoxSetParentCount == rightBoxSetParentCount) && (leftBoxSetContentsLength == rightBoxSetContentsLength))
            {
                BoxSet boxSet;
                String leftBoxSet;
                String rightBoxSet;

                boxSet = leftDvd.BoxSet;
                using (StringWriter sw = new StringWriter())
                {
                    BoxSetSerializer.Serialize(sw, boxSet);
                    leftBoxSet = sw.ToString();
                }
                boxSet = rightDvd.BoxSet;
                using (StringWriter sw = new StringWriter())
                {
                    BoxSetSerializer.Serialize(sw, boxSet);
                    rightBoxSet = sw.ToString();
                }
                CheckListSpecific(leftIsNewer, leftBoxSet, rightBoxSet, leftTextBox, rightTextBox);
            }
        }

        private void WriteBoxSetEntry(Int32 boxSetParentCount, Int32 boxSetContentsLength, TextBox textBox)
        {
            textBox.Text = boxSetParentCount.ToString();
            if (boxSetParentCount == 1)
            {
                textBox.Text += Texts.Parent;
            }
            else
            {
                textBox.Text += Texts.Parents;
            }
            textBox.Text += ", ";
            textBox.Text += boxSetContentsLength.ToString();
            if (boxSetContentsLength == 1)
            {
                textBox.Text += Texts.Child;
            }
            else
            {
                textBox.Text += Texts.Children;
            }
        }

        private void FillMyLinksTextBox(Boolean leftIsNewer, DVD leftDvd, DVD rightDvd, TextBox leftTextBox, TextBox rightTextBox)
        {
            UserLink[] leftMyLinkList;
            UserLink[] rightMyLinkList;

            leftMyLinkList = null;
            if (leftDvd.MyLinks != null)
            {
                leftMyLinkList = leftDvd.MyLinks.UserLinkList;
            }
            rightMyLinkList = null;
            if (rightDvd.MyLinks != null)
            {
                rightMyLinkList = rightDvd.MyLinks.UserLinkList;
            }
            if (CheckListInGeneral(leftIsNewer, leftMyLinkList, rightMyLinkList, leftTextBox, rightTextBox))
            {
                MyLinks myLinks;
                String leftMyLinks;
                String rightMyLinks;

                myLinks = leftDvd.MyLinks;
                using (StringWriter sw = new StringWriter())
                {
                    MyLinksSerializer.Serialize(sw, myLinks);
                    leftMyLinks = sw.ToString();
                }
                myLinks = rightDvd.MyLinks;
                using (StringWriter sw = new StringWriter())
                {
                    MyLinksSerializer.Serialize(sw, myLinks);
                    rightMyLinks = sw.ToString();
                }
                CheckListSpecific(leftIsNewer, leftMyLinks, rightMyLinks, leftTextBox, rightTextBox);
            }
        }

        private void FillTagsTextBox(Boolean leftIsNewer, DVD leftDvd, DVD rightDvd, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftDvd.TagList, rightDvd.TagList, leftTextBox, rightTextBox))
            {
                Tags tags;
                String leftTags;
                String rightTags;

                tags = new Tags();
                tags.TagList = leftDvd.TagList;
                using (StringWriter sw = new StringWriter())
                {
                    Tags.XmlSerializer.Serialize(sw, tags);
                    leftTags = sw.ToString();
                }
                tags.TagList = rightDvd.TagList;
                using (StringWriter sw = new StringWriter())
                {
                    Tags.XmlSerializer.Serialize(sw, tags);
                    rightTags = sw.ToString();
                }
                CheckListSpecific(leftIsNewer, leftTags, rightTags, leftTextBox, rightTextBox);
            }
        }

        private void FillEventsTextBox(Boolean leftIsNewer, DVD leftDvd, DVD rightDvd, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftDvd.EventList, rightDvd.EventList, leftTextBox, rightTextBox))
            {
                Events events;
                String leftEvents;
                String rightEvents;

                events = new Events();
                events.EventList = leftDvd.EventList;
                using (StringWriter sw = new StringWriter())
                {
                    CompareDatabases.Events.XmlSerializer.Serialize(sw, events);
                    leftEvents = sw.ToString();
                }
                events.EventList = rightDvd.EventList;
                using (StringWriter sw = new StringWriter())
                {
                    CompareDatabases.Events.XmlSerializer.Serialize(sw, events);
                    rightEvents = sw.ToString();
                }
                CheckListSpecific(leftIsNewer, leftEvents, rightEvents, leftTextBox, rightTextBox);
            }
        }

        private static void FillCrewTextBox(Boolean leftIsNewer, Object[] leftList, Object[] rightList, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftList, rightList, leftTextBox, rightTextBox))
            {
                CrewInformation ci = new CrewInformation();

                ci.CrewList = leftList;

                String leftInfo = Serializer<CrewInformation>.ToString(ci, CrewInformation.DefaultEncoding);

                ci.CrewList = rightList;

                String rightInfo = Serializer<CrewInformation>.ToString(ci, CrewInformation.DefaultEncoding);

                CheckListSpecific(leftIsNewer, leftInfo, rightInfo, leftTextBox, rightTextBox);
            }
        }

        private static void FillCastTextBox(Boolean leftIsNewer, Object[] leftList, Object[] rightList, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftList, rightList, leftTextBox, rightTextBox))
            {
                CastInformation ci = new CastInformation();

                ci.CastList = leftList;

                String leftInfo = Serializer<CastInformation>.ToString(ci, CastInformation.DefaultEncoding);

                ci.CastList = rightList;

                String rightInfo = Serializer<CastInformation>.ToString(ci, CastInformation.DefaultEncoding);

                CheckListSpecific(leftIsNewer, leftInfo, rightInfo, leftTextBox, rightTextBox);
            }
        }

        private static void CheckListSpecific(Boolean leftIsNewer, String leftInfo, String rightInfo, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (leftInfo != rightInfo)
            {
                if (leftIsNewer)
                {
                    leftTextBox.Text += Texts.ButDifferent;
                }
                else
                {
                    rightTextBox.Text += Texts.ButDifferent;
                }
                ColourizeTextBox(leftIsNewer, leftTextBox, rightTextBox);
            }
        }

        private static Boolean CheckListInGeneral(Boolean leftIsNewer, Object[] leftList, Object[] rightList, TextBox leftTextBox, TextBox rightTextBox)
        {
            Int32 leftLength;
            Int32 rightLength;

            leftLength = 0;
            if (leftList != null)
            {
                leftLength = leftList.Length;
            }
            WriteEntry(leftLength, leftTextBox);
            rightLength = 0;
            if (rightList != null)
            {
                rightLength = rightList.Length;
            }
            WriteEntry(rightLength, rightTextBox);
            if (leftLength != rightLength)
            {
                ColourizeTextBox(leftIsNewer, leftTextBox, rightTextBox);
                return (false);
            }
            return (true);
        }

        private static void WriteEntry(Int32 length, TextBox textBox)
        {
            textBox.Text = length.ToString();
            if (length != 1)
            {
                textBox.Text += Texts.Entries;
            }
            else
            {
                textBox.Text += Texts.Entry;
            }
        }

        private static void ColourizeTextBox(Boolean leftIsNewer, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (leftTextBox.Text != rightTextBox.Text)
            {
                if (leftIsNewer)
                {
                    leftTextBox.BackColor = Color.LightGreen;
                    rightTextBox.BackColor = Color.Salmon;
                }
                else
                {
                    rightTextBox.BackColor = Color.LightGreen;
                    leftTextBox.BackColor = Color.Salmon;
                }
            }
        }

        private static void ColourizePicker(Boolean leftIsNewer, BCDateTimePicker leftPicker, BCDateTimePicker rightPicker)
        {
            if (leftPicker.Value != rightPicker.Value)
            {
                if (leftPicker.Value > rightPicker.Value)
                {
                    leftPicker.BackDisabledColor = Color.LightGreen;
                    rightPicker.BackDisabledColor = Color.Salmon;
                }
                else
                {
                    rightPicker.BackDisabledColor = Color.LightGreen;
                    leftPicker.BackDisabledColor = Color.Salmon;
                }
            }
        }

        private void OnCompareFormLoad(Object sender, EventArgs e)
        {
            this.LayoutForm();
        }

        private void LayoutForm()
        {
            this.Left = this.Settings.DetailsForm.Left;
            this.Top = this.Settings.DetailsForm.Top;
        }

        private void OnCompareFormFormClosing(Object sender, FormClosingEventArgs e)
        {
            this.Settings.DetailsForm.Left = this.Left;
            this.Settings.DetailsForm.Top = this.Top;
        }

        private void OnWinMergeButtonClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;

                leftFile = Path.GetTempFileName();
                this.m_LeftDvd.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                this.m_RightDvd.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private static void StartWinMerge(String fullPath, String leftFile, String rightFile)
        {
            Process process;
            StringBuilder parameters;

            process = new Process();
            parameters = new StringBuilder("/e /u /wl /wr ");
            parameters.Append("\"");
            parameters.Append(leftFile);
            parameters.Append("\" \"");
            parameters.Append(rightFile);
            parameters.Append("\"");
            process.StartInfo = new ProcessStartInfo(fullPath, parameters.ToString());
            process.Start();
        }

        private Boolean CheckWinMergeExistence(out String fullPath)
        {
            fullPath = Environment.ExpandEnvironmentVariables(this.Settings.DefaultValues.WinMergePath);
            if (File.Exists(fullPath) == false)
            {
                MessageBox.Show(this, MessageBoxTexts.WinMergeMissingError, MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (false);
            }
            return (true);
        }

        private void OnDetailsFormShown(Object sender, EventArgs e)
        {
            this.WinMergeButton.Focus();
        }

        private void OnCloseButtonClick(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCrewMembersClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                CrewInformation ci;

                ci = new CrewInformation();
                leftFile = Path.GetTempFileName();
                ci.Title = this.m_LeftDvd.Title;
                ci.CrewList = this.m_LeftDvd.CrewList;
                ci.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                ci.Title = this.m_RightDvd.Title;
                ci.CrewList = this.m_RightDvd.CrewList;
                ci.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnEventsClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                Events events;

                events = new Events();
                leftFile = Path.GetTempFileName();
                events.EventList = this.m_LeftDvd.EventList;
                events.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                events.EventList = this.m_RightDvd.EventList;
                events.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnCastMembersClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                CastInformation ci;

                ci = new CastInformation();
                leftFile = Path.GetTempFileName();
                ci.Title = this.m_LeftDvd.Title;
                ci.CastList = this.m_LeftDvd.CastList;
                ci.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                ci.Title = this.m_RightDvd.Title;
                ci.CastList = this.m_RightDvd.CastList;
                ci.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnLeftTagsClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                Tags tags;

                tags = new Tags();
                leftFile = Path.GetTempFileName();
                tags.TagList = this.m_LeftDvd.TagList;
                tags.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                tags.TagList = this.m_RightDvd.TagList;
                tags.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnLeftMyLinksClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                MyLinks myLinks;

                leftFile = Path.GetTempFileName();
                myLinks = this.m_LeftDvd.MyLinks;
                SerializeMyLinks(leftFile, myLinks);
                rightFile = Path.GetTempFileName();
                myLinks = this.m_RightDvd.MyLinks;
                SerializeMyLinks(rightFile, myLinks);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private static void SerializeMyLinks(String fileName, MyLinks instance)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlTextWriter xtw = new XmlTextWriter(fs, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.Indented;
                    MyLinksSerializer.Serialize(xtw, instance);
                }
            }
        }

        private void OnLeftBoxSetButtonClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                BoxSet boxSet;

                leftFile = Path.GetTempFileName();
                boxSet = this.m_LeftDvd.BoxSet;
                SerializeBoxSet(leftFile, boxSet);
                rightFile = Path.GetTempFileName();
                boxSet = this.m_RightDvd.BoxSet;
                SerializeBoxSet(rightFile, boxSet);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private static void SerializeBoxSet(String fileName, BoxSet instance)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlTextWriter xtw = new XmlTextWriter(fs, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.Indented;
                    BoxSetSerializer.Serialize(xtw, instance);
                }
            }
        }
    }
}