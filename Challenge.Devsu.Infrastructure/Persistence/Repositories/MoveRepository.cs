using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.Interfaces;
using Challenge.Devsu.Infrastructure.Persistence.Contexts;

namespace Challenge.Devsu.Infrastructure.Persistence.Repositories
{
    public class MoveRepository : GenericRepository<Move>, IMoveRepository
    {
        public MoveRepository(DbDataContext context) : base(context)
        {
        }
    }
}
