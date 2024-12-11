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

            if (this.unitOfWork.SalaRepository.Any(s => s.SalaId == solicitacaoReserva.SalaId && s.Capacidade < solicitacaoReserva.QtdePessoas))
                return "A sala escolhida não acolhe a quantidade de pessoas informada.";

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

                    var email = await emailService.EnviarEmailAsync(usuario.Email, "Confirmação da Reserva", $"A reserva na data {solicitacaoReserva.DataHoraReserva.ToString("dd/MM/yyyy HH:mm")} foi efetivada para {solicitacaoReserva.QtdeHorasUtilizacao} horas.");

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

        public async Task<List<ListarReservaRetornoModel>> ListarReservas(int usuarioId)
        {
            return await this.unitOfWork.ReservaRepository
            .Where(s => s.UsuarioId == usuarioId)
            .Select(x => new ListarReservaRetornoModel
            {
                NomeSala = x.Sala.Nome,
                QtdePessoas = x.QtdePessoas,
                DataHoraReserva = x.DataHora.ToString("dd/MM/yyyy HH:mm"),
                NomeUsuario = x.Usuario.Nome
            })
            .ToListAsync();
        }
    }
}
