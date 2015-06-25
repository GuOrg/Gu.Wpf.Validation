namespace Gu.Wpf.Validation.Internals
{
    using System.ComponentModel;
    using System.Windows;

    public static class DesignMode
    {
        private static readonly DependencyObject Dummy = new DependencyObject();
        internal static bool? IsInDesignModeForTests = null;

        public static bool IsInDesignMode
        {
            get { return IsInDesignModeForTests ?? DesignerProperties.GetIsInDesignMode(Dummy); }
        }
    }
}
