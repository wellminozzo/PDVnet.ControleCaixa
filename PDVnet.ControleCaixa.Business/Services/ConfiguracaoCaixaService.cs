using Microsoft.EntityFrameworkCore;
using PDVnet.ControleCaixa.Data.Contexts;
using PDVnet.ControleCaixa.Model.Caixa;

namespace PDVnet.ControleCaixa.Business.Services;

public class ConfiguracaoCaixaService
{
    private readonly AppDbContext _context;

    public ConfiguracaoCaixaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> ObterSaldoInicialAsync()
    {
        var config = await _context.ConfiguracoesCaixa.FirstOrDefaultAsync();
        return config?.SaldoInicial ?? 0;
    }

    public async Task<decimal> ObterSaldoMinimoAsync()
    {
        var config = await _context.ConfiguracoesCaixa.FirstOrDefaultAsync();
        return config?.SaldoMinimo ?? 100m;
    }

    public async Task SalvarConfiguracaoAsync(decimal saldoInicial, decimal saldoMinimo)
    {
        var config = await _context.ConfiguracoesCaixa.FirstOrDefaultAsync();

        if (config == null)
        {
            config = new ConfiguracaoCaixa
            {
                SaldoInicial = saldoInicial,
                SaldoMinimo = saldoMinimo,
                DataAtualizacao = DateTime.Now
            };
            _context.ConfiguracoesCaixa.Add(config);
        }
        else
        {
            config.SaldoInicial = saldoInicial;
            config.SaldoMinimo = saldoMinimo;
            config.DataAtualizacao = DateTime.Now;
        }

        await _context.SaveChangesAsync();
    }
}

