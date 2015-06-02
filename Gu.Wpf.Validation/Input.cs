namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Internals;
    using StringConverters;

    public static class Input
    {
        /// <summary>
        /// This is used to get a notification when Value is bound even if the value is null.
        /// </summary>
        private static readonly object Unset = new object();
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(object),
            typeof(Input),
            new FrameworkPropertyMetadata(Unset, OnValueChanged, OnValueCoerce)
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus
            });

        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(IFormatProvider),
            typeof(Input),
            new FrameworkPropertyMetadata(
                CultureInfo.GetCultureInfo("en-US"), // Think this is the default in WPF
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty NumberStylesProperty = DependencyProperty.RegisterAttached(
            "NumberStyles",
            typeof(NumberStyles),
            typeof(Input),
            new FrameworkPropertyMetadata(NumberStyles.None, FrameworkPropertyMetadataOptions.Inherits, null, OnCoerceNumberStyles));

        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.RegisterAttached(
            "DecimalDigits",
            typeof(int?),
            typeof(Input),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.RegisterAttached(
            "IsRequired",
            typeof(bool),
            typeof(Input),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PatternProperty = DependencyProperty.RegisterAttached(
            "Pattern",
            typeof(string),
            typeof(Input),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty MinProperty = DependencyProperty.RegisterAttached(
            "Min",
            typeof(object),
            typeof(Input),
            new PropertyMetadata(default(object), null, OnMinCoerce));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.RegisterAttached(
            "Max",
            typeof(object),
            typeof(Input),
            new PropertyMetadata(default(object), null, OnMaxCoerce));

        public static readonly DependencyProperty ValidationTriggerProperty = DependencyProperty.RegisterAttached(
            "ValidationTrigger", 
            typeof (UpdateSourceTrigger), 
            typeof (Input), 
            new FrameworkPropertyMetadata(UpdateSourceTrigger.PropertyChanged, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidatorProperty = DependencyProperty.RegisterAttached(
            "Validator",
            typeof(IValidator),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultValidator(), FrameworkPropertyMetadataOptions.Inherits));

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

        public static readonly DependencyProperty TypeRulesProperty = DependencyProperty.RegisterAttached(
            "TypeRules",
            typeof(ITypeRules),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultRules(), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidationRulesProperty = DependencyProperty.RegisterAttached(
            "ValidationRules",
            typeof(IReadOnlyCollection<ValidationRule>),
            typeof(Input),
            new PropertyMetadata(default(IReadOnlyCollection<ValidationRule>), null, OnValidationRulesCoerce));

        private static readonly DependencyPropertyKey SourceValueTypePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "SourceValueType",
            typeof(Type),
            typeof(Input),
            new PropertyMetadata(default(Type), OnSourceValueTypeChanged, OnSourceValueTypeCoerce));

        internal static readonly DependencyProperty SourceValueTypeProperty = SourceValueTypePropertyKey.DependencyProperty;

        public static void SetValue(this TextBox element, object value)
        {
            element.SetValue(ValueProperty, value);
        }

        public static object GetValue(this TextBox element)
        {
            return element.GetValue(ValueProperty);
        }

        public static void SetCulture(this DependencyObject element, IFormatProvider value)
        {
            element.SetValue(CultureProperty, value);
        }

        public static IFormatProvider GetCulture(this DependencyObject element)
        {
            return (IFormatProvider)element.GetValue(CultureProperty);
        }

        public static void SetNumberStyles(this DependencyObject element, NumberStyles value)
        {
            element.SetValue(NumberStylesProperty, value);
        }

        public static NumberStyles GetNumberStyles(this DependencyObject element)
        {
            return (NumberStyles)element.GetValue(NumberStylesProperty);
        }

        public static void SetDecimalDigits(this DependencyObject element, int? value)
        {
            element.SetValue(DecimalDigitsProperty, value);
        }

        public static int? GetDecimalDigits(this DependencyObject element)
        {
            return (int?)element.GetValue(DecimalDigitsProperty);
        }

        public static void SetIsRequired(this DependencyObject element, bool value)
        {
            element.SetValue(IsRequiredProperty, value);
        }

        public static bool GetIsRequired(this DependencyObject element)
        {
            return (bool)element.GetValue(IsRequiredProperty);
        }

        public static void SetPattern(this DependencyObject element, string value)
        {
            element.SetValue(PatternProperty, value);
        }

        public static string GetPattern(this DependencyObject element)
        {
            return (string)element.GetValue(PatternProperty);
        }

        public static void SetMin(this TextBox element, object value)
        {
            element.SetValue(MinProperty, value);
        }

        public static object GetMin(this DependencyObject element)
        {
            return element.GetValue(MinProperty);
        }

        public static void SetMax(this DependencyObject element, object value)
        {
            element.SetValue(MaxProperty, value);
        }

        public static object GetMax(this DependencyObject element)
        {
            return (object)element.GetValue(MaxProperty);
        }

        public static void SetValidationTrigger(this DependencyObject element, UpdateSourceTrigger value)
        {
            element.SetValue(ValidationTriggerProperty, value);
        }

        public static UpdateSourceTrigger GetValidationTrigger(this DependencyObject element)
        {
            return (UpdateSourceTrigger)element.GetValue(ValidationTriggerProperty);
        }

        public static void SetValidator(this DependencyObject element, IValidator value)
        {
            element.SetValue(ValidatorProperty, value);
        }

        public static IValidator GetValidator(this DependencyObject element)
        {
            return (IValidator)element.GetValue(ValidatorProperty);
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

        public static void SetTypeRules(this DependencyObject element, ITypeRules value)
        {
            element.SetValue(TypeRulesProperty, value);
        }

        public static ITypeRules GetTypeRules(this DependencyObject element)
        {
            return (ITypeRules)element.GetValue(TypeRulesProperty);
        }

        public static void SetValidationRules(this TextBox element, IReadOnlyCollection<ValidationRule> value)
        {
            element.SetValue(ValidationRulesProperty, value);
        }

        public static IReadOnlyCollection<ValidationRule> GetValidationRules(this DependencyObject element)
        {
            return (IReadOnlyCollection<ValidationRule>)element.GetValue(ValidationRulesProperty);
        }

        internal static void SetSourceValueType(this DependencyObject element, Type value)
        {
            element.SetValue(SourceValueTypePropertyKey, value);
        }

        internal static Type GetSourceValueType(this DependencyObject element)
        {
            return (Type)element.GetValue(SourceValueTypeProperty);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null)
            {
                return;
            }

            if (e.OldValue == Unset)
            {
                if (textBox.IsLoaded)
                {
                    Setup(textBox);
                }
                else
                {
                    textBox.Loaded += OnLoaded;
                }
            }
        }

        private static void OnSourceValueTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                d.CoerceValue(NumberStylesProperty);
                d.CoerceValue(StringConverterProperty);
                d.CoerceValue(ValidationRulesProperty);
            }
        }

        private static void OnStringConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MinProperty);
            d.CoerceValue(MaxProperty);
        }

        private static object OnValueCoerce(DependencyObject d, object basevalue)
        {
            if (d.GetSourceValueType() == null && basevalue != Unset)
            {
                d.CoerceValue(SourceValueTypeProperty);
            }
            return basevalue;
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

        private static object OnSourceValueTypeCoerce(DependencyObject d, object basevalue)
        {
            if (basevalue == null)
            {
                var expression = BindingOperations.GetBindingExpression(d, ValueProperty);
                var sourceValueType = expression.GetSourceValueType();
                return sourceValueType;
            }
            return basevalue;
        }

        private static object OnValidationRulesCoerce(DependencyObject d, object basevalue)
        {
            if (basevalue != null)
            {
                return basevalue;
            }
            var typeRules = d.GetTypeRules();
            var type = d.GetSourceValueType();
            if (typeRules != null && type != null)
            {
                return typeRules.Get(type);
            }
            return null;
        }

        private static object OnMinCoerce(DependencyObject d, object basevalue)
        {
            return CoerceMinMax(d, basevalue);
        }

        private static object OnMaxCoerce(DependencyObject d, object basevalue)
        {
            return CoerceMinMax(d, basevalue);
        }

        private static object CoerceMinMax(DependencyObject d, object basevalue)
        {
            var s = basevalue as string;
            if (s == null)
            {
                return basevalue;
            }
            var textBox = (TextBox)d;
            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return basevalue;
            }
            object result;
            converter.TryParse(basevalue, textBox, out result);
            return result;
        }

        private static void OnLoaded(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Loaded -= OnLoaded;
            Setup(textBox);
        }

        private static void Setup(TextBox textBox)
        {
            var validator = textBox.GetValidator();
            validator.Bind(textBox);
        }
    }
}