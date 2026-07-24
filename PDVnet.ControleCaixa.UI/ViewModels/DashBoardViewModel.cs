using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Model.Caixa;
using System.Collections.ObjectModel;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class DashBoardViewModel : BaseViewModel
{
    private readonly MovimentacaoService _movimentacaoService;

    public ObservableCollection<MovimentacaoCaixa> UltimasMovimentacoes { get; set; }
        = new();

    public DashBoardViewModel(MovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    public async Task ObterUltimosCinco()
    {
        var movimentacoes = await _movimentacaoService.ObterUltimasMovimentacoesAsync();

        UltimasMovimentacoes.Clear();

        foreach (var item in movimentacoes)
        {
            UltimasMovimentacoes.Add(item);
        }
    }
}
