using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Model.Caixa;
using PDVnet.ControleCaixa.UI.Commands;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class FluxoDeCaixaViewModel : BaseViewModel
{
    private readonly MovimentacaoService _movimentacaoService;
    public ObservableCollection<MovimentacaoCaixa> Movimentacoes { get; } = new();

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
        .Where(m => m.Tipo == 0)
        .Sum(m => m.Valor);

    public decimal TotalSaidas => Movimentacoes
        .Where(m => m.Tipo == 1)
        .Sum(m => m.Valor);

    public decimal Saldo => TotalEntradas - TotalSaidas;

    public ICommand SalvarCommand { get; }
    public ICommand LimparCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CarregarCommand { get; }
    public ICommand FiltrarCommand { get; }
    public ICommand LimparFiltroCommand { get; }

    public FluxoDeCaixaViewModel(MovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;

       // SalvarCommand = new RelayCommand(async _ => await SalvarAsync(), _ => PodeSalvar());
        //LimparCommand = new RelayCommand(_ => LimparFormulario());
        ExcluirCommand = new RelayCommand(async param => await ExcluirAsync(param), param => param != null);
        CarregarCommand = new RelayCommand(async _ => await CarregarMovimentacoesAsync());
        FiltrarCommand = new RelayCommand(_ => AplicarFiltro());
        LimparFiltroCommand = new RelayCommand(_ => LimparFiltros());

        // Carrega automaticamente
        _ = CarregarMovimentacoesAsync();
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
            filtrada = filtrada.Where(m => m.Tipo == FiltroTipo.Value);
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

    private void LimparFiltros()
    {
        TextoBusca = string.Empty;
        FiltroTipo = null;
        AplicarFiltro();
    }

    private void AtualizarTotalizadores()
    {
        OnPropertyChanged(nameof(TotalEntradas));
        OnPropertyChanged(nameof(TotalSaidas));
        OnPropertyChanged(nameof(Saldo));
    }
}
