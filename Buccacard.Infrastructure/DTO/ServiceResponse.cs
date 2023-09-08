namespace Buccacard.Infrastructure.DTO
{
    public class ServiceResponse<T>
    {
        public string Message { get; set; }

        public bool Status { get; set; }

        public T Data { get; set; }
    }
}
