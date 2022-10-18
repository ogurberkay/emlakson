using Newtonsoft.Json;
namespace Business.Misc;

public class ApiResponse
{
    public string Version { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int StatusCode { get; set; }

    public string Message { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? IsError { get; set; }

    public object ResponseException { get; set; }

    public object Result { get; set; }

    [JsonConstructor]
    public ApiResponse(string message, object result = null, int statusCode = 200, string apiVersion = "1.0.0.0")
    {
        this.StatusCode = statusCode;
        this.Message = message;
        this.Result = result;
        this.Version = apiVersion;
    }
    
    public ApiResponse(int statusCode = 200,object result = null,string apiVersion = "1.0.0.0")
    {
        this.StatusCode = statusCode;
        this.Result = result;
        this.Version = apiVersion;
    }

    public ApiResponse(object result, int statusCode = 200)
    {
        this.StatusCode = statusCode;
        this.Result = result;
    }

    public ApiResponse(int statusCode, object apiError)
    {
        this.StatusCode = statusCode;
        this.ResponseException = apiError;
        this.IsError = new bool?(true);
    }

    public ApiResponse(int statusCode)
    {
        this.StatusCode = statusCode;
    }
}