# Gu.Wpf.Validation

A set of attached properties for TextBox.

Sample:

```
<TextBox validation:Input.Value="{Binding DoubleValue}"
         validation:Input.NumberStyles="AllowDecimalPoint,AllowLeadingSign,AllowLeadingWhite"
         validation:Input.Min="-10"
         validation:Input.Max="10"
         validation:Input.DecimalDigits="2"
         validation:Input.ValidationTrigger="PropertyChanged" />

<TextBox validation:Input.Value="{Binding StringValue}"
         validation:Input.Pattern="a\d+"
         validation:Input.IsRequired="True"
         validation:Input.ValidationTrigger="PropertyChanged" />
```
For more samples see demo project.

- Creates a binding Text > validation:Input.Value internally. The binding has validationrules so it uses standard WPF validation.
- No custom controls.
- Higly configurable.
  - IValidator, plug in a validator of your own.
  - ITypeNumberStyles, plug in default numberstyles of choice.
  - ITypeRules, plug in default validationrules of choice.
  - ITypeStringConverters, plug in custom handling of to/from string.
  - All ^ can of course be bound per instance.
- Validates as you type or on lost focus.
