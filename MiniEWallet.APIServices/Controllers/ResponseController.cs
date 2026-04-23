using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEWallet.Domains.Response_Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniEWallet.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        [HttpGet("Response")]
        public IActionResult Execute(object model)
        {
            JObject retObj = JObject.Parse(JsonConvert.SerializeObject(model));

            if (retObj.ContainsKey("trnRsp") || retObj.ContainsKey("respose"))
            {
                BaseResponseModel Tmodel = retObj.ContainsKey("trnRsp") ?
                    JsonConvert.DeserializeObject<BaseResponseModel>(retObj["trnRsp"]!.ToString())!
                    :
                    JsonConvert.DeserializeObject<BaseResponseModel>(retObj["respose"]!.ToString())!;

                if (Tmodel.RespType == EnumRspType.DataNotExist)
                {
                    return BadRequest(Tmodel);
                }
                if (Tmodel.RespType == EnumRspType.ValidationError)
                {
                    return BadRequest(Tmodel);
                }
                if (Tmodel.RespType == EnumRspType.SystemError)
                {
                    return StatusCode(500, Tmodel);
                }
                return Ok(Tmodel);
            }
            return StatusCode(503, "Invalid Response Model");
        }

        public IActionResult Execute1<T>(ResultModel<T> model)
        {
            if (model.isSuccess)
            {
                return Ok(model);
            }
            if (model.isValidationError)
            {
                return BadRequest(model);
            }
            if (model.isSystemError)
            {
                return BadRequest(model);
            }
            return Ok(model);
        }

    }
}
