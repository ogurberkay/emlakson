using Data.Enum;

namespace Core.Results.Abstract;

public interface IResult
{
    public ResultStatusEnum ResultStatus { get; }
    public string Message { get;  }
    public Exception Exception { get; set; }
}