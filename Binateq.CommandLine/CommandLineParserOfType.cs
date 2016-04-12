namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class CommandLineParserOfType<TCommand> : TypedCommandParser
    {
        private readonly ICollection<PropertyInfo> _required;
        private readonly ICollection<PropertyInfo> _optional;

        public CommandLineParserOfType(CommandPattern pattern)
            : base(typeof (TCommand), pattern)
        {
            _required = new List<PropertyInfo>();
            _optional = new List<PropertyInfo>();
        }

        public CommandLineParserOfType<TCommand> WithRequired<TProperty>(Expression<Func<TCommand, TProperty>>  selector)
        {
            var propertyInfo = selector.ToPropertyInfo();
            _required.Add(propertyInfo);

            return this;
        }

        public CommandLineParserOfType<TCommand> WithOptional<TProperty>(Expression<Func<TCommand, TProperty>> selector)
        {
            var propertyInfo = selector.ToPropertyInfo();
            _optional.Add(propertyInfo);

            return this;
        }

        protected internal override bool TryApply<TCommandInterface>(string[] args, out TCommandInterface result)
        {
            result = default(TCommandInterface);

            if (args.IsNullOrEmpty())
                return false;

            if (!string.Equals(args[0], NormalizedCommandName, StringComparison.Ordinal))
                return false;

            result = CreateCommand<TCommandInterface>();
            return true;
        }
    }
}