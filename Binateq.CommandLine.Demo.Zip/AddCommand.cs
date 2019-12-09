namespace Binateq.CommandLine.Demo.Zip
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;

    class AddCommand : ICommand
    {
        public int Level { get; set; } = 0;

        public IReadOnlyList<string> Files { get; set; }

        public void Run()
        {
            if (Files.Count == 0)
                throw new InvalidOperationException("Missing archive's name.");

            if (Files.Count == 1)
                throw new InvalidOperationException("Missing files to archive.");

            var buffer = new byte[1 << 16];

            using (var archive = ZipFile.Open(Files[0], ZipArchiveMode.Create))
            {
                for (int i = 1; i < Files.Count; i++)
                {
                    var entry = archive.CreateEntry(Files[i], (CompressionLevel)Level);
                    using (var output = entry.Open())
                    using (var input = File.OpenRead(Files[i]))
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                            output.Write(buffer, 0, read);
                    }

                    Console.WriteLine(Files[i]);
                }
            }
        }
    }
}
