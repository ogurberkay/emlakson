using Core.Results.Abstract;
using Data.Enum;

namespace Core.Results.Concrete;

public class DataResult<T> : IDataResult<T>
{

    public DataResult(ResultStatusEnum resultStatusEnum, T data)
    {
        ResultStatus = resultStatusEnum;
        Data = data;
    }

    public DataResult(ResultStatusEnum resultStatusEnum, string message, T data)
    {
        ResultStatus = resultStatusEnum;
        Message = message;
        Data = data;
    }

    public DataResult(ResultStatusEnum resultStatusEnum, string message, T data, Exception exception)
    {
        ResultStatus = resultStatusEnum;
        Message = message;
        Data = data;
        Exception = exception;
    }
    public ResultStatusEnum ResultStatus { get; }
    public string Message { get; }
    public Exception Exception { get; set; }
    public T Data { get; }
}
