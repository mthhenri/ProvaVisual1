using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.DTOs;
using Prova.Models;

namespace Prova.Controllers
{
    [Route("api/folha")]
    [ApiController]
    public class FolhaPagamentoController : ControllerBase
    {
        private readonly AppDatabase _ctx; 

        public FolhaPagamentoController(AppDatabase ctx)
        {
            _ctx = ctx;
        }


        [HttpPost("cadastrar")]
        public IActionResult CriarFolha([FromBody] FolhaPagamentoDTO folhaPagamentoDTO)
        {
            try
            {
                if (folhaPagamentoDTO == null)
                {
                    return BadRequest("Dados inválidos");
                }

                Funcionario? funcionario = _ctx.Funcionarios.FirstOrDefault(f => f.FuncionarioId == folhaPagamentoDTO.FuncionarioId);

                if (funcionario == null)
                {
                    return NotFound("Funcionário não encontrado");
                }

                double salarioBruto = folhaPagamentoDTO.Quantidade * folhaPagamentoDTO.Valor;

                double impostoRenda = 0;
                if (salarioBruto <= 1903.98)
                {
                    // Nenhuma alíquota e parcela a deduzir para essa faixa
                    impostoRenda = 0.0;
                }
                else if (salarioBruto <= 2826.65)
                {
                    impostoRenda = (salarioBruto * 0.075) - 142.80;
                }
                else if (salarioBruto <= 3751.05)
                {
                    impostoRenda = (salarioBruto * 0.15) - 354.80;
                }
                else if (salarioBruto <= 4664.68)
                {
                    impostoRenda = (salarioBruto * 0.225) - 636.13;
                }
                else
                {
                    impostoRenda = (salarioBruto * 0.275) - 869.36;
                }

                double inss = 0;
                if (salarioBruto <= 1693.72)
                {
                    inss = salarioBruto * 0.08;
                }
                else if (salarioBruto <= 2822.90)
                {
                    inss = salarioBruto * 0.09;
                }
                else if (salarioBruto <= 5645.80)
                {
                    inss = salarioBruto * 0.11;
                }
                else
                {
                    inss = 621.03; // Desconto fixo
                }

                double fgts = salarioBruto * 0.08;

                double salLiquido = salarioBruto - impostoRenda - inss;

                FolhaPagamento? folhaPagamento = new()
                {
                    ValorHora = folhaPagamentoDTO.Valor,
                    QuantidadeHoras = folhaPagamentoDTO.Quantidade,
                    Mes = folhaPagamentoDTO.Mes,
                    Ano = folhaPagamentoDTO.Ano,
                    SalarioBruto = salarioBruto,
                    ImpostoRenda = impostoRenda,
                    INSS = inss,
                    FGTS = fgts,
                    SalarioLiquido = salLiquido,
                    Funcionario = funcionario,
                    FuncionarioId = funcionario.FuncionarioId,
                };

                _ctx.Folhas.Add(folhaPagamento);
                _ctx.SaveChanges();

                return Created("Folha de pagamento cadastrada", folhaPagamento);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("listar")]
        public IActionResult ListarFolhas()
        {
            try
            {
                List<FolhaPagamento>? folhas = _ctx.Folhas.Include(fp => fp.Funcionario).ToList();
                return Ok(folhas);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("buscar/{cpf}/{mes}/{ano}")]
        public IActionResult ListarFolhaPorCpfMesAno([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano)
        {
            try
            {
                Funcionario? funcionario = _ctx.Funcionarios.FirstOrDefault(f => f.CPF == cpf);

                if (funcionario == null)
                {
                    return NotFound("Funcionário não encontrado");
                }

                FolhaPagamento? folhaPagamento = _ctx.Folhas.FirstOrDefault(f => f.FuncionarioId == funcionario.FuncionarioId 
                    && f.Mes == mes 
                    && f.Ano == ano);

                if (folhaPagamento == null)
                {
                    return NotFound("Folha de pagamento não encontrada");
                }
                
                return Ok(folhaPagamento);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
