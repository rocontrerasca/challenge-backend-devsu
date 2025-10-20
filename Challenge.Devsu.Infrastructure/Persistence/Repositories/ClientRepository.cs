using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.Interfaces;
using Challenge.Devsu.Infrastructure.Persistence.Contexts;

namespace Challenge.Devsu.Infrastructure.Persistence.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(DbDataContext context) : base(context)
        {
        }
    }
}
