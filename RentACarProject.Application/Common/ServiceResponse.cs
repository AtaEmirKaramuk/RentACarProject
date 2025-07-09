namespace RentACarProject.Application.Common
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "İşlem başarılı.";
        public string? Code { get; set; }
        public T? Data { get; set; }
    }
}
