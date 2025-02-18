using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using DateTimePickerWithBackColor;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400;
using DoenaSoft.ToolBox.Generics;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    //[System.Runtime.InteropServices.ComVisible(false)]
    public partial class DetailsForm : Form
    {
        private readonly DVD m_LeftDvd;

        private readonly DVD m_RightDvd;

        private static XmlSerializer s_MyLinksSerializer;

        private static XmlSerializer s_BoxSetSerializer;

        private readonly Settings m_Settings;

        private Settings Settings
        {
            [DebuggerStepThrough()]
            get
            {
                return (m_Settings);
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

            m_Settings = settings;
            this.InitializeComponent();
            ci = CultureInfo.CurrentCulture;
            format = ci.DateTimeFormat.ShortDatePattern + "\t" + ci.DateTimeFormat.LongTimePattern;
            LeftProfileTimestampPicker.CustomFormat = format;
            LeftLastEditedPicker.CustomFormat = format;
            RightProfileTimestampPicker.CustomFormat = format;
            RightLastEditedPicker.CustomFormat = format;
            m_LeftDvd = leftDvd;
            m_RightDvd = rightDvd;
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

            LeftUpcTextBox.Text = leftDvd.UPC;
            RightUpcTextBox.Text = rightDvd.UPC;

            LeftTitleTextBox.Text = leftDvd.Title;
            RightTitleTextBox.Text = rightDvd.Title;
            ColourizeTextBox(leftIsNewer, LeftTitleTextBox, RightTitleTextBox);

            LeftEditionTextBox.Text = leftDvd.Edition;
            RightEditionTextBox.Text = rightDvd.Edition;
            ColourizeTextBox(leftIsNewer, LeftEditionTextBox, RightEditionTextBox);

            LeftOriginalTitleTextBox.Text = leftDvd.OriginalTitle;
            RightOriginalTitleTextBox.Text = rightDvd.OriginalTitle;
            ColourizeTextBox(leftIsNewer, LeftOriginalTitleTextBox, RightOriginalTitleTextBox);

            LeftLocalityTextBox.Text = leftDvd.ID_LocalityDesc;
            RightLocalityTextBox.Text = rightDvd.ID_LocalityDesc;

            LeftProfileTimestampPicker.Value = leftDvd.ProfileTimestamp;
            RightProfileTimestampPicker.Value = rightDvd.ProfileTimestamp;
            ColourizePicker(leftIsNewer, LeftProfileTimestampPicker, RightProfileTimestampPicker);

            if (leftDvd.LastEditedSpecified)
            {
                LeftLastEditedPicker.Value = leftDvd.LastEdited;
            }
            else
            {
                LeftLastEditedPicker.Value = LeftLastEditedPicker.MinDate;
            }
            if (rightDvd.LastEditedSpecified)
            {
                RightLastEditedPicker.Value = rightDvd.LastEdited;
            }
            else
            {
                RightLastEditedPicker.Value = LeftLastEditedPicker.MinDate;
            }
            ColourizePicker(leftIsNewer, LeftLastEditedPicker, RightLastEditedPicker);

            if ((leftDvd.PurchaseInfo != null) && (leftDvd.PurchaseInfo.DateSpecified))
            {
                LeftPurchaseDatePicker.Value = leftDvd.PurchaseInfo.Date;
            }
            else
            {
                LeftPurchaseDatePicker.Value = LeftLastEditedPicker.MinDate;
            }
            if ((rightDvd.PurchaseInfo != null) && (rightDvd.PurchaseInfo.DateSpecified))
            {
                RightPurchaseDatePicker.Value = rightDvd.PurchaseInfo.Date;
            }
            else
            {
                RightPurchaseDatePicker.Value = LeftLastEditedPicker.MinDate;
            }
            ColourizePicker(leftIsNewer, LeftPurchaseDatePicker, RightPurchaseDatePicker);

            if (leftDvd.PurchaseInfo != null)
            {
                LeftPurchasePlaceTextBox.Text = leftDvd.PurchaseInfo.Place;
            }
            if (rightDvd.PurchaseInfo != null)
            {
                RightPurchasePlaceTextBox.Text = rightDvd.PurchaseInfo.Place;
            }
            ColourizeTextBox(leftIsNewer, LeftPurchasePlaceTextBox, RightPurchasePlaceTextBox);

            if ((leftDvd.PurchaseInfo != null) && (leftDvd.PurchaseInfo.Price != null))
            {
                LeftPurchasePriceTextBox.Text = leftDvd.PurchaseInfo.Price.FormattedValue;
            }
            if ((rightDvd.PurchaseInfo != null) && (rightDvd.PurchaseInfo.Price != null))
            {
                RightPurchasePriceTextBox.Text = rightDvd.PurchaseInfo.Price.FormattedValue;
            }
            ColourizeTextBox(leftIsNewer, LeftPurchasePriceTextBox, RightPurchasePriceTextBox);

            FillCastTextBox(leftIsNewer, leftDvd.CastList, rightDvd.CastList, LeftCastTextBox, RightCastTextBox);

            FillCrewTextBox(leftIsNewer, leftDvd.CrewList, rightDvd.CrewList, LeftCrewTextBox, RightCrewTextBox);

            this.FillEventsTextBox(leftIsNewer, leftDvd, rightDvd, LeftEventsTextBox, RightEventsTextBox);

            this.FillTagsTextBox(leftIsNewer, leftDvd, rightDvd, LeftTagsTextBox, RightTagsTextBox);

            this.FillMyLinksTextBox(leftIsNewer, leftDvd, rightDvd, LeftMyLinksTextBox, RightMyLinksTextBox);

            this.FillBoxSetTextBox(leftIsNewer, leftDvd, rightDvd, LeftBoxSetTextBox, RightBoxSetTextBox);
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
            this.WriteBoxSetEntry(leftBoxSetParentCount, leftBoxSetContentsLength, leftTextBox);
            this.WriteBoxSetEntry(rightBoxSetParentCount, rightBoxSetContentsLength, rightTextBox);
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
                Tags tags = new Tags();

                tags.TagList = leftDvd.TagList;

                String leftTags = XmlSerializer<Tags>.ToString(tags);

                tags.TagList = rightDvd.TagList;

                String rightTags = XmlSerializer<Tags>.ToString(tags);

                CheckListSpecific(leftIsNewer, leftTags, rightTags, leftTextBox, rightTextBox);
            }
        }

        private void FillEventsTextBox(Boolean leftIsNewer, DVD leftDvd, DVD rightDvd, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftDvd.EventList, rightDvd.EventList, leftTextBox, rightTextBox))
            {
                Events events = new Events();

                events.EventList = leftDvd.EventList;

                String leftEvents = XmlSerializer<Events>.ToString(events);

                events.EventList = rightDvd.EventList;

                String rightEvents = XmlSerializer<Events>.ToString(events);

                CheckListSpecific(leftIsNewer, leftEvents, rightEvents, leftTextBox, rightTextBox);
            }
        }

        private static void FillCrewTextBox(Boolean leftIsNewer, Object[] leftList, Object[] rightList, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftList, rightList, leftTextBox, rightTextBox))
            {
                CrewInformation ci = new CrewInformation();

                ci.CrewList = leftList;

                String leftInfo = XmlSerializer<CrewInformation>.ToString(ci, CrewInformation.DefaultEncoding);

                ci.CrewList = rightList;

                String rightInfo = XmlSerializer<CrewInformation>.ToString(ci, CrewInformation.DefaultEncoding);

                CheckListSpecific(leftIsNewer, leftInfo, rightInfo, leftTextBox, rightTextBox);
            }
        }

        private static void FillCastTextBox(Boolean leftIsNewer, Object[] leftList, Object[] rightList, TextBox leftTextBox, TextBox rightTextBox)
        {
            if (CheckListInGeneral(leftIsNewer, leftList, rightList, leftTextBox, rightTextBox))
            {
                CastInformation ci = new CastInformation();

                ci.CastList = leftList;

                String leftInfo = XmlSerializer<CastInformation>.ToString(ci, CastInformation.DefaultEncoding);

                ci.CastList = rightList;

                String rightInfo = XmlSerializer<CastInformation>.ToString(ci, CastInformation.DefaultEncoding);

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

            if (this.CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;

                leftFile = Path.GetTempFileName();
                m_LeftDvd.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                m_RightDvd.Serialize(rightFile);
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
            WinMergeButton.Focus();
        }

        private void OnCloseButtonClick(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCrewMembersClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (this.CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                CrewInformation ci;

                ci = new CrewInformation();
                leftFile = Path.GetTempFileName();
                ci.Title = m_LeftDvd.Title;
                ci.CrewList = m_LeftDvd.CrewList;
                ci.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                ci.Title = m_RightDvd.Title;
                ci.CrewList = m_RightDvd.CrewList;
                ci.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnEventsClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (this.CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                Events events;

                events = new Events();
                leftFile = Path.GetTempFileName();
                events.EventList = m_LeftDvd.EventList;
                XmlSerializer<Events>.Serialize(leftFile, events);
                rightFile = Path.GetTempFileName();
                events.EventList = m_RightDvd.EventList;
                XmlSerializer<Events>.Serialize(rightFile, events);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnCastMembersClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (this.CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                CastInformation ci;

                ci = new CastInformation();
                leftFile = Path.GetTempFileName();
                ci.Title = m_LeftDvd.Title;
                ci.CastList = m_LeftDvd.CastList;
                ci.Serialize(leftFile);
                rightFile = Path.GetTempFileName();
                ci.Title = m_RightDvd.Title;
                ci.CastList = m_RightDvd.CastList;
                ci.Serialize(rightFile);
                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnLeftTagsClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (this.CheckWinMergeExistence(out fullPath))
            {
                Tags tags = new Tags();

                String leftFile = Path.GetTempFileName();

                tags.TagList = m_LeftDvd.TagList;

                XmlSerializer<Tags>.Serialize(leftFile, tags);

                String rightFile = Path.GetTempFileName();

                tags.TagList = m_RightDvd.TagList;

                XmlSerializer<Tags>.Serialize(rightFile, tags);

                StartWinMerge(fullPath, leftFile, rightFile);
            }
        }

        private void OnLeftMyLinksClick(Object sender, EventArgs e)
        {
            String fullPath;

            if (this.CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                MyLinks myLinks;

                leftFile = Path.GetTempFileName();
                myLinks = m_LeftDvd.MyLinks;
                SerializeMyLinks(leftFile, myLinks);
                rightFile = Path.GetTempFileName();
                myLinks = m_RightDvd.MyLinks;
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

            if (this.CheckWinMergeExistence(out fullPath))
            {
                String leftFile;
                String rightFile;
                BoxSet boxSet;

                leftFile = Path.GetTempFileName();
                boxSet = m_LeftDvd.BoxSet;
                SerializeBoxSet(leftFile, boxSet);
                rightFile = Path.GetTempFileName();
                boxSet = m_RightDvd.BoxSet;
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