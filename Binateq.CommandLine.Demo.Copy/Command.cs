namespace Binateq.CommandLine.Demo.Copy
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Command
    {
        public bool IsHelp { get; set; }

        public bool IsForce { get; set; }

        public IReadOnlyList<string> Files { get; set; }

        public uint Size { get; set; } = 1 << 16;

        public void Run()
        {
            if (IsHelp || Files.Count == 0)
            {
                Console.WriteLine("Usage: democopy [options] source-file+ destination-file");
                Console.WriteLine("Options are:");
                Console.WriteLine("  -h | --help   Print help.");
                Console.WriteLine("  -f | --force  Overwrite destination file if it exists.");
                Console.WriteLine("  -size:N       Set buffer size to N bytes (1024 >= N >= 1048576.");

                return;
            }

            if (Files.Count < 2)
                throw new InvalidOperationException("There are no source files to copy.");

            if (Size < 1024 || Size >= 1024 * 1024)
                throw new InvalidOperationException("Too small or too large buffer size.");

            Copy();
        }

        private void Copy()
        {
            var destinationFile = Files[Files.Count - 1];
            var sourceFiles = new string[Files.Count - 1];
            var buffer = new byte[Size];

            for (int i = 0; i < sourceFiles.Length; i++)
                sourceFiles[i] = Files[i];

            if (File.Exists(destinationFile) && !IsForce)
                throw new InvalidOperationException("Destination file already exists. Specify --force parameter to overwrite.");

            using (var outputStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                foreach (var sourceFile in sourceFiles)
                {
                    using (var inputStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    {
                        int read;
                        while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                            outputStream.Write(buffer, 0, read);
                    }

                    Console.WriteLine(sourceFile);
                }
            }
        }
    }
}
