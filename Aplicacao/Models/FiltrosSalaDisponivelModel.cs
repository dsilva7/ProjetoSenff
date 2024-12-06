using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Models
{
    public class FiltrosSalaDisponivelModel
    {
        public DateTime DataHoraDesejada { get; set; }
        public int CapacidadeMaxima { get; set; }
        public string Recursos { get; set; }
    }
}
