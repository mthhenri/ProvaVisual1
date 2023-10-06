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

            return Ok("Funcionário criado com sucesso");
        }
    }
}
