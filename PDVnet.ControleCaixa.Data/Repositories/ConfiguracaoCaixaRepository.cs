using Microsoft.Data.SqlClient;
using PDVnet.ControleCaixa.Model.Caixa;

namespace PDVnet.ControleCaixa.Data.Repositories;

public class ConfiguracaoCaixaRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ConfiguracaoCaixaRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<decimal> ObterSaldoInicialAsync()
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand("SELECT TOP 1 SaldoInicial FROM ConfiguracoesCaixa", conexao);

        await conexao.OpenAsync();
        var result = await comando.ExecuteScalarAsync();
        return result != null ? Convert.ToDecimal(result) : 0;
    }

    public async Task<decimal> ObterSaldoMinimoAsync()
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand("SELECT TOP 1 SaldoMinimo FROM ConfiguracoesCaixa", conexao);

        await conexao.OpenAsync();
        var result = await comando.ExecuteScalarAsync();
        return result != null ? Convert.ToDecimal(result) : 100m;
    }

    public async Task SalvarConfiguracaoAsync(decimal saldoInicial, decimal saldoMinimo)
    {
        using var conexao = _connectionFactory.CriarConexao();
        await conexao.OpenAsync();

        using var verificaCmd = new SqlCommand("SELECT COUNT(1) FROM ConfiguracoesCaixa", conexao);
        var existe = Convert.ToInt32(await verificaCmd.ExecuteScalarAsync()) > 0;

        using var cmd = existe
            ? new SqlCommand(@"
                UPDATE ConfiguracoesCaixa
                SET SaldoInicial = @SaldoInicial, SaldoMinimo = @SaldoMinimo, DataAtualizacao = @Data
                WHERE Id = (SELECT TOP 1 Id FROM ConfiguracoesCaixa)", conexao)
            : new SqlCommand(@"
                INSERT INTO ConfiguracoesCaixa (SaldoInicial, SaldoMinimo, DataAtualizacao)
                VALUES (@SaldoInicial, @SaldoMinimo, @Data)", conexao);

        cmd.Parameters.AddWithValue("@SaldoInicial", saldoInicial);
        cmd.Parameters.AddWithValue("@SaldoMinimo", saldoMinimo);
        cmd.Parameters.AddWithValue("@Data", DateTime.Now);

        await cmd.ExecuteNonQueryAsync();
    }
}
