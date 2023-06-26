using API.DTOs.Universities;

namespace API.Contracts;

public interface IEducationService
{
    IEnumerable<GetEducationDto> GetEducation();
}