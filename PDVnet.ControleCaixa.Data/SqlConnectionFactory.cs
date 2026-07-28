using Microsoft.Data.SqlClient;

namespace PDVnet.ControleCaixa.Data;

public class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CriarConexao()
    {
        return new SqlConnection(_connectionString);
    }
}
