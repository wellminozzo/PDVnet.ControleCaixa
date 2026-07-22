using Microsoft.EntityFrameworkCore;
using PDVnet.ControleCaixa.Model.Caixa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PDVnet.ControleCaixa.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //aqui colocaremos a configuração de conexao com o banco de dados
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=PDVnetControleCaixa;Trusted_Connection=True;TrustServerCertificate=True;");
            //"Server=WELLBON;Database=PDVnetControleCaixa;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<MovimentacaoCaixa> MovimentacaoCaixa {  get; set; }
        
    }
}
