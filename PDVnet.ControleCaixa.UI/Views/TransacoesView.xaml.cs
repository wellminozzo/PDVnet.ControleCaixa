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
        e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]*$");
    }
}
