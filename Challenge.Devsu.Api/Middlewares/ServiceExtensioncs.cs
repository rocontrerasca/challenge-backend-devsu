using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Application.Mappings;
using Challenge.Devsu.Application.UseCases;
using Challenge.Devsu.Core.Interfaces;
using Challenge.Devsu.Infrastructure.Configurations;
using Challenge.Devsu.Infrastructure.Persistence.Repositories;

namespace Challenge.Devsu.Api.Middlewares
{
    public static class ServiceExtensioncs
    {
        /// <summary>
        /// Register all necessary dependencies in the service container.
        /// </summary>
        /// <param name="collection">
        /// Collection of services to which the dependencies will be added.
        /// </param>
        public static void RegisterDependencies(this IServiceCollection collection)
        {
            collection.AddMemoryCache();

            //Inyección de repositorios
            collection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            collection.AddScoped<IClientRepository, ClientRepository>();
            collection.AddScoped<IMoveRepository, MoveRepository>();
            collection.AddScoped<IAccountRepository, AccountRepository>();
            collection.AddScoped<ILogRepository, LogRepository>();

            //Inyección de casos de uso
            collection.AddScoped<IClientUseCase, ClientUseCase>();
            collection.AddScoped<IAccountUseCase, AccountUseCase>();
            collection.AddScoped<IMoveUseCase, MoveUseCase>();
            collection.AddScoped<IClientUseCase, ClientUseCase>();
            collection.AddScoped<ILogUseCase, LogUseCase>();


            collection.AddScoped<DatabaseConfigService>();

            //Mappers
            collection.AddAutoMapper(typeof(ClientMapper).Assembly);
            collection.AddAutoMapper(typeof(AccountMapper).Assembly);
            collection.AddAutoMapper(typeof(MoveMapper).Assembly);
        }
    }
}
