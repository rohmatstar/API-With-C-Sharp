using API.Models;

namespace API.Contracts;

public interface IEducationRepository : IGeneralRepository<Education>
{
    IEnumerable<Education> GetByName(string name);
}
