namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Describes the methods to build a command line parser.
    /// </summary>
    public abstract class Parser
    {
        public static Parser<TCommand> Simple<TCommand>()
        {
            return new SimpleParser<TCommand>();
        }
    }

    /// <summary>
    /// Describes abstarct command parser.
    /// </summary>
    public abstract class Parser<TCommand>
    {
        protected internal const string NonamedIndexer = "@";

        private readonly IDictionary<string, PropertyInfo> _properties;

        protected internal IReadOnlyDictionary<string, PropertyInfo> Properties => (IReadOnlyDictionary<string, PropertyInfo>)_properties;

        protected Parser()
        {
            _properties = GetPublicInstanceSettableProperties().ToDictionary(x => x.Name, StringComparer.InvariantCultureIgnoreCase);
        }

        internal static IEnumerable<PropertyInfo> GetPublicInstanceSettableProperties()
        {
            return from property in typeof(TCommand).GetProperties()
                   let method = property.GetSetMethod(nonPublic: true)
                   where property.CanWrite && method.IsPublic && !method.IsStatic
                   select property;
        }

        /// <summary>
        /// Parses arguments of command line using specified options.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        /// <param name="settings">Settings.</param>
        /// <returns>The instance of command object.</returns>
        public TCommand Parse(string[] args, Settings settings)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var scanner = new Scanner(args);

            if (TryParse(scanner, settings, out TCommand command))
                return command;

            throw new FormatException("Unrecognized command line.");
        }

        /// <summary>
        /// Parses arguments of command line.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        /// <returns>The instance of command object.</returns>
        public TCommand Parse(string[] args)
        {
            var defaultSettings = new Settings();

            return Parse(args, defaultSettings);
        }

        /// <summary>
        /// Links command's property with option's name.
        /// </summary>
        /// <param name="selector">A lambda expression representing the property to be linked (<code>x => x.IsForce</code>).</param>
        /// <param name="name">A name of option including prefix (<code>-f</code>).</param>
        /// <returns>A parser with linked property and option.</returns>
        public Parser<TCommand> Named(Expression<Func<TCommand, object>> selector, string name)
        {
            var info = GetPropertyInfo(selector);

            _properties[name] = info;

            return this;
        }

        /// <summary>
        /// Links command's property with option's name and alternate name.
        /// </summary>
        /// <param name="selector">A lambda expression representing the property to be linked (<code>x => x.IsForce</code>).</param>
        /// <param name="name">A name of option including prefix (<code>-f</code>).</param>
        /// <param name="name">An alternate name of option including prefix (<code>--force</code>).</param>
        /// <returns>A parser with linked property and option.</returns>
        public Parser<TCommand> Named(Expression<Func<TCommand, object>> selector, string name, string alternateName)
        {
            var info = GetPropertyInfo(selector);

            _properties[name] = info;
            _properties[alternateName] = info;

            return this;
        }

        internal static PropertyInfo GetPropertyInfo<TResult>(Expression<Func<TCommand, TResult>> selector)
        {
            if (selector.NodeType != ExpressionType.Lambda)
                throw new ArgumentException("Select must be lambda expression.", nameof(selector));

            var lambda = (LambdaExpression)selector;

            var memberExpression = ExtractMemberExpression(lambda.Body);

            if (memberExpression == null)
                throw new ArgumentException("Select must be member access expression.", nameof(selector));

            if (memberExpression.Member.DeclaringType == null)
                throw new InvalidOperationException("Property does not have declaring type.");

            return memberExpression.Member.DeclaringType.GetProperty(memberExpression.Member.Name);
        }

        private static MemberExpression ExtractMemberExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
                return (MemberExpression)expression;

            if (expression.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)expression).Operand;
                return ExtractMemberExpression(operand);
            }

            return null;
        }

        /// <summary>
        /// Links command's property with a list of unnamed properties.
        /// </summary>
        /// <param name="selector">A lambda expression representing the property to be linked (<code>x => x.Files</code>).</param>
        /// <returns>A parser with linked property.</returns>
        public Parser<TCommand> Nonamed(Expression<Func<TCommand, IEnumerable<string>>> selector)
        {
            var info = GetPropertyInfo(selector);

            _properties[NonamedIndexer] = info;

            return this;
        }

        /// <summary>
        /// Parses arguments of command line using specified settings.
        /// </summary>
        /// <param name="scanner">Iterator of arguments.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="command">Created command object which initialized properties.</param>
        /// <returns><c>true</c> if arguments was parsed successfully; otherwise, <c>false</c>.</returns>
        protected internal abstract bool TryParse(Scanner scanner, Settings settings, out TCommand command);
    }
}