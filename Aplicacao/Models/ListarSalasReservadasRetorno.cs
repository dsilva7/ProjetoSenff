using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Models
{
    public class ListarSalasReservadasRetorno
    {
        public string NomeSala { get; set; }
        public string NomeUsuarioReserva { get; set; }
        public string DataHoraReserva { get; set; }
        public int QtdePessoas { get; set; }
    }
}
