using RentACarProject.Domain.Entities;

public interface ILogRepository
{
    Task AddAsync(Log log);
}
