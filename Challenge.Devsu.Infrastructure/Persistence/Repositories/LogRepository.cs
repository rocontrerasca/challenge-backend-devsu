using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Infrastructure.Persistence.Contexts;

namespace Challenge.Devsu.Infrastructure.Persistence.Repositories
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        public LogRepository(DbDataContext context) : base(context)
        {
        }
    }
}
