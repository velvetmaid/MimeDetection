using System;
using System.Collections.Generic;

namespace MimeDetection
{
    internal static class MimeTypeMap
    {
        private static readonly Lazy<IDictionary<string, string>> _mappings
            = new Lazy<IDictionary<string, string>>(BuildMappings, isThreadSafe: true);

        public static IReadOnlyDictionary<string, string> Mappings
            => (IReadOnlyDictionary<string, string>)_mappings.Value;

        private static IDictionary<string, string> BuildMappings()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // --- contoh entri, tambahkan seluruh Big Freaking List di sini ---
                {".pdf",  "application/pdf"},
                {".png",  "image/png"},
                {".jpg",  "image/jpeg"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".zip",  "application/zip"},
                // … lanjutkan ratusan/ ribuan entri lainnya …
            };

            return map;
        }
    }
}
