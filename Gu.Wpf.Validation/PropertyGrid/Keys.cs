namespace Gu.Wpf.Validation
{
    using System.Runtime.CompilerServices;
    using System.Windows;

    public static class Keys
    {
        public static ResourceKey SettingsListStyleKey { get; } = CreateKey();

        public static ResourceKey NestedListStyleKey { get; } = CreateKey();

        public static ResourceKey HeaderStyleKey { get; } = CreateKey();

        public static ResourceKey SettingStyleKey { get; } = CreateKey();

        public static ResourceKey PlainTemplateKey { get; } = CreateKey();

        public static ResourceKey SymbolTemplateKey { get; } = CreateKey();

        public static object HelpTextAndSymbolTemplateKey { get; } = CreateKey();

        public static object SymbolPathStyleKey { get; } = CreateKey();

        public static object HelpErrorTextAndSymbolTemplateKey { get; } = CreateKey();

        public static object HelpTemplateKey { get; } = CreateKey();

        public static ResourceKey SettingStyleSelectorKey { get; } = CreateKey();

        public static ResourceKey ErrorSymbolTemplateSelectorKey { get; } = CreateKey();

        public static ResourceKey CheckmarkGeometryKey { get; } = CreateKey();

        public static ResourceKey ErrorGeometryKey { get; } = CreateKey();

        public static ResourceKey RequiredGeometryKey { get; } = CreateKey();

        public static ResourceKey QuestionGeometryKey { get; } = CreateKey();

        public static object ErrorTextAndSymbolTemplateKey { get; } = CreateKey();

        public static object ErrorTemplateSelectorKey { get; } = CreateKey();

        public static object ErrorTemplateKey { get; } = CreateKey();

        private static ComponentResourceKey CreateKey([CallerMemberName] string caller = null)
        {
            return new ComponentResourceKey(typeof(Keys), caller); ;
        }
    }
}
