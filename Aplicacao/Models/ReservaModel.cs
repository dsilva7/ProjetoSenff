using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Models
{
    public class ReservaModel
    {
        public int SalaId { get; set; }
        public DateTime DataHoraReserva { get; set; }
        public int UsuarioId { get; set; }
        public int QtdePessoas { get; set; }
        public int QtdeHorasUtilizacao { get; set; }
    }
}
