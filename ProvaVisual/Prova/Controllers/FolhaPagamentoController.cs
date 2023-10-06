using Microsoft.AspNetCore.Mvc;
using Prova.Controllers;
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
        public async Task<IActionResult> CriarFolha([FromBody] FolhaPagamentoDTO folhaPagamentoDTO)
        {
            if (folhaPagamentoDTO == null)
        {
            return BadRequest("Dados inválidos");
        }

        var folhaPagamento = new FolhaPagamento
        {
            FuncionarioId = folhaPagamentoDTO.FuncionarioId,
        };

            _ctx.Folhas.Add(folhaPagamento);
            await _ctx.SaveChangesAsync();

            return Ok("Folha de pagamento criada com sucesso!");
        }

        [HttpGet("listar")]
        public IActionResult ListarFolhas()
        {
            var folhas = _ctx.Folhas.ToList();
            return Ok(folhas);
        }

        [HttpGet("buscar")]
        public IActionResult BuscarFolhaPorCpfMesAno(string cpf, int mes, int ano)
        {
        var funcionario = _ctx.Funcionarios.FirstOrDefault(f => f.CPF == cpf);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado");
            }

        var folhaPagamento = _ctx.Folhas.FirstOrDefault(fp => 
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
    }
}
