namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.StringConverters;

    public static partial class Input
    {
        public static readonly ConcurrentDictionary<int, string> Formats = new ConcurrentDictionary<int, string>();

        public static readonly RoutedEvent FormattingDirtyEvent = EventManager.RegisterRoutedEvent(
            "FormattingDirty",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(Input));

        internal static readonly RoutedEventArgs FormattingDirtyArgs = new RoutedEventArgs(Input.FormattingDirtyEvent);
        
        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(IFormatProvider),
            typeof(Input),
            new FrameworkPropertyMetadata(
                CultureInfo.GetCultureInfo("en-US"), // Think this is the default in WPF
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

        public static readonly DependencyProperty NumberStylesProperty = DependencyProperty.RegisterAttached(
            "NumberStyles",
            typeof(NumberStyles),
            typeof(Input),
            new FrameworkPropertyMetadata(NumberStyles.None, FrameworkPropertyMetadataOptions.Inherits, OnNumberStylesChanged, OnCoerceNumberStyles));

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.RegisterAttached(
            "StringFormat",
            typeof(string),
            typeof(Input),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits, OnStringFormatChanged));

        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.RegisterAttached(
            "DecimalDigits",
            typeof(int?),
            typeof(Input),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnDecimalDigitsChanged));

        public static readonly DependencyProperty FormatterProperty = DependencyProperty.RegisterAttached(
            "Formatter",
            typeof(IFormatter),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultFormatter(), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty NumberStylesMapperProperty = DependencyProperty.RegisterAttached(
            "NumberStylesMapper",
            typeof(ITypeNumberStyles),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultNumberStyles(), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty StringConvertersProperty = DependencyProperty.RegisterAttached(
            "StringConverters",
            typeof(ITypeStringConverters),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultStringConverters(), FrameworkPropertyMetadataOptions.Inherits, null, OnStringConvertersCoerce));

        public static readonly DependencyProperty StringConverterProperty = DependencyProperty.RegisterAttached(
            "StringConverter",
            typeof(IStringConverter),
            typeof(Input),
            new PropertyMetadata(default(IStringConverter), OnStringConverterChanged, OnStringConverterCoerce));

        public static void AddFormattingDirtyHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(FormattingDirtyEvent, handler);
        }

        public static void RemoveFormattingDirtyHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(FormattingDirtyEvent, handler);
        }

        public static void SetCulture(this DependencyObject element, IFormatProvider value)
        {
            element.SetValue(CultureProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IFormatProvider GetCulture(this DependencyObject element)
        {
            return (IFormatProvider)element.GetValue(CultureProperty);
        }

        public static void SetNumberStyles(this DependencyObject element, NumberStyles value)
        {
            element.SetValue(NumberStylesProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static NumberStyles GetNumberStyles(this DependencyObject element)
        {
            return (NumberStyles)element.GetValue(NumberStylesProperty);
        }

        public static void SetStringFormat(this DependencyObject element, string value)
        {
            element.SetValue(StringFormatProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetStringFormat(this DependencyObject element)
        {
            return (string)element.GetValue(StringFormatProperty);
        }

        public static void SetDecimalDigits(this DependencyObject element, int? value)
        {
            element.SetValue(DecimalDigitsProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static int? GetDecimalDigits(this DependencyObject element)
        {
            return (int?)element.GetValue(DecimalDigitsProperty);
        }

        public static void SetFormatter(this DependencyObject element, IFormatter value)
        {
            element.SetValue(FormatterProperty, value);
        }

        public static IFormatter GetFormatter(this DependencyObject element)
        {
            return (IFormatter)element.GetValue(FormatterProperty);
        }

        public static void SetNumberStylesMapper(this DependencyObject element, ITypeNumberStyles value)
        {
            element.SetValue(NumberStylesMapperProperty, value);
        }

        public static ITypeNumberStyles GetNumberStylesMapper(this DependencyObject element)
        {
            return (ITypeNumberStyles)element.GetValue(NumberStylesMapperProperty);
        }

        public static void SetStringConverters(this DependencyObject element, ITypeStringConverters value)
        {
            element.SetValue(StringConvertersProperty, value);
        }

        public static ITypeStringConverters GetStringConverters(this DependencyObject element)
        {
            return (ITypeStringConverters)element.GetValue(StringConvertersProperty);
        }

        public static void SetStringConverter(this TextBox element, IStringConverter value)
        {
            element.SetValue(StringConverterProperty, value);
        }

        public static IStringConverter GetStringConverter(this TextBox element)
        {
            return (IStringConverter)element.GetValue(StringConverterProperty);
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null && textBox.GetSourceValueType() != null)
            {
                RawValueTracker.Update(textBox);
                textBox.RaiseEvent(FormattingDirtyArgs);
            }
        }

        private static void OnNumberStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.RaiseEvent(FormattingDirtyArgs);
            }
        }

        private static void OnDecimalDigitsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var digits = e.NewValue as int?;
            if (digits != null)
            {
                if (digits < 0)
                {
                    var format = Formats.GetOrAdd(digits.Value, d => String.Format("0.{0}", new string('#', -1 * d)));
                    o.SetStringFormat(format);
                }
                else
                {
                    var format = Formats.GetOrAdd(digits.Value, d => String.Format("F{0}", d));
                    o.SetStringFormat(format);
                }
            }
        }

        private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.RaiseEvent(FormattingDirtyArgs);
            }
        }

        private static void OnStringConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.CoerceValue(MinProperty);
                textBox.CoerceValue(MaxProperty);
                RawValueTracker.Update(textBox);
                textBox.RaiseEvent(FormattingDirtyArgs);
            }
        }

        private static object OnStringConvertersCoerce(DependencyObject d, object basevalue)
        {
            if (basevalue == null)
            {
                return null;
            }
            var type = d.GetSourceValueType();
            if (type != null)
            {
                d.CoerceValue(StringConverterProperty);
            }
            return basevalue;
        }

        private static object OnStringConverterCoerce(DependencyObject d, object basevalue)
        {
            var stringConverters = d.GetStringConverters();
            var type = d.GetSourceValueType();
            if (basevalue == null && stringConverters != null && type != null)
            {
                basevalue = stringConverters.Get(type);
            }
            return basevalue;
        }

        private static object OnCoerceNumberStyles(DependencyObject d, object basevalue)
        {
            if (!Equals(basevalue, NumberStyles.None))
            {
                return basevalue;
            }
            var mapper = d.GetNumberStylesMapper();
            if (mapper == null)
            {
                return NumberStyles.None;
            }
            var type = d.GetSourceValueType();
            if (type == null)
            {
                return NumberStyles.None;
            }
            return mapper.Get(type);
        }
    }
}
