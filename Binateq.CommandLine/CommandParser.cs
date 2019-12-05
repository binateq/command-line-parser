namespace Binateq.CommandLine
{
    using System;

    internal class CommandParser<TCommandBase, TCommand> : Parser<TCommandBase, TCommand>
    {
        private readonly string _name;
        private readonly string _altername;

        internal CommandParser(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        internal CommandParser(string name, string altername)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _altername = altername ?? throw new ArgumentNullException(nameof(altername));
        }

        protected internal override bool TryParse(Scanner scanner, Settings settings, out TCommandBase command)
        {
            command = default;
            if (!IsItMe(scanner.Current))
                return false;

            command = (TCommandBase)settings.Resolve(typeof(TCommand));

            if (scanner.HasNext)
                scanner.Next();

            var options = new Options(scanner, settings.OptionPrefixes, settings.OptionSeparators, settings.ToPropertyName);

            UpdateNamedProperties(command, options.Named);
            UpdateNonamedIndexer(command, options.Unnamed);

            return true;
        }

        protected internal bool IsItMe(string name)
        {
            if (string.Equals(name, _name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return string.Equals(name, _altername, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
