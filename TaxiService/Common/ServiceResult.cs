using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaxiService.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }

        public static ServiceResult Ok(string message = "Success") =>
            new() { Success = true, Message = message, StatusCode = StatusCodes.Status200OK };

        public static ServiceResult Created(string message = "Created") =>
            new() { Success = true, Message = message, StatusCode = StatusCodes.Status201Created };

        public static ServiceResult Fail(string message, int statusCode = 400) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status400BadRequest };

        public static ServiceResult NotFound(string message, int statusCode = 404) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status404NotFound };

        public static ServiceResult Conflict(string message, int statusCode = 409) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status409Conflict };

        public static ServiceResult Unauthorized(string message, int statusCode = 401) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status401Unauthorized };

        public static ServiceResult Forbidden(string message, int statusCode = 403) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status403Forbidden };

    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(T data, string message = "Success") =>
            new() { Success = true, Message = message, StatusCode = StatusCodes.Status200OK, Data = data };

        public static ServiceResult<T> Created(T data, string message = "Created") =>
            new() { Success = true, Message = message, StatusCode = StatusCodes.Status201Created, Data = data };

        public static new ServiceResult<T> Fail(string message, int statusCode = 400) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status400BadRequest };

        public static new ServiceResult<T> NotFound(string message, int statusCode = 404) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status404NotFound };

        public static new ServiceResult<T> Conflict(string message, int statusCode = 409) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status409Conflict };

        public static new ServiceResult<T> Unauthorized(string message, int statusCode = 401) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status401Unauthorized };

        public static new ServiceResult<T> Forbidden(string message, int statusCode = 403) =>
            new() { Success = false, Message = message, StatusCode = StatusCodes.Status403Forbidden };
    }
}
