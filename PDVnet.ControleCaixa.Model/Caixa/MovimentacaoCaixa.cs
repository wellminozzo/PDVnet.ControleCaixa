using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PDVnet.ControleCaixa.Model.Caixa;

public class MovimentacaoCaixa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(100)]
    public string Descricao { get; set; } = string.Empty;

    public int Tipo { get; set; }

    public string Categoria { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser positivo")]
    public decimal Valor { get; set; }

    public DateTime DataMovimento { get; set; }

    public bool Status { get; set; } = false;

}
