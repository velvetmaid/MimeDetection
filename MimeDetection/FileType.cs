public class FileType
{
    public string Name { get; }
    public string Extension { get; }
    public byte?[] MagicSequence { get; }
    public int MaximumStartLocation => 0; // bisa diubah kalau kamu mau offset detection

    public FileType(string name, string extension, byte?[] magicSequence)
    {
        Name = name;
        Extension = extension;
        MagicSequence = magicSequence;
    }
}
