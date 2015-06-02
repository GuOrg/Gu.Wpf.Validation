namespace Gu.Wpf.Validation
{
    using System.Windows.Controls;

    public interface IFormatter
    {
        void Bind(TextBox textBox);
    }
}