using System;
using System.Collections.Generic;
using System.Text;

namespace PDVnet.ControleCaixa.Model.Caixa;

public class MovimentacaoCaixa
{

    public Guid Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public int Tipo { get; set; }

    public string Categoria { get; set; } = string.Empty;

    public int Valor { get; set; }

    public DateTime DataMovimento { get; set; }

    public bool Status { get; set; } = false;

}
