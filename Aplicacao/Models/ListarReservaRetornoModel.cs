using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Models
{
    public class ListarReservaRetornoModel
    {
        public string NomeSala { get; set; }
        public int QtdePessoas { get; set; }
        public string DataHoraReserva { get; set; }
        public string NomeUsuario { get; set; }
    }
}
