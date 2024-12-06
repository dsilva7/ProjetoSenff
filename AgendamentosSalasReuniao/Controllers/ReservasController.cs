using Aplicacao.Models;
using Aplicacao.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosSalasReuniao.Controllers
{
    public class ReservasController : ControllerBase
    {
        private ReservasService reservasService;

        public ReservasController(ReservasService reservasService)
        {
            this.reservasService = reservasService;
        }

        [HttpPost("reserva")]
        public async Task<string> EfetivarReserva([FromBody] ReservaModel reserva)
        {
            return await this.reservasService.EfetivarReserva(reserva);
        }
    }
}
