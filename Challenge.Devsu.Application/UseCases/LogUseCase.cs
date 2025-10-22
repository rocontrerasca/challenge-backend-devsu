using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.UseCases
{
    public class LogUseCase: ILogUseCase
    {
        private readonly ILogRepository _log;
        public LogUseCase(ILogRepository log)
        {
            _log = log;
        }
        public async Task Create(Guid? resourceId, string message)
        {
            var log = new Log
            {
                ResourceId = resourceId,
                Message = message
            };
            await _log.AddAsync(log);
        }
    }
}
