namespace Binateq.CommandLine.Demo.Zip
{
    using System;
    using System.Collections.Generic;
    using System.IO.Compression;

    class ListCommand : ICommand
    {
        public IReadOnlyList<string> Files { get; set; }

        public void Run()
        {
            if (Files.Count == 0)
                throw new InvalidOperationException("Missing archive's name.");

            using (var archive = ZipFile.Open(Files[0], ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                    Console.WriteLine(entry.FullName);
            }
        }
    }
}
