using System;

namespace DoenaSoft.DVDProfiler.CompareDatabases
{
    [Serializable()]
    public class DefaultValues
    {
        private String m_WinMergePath;

        public String WinMergePath
        {
            get
            {
                if (String.IsNullOrEmpty(this.m_WinMergePath))
                {
                    String path;

                    path = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                    if (String.IsNullOrEmpty(path))
                    {
                        return (@"%ProgramFiles%\WinMerge\WinMergeU.exe");
                    }
                    else
                    {
                        return (@"%ProgramFiles(x86)%\WinMerge\WinMergeU.exe");
                    }
                }
                return (this.m_WinMergePath);
            }
            set
            {
                this.m_WinMergePath = value;
            }
        }
    }
}