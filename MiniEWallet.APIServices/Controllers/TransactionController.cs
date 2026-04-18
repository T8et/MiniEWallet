using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MiniEWallet.Domains.Services;

namespace MiniEWallet.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ResponseController
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
            return Execute(data);
        }

        [HttpPut("MakeTransfer")]
        public async Task<IActionResult> MakeTransfer(int frid, int toid, int pass, int amt)
        {
            var data = await _service.MakeTransfer(frid, toid, pass, amt);
            return Execute(data);
        }

        [HttpPut("MakeWithDrawl")]
        public async Task<IActionResult> MakeWithDrawl(int id, int pass, int amount)
        {
            var data = await _service.MakeWithDrawl(id, pass, amount);
            return Execute(data);
        }

        [HttpPost("CreateTranType")]
        public async Task<IActionResult> CreateTranType(string description)
        {
            var data = await _service.CreateTranType(description);
            return Execute(data);
        }
    }
}
