using Microsoft.EntityFrameworkCore;
using PDVnet.ControleCaixa.Data.Contexts;
using PDVnet.ControleCaixa.Model.Caixa;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PDVnet.ControleCaixa.Data.Repositories;

public class MovimentacaoRepository
{
    private readonly AppDbContext _context;

    public MovimentacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovimentacaoCaixa>> GetAllAsync()
    {
        return await _context.MovimentacaoCaixa.ToListAsync();
    }

    public async Task<MovimentacaoCaixa?> GetByIdAsync(int id)
    {
        return await _context.MovimentacaoCaixa.FindAsync(id);
    }

    public async Task AddAsync(MovimentacaoCaixa movimentacao)
    {
        _context.MovimentacaoCaixa.Add(movimentacao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MovimentacaoCaixa movimentacao)
    {
        _context.MovimentacaoCaixa.Update(movimentacao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var movimentacao = await _context.MovimentacaoCaixa.FindAsync(id);
        if (movimentacao != null)
        {
            _context.MovimentacaoCaixa.Remove(movimentacao);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<MovimentacaoCaixa>> ObterUltimasMovimentacoesAsync(int quantidade = 5)
    {
        return await _context.MovimentacaoCaixa
            .OrderByDescending(x => x.DataMovimento)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<decimal> ObterTotalEntradasHojeAsync()
    {
        var hoje = DateTime.Today;

        return await _context.MovimentacaoCaixa
            .Where(x =>
                x.Tipo == 0 &&
                x.DataMovimento.Date == hoje)
            .SumAsync(x => (decimal?)x.Valor) ?? 0;
    }

    public async Task<decimal> ObterTotalSaidasHojeAsync()
    {
        var hoje = DateTime.Today;

        return await _context.MovimentacaoCaixa
            .Where(x =>
                x.Tipo == 1 &&
                x.DataMovimento.Date == hoje)
            .SumAsync(x => (decimal?)x.Valor) ?? 0;
    }

    public async Task<decimal> ObterSaldoTotalAsync()
    {
        return await _context.MovimentacaoCaixa
            .AsNoTracking()
            .SumAsync(x => x.Tipo == 0
                ? x.Valor
                : -x.Valor);
    }



}
