using Microsoft.Data.SqlClient;
using PDVnet.ControleCaixa.Model.Caixa;
using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.Data.Repositories;

public class MovimentacaoRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public MovimentacaoRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<MovimentacaoCaixa>> GetAllAsync()
    {
        var lista = new List<MovimentacaoCaixa>();
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(
            "SELECT Id, Descricao, Tipo, Categoria, Valor, DataMovimento, Status FROM MovimentacaoCaixa",
            conexao);

        await conexao.OpenAsync();
        using var reader = await comando.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(MapearMovimentacao(reader));
        }
        return lista;
    }

    public async Task<MovimentacaoCaixa?> GetByIdAsync(int id)
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(
            "SELECT Id, Descricao, Tipo, Categoria, Valor, DataMovimento, Status FROM MovimentacaoCaixa WHERE Id = @Id",
            conexao);
        comando.Parameters.AddWithValue("@Id", id);

        await conexao.OpenAsync();
        using var reader = await comando.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapearMovimentacao(reader);
        }
        return null;
    }

    public async Task AddAsync(MovimentacaoCaixa movimentacao)
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(@"
            INSERT INTO MovimentacaoCaixa (Descricao, Tipo, Categoria, Valor, DataMovimento, Status)
            VALUES (@Descricao, @Tipo, @Categoria, @Valor, @DataMovimento, @Status);
            SELECT SCOPE_IDENTITY();", conexao);

        comando.Parameters.AddWithValue("@Descricao", movimentacao.Descricao);
        comando.Parameters.AddWithValue("@Tipo", (int)movimentacao.Tipo);
        comando.Parameters.AddWithValue("@Categoria", (object?)movimentacao.Categoria ?? DBNull.Value);
        comando.Parameters.AddWithValue("@Valor", movimentacao.Valor);
        comando.Parameters.AddWithValue("@DataMovimento", movimentacao.DataMovimento);
        comando.Parameters.AddWithValue("@Status", (int)movimentacao.Status);

        await conexao.OpenAsync();
        var id = Convert.ToInt32(await comando.ExecuteScalarAsync());
        movimentacao.Id = id;
    }

    public async Task UpdateAsync(MovimentacaoCaixa movimentacao)
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(@"
            UPDATE MovimentacaoCaixa
            SET Descricao = @Descricao, Tipo = @Tipo, Categoria = @Categoria,
                Valor = @Valor, DataMovimento = @DataMovimento, Status = @Status
            WHERE Id = @Id", conexao);

        comando.Parameters.AddWithValue("@Id", movimentacao.Id);
        comando.Parameters.AddWithValue("@Descricao", movimentacao.Descricao);
        comando.Parameters.AddWithValue("@Tipo", (int)movimentacao.Tipo);
        comando.Parameters.AddWithValue("@Categoria", (object?)movimentacao.Categoria ?? DBNull.Value);
        comando.Parameters.AddWithValue("@Valor", movimentacao.Valor);
        comando.Parameters.AddWithValue("@DataMovimento", movimentacao.DataMovimento);
        comando.Parameters.AddWithValue("@Status", (int)movimentacao.Status);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand("DELETE FROM MovimentacaoCaixa WHERE Id = @Id", conexao);
        comando.Parameters.AddWithValue("@Id", id);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task<List<MovimentacaoCaixa>> ObterUltimasMovimentacoesAsync(int quantidade = 5)
    {
        var lista = new List<MovimentacaoCaixa>();
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(@"
            SELECT TOP (@Quantidade) Id, Descricao, Tipo, Categoria, Valor, DataMovimento, Status
            FROM MovimentacaoCaixa
            ORDER BY DataMovimento DESC", conexao);
        comando.Parameters.AddWithValue("@Quantidade", quantidade);

        await conexao.OpenAsync();
        using var reader = await comando.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(MapearMovimentacao(reader));
        }
        return lista;
    }

    public async Task<decimal> ObterTotalEntradasHojeAsync()
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(@"
            SELECT ISNULL(SUM(Valor), 0)
            FROM MovimentacaoCaixa
            WHERE Tipo = @Tipo AND CAST(DataMovimento AS DATE) = CAST(GETDATE() AS DATE)", conexao);
        comando.Parameters.AddWithValue("@Tipo", (int)TipoMovimentacao.Entrada);

        await conexao.OpenAsync();
        return Convert.ToDecimal(await comando.ExecuteScalarAsync());
    }

    public async Task<decimal> ObterTotalSaidasHojeAsync()
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(@"
            SELECT ISNULL(SUM(Valor), 0)
            FROM MovimentacaoCaixa
            WHERE Tipo = @Tipo AND CAST(DataMovimento AS DATE) = CAST(GETDATE() AS DATE)", conexao);
        comando.Parameters.AddWithValue("@Tipo", (int)TipoMovimentacao.Saida);

        await conexao.OpenAsync();
        return Convert.ToDecimal(await comando.ExecuteScalarAsync());
    }

    public async Task<decimal> ObterSaldoTotalAsync()
    {
        using var conexao = _connectionFactory.CriarConexao();
        using var comando = new SqlCommand(@"
            SELECT ISNULL(SUM(CASE WHEN Tipo = 0 THEN Valor ELSE -Valor END), 0)
            FROM MovimentacaoCaixa", conexao);

        await conexao.OpenAsync();
        return Convert.ToDecimal(await comando.ExecuteScalarAsync());
    }

    private static MovimentacaoCaixa MapearMovimentacao(SqlDataReader reader)
    {
        return new MovimentacaoCaixa
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
            Tipo = (TipoMovimentacao)reader.GetInt32(reader.GetOrdinal("Tipo")),
            Categoria = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Categoria")),
            Valor = reader.GetDecimal(reader.GetOrdinal("Valor")),
            DataMovimento = reader.GetDateTime(reader.GetOrdinal("DataMovimento")),
            Status = (SituacaoStatus)reader.GetInt32(reader.GetOrdinal("Status"))
        };
    }
}
