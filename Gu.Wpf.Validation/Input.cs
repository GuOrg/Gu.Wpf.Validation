namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.StringConverters;

    public static partial class Input
    {
        public static readonly RoutedEvent ValidationDirtyEvent = EventManager.RegisterRoutedEvent(
            "ValidationDirty",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(Input));

        /// <summary>
        /// This is used to get a notification when Value is bound even if the value is null.
        /// </summary>
        private static readonly object Unset = "Input.Unset";
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(object),
            typeof(Input),
            new FrameworkPropertyMetadata(Unset, OnValueChanged)
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
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

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
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty MinProperty = DependencyProperty.RegisterAttached(
            "Min",
            typeof(object),
            typeof(Input),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.Inherits, null, OnMinCoerce));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.RegisterAttached(
            "Max",
            typeof(object),
            typeof(Input),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.Inherits, null, OnMaxCoerce));

        public static readonly DependencyProperty ValidationTriggerProperty = DependencyProperty.RegisterAttached(
            "ValidationTrigger",
            typeof(UpdateSourceTrigger),
            typeof(Input),
            new FrameworkPropertyMetadata(UpdateSourceTrigger.PropertyChanged, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty OnValidationErrorStrategyProperty = DependencyProperty.RegisterAttached(
            "OnValidationErrorStrategy",
            typeof(OnValidationErrorStrategy),
            typeof(Input),
            new FrameworkPropertyMetadata(OnValidationErrorStrategy.Default, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidatorProperty = DependencyProperty.RegisterAttached(
            "Validator",
            typeof(IValidator),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultValidator(), FrameworkPropertyMetadataOptions.Inherits));

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

        public static readonly DependencyProperty TypeRulesProperty = DependencyProperty.RegisterAttached(
            "TypeRules",
            typeof(ITypeRules),
            typeof(Input),
            new FrameworkPropertyMetadata(new DefaultRules(), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidationRulesProperty = DependencyProperty.RegisterAttached(
            "ValidationRules",
            typeof(IReadOnlyCollection<ValidationRule>),
            typeof(Input),
            new FrameworkPropertyMetadata(
                default(IReadOnlyCollection<ValidationRule>),
                FrameworkPropertyMetadataOptions.NotDataBindable,
                null, OnValidationRulesCoerce));

        public static readonly DependencyProperty SourceValueTypeProperty = DependencyProperty.RegisterAttached(
            "SourceValueType",
            typeof(Type),
            typeof(Input),
            new PropertyMetadata(default(Type), OnSourceValueTypeChanged, OnSourceValueTypeCoerce));

        public static void SetValue(this TextBox element, object value)
        {
            element.SetValue(ValueProperty, value);
        }

        public static void AddValidationDirtyHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(ValidationDirtyEvent, handler);
        }

        public static void RemoveValidationDirtyHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ValidationDirtyEvent, handler);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static object GetValue(this TextBox element)
        {
            return element.GetValue(ValueProperty);
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

        public static void SetIsRequired(this DependencyObject element, bool value)
        {
            element.SetValue(IsRequiredProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsRequired(this DependencyObject element)
        {
            return (bool)element.GetValue(IsRequiredProperty);
        }

        public static void SetPattern(this DependencyObject element, string value)
        {
            element.SetValue(PatternProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetPattern(this DependencyObject element)
        {
            return (string)element.GetValue(PatternProperty);
        }

        public static void SetMin(this TextBox element, object value)
        {
            element.SetValue(MinProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static object GetMin(this DependencyObject element)
        {
            return element.GetValue(MinProperty);
        }

        public static void SetMax(this DependencyObject element, object value)
        {
            element.SetValue(MaxProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static object GetMax(this DependencyObject element)
        {
            return (object)element.GetValue(MaxProperty);
        }

        public static void SetValidationTrigger(this DependencyObject element, UpdateSourceTrigger value)
        {
            element.SetValue(ValidationTriggerProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static UpdateSourceTrigger GetValidationTrigger(this DependencyObject element)
        {
            return (UpdateSourceTrigger)element.GetValue(ValidationTriggerProperty);
        }

        public static void SetOnValidationErrorStrategy(this DependencyObject element, OnValidationErrorStrategy value)
        {
            element.SetValue(OnValidationErrorStrategyProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static OnValidationErrorStrategy GetOnValidationErrorStrategy(this DependencyObject element)
        {
            return (OnValidationErrorStrategy)element.GetValue(OnValidationErrorStrategyProperty);
        }

        public static void SetValidator(this DependencyObject element, IValidator value)
        {
            element.SetValue(ValidatorProperty, value);
        }

        public static IValidator GetValidator(this DependencyObject element)
        {
            return (IValidator)element.GetValue(ValidatorProperty);
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

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static IReadOnlyCollection<ValidationRule> GetValidationRules(this DependencyObject element)
        {
            return (IReadOnlyCollection<ValidationRule>)element.GetValue(ValidationRulesProperty);
        }

        public static void SetSourceValueType(this DependencyObject element, Type value)
        {
            element.SetValue(SourceValueTypeProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static Type GetSourceValueType(this DependencyObject element)
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
            //if (e.NewValue != null)
            //{
            //    var sourceValueType = textBox.GetSourceValueType();
            //    var type = e.NewValue.GetType();
            //    if (sourceValueType == null)
            //    {
            //        textBox.SetSourceValueType(type);
            //    }

            //    else if (sourceValueType != type)
            //    {
            //        if (sourceValueType.IsNullable())
            //        {
            //            if (Nullable.GetUnderlyingType(sourceValueType) != type)
            //            {
            //                textBox.SetSourceValueType(type);
            //            }
            //        }
            //        else
            //        {
            //            textBox.SetSourceValueType(type);
            //        }
            //    }
            //}
            if (textBox.GetSourceValueType() == null)
            {
                var expression = BindingOperations.GetBindingExpression(textBox, ValueProperty);
                if (expression != null)
                {
                    var converter = expression.ParentBinding.Converter;
                    if (converter != null)
                    {
                        if (DesignMode.IsInDesignMode)
                        {
                            var message = string.Format("Cannot figure out SourceValueType when binding with converter tat can produce null.{0}Provide explicit SourceValueType", Environment.NewLine);
                            throw new ArgumentException(message);
                        }
                    }
                    else
                    {
                        var fromBinding = expression.GetSourceValueType();
                        textBox.SetSourceValueType(fromBinding);
                    }
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
                var textBox = (TextBox)d;
                Setup(textBox); // rebind
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
            }
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null && textBox.GetSourceValueType() != null)
            {
                RawValueTracker.Update(textBox);
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
            var textBox = d as TextBox;
            {
                if (textBox == null)
                {
                    return basevalue;
                }
            }
            var s = basevalue as string;
            if (s == null)
            {
                return basevalue;
            }

            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return basevalue;
            }
            object result;
            converter.TryParse(basevalue, textBox, out result);
            return result;
        }

        private static void Setup(TextBox textBox)
        {
            var validator = textBox.GetValidator();
            if (validator != null)
            {
                validator.Bind(textBox);
            }
            var formatter = textBox.GetFormatter();
            if (formatter != null)
            {
                formatter.Bind(textBox);
            }
        }
    }
}