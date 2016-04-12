namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Implements extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Combines two specified functions.
        /// </summary>
        /// <param name="first">First function.</param>
        /// <param name="second">Second function.</param>
        /// <returns>Combined function that calls first functions and passes its result to second function.</returns>
        public static Func<T1, TResult> Composite<T1, TIntermediate, TResult>(this Func<T1, TIntermediate> first, Func<TIntermediate, TResult> second)
        {
            return (arg) => second(first(arg));
        }

        /// <summary>
        /// Converts <see cref="MemberExpression">member expression</see> to <see cref="PropertyInfo">property information</see>.
        /// </summary>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <typeparam name="TObject">Type of object.</typeparam>
        /// <param name="selector">Member selector expression.</param>
        /// <returns>Property information for specified member.</returns>
        public static PropertyInfo ToPropertyInfo<TProperty, TObject>(this Expression<Func<TObject, TProperty>> selector)
        {
            Expression body = selector;

            if (body is LambdaExpression)
                body = ((LambdaExpression)body).Body;

            if (body.NodeType != ExpressionType.MemberAccess)
                throw new InvalidOperationException();

            return (PropertyInfo)((MemberExpression)body).Member;
        }

        /// <summary>
        /// Sets property values in the specified object.
        /// </summary>
        /// <typeparam name="TObject">Type of object.</typeparam>
        /// <param name="propertyValues">The properties and values collection.</param>
        /// <param name="object">The object.</param>
        public static void Set<TObject>(this IDictionary<PropertyInfo, object> propertyValues, TObject @object)
        {
            foreach (var propertyValue in propertyValues)
            {
                var propertyInfo = propertyValue.Key;
                var value = propertyValue.Value;

                propertyInfo.SetValue(@object, value, null);
            }
        }

        public static bool IsNullOrEmpty(this string[] args)
        {
            return args == null || args.Length == 0;
        }
    }
}