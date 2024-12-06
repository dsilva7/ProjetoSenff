namespace Dominios.Entities
{
    public class Reserva
    {
        public long ReservaId { get; set; }
        public int SalaId { get; set; }
        public Sala Sala { get; set; }
        public int QtdePessoas { get; set; }
        public DateTime DataHora { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
