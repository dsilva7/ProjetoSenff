using Aplicacao.Models;
using Dominios.Entities;
using FluentValidation;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
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
            {
                var erros = string.Join(", ", resultadoValidacao.Errors.Select(e => e.ErrorMessage));

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
            // Divide os recursos informados em uma lista
            List<string> recursosFiltrados = !string.IsNullOrWhiteSpace(filtros.Recursos)
                ? filtros.Recursos.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>();

            // Busca as salas que contenham todos os recursos especificados
            var querySalas = this.unitOfWork.SalaRepository.AsQueryable();
            if (recursosFiltrados.Any())
            {
                querySalas = querySalas.Where(s =>
                    recursosFiltrados.All(r => s.Recursos.Contains(r)));
            }
             
            var salas = await querySalas.ToListAsync();

            // Busca salas ocupadas no horário desejado
            var salasComReservas = await this.unitOfWork.ReservaRepository
                .Where(r =>
                    filtros.DataHoraDesejada >= r.DataHora &&
                    filtros.DataHoraDesejada <= r.DataHoraFinal)
                .Select(s => s.SalaId)
                .ToListAsync();

            // Filtra apenas as salas disponíveis
            var salasDisponiveis = salas
                .Where(s => !salasComReservas.Contains(s.SalaId) && s.Capacidade >= filtros.CapacidadeMaxima)
                .Select(x => new ListarSalasRetornoModel
                {
                    NomeSala = x.Nome,
                    CapacidadeMaxima = x.Capacidade,
                    Recursos = x.Recursos
                })
                .ToList();

            return salasDisponiveis;
        }


        public async Task<List<ListarSalasReservadasRetorno>> ListarSalasReservadas(int salaId)
        {
            return this.unitOfWork.ReservaRepository
                .Where(s => s.SalaId == salaId)
                .Select(x => new ListarSalasReservadasRetorno()
                {
                    NomeSala = x.Sala.Nome,
                    NomeUsuarioReserva = x.Usuario.Nome,
                    QtdePessoas = x.QtdePessoas,
                    DataHoraReserva = x.DataHora.ToString("dd/MM/yyyy HH:mm")
                })
                .ToList();
        }
    }
}
