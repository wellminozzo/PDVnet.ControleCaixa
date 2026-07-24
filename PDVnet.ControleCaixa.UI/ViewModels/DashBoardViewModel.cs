using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Model.Caixa;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class DashBoardViewModel : BaseViewModel
{
    private readonly MovimentacaoService _movimentacaoService;

    private decimal _entradasHoje;
    public decimal EntradasHoje
    {
        get => _entradasHoje;
        set
        {
            _entradasHoje = value;
            OnPropertyChanged();
        }
    }

    private decimal _saidasHoje;
    public decimal SaidasHoje
    {
        get => _saidasHoje;
        set
        {
            _saidasHoje = value;
            OnPropertyChanged();
        }
    }

    private decimal _saldoTotal;
    public decimal SaldoTotal
    {
        get => _saldoTotal;
        set
        {
            _saldoTotal = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<MovimentacaoCaixa> UltimasMovimentacoes { get; set; }
        = new();

    public DashBoardViewModel(MovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
        _ = CarregarResumoAsync();
    }

    public async Task CarregarResumoAsync()
    {
        EntradasHoje = await _movimentacaoService.ObterTotalEntradasHojeAsync();
        SaidasHoje = await _movimentacaoService.ObterTotalSaidasHojeAsync();
        SaldoTotal = await _movimentacaoService.ObterSaldoTotalAsync();
        await ObterUltimosCinco();

    }

    private async Task CarregarAsync()
    {
        await ObterUltimosCinco();
    }

    public async Task ObterUltimosCinco()
    {
        var movimentacoes = await _movimentacaoService.ObterUltimasMovimentacoesAsync();

        Debug.WriteLine($"Quantidade: {movimentacoes.Count}");

        UltimasMovimentacoes.Clear();

        foreach (var item in movimentacoes)
        {
            UltimasMovimentacoes.Add(item);
        }
    }

    
}
