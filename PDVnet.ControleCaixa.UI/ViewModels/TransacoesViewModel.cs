using PDVnet.ControleCaixa.Model.Caixa;
using System.Collections.ObjectModel;
using PDVnet.ControleCaixa.Business.Services;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class TransacoesViewModel : BaseViewModel
{
    private MovimentacaoService _service;

    public ObservableCollection<MovimentacaoCaixa> Movimentacoes { get; set; }


    public TransacoesViewModel(MovimentacaoService service)
    {
        _service = service;
        Movimentacoes = new ObservableCollection<MovimentacaoCaixa>();
    }

    private void AddMovimentacao(MovimentacaoCaixa movimentacao)
    {
        _service.AddMovimentacao(movimentacao);
    }

}


