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
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=PDVnetControleCaixa;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<MovimentacaoCaixa> MovimentacaoCaixa {  get; set; }

    }
}
