# Анализатор командной строки

## Переводы

1. [English](README.md)

## Содержание

* [Быстрый старт](#быстрый-старт)
* [Простой анализатор](#простой-анализатор)
* [Командная строка с командами](#командная-строка-с-командами)

## Быстрый старт

Загрузите проект **Binateq.CommandLine.Demo.Copy**. Эта программа копирует файлы, как это делают
**copy** и **cp**.

В простейшем случае программа делает копию файла с новым именем.

```
democopy source.file destination.file
```

Если **destination.file** уже существует, программа не станет его перезаписывать и выдаст предупрежедение.
Чтобы перезаписать файл, надо добавить опцию **-f** или **--force**.

```
democopy source.file destination.file -f
```

Во время копирования программа читает в буфер кусок исходного файла и затем записывает этот буфер на диск.
Размер буфера в байтах может быть задан с помощью параметра **--size**.

```
democopy source.file destination.file --size=1024
```

Посмотрим на исходный код **Program.cs**

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

Простая задача решается простой программой. Мы видим, что библиотека позволяет *собрать* анализатор
из компонентов. Собранный анализатор разбират командную строку и создает объект `command`,
который в конечном итоге и копирует файл.

Взглянем на класс `Command`.

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

Это паттерн *Command* (*Команда*), и не случайно класс назван именно так. Анализатор разбирает
командную строку и заполняет свойства объекта `Command` — `IsHelp`, `IsForce`, `Files` и `Size`.

Запуск команды мы осуществляем в явном виде, вызывав метод `Run`. Название методу мы можем дать
другое — `Do` или `Execute` — потому что библиотека не заставляет нас использовать какой-то определённый
интерфейс команды.

## Простой анализатор

Простой анализатор подходит, если нам нужны несколько имён файлов и несколько опций в командной строке,
как в программе **democopy**.

Создадим класс, свойства которого будут отражать нужную функциональность.

Если нам нужна опция **--size**, значением которой будет целое число, добавим публичное изменяемое
свойство `Size` целого типа. Если нам нужна опция **--help** как флаг без значения, добавим логическое
свойство `Help`.

Команда должна иметь метод, который будет вызван, когда мы захотим получить результат. Назовём его `Do`.

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

Осталось сконструировать анализатор и передать ему командную строку.

```
static void Main(string[] args)
{
    var parser = Parser.Simple<Foo>();

    var foo = parser.Parse(args);
    foo.Do();
}
```

## Командная строка с командами

Загрузите проект **Binateq.CommandLine.Demo.Zip**. Эта программа умеет упаковывать файлы в
архив ZIP и распаковывать их.

В таких программах, как **arj** или **git** помимо опций и имён файлов появляются команды.

Чтобы упаковать файлы с помощью **arj**, мы пишем

```
arj a -d2 archive file1 file2 file3
```

Чтобы зафиксировать изменения в git

```
git commit -m "Comment"
```

Первый параметр программы (**a** у **arj** и **commit** у **git**) — это название команды.
Иногда программы понимают и полные названия комманд, такие как **remove**, и сокращённые,
такие как **r**.

В нашем проекте мы реализуем четыре команды: **add**, **extract**, **list** и **help**.
Для каждой команды создадим класс. Назовём классы `AddCommand`, `ExtractCommand`,
`ListCommand` и `HelpCommand`.

В паттерне *Command* реализации команд разделяют общий интерфейс. Опишем его.

```
interface ICommand
{
    void Run();
}
```

