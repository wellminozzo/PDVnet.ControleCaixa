using System;
using System.Collections.Generic;
using System.Text;
using PDVnet.ControleCaixa.Model.Caixa;
using PDVnet.ControleCaixa.Data.Repositories;

namespace PDVnet.ControleCaixa.Business.Services;

public class MovimentacaoService
{
    private readonly MovimentacaoRepository _repository;

    public MovimentacaoService(MovimentacaoRepository repository)
    {
        _repository = repository;
    }

    public Task<List<MovimentacaoCaixa>> ListarMovimentacoesAsync() => _repository.GetAllAsync();

    public Task<MovimentacaoCaixa?> ObterMovimentacaoAsync(int id) => _repository.GetByIdAsync(id);

    public async Task SalvarMovimentacaoAsync(MovimentacaoCaixa movimentacao)
    {
        

        if (movimentacao.Id == 0)
        {
            await _repository.AddAsync(movimentacao);
        }
        else
        {
            await _repository.UpdateAsync(movimentacao);
        }
    }

    public Task ExcluirMovimentacaoAsync(int id) => _repository.DeleteAsync(id);
    
}
