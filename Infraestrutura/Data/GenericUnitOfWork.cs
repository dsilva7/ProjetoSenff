using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Data
{
    public abstract class GenericUnitOfWork<T> where T : DbContext
    {
        public readonly T context;

        public GenericUnitOfWork(T context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }

    }
}
