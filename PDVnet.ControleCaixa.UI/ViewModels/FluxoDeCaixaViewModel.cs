using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Model.Caixa;
using PDVnet.ControleCaixa.Model.Enums;
using PDVnet.ControleCaixa.UI.Commands;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class FluxoDeCaixaViewModel : BaseViewModel
{
    private readonly MovimentacaoService _movimentacaoService;
    private readonly ConfiguracaoCaixaService _configuracaoCaixaService;
    public ObservableCollection<MovimentacaoCaixa> Movimentacoes { get; } = new();
    

    // ==================== SALDO INICIAL ====================

    private decimal _saldoMinimo = 100m;
    public decimal SaldoMinimo
    {
        get => _saldoMinimo;
        set
        {
            _saldoMinimo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SaldoBaixo));
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
            OnPropertyChanged(nameof(Saldo));
        }
    }

    private bool _editandoSaldoInicial;
    public bool EditandoSaldoInicial
    {
        get => _editandoSaldoInicial;
        set { _editandoSaldoInicial = value; OnPropertyChanged(); }
    }

    // ==================== FILTROS E BUSCA ====================

    private string _textoBusca = string.Empty;
    public string TextoBusca
    {
        get => _textoBusca;
        set
        {
            _textoBusca = value;
            OnPropertyChanged();
            AplicarFiltro();  // Filtra ao digitar
        }
    }

    private int? _filtroTipo = null;  // null = todos, 0 = entrada, 1 = saída
    public int? FiltroTipo
    {
        get => _filtroTipo;
        set
        {
            _filtroTipo = value;
            OnPropertyChanged();
            AplicarFiltro();
        }
    }

    // Lista filtrada (pode ser usada em vez de Movimentacoes na View)
    private ObservableCollection<MovimentacaoCaixa> _movimentacoesFiltradas = new();
    public ObservableCollection<MovimentacaoCaixa> MovimentacoesFiltradas
    {
        get => _movimentacoesFiltradas;
        set { _movimentacoesFiltradas = value; OnPropertyChanged(); }
    }

    // ==================== SELEÇÃO ====================

    private MovimentacaoCaixa? _movimentacaoSelecionada;
    public MovimentacaoCaixa? MovimentacaoSelecionada
    {
        get => _movimentacaoSelecionada;
        set
        {
            _movimentacaoSelecionada = value;
            OnPropertyChanged();
            // Preenche o formulário ao selecionar
            if (value != null) ;
        }
    }

    // ==================== TOTALIZADORES ====================

    public decimal TotalEntradas => Movimentacoes
        .Where(m => m.Tipo == TipoMovimentacao.Entrada)
        .Sum(m => m.Valor);

    public decimal TotalSaidas => Movimentacoes
        .Where(m => m.Tipo == TipoMovimentacao.Saida)
        .Sum(m => m.Valor);

    public decimal Saldo => SaldoInicial + TotalEntradas - TotalSaidas;

    public bool SaldoBaixo => Saldo < SaldoMinimo;


    public ICommand SalvarCommand { get; }
    public ICommand LimparCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CarregarCommand { get; }
    public ICommand FiltrarCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand LimparFiltroCommand { get; }
    public ICommand EditarSaldoInicialCommand { get; }
    public ICommand SalvarSaldoInicialCommand { get; }
    public ICommand CancelarSaldoInicialCommand { get; }


    public FluxoDeCaixaViewModel(MovimentacaoService movimentacaoService, ConfiguracaoCaixaService configuracaoCaixaService)
    {
        _movimentacaoService = movimentacaoService;
        _configuracaoCaixaService = configuracaoCaixaService;

        SalvarCommand = new RelayCommand(async _ => await SalvarAsync());
        //LimparCommand = new RelayCommand(_ => LimparFormulario());
        ExcluirCommand = new RelayCommand(async param => await ExcluirAsync(param), param => param != null);
        CarregarCommand = new RelayCommand(async _ => await CarregarMovimentacoesAsync());
        FiltrarCommand = new RelayCommand(_ => AplicarFiltro());
        LimparFiltroCommand = new RelayCommand(_ => LimparFiltros());
        EditarCommand = new RelayCommand(async param => await EditarAsync(param), param => param != null);

        EditarSaldoInicialCommand = new RelayCommand(_ => EditandoSaldoInicial = true);
        SalvarSaldoInicialCommand = new RelayCommand(async _ => await SalvarSaldoInicialAsync());
        CancelarSaldoInicialCommand = new RelayCommand(async _ => await CancelarEdicaoSaldoInicialAsync());

        _ = InicializarAsync();

        // Carrega automaticamente
        
    }

    private async Task InicializarAsync()
    {
        await CarregarSaldoInicialAsync();
        await CarregarMovimentacoesAsync();

    }

    private async Task CarregarSaldoInicialAsync()
    {
        try
        {
            SaldoInicial = await _configuracaoCaixaService.ObterSaldoInicialAsync();
            SaldoMinimo = await _configuracaoCaixaService.ObterSaldoMinimoAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao carregar configuração: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SalvarSaldoInicialAsync()
    {
        try
        {
            await _configuracaoCaixaService.SalvarConfiguracaoAsync(SaldoInicial, SaldoMinimo);
            EditandoSaldoInicial = false;
            OnPropertyChanged(nameof(Saldo));
            OnPropertyChanged(nameof(SaldoBaixo));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar configuração: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SalvarAsync()
    {
        if (MovimentacaoSelecionada is null) return;

        try
        {
            await _movimentacaoService.SalvarMovimentacaoAsync(MovimentacaoSelecionada);
            await CarregarMovimentacoesAsync();
            MovimentacaoSelecionada = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task CancelarEdicaoSaldoInicialAsync()
    {
        // desfaz alteração não salva, recarregando o valor persistido
        await CarregarSaldoInicialAsync();
        EditandoSaldoInicial = false;
    }
    //private void PreencherFormulario(MovimentacaoCaixa mov)
    //{
    //    Id = mov.Id;
    //    Tipo = mov.Tipo;
    //    Valor = mov.Valor;
    //    DataMovimento = mov.DataMovimento;
    //    Descricao = mov.Descricao;
    //    CategoriaSelecionada = mov.Categoria;
    //    Status = mov.Status;
    //}

    private async Task CarregarMovimentacoesAsync()
    {
        try
        {
            var lista = await _movimentacaoService.ListarMovimentacoesAsync();

            Movimentacoes.Clear();
            foreach (var item in lista.OrderByDescending(m => m.DataMovimento))
            {
                Movimentacoes.Add(item);
            }

            AplicarFiltro();
            AtualizarTotalizadores();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao carregar: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void AplicarFiltro()
    {
        var filtrada = Movimentacoes.AsEnumerable();

        // Filtro por texto (descrição ou categoria)
        if (!string.IsNullOrWhiteSpace(TextoBusca))
        {
            var busca = TextoBusca.ToLower();
            filtrada = filtrada.Where(m =>
                m.Descricao.ToLower().Contains(busca) ||
                m.Categoria.ToLower().Contains(busca));
        }

        // Filtro por tipo
        if (FiltroTipo.HasValue)
        {
            filtrada = filtrada.Where(m =>
             m.Tipo == (TipoMovimentacao)FiltroTipo.Value);
        }

        MovimentacoesFiltradas = new ObservableCollection<MovimentacaoCaixa>(filtrada);
    }

    private async Task ExcluirAsync(object? param)
    {
        if (param is not MovimentacaoCaixa mov) return;

        var resultado = MessageBox.Show(
            $"Excluir '{mov.Descricao}'?",
            "Confirmar",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (resultado != MessageBoxResult.Yes) return;

        try
        {
            await _movimentacaoService.ExcluirMovimentacaoAsync(mov.Id);
            MessageBox.Show("Excluído!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            await CarregarMovimentacoesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task EditarAsync(object? param)
    {
        if (param is MovimentacaoCaixa mov)
        {
            MovimentacaoSelecionada = new MovimentacaoCaixa
            {
                Id = mov.Id,
                Tipo = mov.Tipo,
                Descricao = mov.Descricao,
                Valor = mov.Valor,
                DataMovimento = mov.DataMovimento,
                Categoria = mov.Categoria,
                Status = mov.Status
            };
        }
    }

    private void LimparFiltros()
    {
        TextoBusca = string.Empty;
        FiltroTipo = null;
        AplicarFiltro();
    }

    //private void PreencherFormulario(MovimentacaoCaixa mov)
    //{
    //    Id = mov.Id;
    //    Descricao = mov.Descricao;
    //    Valor = mov.Valor;
    //    DataMovimento = mov.DataMovimento;
    //    CategoriaSelecionada = mov.Categoria;
    //    Status = mov.Status;


    //}

    private void AtualizarTotalizadores()
    {
        OnPropertyChanged(nameof(TotalEntradas));
        OnPropertyChanged(nameof(TotalSaidas));
        OnPropertyChanged(nameof(Saldo));
        OnPropertyChanged(nameof(SaldoBaixo));
    }
}
