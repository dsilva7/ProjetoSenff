using Dominios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Data
{
    public class UnitOfWork : GenericUnitOfWork<SenffContext>
    {
        public UnitOfWork(SenffContext context) : base(context)
        {
        }

        private GenericRepository<SenffContext, Sala> salaRepository;
        public GenericRepository<SenffContext, Sala> SalaRepository
        {
            get
            {
                return this.salaRepository = this.salaRepository ?? new GenericRepository<SenffContext, Sala>(this.context);
            }
        }

        private GenericRepository<SenffContext, Reserva> reservaRepository;
        public GenericRepository<SenffContext, Reserva> ReservaRepository
        {
            get
            {
                return this.reservaRepository = this.reservaRepository ?? new GenericRepository<SenffContext, Reserva>(this.context);
            }
        }
    }
}
