# Command Line Parser

## Translations

1. [Русский](README.RU.md)

## Content

* [Quick Start](#quick-start)
* [Simple Parser](#simple-parser)
* [Parser With Commands](#parser-with-commands)
* [Named Parameters](#named-parameters)
* [Nameless Parameters](#nameless-parameters)

## Quick Start

Let's open the project **Binateq.CommandLine.Demo.Copy**. This is a program that copies
the files like utilities **copy** or **cp**.

In simplest case the program makes a copy of a file with a new name.

```
democopy source.file destination.file
```

If **destination.file** already exists, then the program doesn't rewrite it and prints the warning message.

To force rewrite you need use the option **-f** or **--force**.

```
democopy source.file destination.file -f
```

While copying a file the program reads a part of source file in the buffer and then writers this buffer to the destination file.

The size of buffer may be set with the help of parameter **--size**.

```
democopy source.file destination.file --size=1024
```

Let's see the source code of **Program.cs**

```
static void Main(string[] args)
{
    var parser = Parser.Simple<Command>()
                       .Named(x => x.IsHelp, "-h", "--help")
                       .Named(x => x.IsForce, "-f", "--force")
                       .Nonamed(x => x.Files);

    try
    {
        var command = parser.Parse(args);

        command.Run();
    }
    catch (Exception exception)
    {
        Console.Error.WriteLine(exception.Message);
    }
}
```

Simple problem is solved with a simple program. The library let us *combine* parser from the components. Result parsers can analyze the command lines and create object `command` with specified properties.

Let's see the source code of the `Command` class.

```
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

        using (var output = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
        {
            foreach (var sourceFile in sourceFiles)
            {
                using (var input = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        output.Write(buffer, 0, read);
                }

                Console.WriteLine(sourceFile);
            }
        }
    }
}
```

This is the pattern *Command*. It's not accident that the class is named that way.
The parser analyzes a command line and fills the properties of `Command`: `IsHelp`, `IsForce`, `Files`, and `Size`.

To execute the command we should call the method `Run`. We can named it as we wish — `Do` or `Execute`. The library doesn't require us to use specific command interface.

## Simple Parser

You can use simple parser if you need a few file names and a few options in a command line, like in the **democopy** program.

Let's make the class with properties that match to required parameters.

If we need the option **--size** with the integer type, let's append public mutable integer property named `Size`. If we need the option **--help** as a flat, let's append boolean property `Help`.

The command should have a method that will be called when we want have a result. Let's call it `Do`.

```
class Foo
{
    public int Size { get; set; }

	public bool Help { get; set; }

	public void Do()
	{
	    if (Help)
		  Console.WriteLine("Usage: program --size=N | --help");
		else
		  Console.WriteLine("Square with side {0} equals to {1}.", Size, Size * Size);
	}
}
```

Finally we should construct the pareser and call the method `Parse`.

```
static void Main(string[] args)
{
    var parser = Parser.Simple<Foo>();

    var foo = parser.Parse(args);
    foo.Do();
}
```

## Parser With Commands

Let's open the project **Binateq.CommandLine.Demo.Zip**. The program can pack files into ZIP archive, and extract them.

In the programs like **arj** or **git** in addition to options and files there are commands too.

To pack the files in **arj** we run

```
arj a -d2 archive file₁ file₂ ... fileₙ
```

To commit changes in **git** we run

```
git commit -m "Comment"
```

The first parameter of the program (**a** of **arj** and **commit** of **git**) is a command name. Sometimes programs recognize as full command names like **remove** as short names like **r**.

We implement four command in the project **Binateq.CommandLine.Demo.Zip**: **add**, **extract**, **list**, and **help**.

We need create separate class for each of them. Let's name them `AddCommand`, `ExtractCommand`, `ListCommand`, and `HelpCommand`.

In the pattern *Command* different commands implement common interface or extend common abstract class.


```
interface ICommand
{
    void Run();
}

class AddCommand : ICommand
{
    public void Run() {  }
}
```

Finally, let's describe the pattern of the command line inside **Program.cs** file.

```
var parser = Parser.Command<ICommand, AddCommand>("add", "a")
           | Parser.Command<ICommand, ExtractCommand>("extract", "x")
           | Parser.Command<ICommand, ListCommand>("list", "l")
           | Parser.Default<ICommand, HelpCommand>("help", "h")
           ;

var command = parser.Parse(args);
command.Run();
```

If the first parameter of command line will be **add** or **a**, then the parser will create the instance of the `AddCommand` class.

The method `parser.Parse(args)` will return an object that implements `ICommand`, or raises an exception.

## Named Parameters

The **add** command let us set level of packing, as a number 0, 1, or 2.

```
demozip add archive -level=2 file₁ file₂ ... fileₙ
```

The `level` parameter called *named* parameter because has the name. By default, all mutable public properties of command class are named parameters.

```
class AddCommand : ICommand
{
    public int Level { get; set; } = 0;

    public void Run() {  }
}
```

Sometimes we want to set short name for named parameter.

```
var parser = Parser.Command<ICommand, AddCommand>("add", "a")
                   .Named(x => x.Level, "-l")
             . . .
```

Sometimes parameter has both short and full name.

```
var parser = Parser.Command<ICommand, AddCommand>("add", "a")
                   .Named(x => x.Level, "-l", "--level")
             . . .
```

## Nameless Parameters

The command **add** can process a few file names. First of them is the archive's name, and other are names of files to pack.

```
demozip add archive file₁ file₂ ... fileₙ
```

All parameters that start with *hyphen* (-) are named. All other parameters are *nameless*. All of them store in a single property, that has the type `IEnumerable<string>`, `IReadOnlyCollection<string>`, or `IReadOnlyList<string>`.

```
class AddCommand : ICommand
{
    public int Level { get; set; } = 0;

    public IReadOnlyList<string> Files { get; set; }

    public void Run() {  }
}
```

```
var parser = Parser.Command<ICommand, AddCommand>("add", "a")
                   .Named(x => x.Level, "-l", "--level")
                   .Nonamed(x => x.Files)
             . . .
```

Implementation of the **add** command.

```
class AddCommand : ICommand
{
    public int Level { get; set; } = 0;

    public uint Size { get; set; } = 1 << 16;

    public IReadOnlyList<string> Files { get; set; }

    public void Run()
    {
        if (Files.Count == 0)
            throw new InvalidOperationException("Missing archive's name.");

        if (Files.Count == 1)
            throw new InvalidOperationException("Missing files to archive.");

        var buffer = new byte[Size];

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
```

The method `Run` does all the work. The parser fills values of properties `Level`, `Size`, and `Files` while anaylyzing command line, so `Run` can use them.
