namespace Gu.Wpf.Validation
{
    using System.Windows.Controls;

    public interface IValidator
    {
        void Bind(TextBox textBox);
    }
}