using Microsoft.AspNetCore.Mvc;
using Prova.Data;

namespace Prova.Controllers;
public class FolhaPagamentoController : ControllerBase
{
    private readonly AppDatabase _ctx;
    public FolhaPagamentoController(AppDatabase ctx)
    {
        _ctx = ctx;
    }


    //Metodos
}
