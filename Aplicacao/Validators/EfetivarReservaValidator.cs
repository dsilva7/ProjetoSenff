using Aplicacao.Models;
using FluentValidation;

namespace Aplicacao.Validators
{
    public class EfetivarReservaValidator : AbstractValidator<ReservaModel>
    {
        public EfetivarReservaValidator()
        {
            RuleFor(x => x.SalaId)
                .GreaterThan(0).WithMessage("É necessário escolher uma sala para reserva.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("É necessário informar o responsável pela reserva.");

            RuleFor(x => x.QtdePessoas)
                .GreaterThan(0).WithMessage("É necessário informar a quantidade de pessoas.");

            RuleFor(x => x.DataHoraReserva)
                .NotEmpty().WithMessage("É necessário informar a data para reserva.");
        }
    }
}
