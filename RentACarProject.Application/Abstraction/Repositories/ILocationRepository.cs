using RentACarProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<List<Location>> GetAllLocationsAsync();

        Task<Location?> GetLocationByIdAsync(Guid locationId);
    }
}
