using Microsoft.AspNetCore.Mvc;
using Prova.Data;
using Prova.DTOs;
using Prova.Models;

namespace Prova.Controllers;
[ApiController]
[Route("api/funcionario")]
public class FuncionarioController : ControllerBase
{
    private readonly AppDatabase _ctx;
    public FuncionarioController(AppDatabase ctx)
    {
        _ctx = ctx;
    }

    //Metodos
    [HttpGet("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Funcionario>? funcionarios = _ctx.Funcionarios.ToList();
            if(funcionarios == null)
            {
                return NotFound();
            }
            return Ok(funcionarios);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return NotFound(ex.Message);
        }
    }

    [HttpPost("cadastrar")]
    public IActionResult Cadastrar([FromBody] FuncionarioDTO funcionarioDTO)
    {
        try
        {
            Funcionario? funcionario = new()
            {
                Nome = funcionarioDTO.Nome,
                CPF = funcionarioDTO.CPF
            };

            _ctx.Funcionarios.Add(funcionario);
            _ctx.SaveChanges();
            return Created("", funcionario);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return NotFound(ex.Message);
        }
    }

}
