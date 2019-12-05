namespace Binateq.CommandLine
{
    internal class SimpleParser<TCommand> : Parser<TCommand, TCommand>
    {
        protected internal override bool TryParse(Scanner scanner, Settings settings, out TCommand command)
        {
            command = (TCommand)settings.Resolve(typeof(TCommand));
            var options = new Options(scanner, settings.OptionPrefixes, settings.OptionSeparators, settings.ToPropertyName);

            UpdateNamedProperties(command, options.Named);
            UpdateNonamedIndexer(command, options.Unnamed);

            return true;
        }
    }
}
