using Challenge.Devsu.Api.Controllers;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace Challenge.Devsu.Tests.UnitTests.ControllerTests
{
    public class ClientControllerTests
    {
        private (ClientController ctrl, Mock<IClientUseCase> uc, Mock<ILogUseCase> log, Mock<ILogger<ClientController>> logger) Make()
        {
            var uc = new Mock<IClientUseCase>(MockBehavior.Strict);
            var log = new Mock<ILogUseCase>(MockBehavior.Loose);
            var logger = new Mock<ILogger<ClientController>>(MockBehavior.Loose);
            return (new ClientController(uc.Object, logger.Object, log.Object), uc, log, logger);
        }

        [Fact]
        public async Task Get_Success()
        {
            var (ctrl, uc, _, _) = Make();
            uc.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<ClientResponseDto>
            {
                new ClientResponseDto { ClientId = Guid.NewGuid(), FullName = "Jose" }
            });

            var res = await ctrl.Get();
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ClientResponseDto>>>(json)!;

            payload.Data.Should().NotBeEmpty();
            uc.VerifyAll();
        }

        [Fact]
        public async Task GetById_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var id = Guid.NewGuid();
            uc.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new ClientResponseDto { ClientId = id, FullName = "Jose" });

            var res = await ctrl.GetById(id);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var item = JsonSerializer.Deserialize<ApiResponse<ClientResponseDto>>(json)!;

            item.Data.ClientId.Should().Be(id);
            uc.VerifyAll();
        }

        [Fact]
        public async Task Create_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var dto = new ClientDto
            {
                FullName = "Nueva",
                Gender = "F",
                Age = 29,
                IdentificationNumber = "ID1",
                Address = "Dir",
                PhoneNumber = "0987654321",
                Password = "pwd"
            };
            var created = new ClientResponseDto { ClientId = Guid.NewGuid(), FullName = dto.FullName };
            uc.Setup(x => x.CreateAsync(dto)).ReturnsAsync(created);

            var res = await ctrl.Create(dto);
            var createdRes = Assert.IsType<ObjectResult>(res); // 201 (Created, o ObjectResult con 201)
            var json = JsonSerializer.Serialize(createdRes.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<ClientResponseDto>>(json)!;

            payload.Data.FullName.Should().Be("Nueva");
            uc.VerifyAll();
        }

        [Fact]
        public async Task Update_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var upd = new ClientUpdateDto
            {
                ClientId = Guid.NewGuid(),
                FullName = "Actualizada",
                Gender = "F",
                Age = 31,
                Address = "Dir2",
                PhoneNumber = "1234567890",
                Active = true
            };
            uc.Setup(x => x.UpdateAsync(upd)).ReturnsAsync(upd);

            var res = await ctrl.Update(upd);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<ClientUpdateDto>>(json)!;

            payload.Data.FullName.Should().Be("Actualizada");
            uc.VerifyAll();
        }

        [Fact]
        public async Task DeleteById_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var id = Guid.NewGuid();
            uc.Setup(x => x.DeleteByIdAsync(id)).ReturnsAsync(new ClientResponseDto { ClientId = id });

            var res = await ctrl.DeleteById(id);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<ClientResponseDto>>(json)!;

            payload.Data.ClientId.Should().Be(id);
            uc.VerifyAll();
        }
    }
}
