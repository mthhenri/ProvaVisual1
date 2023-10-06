using Microsoft.AspNetCore.Mvc;
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


        [HttpPost("criar")]
        public IActionResult CriarFolha([FromBody] FolhaPagamentoDTO folhaPagamentoDTO)
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
            var folhaPagamento = new FolhaPagamento
            {
            FuncionarioId = folhaPagamentoDTO.FuncionarioId,
            };

            _ctx.Folhas.Add(folhaPagamento);
            _ctx.SaveChanges();

            return Ok("Folha de pagamento criada com sucesso!");
        }

        [HttpGet("listar")]
        public IActionResult ListarFolhas()
        {
            try
            {
                List<FolhaPagamento>? folhas = _ctx.Folhas.ToList();
                return Ok(folhas);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("buscar/{cpf}/{mes}/ano")]
        public IActionResult ListarFolhaPorCpfMesAno([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano)
        {
            try
            {
                Funcionario? funcionario = _ctx.Funcionarios.FirstOrDefault(f => f.CPF == cpf);

                if (funcionario == null)
                {
                    return NotFound("Funcionário não encontrado");
                }

                FolhaPagamento? folhaPagamento = _ctx.Folhas.FirstOrDefault(fp => 
                    fp.FuncionarioId == funcionario.FuncionarioId &&
                    fp.Mes == mes &&
                    fp.Ano == ano);

                if (folhaPagamento == null)
                {
                    return NotFound("Folha de pagamento não encontrada");
                }

                var resultado = new
                {
                    CPF = funcionario.CPF,
                    Mes = folhaPagamento.Mes,
                    Ano = folhaPagamento.Ano
                };
                return Ok(resultado);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
