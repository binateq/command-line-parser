#nullable enable

namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;

    public class Scanner
    {
        private readonly IEnumerator<string> _iterator;

        public bool HasNext { get; private set; }

        public string? Current { get; private set; }

        public Scanner(IEnumerable<string> args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            _iterator = args.GetEnumerator();

            HasNext = _iterator.MoveNext();
            Current = HasNext ? _iterator.Current : null;
        }

        public void Next()
        {
            if (!HasNext)
                throw new InvalidOperationException();

            HasNext = _iterator.MoveNext();
            Current = HasNext ? _iterator.Current : null;
        }
    }
}
