using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEWallet.Database.Models;
using MiniEWallet.Domains.Services;

namespace MiniEWallet.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ResponseController
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
            return Execute(data);
        }
    }
}
