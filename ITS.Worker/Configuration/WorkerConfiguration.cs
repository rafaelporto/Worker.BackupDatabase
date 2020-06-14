using System;
using System.IO;

namespace ITS.Worker.Configuration
{
    public class WorkerConfiguration
    {
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int Interval { get; set; }

        public string GetSourceFileName()
        {
            return Path.ChangeExtension(FileName, FileExtension);
        }

        public string GetTargetFileName()
        {
            var fileName = string.Concat(FileName, "_BKP_", DateTimeOffset.Now.ToString("dd_MM_yyyy"));
            fileName = Path.ChangeExtension(fileName, FileExtension);

            return fileName;
        }
    }
}
