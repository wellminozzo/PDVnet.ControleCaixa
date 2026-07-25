using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.Model.Caixa;

public class MovimentacaoCaixa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(100)]
    public string Descricao { get; set; } = string.Empty;

    public TipoMovimentacao Tipo { get; set; }

    public string Categoria { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser positivo")]
    public decimal Valor { get; set; }

    public DateTime DataMovimento { get; set; }

    public SituacaoStatus Status { get; set; } 

}
