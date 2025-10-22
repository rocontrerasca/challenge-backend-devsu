
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Interfaces
{
    public interface ILogUseCase
    {
        Task Create(Guid? resourceId, string message);
    }
}
