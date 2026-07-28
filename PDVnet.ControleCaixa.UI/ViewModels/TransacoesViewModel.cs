using Microsoft.Data.SqlClient;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Model.Caixa;
using PDVnet.ControleCaixa.Model.Enums;
using PDVnet.ControleCaixa.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class TransacoesViewModel : BaseViewModel
{
    private readonly MovimentacaoService _movimentacaoService;

    // ==================== PROPRIEDADES DO FORMULÁRIO ====================

    private int _id;
    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    private TipoMovimentacao _tipo = TipoMovimentacao.Entrada;
    public TipoMovimentacao Tipo
    {
        get => _tipo;
        set
        {
            _tipo = value;
            OnPropertyChanged();
        }
    }

    private decimal _valor;
    public decimal Valor
    {
        get => _valor;
        set { _valor = value; OnPropertyChanged(); }
    }

    private DateTime _dataMovimento = DateTime.Today;
    public DateTime DataMovimento
    {
        get => _dataMovimento;
        set { _dataMovimento = value; OnPropertyChanged(); }
    }

    private string _descricao = string.Empty;
    public string Descricao
    {
        get => _descricao;
        set { _descricao = value; OnPropertyChanged(); }
    }

    private string _categoria = string.Empty;
    public string CategoriaSelecionada
    {
        get => _categoria;
        set { _categoria = value; OnPropertyChanged(); }
    }

    private SituacaoStatus _status = SituacaoStatus.Ativo;
    public SituacaoStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }

    // ==================== LISTAS E SELEÇÃO ====================

    public ObservableCollection<MovimentacaoCaixa> Movimentacoes { get; } = new();
    

    private MovimentacaoCaixa? _movimentacaoSelecionada;
    public MovimentacaoCaixa? MovimentacaoSelecionada
    {
        get => _movimentacaoSelecionada;
        set
        {
            _movimentacaoSelecionada = value;
            OnPropertyChanged();
            if (value != null) PreencherFormulario(value);
        }
    }

    // ==================== COMANDOS ====================

    public ICommand SalvarCommand { get; }
    public ICommand LimparCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CarregarCommand { get; }

    // ==================== CONSTRUTOR ====================

    public TransacoesViewModel(MovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;

        SalvarCommand = new RelayCommand(async _ => await SalvarAsync(), _ => PodeSalvar());
        LimparCommand = new RelayCommand(_ => LimparFormulario());
        EditarCommand = new RelayCommand(async param => await EditarAsync(param), param => param != null);
        ExcluirCommand = new RelayCommand(async param => await ExcluirAsync(param), param => param != null);
        CarregarCommand = new RelayCommand(async _ => await CarregarMovimentacoesAsync());

        // Carrega automaticamente ao iniciar
        _ = CarregarMovimentacoesAsync();
    }

    // ==================== MÉTODOS PRIVADOS ====================

    private bool PodeSalvar()
    {
        return !string.IsNullOrWhiteSpace(Descricao) && Valor > 0;
    }

    private void PreencherFormulario(MovimentacaoCaixa mov)
    {
        Id = mov.Id;
        Descricao = mov.Descricao;
        Valor = mov.Valor;
        DataMovimento = mov.DataMovimento;
        CategoriaSelecionada = mov.Categoria;
        Status = mov.Status;

        
    }

    private void LimparFormulario()
    {
        Id = 0;
        
        Valor = 0;
        DataMovimento = DateTime.Today;
        Descricao = string.Empty;
        CategoriaSelecionada = string.Empty;
        Status = 0;
        MovimentacaoSelecionada = null;
    }

    // ==================== OPERAÇÕES CRUD ====================

    private async Task CarregarMovimentacoesAsync()
    {
        try
        {
            var lista = await _movimentacaoService.ListarMovimentacoesAsync();
            Movimentacoes.Clear();
            foreach (var item in lista)
            {
                Movimentacoes.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao carregar: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SalvarAsync()
    {
        try
        {
            var movimentacao = new MovimentacaoCaixa
            {
                Descricao = Descricao,
                Tipo = Tipo,
                Valor = Valor,
                DataMovimento = DateTime.Now,
                Categoria = CategoriaSelecionada,
                Status = Status
            };

            await _movimentacaoService.SalvarMovimentacaoAsync(movimentacao);

            var mensagem = Id == 0 ? "Lançamento criado com sucesso!" : "Lançamento atualizado com sucesso!";
            MessageBox.Show(mensagem, "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();
            await CarregarMovimentacoesAsync();
        }
        catch (SqlException sqlEx)
        {
            MessageBox.Show($"Erro no banco de dados:\n{sqlEx.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task EditarAsync(object? param)
    {
        if (param is MovimentacaoCaixa mov)
        {
            PreencherFormulario(mov);
        }
    }

    private async Task ExcluirAsync(object? param)
    {
        if (param is not MovimentacaoCaixa mov) return;

        var resultado = MessageBox.Show(
            $"Deseja realmente excluir '{mov.Descricao}'?",
            "Confirmar Exclusão",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (resultado != MessageBoxResult.Yes) return;

        try
        {
            await _movimentacaoService.ExcluirMovimentacaoAsync(mov.Id);
            MessageBox.Show("Excluído com sucesso!", "Sucesso",
                MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();
            await CarregarMovimentacoesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


