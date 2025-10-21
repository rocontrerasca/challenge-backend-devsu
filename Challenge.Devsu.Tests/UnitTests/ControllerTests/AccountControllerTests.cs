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
    public class AccountControllerTests
    {
        private (AccountController ctrl, Mock<IAccountUseCase> uc, Mock<ILogUseCase> log, Mock<ILogger<AccountController>> logger) Make()
        {
            var uc = new Mock<IAccountUseCase>(MockBehavior.Strict);
            var log = new Mock<ILogUseCase>(MockBehavior.Loose);
            var logger = new Mock<ILogger<AccountController>>(MockBehavior.Loose);
            return (new AccountController(uc.Object, logger.Object, log.Object), uc, log, logger);
        }

        [Fact]
        public async Task Get_Success()
        {
            var (ctrl, uc, _, _) = Make();
            uc.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<AccountResponseDto>
            {
                new AccountResponseDto { AccountId = Guid.NewGuid(), AccountNumber = "123", AccountType = AccountType.Ahorros, Active = true }
            });

            var res = await ctrl.Get();
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<IEnumerable<AccountResponseDto>>>(json)!;
            payload.Data.Should().NotBeEmpty();
            uc.VerifyAll();
        }

        [Fact]
        public async Task GetById_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var id = Guid.NewGuid();
            uc.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new AccountResponseDto { AccountId = id, AccountNumber = "123", AccountType = AccountType.Ahorros });

            var res = await ctrl.GetById(id);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var item = JsonSerializer.Deserialize<ApiResponse<AccountResponseDto>>(json)!;
            item.Data.AccountId.Should().Be(id);
            uc.VerifyAll();
        }

        [Fact]
        public async Task Create_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var dto = new AccountDto { AccountNumber = "123", AccountType = AccountType.Ahorros, InitialBalance = 100, ClientRefId = Guid.NewGuid(), Active = true };
            var created = new AccountResponseDto { AccountId = Guid.NewGuid(), AccountNumber = dto.AccountNumber, AccountType = dto.AccountType, Active = true };
            uc.Setup(x => x.CreateAsync(dto)).ReturnsAsync(created);

            var res = await ctrl.Create(dto);
            var ok = Assert.IsType<ObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<AccountResponseDto>>(json)!;
            payload.Data.AccountNumber.Should().Be("123");
            uc.VerifyAll();
        }

        [Fact]
        public async Task Update_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var upd = new AccountUpdateDto { AccountId = Guid.NewGuid(), AccountNumber = "999", AccountType = AccountType.Corriente, Active = true };
            uc.Setup(x => x.UpdateAsync(upd)).ReturnsAsync(upd);

            var res = await ctrl.Update(upd);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<AccountUpdateDto>>(json)!;
            payload.Data.AccountNumber.Should().Be("999");
            uc.VerifyAll();
        }

        [Fact]
        public async Task DeleteById_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var id = Guid.NewGuid();
            uc.Setup(x => x.DeleteByIdAsync(id)).ReturnsAsync(new AccountResponseDto { AccountId = id });

            var res = await ctrl.DeleteById(id);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<AccountResponseDto>>(json)!;
            payload.Data.AccountId.Should().Be(id);
            uc.VerifyAll();
        }

        [Fact]
        public async Task GetByClientId_Success()
        {
            var (ctrl, uc, _, _) = Make();
            var clientId = Guid.NewGuid();
            uc.Setup(x => x.GetByClientId(clientId)).ReturnsAsync(new List<AccountResponseDto>
            {
                new AccountResponseDto { AccountId = Guid.NewGuid(), AccountNumber = "C1", AccountType = AccountType.Ahorros, Active = true }
            });

            var res = await ctrl.GetByClientId(clientId);
            var ok = Assert.IsType<OkObjectResult>(res);
            var json = JsonSerializer.Serialize(ok.Value);
            var payload = JsonSerializer.Deserialize<ApiResponse<IEnumerable<AccountResponseDto>>>(json)!;
            payload.Data.Should().NotBeEmpty();
            uc.VerifyAll();
        }
    }
}
