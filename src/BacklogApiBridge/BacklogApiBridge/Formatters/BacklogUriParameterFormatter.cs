using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Refit;

namespace BacklogApiBridge.Formatters
{
    public class BacklogUriParameterFormatter : IUrlParameterFormatter
    {
        static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, EnumMemberAttribute>> EnumMemberCache
            = new ConcurrentDictionary<Type, ConcurrentDictionary<string, EnumMemberAttribute>>();

        public virtual string Format(object parameterValue, ICustomAttributeProvider attributeProvider, Type type)
        {

            // See if we have a format
            var formatString = attributeProvider.GetCustomAttributes(typeof(QueryAttribute), true)
                .OfType<QueryAttribute>()
                .FirstOrDefault()?.Format;

            EnumMemberAttribute enummember = null;
            if (parameterValue != null && type.GetTypeInfo().IsEnum)
            {
                var cached = EnumMemberCache.GetOrAdd(type, t => new ConcurrentDictionary<string, EnumMemberAttribute>());
                enummember = cached.GetOrAdd(parameterValue.ToString(), val => type.GetMember(val).First().GetCustomAttribute<EnumMemberAttribute>());
            }

            return parameterValue == null
                ? null
                : string.Format(CultureInfo.InvariantCulture,
                    string.IsNullOrWhiteSpace(formatString)
                        ? "{0}"
                        : $"{{0:{formatString}}}",
                    enummember?.Value ?? parameterValue);
        }
    }

}
