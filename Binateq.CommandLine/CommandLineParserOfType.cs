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

        public CommandLineParserOfType()
            : base(typeof (TCommand))
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

        protected internal override bool TryApply<TCommandInterface>(string[] args, CommandLineParseOptions options, out TCommandInterface result)
        {
            result = default(TCommandInterface);

            if (args.IsNullOrEmpty())
                return false;

            var normalizedCommandName = GetNormalizedCommandName(options.NormalizeCommandName);

            if (!string.Equals(args[0], normalizedCommandName, options.StringComparison))
                return false;

            result = CreateCommand<TCommandInterface>(options.Resolve);
            return true;
        }
    }
}