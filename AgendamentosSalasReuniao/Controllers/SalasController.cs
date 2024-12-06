using Aplicacao.Models;
using Aplicacao.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosSalasReuniao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class SalasController : ControllerBase
    {
        private SalasService salasService;

        public SalasController(SalasService SalasService)
        {
            this.salasService = SalasService;
        }

        [HttpPost("cadastro")]
        public async Task<string> CadastrarSala([FromBody] CadastroSalaModel cadastroSala)
        {
            return await this.salasService.CadastrarSala(cadastroSala);
        }

        [HttpGet("salas/disponiveis")]
        public async Task<IActionResult> ListarSalasDisponiveis(FiltrosSalaDisponivelModel filtros)
        {
            var salas = await this.salasService.ListarSalasDisponiveis(filtros);
            return Ok(salas);
        }
    }
}

