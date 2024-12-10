using Aplicacao.Models;
using Aplicacao.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentosSalasReuniao.Controllers
{
    public class ReservasController : ControllerBase
    {
        private ReservasService reservasService;
        private UnitOfWork unitOfWork;

        public ReservasController(ReservasService reservasService, UnitOfWork unitOfWork)
        {
            this.reservasService = reservasService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("reserva")]
        public async Task<string> EfetivarReserva([FromBody] ReservaModel reserva)
        {
            return await this.reservasService.EfetivarReserva(reserva);
        }

        [HttpGet("historico")]
        public async Task<IActionResult> ListarReservas(int usuarioId)
        {
            var lista = await this.reservasService.ListarReservas(usuarioId);

            if (!lista.Any())
                return NotFound(new { Message = "Sem histórico de reservas para esse usuário." });

            return Ok(lista);
        }
    }
}
