namespace Gu.Wpf.Validation.Internals
{
    using System.ComponentModel;
    using System.Windows;

    public static class DesignMode
    {
        private static readonly DependencyObject Dummy= new DependencyObject();
        public static bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(Dummy);
            }
        }
    }
}
