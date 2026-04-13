using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEWallet.Domains.Response_Models;

public class BaseResponseModel
{
    public string? RespCode { get; set; }

    public string? RespDesc { get; set; }

    public EnumRspType RespType { get; set; }

    public bool isSuccess { get; set; }

    public bool isError { get { return !isSuccess; } }

    public static BaseResponseModel Success(string resp_code, string resp_desc)
    {
        return new BaseResponseModel()
        {
            isSuccess = true,
            RespCode = resp_code,
            RespDesc = resp_desc,
            RespType = EnumRspType.Success,
        };
    }

    public static BaseResponseModel ValidationError(string resp_code, string resp_desc)
    {
        return new BaseResponseModel()
        {
            isSuccess = false,
            RespCode = resp_code,
            RespDesc = resp_desc,
            RespType = EnumRspType.ValidationError,
        };
    }
}

public enum EnumRspType
{
    None,
    Success,
    ValidationError,
    SystemError
}