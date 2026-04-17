using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEWallet.Domains.Services;

namespace MiniEWallet.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TranServices _service;

        public TransactionController()
        {
            _service = new TranServices();
        }

        [HttpPut("MakeDeposit")]
        public async Task<IActionResult> MakeDeposit(int id, int amount)
        {
            var data = await _service.MakeDeposit(id, amount);
            return Ok(data);
        }

        [HttpPut("MakeTransfer")]
        public async Task<IActionResult> MakeTransfer(int frid, int toid, int pass, int amt)
        {
            var data = await _service.MakeTransfer(frid, toid, pass, amt);
            return Ok(data);
        }

        [HttpPut("MakeWithDrawl")]
        public async Task<IActionResult> MakeWithDrawl(int id, int pass, int amount)
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
