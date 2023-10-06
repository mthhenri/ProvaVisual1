using System.ComponentModel.DataAnnotations;

namespace Prova.Models;
public class FolhaPagamento
{
 public int FolhaId { get; set; }
 public double ValorHora { get; set; }
 public int QuantidadeHoras { get; set; }
 public int Mes{ get; set; }
 public int Ano{ get; set; }
 public double SalarioBruto { get; set; }
 public double ImpostoRenda  { get; set; }
 public double INSS { get; set; }
 public double FGTS { get; set; }
 public int FuncionarioId { get; set; }
 
}
