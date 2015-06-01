namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using StringConverters;

    public class DefaultStringConverters : ITypeStringConverters
    {
        internal static readonly ConcurrentDictionary<Type, IStringConverter> Converters = new ConcurrentDictionary<Type, IStringConverter>();

        static DefaultStringConverters()
        {
            var converterTypes = typeof (IStringConverter).Assembly.GetTypes()
                .Where(
                    x => typeof (IStringConverter).IsAssignableFrom(x) && (x.IsClass || x.IsValueType) && !x.IsAbstract)
                .ToArray();
            foreach (var type in converterTypes)
            {
                var converter = (IStringConverter)Activator.CreateInstance(type);
                Converters.AddOrUpdate(converter.Type, converter, (t, c) => converter);
            }
        }

        public virtual IStringConverter Get(Type type)
        {
            if (type == null)
            {
                return null;
            }

            IStringConverter converter;
            if (Converters.TryGetValue(type, out converter))
            {
                return converter;
            }
            throw new ArgumentException(string.Format("Did not find a converter for {0}", type.FullName));
        }
    }
}
