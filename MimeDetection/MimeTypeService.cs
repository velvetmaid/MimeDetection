using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MimeDetection
{
    public class MimeTypeService
    {
        private static readonly IReadOnlyDictionary<string, string> _map = MimeTypeMap.Mappings;

        // Signature-based detection (unik dan ambigu)
        private static readonly IList<FileType> knownFileTypes = new List<FileType>
        {
            new FileType("Bitmap", ".bmp", new byte?[] { 0x42, 0x4D }),
            new FileType("PNG", ".png", new byte?[] { 0x89, 0x50, 0x4E, 0x47 }),
            new FileType("JPEG", ".jpg", new byte?[] { 0xFF, 0xD8, 0xFF }),
            new FileType("GIF", ".gif", new byte?[] { 0x47, 0x49, 0x46, 0x38 }),
            new FileType("ICO", ".ico", new byte?[] { 0x00, 0x00, 0x01, 0x00 }),
        };

        private static readonly IList<FileType> UnknownFileTypes = new List<FileType>
        {
            new FileType("Excel Open XML", ".xlsx", new byte?[] { 0x50, 0x4B, 0x03, 0x04 }),
            new FileType("Word Open XML", ".docx", new byte?[] { 0x50, 0x4B, 0x03, 0x04 }),
            new FileType("PowerPoint Open XML", ".pptx", new byte?[] { 0x50, 0x4B, 0x03, 0x04 }),
            new FileType("Excel Open XML", ".xlsx", new byte?[] { 0x50, 0x4B, 0x05, 0x06 }),
            new FileType("Word Open XML", ".docx", new byte?[] { 0x50, 0x4B, 0x05, 0x06 }),
            new FileType("PowerPoint Open XML", ".pptx", new byte?[] { 0x50, 0x4B, 0x05, 0x06 }),
            new FileType("Excel Open XML", ".xlsx", new byte?[] { 0x50, 0x4B, 0x07, 0x08 }),
            new FileType("Word Open XML", ".docx", new byte?[] { 0x50, 0x4B, 0x07, 0x08 }),
            new FileType("PowerPoint Open XML", ".pptx", new byte?[] { 0x50, 0x4B, 0x07, 0x08 }),
        }
        .OrderBy(f => f.MaximumStartLocation)
        .ThenBy(f => f.MagicSequence.Length)
        .ToList();

        // 🔍 Signature matching
        private bool StartOfFileContainsFileType(FileType type, byte[] startOfFile)
        {
            var sig = type.MagicSequence;
            if (startOfFile.Length < sig.Length) return false;

            for (int i = 0; i < sig.Length; i++)
            {
                if (sig[i].HasValue && startOfFile[i] != sig[i].Value)
                    return false;
            }

            return true;
        }

        // Main detection method
        public FileTypeRecord GetFileType(string fileName, byte[]? content = null)
        {
            if (content != null)
            {
                foreach (var type in knownFileTypes)
                {
                    if (StartOfFileContainsFileType(type, content))
                        return new FileTypeRecord(type.Extension, _map.TryGetValue(type.Extension, out var mime) ? mime : "application/octet-stream");
                }

                foreach (var type in UnknownFileTypes)
                {
                    if (StartOfFileContainsFileType(type, content))
                    {
                        var ext = Path.GetExtension(fileName);
                        if (ext.Equals(type.Extension, StringComparison.OrdinalIgnoreCase))
                            return new FileTypeRecord(type.Extension, _map.TryGetValue(type.Extension, out var mime) ? mime : "application/zip");

                        return new FileTypeRecord(".zip", "application/zip");
                    }
                }
            }

            var fallbackExt = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(fallbackExt) && _map.TryGetValue(fallbackExt, out var fallbackMime))
                return new FileTypeRecord(fallbackExt, fallbackMime);

            return new FileTypeRecord(fallbackExt ?? string.Empty, "application/octet-stream");
        }

        // 📦 Result container
        public class FileTypeRecord
        {
            public string Extension { get; }
            public string MimeType { get; }

            public FileTypeRecord(string extension, string mimeType)
            {
                Extension = extension;
                MimeType = mimeType;
            }
        }
    }
}
