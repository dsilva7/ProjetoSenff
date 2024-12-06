using Aplicacao.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Validators
{
    public class CadastroSalaValidator : AbstractValidator<CadastroSalaModel>
    {
        public CadastroSalaValidator()
        {
            RuleFor(x => x.NomeSala)
                .NotEmpty().WithMessage("O nome da sala é obrigatório.")
                .Length(3, 100).WithMessage("O nome da sala deve ter entre 5 e 100 caracteres.");

            RuleFor(x => x.CapacidadeMaxima)
                .GreaterThan(0).WithMessage("A capacidade da sala deve ser maior que 0.");

            RuleFor(x => x.Recursos)
                .NotEmpty().WithMessage("Os recursos da sala são obrigatórios.");
        }
    }
}
