namespace Binateq.CommandLine
{
    using System;

    internal class OrParser<TCommandBase> : Parser<TCommandBase>
    {
        private Parser<TCommandBase> _p1;
        private Parser<TCommandBase> _p2;

        public OrParser(Parser<TCommandBase> p1, Parser<TCommandBase> p2)
        {
            _p1 = p1 ?? throw new ArgumentNullException(nameof(p1));
            _p2 = p2 ?? throw new ArgumentNullException(nameof(p2));
        }

        protected internal override bool TryParse(Scanner scanner, Settings settings, out TCommandBase command)
        {
            return _p1.TryParse(scanner, settings, out command)
                || _p2.TryParse(scanner, settings, out command);
        }
    }
}