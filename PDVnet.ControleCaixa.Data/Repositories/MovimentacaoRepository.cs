using PDVnet.ControleCaixa.Data.Contexts;
using PDVnet.ControleCaixa.Model.Caixa;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDVnet.ControleCaixa.Data.Repositories;

public class MovimentacaoRepository
{
    private readonly AppDbContext _context;

    public MovimentacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MovimentacaoCaixa?> GetByIdAsync(int id)
    {
        return await _context.MovimentacaoCaixa.FindAsync(id);
    }

    public async Task AddAsync(MovimentacaoCaixa movimentacao)
    {
        await _context.MovimentacaoCaixa.AddAsync(movimentacao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MovimentacaoCaixa movimentacao)
    {
        _context.MovimentacaoCaixa.Update(movimentacao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MovimentacaoCaixa movimentacao)
    {
        _context.MovimentacaoCaixa.Remove(movimentacao);
        await _context.SaveChangesAsync();
    }

}
