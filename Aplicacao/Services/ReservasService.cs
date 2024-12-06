using Aplicacao.Models;
using Common;
using Dominios.Entities;
using FluentValidation;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                UsuarioId = solicitacaoReserva.UsuarioId,
                DataHoraFinal = solicitacaoReserva.DataHoraReserva.AddHours(solicitacaoReserva.QtdeHorasUtilizacao)
            };

            try
            {
                var usuario = this.unitOfWork.UsuarioRepository
                    .Where(s => s.UsuarioId == solicitacaoReserva.UsuarioId)
                    .FirstOrDefault();                

                if (reservas.Any(s => s.SalaId == solicitacaoReserva.SalaId && solicitacaoReserva.DataHoraReserva >= s.DataHora && solicitacaoReserva.DataHoraReserva <= s.DataHoraFinal))
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
                    this.unitOfWork.ReservaRepository.Add(reserva);

                    await this.unitOfWork.SaveAsync();

                    var email = await emailService.EnviarEmailAsync(usuario.Email, "Confirmação da Reserva", $"A reserva na data {solicitacaoReserva.DataHoraReserva} foi efetivada. Para {solicitacaoReserva.QtdeHorasUtilizacao} horas");

                    if (!email)
                    {
                        return $"Não foi possível solicitar a reserva, verifique o e-mail informado e tente novamente.";
                    }
                }

                return "Solicitação de Reserva confirmada, em breve receberá um retorno no e-mail cadastrado";
            }
            catch (Exception ex)
            {
                return $"Não foi possível reservar a sala: {ex.Message}";
            }
        }
    }
}
