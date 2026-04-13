using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEWallet.Database.Models;
using MiniEWallet.Domains.Services;

namespace MiniEWallet.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccServices _service;

        public AccountsController() 
        { 
            _service = new AccServices();
        }

        [HttpGet("AllAcc")]
        public async Task<IActionResult> GetAcc()
        {
            var data = await _service.GetAccs();
            return Ok(data);
        }

        [HttpGet("AccById")]
        public async Task<IActionResult> GetAccById(int id)
        {
            var data = await _service.GetAccById(id);
            return Ok(data);
        }

        [HttpPost("CreateAcc")]
        public async Task<IActionResult> CreateAccount(TblAccount account)
        {
            var data = await _service.CreateAccount(account);
            return Ok(data);
        }

        [HttpPut("MakeDeposit")]
        public async Task<IActionResult> MakeDeposit(int id,int amount)
        {
            var data = await _service.MakeDeposit(id, amount);
            return Ok(data);
        }

        [HttpPut("MakeTransfer")]
        public async Task<IActionResult> MakeTransfer(int frid,int toid,int pass,int amt)
        {
            var data = await _service.MakeTransfer(frid, toid, pass, amt);
            return Ok(data);
        }

        [HttpPut("MakeWithDrawl")]
        public async Task<IActionResult> MakeWithDrawl(int id,int pass,int amount)
        {
            var data = await _service.MakeWithDrawl(id, pass, amount);
            return Ok(data);
        }

        [HttpPost("CreateTranType")]
        public async Task<IActionResult> CreateTranType(string description)
        {
            var data = await _service.CreateTranType(description);
            return Ok(data);
        }
    }
}
