namespace Gu.Wpf.Validation
{
    using System.Windows.Controls;

    public interface IValidator
    {
        void Bind(System.Windows.Controls.TextBox textBox);
    }
}