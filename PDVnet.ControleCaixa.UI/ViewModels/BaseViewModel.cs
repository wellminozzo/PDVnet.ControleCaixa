using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<string> Categorias { get; } = new()
    {
        "Vendas",
        "Despesas Operacionais",
        "Salários",
        "Impostos",
        "Suprimentos",
        "Outros"
    };
}
