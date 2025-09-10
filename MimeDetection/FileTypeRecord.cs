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