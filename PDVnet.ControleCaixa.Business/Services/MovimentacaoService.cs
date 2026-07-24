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

    public async Task<List<MovimentacaoCaixa>> ObterUltimasMovimentacoesAsync()
    {
        return await _repository.ObterUltimasMovimentacoesAsync(5);
    }

    public async Task<decimal> ObterTotalEntradasHojeAsync()
    {
        return await _repository.ObterTotalEntradasHojeAsync();
    }

    public async Task<decimal> ObterTotalSaidasHojeAsync()
    {
        return await _repository.ObterTotalSaidasHojeAsync();
    }

    public async Task<decimal> ObterSaldoTotalAsync()
    {
        return await _repository.ObterSaldoTotalAsync();
    }

}
