namespace MimeDetection
{
    public class FileTypeRecord
    {
        public string Extension { get; }
        public string MimeType { get; }
        public string ErrorMessage { get; }

        public FileTypeRecord(string extension, string mimeType, string errorMessage = "")
        {
            Extension = extension;
            MimeType = mimeType;
            ErrorMessage = errorMessage;
        }
    }
}
