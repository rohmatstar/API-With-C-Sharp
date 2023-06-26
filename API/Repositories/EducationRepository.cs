using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EducationRepository : GeneralRepository<Education>, IEducationRepository
    {
        public EducationRepository(BookingDbContext context) : base(context) { }

        public IEnumerable<Education> GetByName(string name)
        {
            return _context.Set<Education>().Where(u => u.Major.Contains(name));
        }
    }
}