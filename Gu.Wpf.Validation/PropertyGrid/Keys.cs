namespace Gu.Wpf.Validation
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public static class Keys
    {
        private static readonly Dictionary<string, ComponentResourceKey> Cache = new Dictionary<string, ComponentResourceKey>();

        public static ResourceKey SettingsListStyleKey { get { return Get(); } }

        public static ResourceKey NestedListStyleKey { get { return Get(); } }

        public static ResourceKey HeaderStyleKey { get { return Get(); } }

        public static ResourceKey SettingStyleKey { get { return Get(); } }
        
        public static ResourceKey PlainTemplateKey { get { return Get(); } }
        
        public static ResourceKey SymbolTemplateKey { get { return Get(); } }
        
        public static object HelpTextAndSymbolTemplateKey { get { return Get(); } }
        
        public static object SymbolPathStyleKey { get { return Get(); } }
        
        public static object HelpErrorTextAndSymbolTemplateKey { get { return Get(); } }
        
        public static object HelpTemplateKey { get { return Get(); } }

        public static ResourceKey SettingStyleSelectorKey { get { return Get(); } }

        public static ResourceKey ErrorSymbolTemplateSelectorKey { get { return Get(); } }
        
        public static ResourceKey CheckmarkGeometryKey { get { return Get(); } }
        
        public static ResourceKey ErrorGeometryKey { get { return Get(); } }
        
        public static ResourceKey RequiredGeometryKey { get { return Get(); } }
        
        public static ResourceKey QuestionGeometryKey { get { return Get(); } }
        
        public static object ErrorTextAndSymbolTemplateKey { get { return Get(); } }

        public static object ErrorTemplateSelectorKey { get { return Get(); } }
        
        public static object ErrorTemplateKey { get { return Get(); } }

        private static ComponentResourceKey Get([CallerMemberName] string caller = null)
        {
            ComponentResourceKey key;
            if (!Cache.TryGetValue(caller, out key))
            {
                key = new ComponentResourceKey(typeof(Keys), caller);
                Cache.Add(caller, key);
            }
            return key;
        }
    }
}
