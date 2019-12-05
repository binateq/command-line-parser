namespace Binateq.CommandLine
{
    internal class DefaultParser<TCommandBase, TCommand> : CommandParser<TCommandBase, TCommand>
    {
        public DefaultParser(string name) : base(name) { }

        public DefaultParser(string name, string altername) : base(name, altername) { }

        protected internal override bool TryParse(Scanner scanner, Settings settings, out TCommandBase command)
        {
            if (IsItMe(scanner.Current))
                scanner.Next();

            command = (TCommandBase)settings.Resolve(typeof(TCommand));

            var options = new Options(scanner, settings.OptionPrefixes, settings.OptionSeparators, settings.ToPropertyName);

            UpdateNamedProperties(command, options.Named);
            UpdateNonamedIndexer(command, options.Unnamed);

            return true;
        }
    }
}