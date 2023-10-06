using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Controllers;
using Prova.Data;
using Prova.DTOs;
using Prova.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CriarFuncionario([FromBody] FuncionarioDTO funcionarioDTO)
        {
            if (funcionarioDTO == null)
            {
                return BadRequest("Dados inválidos");
            }

            var funcionario = new Funcionario
            {
                Nome = funcionarioDTO.Nome,
            };

            _ctx.Funcionarios.Add(funcionario);
            await _ctx.SaveChangesAsync();

            return Ok("Folha de pagamento criada com sucesso!");
        }

        [HttpGet("listar")]
        public IActionResult ListarFolhas()
        {
            var folhas = _ctx.Folhas.ToList();
            return Ok(folhas);
        }

        [HttpGet("listarPorCpfMesAno")]
        public IActionResult ListarFolhaPorCpfMesAno(string cpf, int mes, int ano)
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
