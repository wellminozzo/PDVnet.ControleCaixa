using System;
using System.Collections.Generic;
using System.Text;

namespace PDVnet.ControleCaixa.Model.Caixa;

public class ConfiguracaoCaixa
{
    public int Id { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal SaldoMinimo { get; set; } = 100m;
    public DateTime DataAtualizacao { get; set; }
}
