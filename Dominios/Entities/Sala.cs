using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominios.Entities
{
    public class Sala
    {
        public int SalaId { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public string Recursos { get; set; }
    }
}
