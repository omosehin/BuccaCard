using Buccacard.Infrastructure.DTO;

namespace Buccacard.Infrastructure.Utility
{
    public interface IResponseService
    {
        ServiceResponse<T> ErrorResponse<T>(string ?message = null);
        ServiceResponse<T> SuccessResponse<T>(T data, string? message =null);
        ServiceResponse<T> ExceptionResponse<T>(Exception exception) where T : class;

    }

    public class ResponseService : IResponseService
    {
        public ServiceResponse<T> ErrorResponse<T>(string? message) => new ServiceResponse<T> { Message = message ?? "Unsuccessful Operation" };

        public ServiceResponse<T> ExceptionResponse<T>(Exception exception) where T : class =>
            new ServiceResponse<T> { Message = exception.Message };

        public ServiceResponse<T> SuccessResponse<T>(T data, string? message = null)
             => new ServiceResponse<T>
             {
                 Message = message ?? "Successful Operation",
                 Status = true,
                 Data = data
             };
    }
}
