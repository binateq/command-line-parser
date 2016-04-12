namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Implements parser what applies any command line.
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public class CommandLineParserDefault<TCommand> : TypedCommandParser
    {
        private readonly IDictionary<PropertyInfo, object> _propertyValues;

        /// <summary>
        /// Initializes new instance of <see cref="CommandLineParserDefault{TCommand}"/> type.
        /// </summary>
        public CommandLineParserDefault()
            : base(typeof (TCommand))
        {
            _propertyValues = new Dictionary<PropertyInfo, object>();
        }

        /// <summary>
        /// Stores property and value to set, if the parser will be applied.
        /// </summary>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="selector">Member expression to select property.</param>
        /// <param name="value">Value of property.</param>
        /// <returns>Same parser to organize chain of setters.</returns>
        public virtual CommandLineParserDefault<TCommand> WithProperty<TProperty>(Expression<Func<TCommand, TProperty>> selector, TProperty value)
        {
            var propertyInfo = selector.ToPropertyInfo();
            _propertyValues.Add(propertyInfo, value);

            return this;
        }

        #region CommandLineParser implementation

        protected internal override bool TryApply<TCommandInterface>(string[] args, CommandLineParseOptions options, out TCommandInterface result)
        {
            result = CreateCommand<TCommandInterface>(options.Resolve);

            return true;
        }

        #endregion

        #region TypedCommandParser implementation

        protected internal override TCommandInterface CreateCommand<TCommandInterface>(Func<Type, object> resolve)
        {
            var result = base.CreateCommand<TCommandInterface>(resolve);
            _propertyValues.Set(result);

            return result;
        }

        #endregion
    }
}