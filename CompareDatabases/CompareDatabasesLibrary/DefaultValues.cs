using System;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    //[System.Runtime.InteropServices.ComVisible(false)]
    [Serializable]
    public class DefaultValues
    {
        private String m_WinMergePath;

        public String WinMergePath
        {
            get
            {
                if (String.IsNullOrEmpty(m_WinMergePath))
                {
                    String path = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

                    if (String.IsNullOrEmpty(path))
                    {
                        return (@"%ProgramFiles%\WinMerge\WinMergeU.exe");
                    }
                    else
                    {
                        return (@"%ProgramFiles(x86)%\WinMerge\WinMergeU.exe");
                    }
                }

                return (m_WinMergePath);
            }
            set
            {
                m_WinMergePath = value;
            }
        }
    }
}