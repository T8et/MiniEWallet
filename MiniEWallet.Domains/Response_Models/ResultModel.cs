using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEWallet.Domains.Response_Models;

public class ResultModel<T>
{
    public bool isSuccess { get; set; }

    public bool isError { get { return !isSuccess; } }

    public bool isValidationError {  get { return RespType == EnumResType.ValidationError; } }

    public bool isSystemError { get { return RespType == EnumResType.SystemError; } }

    public bool isdatanotexist { get { return RespType == EnumResType.DataNotExist; } }

    private EnumResType RespType { get; set; }

    public string? RespCode { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }


    public static ResultModel<T> Success(T data, string Message)
    {
        return new ResultModel<T>()
        {
            isSuccess = true,
            Data = data,
            Message = Message,
            RespType = EnumResType.Success,
        };
    }

    public static ResultModel<T> ValidationError(string Message, T? data = default)
    {
        return new ResultModel<T>()
        {
            isSuccess = false,
            Data = data,
            Message = Message,
            RespType = EnumResType.ValidationError,
        };
    }

    public static ResultModel<T> DataNotExist(string Message, T? data = default)
    {
        return new ResultModel<T>()
        {
            isSuccess = false,
            Data = data,
            Message = Message,
            RespType = EnumResType.ValidationError,
        };
    }

    public static ResultModel<T> SystemError(string Message, T? data = default)
    {
        return new ResultModel<T>()
        {
            isSuccess = false,
            Data = data,
            Message = Message,
            RespType = EnumResType.SystemError,
        };
    }
}

public enum EnumResType
{
    None,
    Success,
    ValidationError,
    SystemError,
    DataNotExist
}

