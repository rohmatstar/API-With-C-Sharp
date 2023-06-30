using API.Contracts;
using API.DTOs.Educations;
using API.Models;

namespace API.Services;

public class EducationService
{
    private readonly IEducationRepository _educationRepository;

    public EducationService(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    public IEnumerable<GetEducationDto>? GetEducation()
    {
        var educations = _educationRepository.GetAll();
        if (!educations.Any())
        {
            return null; // No educations found
        }

        var toDto = educations.Select(education =>
                                            new GetEducationDto
                                            {
                                                Guid = education.Guid,
                                                Major = education.Major,
                                                Degree = education.Degree,
                                                Gpa = education.Gpa
                                            }).ToList();

        return toDto; // Educations found
    }

    public IEnumerable<GetEducationDto>? GetEducation(string name)
    {
        var educations = _educationRepository.GetByName(name);
        if (!educations.Any())
        {
            return null; // No educations found
        }

        var toDto = educations.Select(education =>
                                            new GetEducationDto
                                            {
                                                Major = education.Major,
                                                Degree = education.Degree,
                                                Gpa = education.Gpa
                                            }).ToList();

        return toDto; // Educations found
    }

    public GetEducationDto? GetEducation(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);
        if (education is null)
        {
            return null; // Education not found
        }

        var toDto = new GetEducationDto
        {
            Major = education.Major,
            Degree = education.Degree,
            Gpa = education.Gpa
        };

        return toDto; // Educations found
    }

    public GetEducationDto? CreateEducation(NewEducationDto newEducationDto)
    {
        var education = new Education
        {
            Major = newEducationDto.Major,
            Degree = newEducationDto.Degree,
            Gpa = newEducationDto.Gpa,
            Guid = newEducationDto.Guid,
            UniversityGuid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null; // Education not created
        }

        var toDto = new GetEducationDto
        {
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            Gpa = createdEducation.Gpa
        };

        return toDto; // Education created
    }

    public int UpdateEducation(UpdateEducationDto updateEducationDto)
    {
        var isExist = _educationRepository.IsExist(updateEducationDto.Guid);
        if (!isExist)
        {
            return -1; // Education not found
        }

        var getEducation = _educationRepository.GetByGuid(updateEducationDto.Guid);

        var education = new Education
        {
            Guid = updateEducationDto.Guid,
            Major = updateEducationDto.Major,
            Degree = updateEducationDto.Degree,
            Gpa = updateEducationDto.Gpa,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEducation!.CreatedDate
        };

        var isUpdate = _educationRepository.Update(education);
        if (!isUpdate)
        {
            return 0; // Education not updated
        }

        return 1;
    }

    public int DeleteEducation(Guid guid)
    {
        var isExist = _educationRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Education not found
        }

        var education = _educationRepository.GetByGuid(guid);
        var isDelete = _educationRepository.Delete(education!);
        if (!isDelete)
        {
            return 0; // Education not deleted
        }

        return 1;
    }
}