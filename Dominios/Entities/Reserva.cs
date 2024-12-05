namespace Dominios.Entities
{
    public class Reserva
    {
        public long Id { get; set; }
        public long SalaId { get; set; }
        public Sala Sala { get; set; }
        public int QtdePessoas { get; set; }
        public DateTime DataHora { get; set; }
    }
}
