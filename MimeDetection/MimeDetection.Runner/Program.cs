using System;
using System.IO;
using MimeDetection;

class Program
{
    static void Main()
    {
        var service = new MimeTypeService();

        Console.WriteLine("=== MIME Detection Test ===");

        // Tes 1: (.pdf)
        var result1 = service.GetFileType("file.pdf");
        Console.WriteLine($"[Ekstensi] file.pdf → {result1.Extension} → {result1.MimeType}");

        // Tes 2: JPEG header with .exe extension
        var jpegHeader = new byte[] { 0xFF, 0xD8, 0xFF };
        var result2 = service.GetFileType("renamed.exe", jpegHeader);
        Console.WriteLine($"[JPEG] renamed.exe → {result2.Extension} → {result2.MimeType}");

        // Tes 3: ZIP header with .docx extension
        var zipHeader = new byte[] { 0x50, 0x4B, 0x03, 0x04 };
        var result3 = service.GetFileType("resume.docx", zipHeader);
        Console.WriteLine($"[ZIP .docx] resume.docx → {result3.Extension} → {result3.MimeType}");

        // Tes 4: ZIP header with .zip extension
        var result4 = service.GetFileType("archive.zip", zipHeader);
        Console.WriteLine($"[ZIP .zip] archive.zip → {result4.Extension} → {result4.MimeType}");

        // Tes 5: Unknown file type
        var result5 = service.GetFileType("unknown.xyz");
        Console.WriteLine($"[Unknown] unknown.xyz → {result5.Extension} → {result5.MimeType}");

        // Tes 6: Uppercase extension .DOCX
        var result6 = service.GetFileType("RESUME.DOCX", zipHeader);
        Console.WriteLine($"[Uppercase .DOCX] RESUME.DOCX → {result6.Extension} → {result6.MimeType}");

        // Tes 7: ZIP header with no extension
        var result7 = service.GetFileType("noext", zipHeader);
        Console.WriteLine($"[ZIP tanpa ekstensi] noext → {result7.Extension} → {result7.MimeType}");

        Console.WriteLine("=== Done ===");

        Console.WriteLine("=== MIME Detection Test dengan file asli ===");

        TestFile(service, @"D:\File Samples\sample1 PDF.pdf");
        TestFile(service, @"D:\File Samples\sample_640×426 GIF.gif");
        TestFile(service, @"D:\File Samples\sample1 MP3.mp3");
        TestFile(service, @"D:\File Samples\zip_2MB ZIP.zip");
        TestFile(service, @"D:\File Samples\sample2 DOC.doc");
        TestFile(service, @"D:\File Samples\sample2 DOCX.docx");
        TestFile(service, @"D:\File Samples\sample1 XLS.xls");
        TestFile(service, @"D:\File Samples\sample2 XLSX.xlsx");
        TestFile(service, @"D:\File Samples\sample2 CSV.csv");
        TestFile(service, @"D:\File Samples\sample2 PPT.ppt");
        TestFile(service, @"D:\File Samples\sample2 RB.rb");
        TestFile(service, @"D:\File Samples\sample3 TXT.txt");

        Console.WriteLine("=== Done ===");
    }

    static void TestFile(MimeTypeService service, string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"[SKIP] {path} (File Not Found!)");
            return;
        }

        byte[] header = File.ReadAllBytes(path);

        var result = service.GetFileType(path, header);
        Console.WriteLine($"{Path.GetFileName(path)} → {result.Extension} → {result.MimeType}");
    }
}

