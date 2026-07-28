using PDVnet.ControleCaixa.Data.Repositories;

namespace PDVnet.ControleCaixa.Business.Services;

public class ConfiguracaoCaixaService
{
    private readonly ConfiguracaoCaixaRepository _repository;

    public ConfiguracaoCaixaService(ConfiguracaoCaixaRepository repository)
    {
        _repository = repository;
    }

    public Task<decimal> ObterSaldoInicialAsync() => _repository.ObterSaldoInicialAsync();

    public Task<decimal> ObterSaldoMinimoAsync() => _repository.ObterSaldoMinimoAsync();

    public Task SalvarConfiguracaoAsync(decimal saldoInicial, decimal saldoMinimo)
        => _repository.SalvarConfiguracaoAsync(saldoInicial, saldoMinimo);
}
