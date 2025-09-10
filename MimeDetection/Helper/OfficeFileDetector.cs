using System.IO.Compression;

namespace MimeDetection.Helper
{
    internal static class OfficeFileDetector
    {
        public static string DetectOfficeFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] signature = new byte[4];
                fs.Read(signature, 0, 4);
                if (signature[0] != 0x50 || signature[1] != 0x4B)
                    return MimeTypeMap.Mappings[".bin"]; // fallback generic

                fs.Seek(0, SeekOrigin.Begin);
                using (var archive = new ZipArchive(fs, ZipArchiveMode.Read, true))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.StartsWith("word/"))
                            return MimeTypeMap.Mappings[".docx"];
                        if (entry.FullName.StartsWith("xl/"))
                            return MimeTypeMap.Mappings[".xlsx"];
                        if (entry.FullName.StartsWith("ppt/"))
                            return MimeTypeMap.Mappings[".pptx"];
                    }
                }
            }

            return MimeTypeMap.Mappings[".zip"];
        }
    }
}
