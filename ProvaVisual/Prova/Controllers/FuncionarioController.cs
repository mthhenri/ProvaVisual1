using Microsoft.AspNetCore.Mvc;
using Prova.Data;

namespace Prova.Controllers;
public class FuncionarioController : ControllerBase
{
    private readonly AppDatabase _ctx;
    public FuncionarioController(AppDatabase ctx)
    {
        _ctx = ctx;
    }

    
}
