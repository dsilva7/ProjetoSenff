using Aplicacao.Models;
using Dominios.Entities;
using FluentValidation;
using Infraestrutura.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Services
{
    public class SalasService
    {
        private UnitOfWork unitOfWork;
        private IValidator<CadastroSalaModel> validator;

        public SalasService(UnitOfWork unitOfWork, IValidator<CadastroSalaModel> validator)
        {
            this.unitOfWork = unitOfWork;
            this.validator = validator;
        }

        public async Task<string> CadastrarSala(CadastroSalaModel cadastro)
        {
            var resultadoValidacao = await validator.ValidateAsync(cadastro);

            if (!resultadoValidacao.IsValid)
            { var erros = string.Join(", ", resultadoValidacao.Errors.Select(e => e.ErrorMessage));
               
                throw new Exception(erros);
            }

            var sala = new Sala
            {
                Nome = cadastro.NomeSala,
                Capacidade = cadastro.CapacidadeMaxima,
                Recursos = cadastro.Recursos
            };

            try
            {
                this.unitOfWork.SalaRepository.Add(sala);
                await this.unitOfWork.SaveAsync();

                return $"Sala {cadastro.NomeSala} cadastrada com sucesso!";
            }
            catch (Exception ex)
            {
                return $"O cadastro da {cadastro.NomeSala} foi recusado: {ex.Message}!";
            }
        }

        public async Task<List<ListarSalasRetornoModel>> ListarSalasDisponiveis(FiltrosSalaDisponivelModel filtros)
        {
            var salasId = this.unitOfWork.SalaRepository
                .Where(s => filtros.Recursos.Contains(s.Recursos))
                .Select(x => x.SalaId)
                .ToList();

            return this.unitOfWork.ReservaRepository
                .Where(s => salasId.Contains(s.SalaId) && filtros.DataHoraDesejada != s.DataHora && filtros.DataHoraDesejada > s.DataHoraFinal)
                .Select(x => new ListarSalasRetornoModel
                {
                    NomeSala = x.Sala.Nome,
                    CapacidadeMaxima = x.Sala.Capacidade,
                    Recursos = x.Sala.Recursos
                })
                .ToList();
        }
    }
}
