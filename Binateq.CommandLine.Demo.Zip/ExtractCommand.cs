namespace Binateq.CommandLine.Demo.Zip
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;

    class ExtractCommand : ICommand
    {
        public IReadOnlyList<string> Files { get; set; }

        public void Run()
        {
            if (Files.Count == 0)
                throw new InvalidOperationException("Missing archive's name.");

            var buffer = new byte[1 << 16];

            using (var archive = ZipFile.Open(Files[0], ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    using (var input = entry.Open())
                    using (var output = File.OpenWrite(entry.FullName))
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                            output.Write(buffer, 0, read);
                    }

                    Console.WriteLine(entry.FullName);
                }
            }
        }
    }
}
