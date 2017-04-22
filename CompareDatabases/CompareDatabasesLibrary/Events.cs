using DoenaSoft.DVDProfiler.DVDProfilerXML.Version390;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    [Serializable()]
    public class Events
    {
        public Event[] EventList;

        private static XmlSerializer s_XmlSerializer;

        [XmlIgnore()]
        public static XmlSerializer XmlSerializer
        {
            get
            {
                if (s_XmlSerializer == null)
                {
                    s_XmlSerializer = new XmlSerializer(typeof(Events));
                }
                return (s_XmlSerializer);
            }
        }

        public static void Serialize(String fileName, Events instance)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlTextWriter xtw = new XmlTextWriter(fs, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.Indented;
                    XmlSerializer.Serialize(xtw, instance);
                }
            }
        }

        public void Serialize(String fileName)
        {
            Serialize(fileName, this);
        }

        public static Events Deserialize(String fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (XmlTextReader xtr = new XmlTextReader(fs))
                {
                    Events instance;

                    instance = (Events)(XmlSerializer.Deserialize(xtr));
                    return (instance);
                }
            }
        }
    }
}