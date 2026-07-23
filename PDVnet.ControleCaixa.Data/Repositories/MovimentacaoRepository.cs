using Microsoft.EntityFrameworkCore;
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

}
