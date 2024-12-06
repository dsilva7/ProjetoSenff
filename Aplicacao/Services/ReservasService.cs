using Aplicacao.Models;
using Common;
using Dominios.Entities;
using FluentValidation;
using Infraestrutura.Data;

namespace Aplicacao.Services
{
    public class ReservasService
    {
        private UnitOfWork unitOfWork;
        private IValidator<ReservaModel> validator;
        private EmailService emailService;

        public ReservasService(UnitOfWork unitOfWork, IValidator<ReservaModel> validator, EmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.validator = validator;
            this.emailService = emailService;
        }

        public async Task<string> EfetivarReserva(ReservaModel solicitacaoReserva)
        {
            var resultadoValidacao = await validator.ValidateAsync(solicitacaoReserva);
            var reservas = this.unitOfWork.ReservaRepository.AsQueryable();

            if (!resultadoValidacao.IsValid)
            {
                var erros = string.Join(", ", resultadoValidacao.Errors.Select(e => e.ErrorMessage));

                throw new Exception(erros);
            }

            var reserva = new Reserva
            {
                SalaId = solicitacaoReserva.SalaId,
                DataHora = solicitacaoReserva.DataHoraReserva,
                QtdePessoas = solicitacaoReserva.QtdePessoas,
                UsuarioId = solicitacaoReserva.UsuarioId
            };

            try
            {
                var usuario = this.unitOfWork.UsuarioRepository
                    .Where(s => s.UsuarioId == solicitacaoReserva.UsuarioId)
                    .FirstOrDefault();

                if (reservas.Any(s => s.SalaId == solicitacaoReserva.SalaId && s.DataHora == solicitacaoReserva.DataHoraReserva))
                {
                    var email = await emailService.EnviarEmailAsync(usuario.Email, "Reserva", $"A reserva na data {solicitacaoReserva.DataHoraReserva} foi recusada, pois, já existe uma reserva nesse horário.");

                    if (!email)
                    {
                        return $"Não foi possível solicitar a reserva, verifique o e-mail informado e tente novamente.";
                    }

                    return "Solicitação de Reserva confirmada, em breve receberá um retorno no e-mail cadastrado";
                }
                else
                {
                    var email = await emailService.EnviarEmailAsync(usuario.Email, "Confirmação da Reserva", $"A reserva na data {solicitacaoReserva.DataHoraReserva} foi efetivada.");

                    if (!email)
                    {
                        return $"Não foi possível solicitar a reserva, verifique o e-mail informado e tente novamente.";
                    }
                }

                this.unitOfWork.ReservaRepository.Add(reserva);

                await this.unitOfWork.SaveAsync();

                return "Solicitação de Reserva confirmada, em breve receberá um retorno no e-mail cadastrado";
            }
            catch (Exception ex)
            {
                return $"Não foi possível reservar a sala: {ex.Message}";
            }
        }
    }
}
