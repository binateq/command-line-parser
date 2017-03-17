# Command Line Parser

## Few examples of command line parameters

```bash
arj a -m3 archive *.*
```
Here `a` is the *command*; `-m3` is the *switch*, or the *option*, or the *named parameter* with the name `m`
and the value `3`; `archive` and `*.*` are *ordered parameters*, first and second respectively. The `archive`
is required, but the `*.*` is optional.

```bash
git commit --message=Message
git commit -m Message
```
Here `commit` is the *command*; `--message` is the *option* and `-m` is an alternate short form of `--message`;
The `Message` is the required parameter for `-m` and value for `--message`.

## Parser

This parser can parsing commands and possible named and ordered parameters in form `--name=value`,
`-name=value`, or `/name:value`.

## `Command` design pattern

Usually you have abstract class or interface that represents a command. This command can be *runned*,
or *started*, or *executed*, or *did*:

```c#
public interface ICommand
{
    // void Run();

    // void Start();

    void Execute();

    // void Do();
    
    // void Undo();
}
```

Then you make specific classes, one for each command, that implement the interface or extend the abstract class.

```c#
public class HelpCommand : ICommand
{
    public string Command { get; set; }

    public bool Verbose { get; set; }

    public void Execute()
    {
        if (Command == null)
        {
            Console.WriteLine("List of commands:");
            Console.WriteLine("help");
            . . .
        }
        else if (Command == "help")
        {
            Console.WriteLine("program-name help");
            Console.WriteLine("  Prints the list of commands.");
            Console.WriteLine();
            Console.WriteLine("program-name help <command> --verbose");
            Console.WriteLine("  Prints help for specified command. --verbose prints detailed help.");

            if (Verbose)
                Console.WriteLine("You can use --verbose options if you didn't understand short command description.");
        }
    }
}
```

Finally you separate the creation of command and its using. For example, you can create a queue
of `ICommand`s, or a stack of `ICommand`s, or implement an atomic execution of `ICommand`s' list, and so on.

This parser creates an instance of specific command, based on command line.

## What command line pattern is?

Command line *pattern* is an object describing all possible commands together with all possible options.
The result of *applying* this pattern to the command line arguments is command object, i.e. object
implementing `ICommand` interface.

You can do with the created object whatever you want, for example you can *execute* it.

```c#
var commandLinePattern = . . .
var args = new[] { "help", "add", "--verbose" };
var command = commandLinePattern.Apply<ICommand>(args);
command.Execute();
```
## Examples

### Command patterns

Imagine we have `start`, `stop`, and `help` commands without parameters. To make the pattern we should first
to develop corresponding classes `StartCommand`, `StopCommand`, and `HelpCommand`. Second we describe a
pattern of command line:

```
command-line-pattern = start | stop | help
```
or, as it should be in C#:

```c#
var commandLinePattern = CommandPattern.OfType<StartCommand>()
                       | CommandPattern.OfType<StopCommand>()
                       | CommandPattern.OfType<HelpCommand>();
```

You may don't use `Command` suffix:

```c#
class Start : ICommand
{
    public void Execute()
    {
        . . .
    }
}

class StopService: ICommand
. . .

class Help: ICommand
. . .

var commandLinePattern = CommandPattern.OfType<Start>()
                       | CommandPattern.OfType<StopService>()
                       | CommandPattern.OfType<Help>();
```

### Default patterns

You can set default command with help of the `CommandPattern.Default` method:

```c#
var commandLinePattern = CommandPattern.OfType<Start>()
                       | CommandPattern.OfType<Stop>()
                       | CommandPattern.OfType<Help>()
                       | CommandPattern.Default<Help>();
```

With empty command line the `Help` command will be created when pattern applying. Default
pattern should be last and it should be single.

You can set default command's properties with the `WithProperty` method:

```c#
var commandLinePattern = CommandPattern.OfType<Start>()
                       | CommandPattern.OfType<Stop>()
                       | CommandPattern.OfType<Help>()
                       | CommandPattern.Default<Help>()
                                       .WithProperty(help => help.Command, "help")
                                       .WithProperty(help => help.Verbose, true);
```

Without command line arguments the pattern will create the `Help` object with `Command` property set to `"help"`,
and `Verbose` property set to `true`.

### Ordered parameters

You can extend command pattern describing ordered parameters:

```c#
public class AddCommand
{
	public string ArchiveName { get; set; }

	public string[] SourceMasks { get; set; }
}

var commandLinePattern = CommandPattern.OfType<AddCommand>()
                                       .WithReqired(command => command.ArchiveName)
                                       .WithOptional(command => command.SourceMasks);
```

When running:

```bash    
    program-name add progarch *.c *.h
```

the property `ArchiveName` will contain `"progarch"`, and the `SourceMasks` will contain `{ "*.c", "*.h" }`.
