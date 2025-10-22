using Challenge.Devsu.Api.Controllers;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Enums;
using Challenge.Devsu.Core.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace Challenge.Devsu.Tests.UnitTests.ControllerTests
{
    public class MoveControllerTests
    {
        private (MoveController ctrl, Mock<IMoveUseCase> uc, Mock<ILogUseCase> log, Mock<ILogger<MoveController>> logger) Make()
        {
            var uc = new Mock<IMoveUseCase>(MockBehavior.Strict);
            var log = new Mock<ILogUseCase>(MockBehavior.Loose);
            var logger = new Mock<ILogger<MoveController>>(MockBehavior.Loose);
            return (new MoveController(uc.Object, logger.Object, log.Object), uc, log, logger);
        }

        [Fact]
        public async Task Get_Success()
        {
            var (ctrl, uc, _, _) = Make();
            uc.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<MoveResponseDto>
            {
                new MoveResponseDto { MoveId = Guid.NewGuid(), Amount = 10, MoveType = MoveType.Credito, AccountRefId = Guid.NewGuid(), Success = true }
            });

            var res = await ctrl.Get();
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<IEnumerable<MoveResponseDto>>>(json)!;

            payload.Data.Should().NotBeEmpty();
            uc.VerifyAll();
        }

        [Fact]
        public async Task GetById_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var id = Guid.NewGuid();
            uc.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new MoveResponseDto { MoveId = id, Amount = 5, MoveType = MoveType.Debito, AccountRefId = Guid.NewGuid() });

            var res = await ctrl.GetById(id);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var item = JsonSerializer.Deserialize<ApiResponse<MoveResponseDto>>(json)!;

            item.Data.MoveId.Should().Be(id);
            uc.VerifyAll();
        }

        [Fact]
        public async Task Create_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var dto = new MoveDto { AccountRefId = Guid.NewGuid(), Amount = 50, MoveType = MoveType.Credito };
            var created = new MoveResponseDto
            {
                MoveId = Guid.NewGuid(),
                Amount = 50,
                MoveType = MoveType.Credito,
                AccountRefId = dto.AccountRefId,
                Success = true
            };
            uc.Setup(x => x.CreateAsync(dto)).ReturnsAsync(created);

            var res = await ctrl.Create(dto);
            var createdRes = Assert.IsType<ObjectResult>(res); // 201
            var json = JsonSerializer.Serialize(createdRes.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<MoveResponseDto>>(json)!;

            payload.Data.Amount.Should().Be(50);
            uc.VerifyAll();
        }

        [Fact]
        public async Task GetByAccountId_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var accId = Guid.NewGuid();
            uc.Setup(x => x.GetByAccountIdAsync(accId)).ReturnsAsync(new List<MoveResponseDto>
            {
                new MoveResponseDto { MoveId = Guid.NewGuid(), Amount = 1, MoveType = MoveType.Credito, AccountRefId = accId }
            });

            var res = await ctrl.GetByAccountId(accId);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<IEnumerable<MoveResponseDto>>>(json)!;

            payload.Data.Should().NotBeEmpty();
            uc.VerifyAll();
        }

        [Fact]
        public async Task Report_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var req = new MoveReportDto { ClientId = Guid.NewGuid(), StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date };

            uc.Setup(x => x.GetMoveReportAsync(req)).ReturnsAsync(new List<MoveReportResponseDto>
            {
                new MoveReportResponseDto
                {
                    Client = "Jose",
                    Account = "123",
                    AccountType = "Ahorros",
                    Amount = 10,
                    InitialBalance = 100,
                    FinalBalance = 110,
                    Success = true,
                    TransactionDate = DateTime.UtcNow
                }
            });

            var res = await ctrl.GetMoveReport(req);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<IEnumerable<MoveReportResponseDto>>>(json)!;

            payload.Data.Should().NotBeEmpty();
            uc.VerifyAll();
        }
    }
}
