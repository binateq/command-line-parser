namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SimpleParser<TCommand> : Parser<TCommand>
    {
        protected internal override bool TryParse(Scanner scanner, Settings settings, out TCommand command)
        {
            var options = new Options(scanner, settings.OptionPrefixes, settings.OptionSeparators, settings.ToPropertyName);
            command = (TCommand)settings.Resolve(typeof(TCommand));

            UpdateNamedProperties(command, options.Named);
            UpdateNonamedIndexer(command, options.Unnamed);

            return true;
        }

        private void UpdateNamedProperties(object command, IReadOnlyDictionary<string, string> named)
        {
            foreach (var nameValue in named)
                UpdateNamedProperty(command, nameValue.Key, nameValue.Value);
        }

        private void UpdateNamedProperty(object command, string name, string stringValue)
        {
            if (!Properties.ContainsKey(name))
                return;

            var type = Properties[name].PropertyType;
            var value = ConvertStringToType(stringValue, type);

            Properties[name].SetValue(command, value);
        }

        private static readonly string[] TrueValues = new[] { null, "", "1", "yes", "true" };
        private static readonly string[] FalseValues = new[] { "0", "no", "false" };

        internal static object ConvertStringToType(string value, Type type)
        {
            if (type == typeof(bool))
            {
                if (TrueValues.Any(x => string.Equals(value, x, StringComparison.InvariantCultureIgnoreCase)))
                    return true;

                if (FalseValues.Any(x => string.Equals(value, x, StringComparison.InvariantCultureIgnoreCase)))
                    return false;

                throw new InvalidOperationException("Invalid boolean value.");
            }

            if (type == typeof(string))
                return value;

            return Convert.ChangeType(value, type);
        }

        private void UpdateNonamedIndexer(object command, IEnumerable<string> unnamed)
        {
            if (Properties[NonamedIndexer] == null)
                return;

            Properties[NonamedIndexer].SetValue(command, unnamed);
        }
    }
}
