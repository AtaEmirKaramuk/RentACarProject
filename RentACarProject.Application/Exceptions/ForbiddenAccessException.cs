namespace RentACarProject.Application.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message = "Bu kaynağa erişim izniniz yok.") : base(message)
        {
        }
    }
}
