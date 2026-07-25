using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Model.Caixa;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class DashBoardViewModel : BaseViewModel
{
    private readonly MovimentacaoService _movimentacaoService;
    private readonly ConfiguracaoCaixaService _configuracaoCaixaService;

    private int _totalMovimentacoes;
    public int TotalMovimentacoes
    {
        get => _totalMovimentacoes;
        set { _totalMovimentacoes = value; OnPropertyChanged(nameof(TotalMovimentacoes)); }
    }

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

    private decimal _saldoInicial;
    public decimal SaldoInicial
    {
        get => _saldoInicial;
        set
        {
            _saldoInicial = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MovimentacaoCaixa.Valor));
        }
    }

    public ObservableCollection<MovimentacaoCaixa> UltimasMovimentacoes { get; set; }
        = new();
    public decimal Saldo => SaldoInicial + EntradasHoje - SaidasHoje;
    public DashBoardViewModel(MovimentacaoService movimentacaoService, ConfiguracaoCaixaService configuracaoCaixaService)
    {
        _movimentacaoService = movimentacaoService;
        _configuracaoCaixaService = configuracaoCaixaService;
        _ = CarregarResumoAsync();
    }

    public async Task CarregarResumoAsync()
    {
        EntradasHoje = await _movimentacaoService.ObterTotalEntradasHojeAsync();
        SaidasHoje = await _movimentacaoService.ObterTotalSaidasHojeAsync();
        SaldoTotal = await _movimentacaoService.ObterSaldoTotalAsync();
        await ObterUltimosCinco();
        await CarregarSaldoInicialAsync();
        await CarregarTotalAsync();
        

    }

    private async Task CarregarTotalAsync()
    {
        var todas = await _movimentacaoService.ListarMovimentacoesAsync();
        TotalMovimentacoes = todas.Count;
    }

    private async Task CarregarSaldoInicialAsync()
    {
        
        SaldoInicial = await _configuracaoCaixaService.ObterSaldoInicialAsync();
             
        
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
