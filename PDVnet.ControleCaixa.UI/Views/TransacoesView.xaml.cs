using PDVnet.ControleCaixa.UI.ViewModels;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDVnet.ControleCaixa.UI.Views;

/// <summary>
/// Interação lógica para TransacoesView.xam
/// </summary>
public partial class TransacoesView : UserControl
{
    public TransacoesView()
    {
        InitializeComponent();
       
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void NumeroTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text)) return;

        var textBox = sender as TextBox;
        if (textBox is null) return;

        var novoTexto = textBox.Text[..textBox.SelectionStart] + e.Text + textBox.Text[(textBox.SelectionStart + textBox.SelectionLength)..];

        if (!decimal.TryParse(novoTexto, out _))
        {
            var textoLimpo = novoTexto;
            if (textoLimpo.EndsWith(',') || textoLimpo.EndsWith('.'))
                textoLimpo += '0';

            if (!decimal.TryParse(textoLimpo, out _))
            {
                e.Handled = true;
                return;
            }
        }

        if (novoTexto.Count(c => c == ',' || c == '.') > 1)
        {
            e.Handled = true;
            return;
        }

        if (novoTexto.Replace(",", "").Replace(".", "").Length > 18)
        {
            e.Handled = true;
            return;
        }
    }
}
