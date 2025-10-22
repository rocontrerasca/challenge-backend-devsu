using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.Interfaces;
using Challenge.Devsu.Infrastructure.Persistence.Contexts;

namespace Challenge.Devsu.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(DbDataContext context) : base(context)
        {
        }
    }
}
