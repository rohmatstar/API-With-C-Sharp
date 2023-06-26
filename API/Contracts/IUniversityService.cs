using API.DTOs.Universities;

namespace API.Contracts;

public interface IUniversityService
{
    IEnumerable<GetEducationDto> GetUniversity();
}