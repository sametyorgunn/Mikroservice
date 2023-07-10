using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public interface ICourseService
    {
        Task<Response<List<CourseDto>>> GetAllAsync();
        Task<Response<CourseDto>> GetByIdAsync(string id);
        Task<Response<CourseDto>> GetByUserIdAsync(string id);
        Task<Response<CourseDto>> CreateAsync(CourseCreateDto dto);
        Task<Response<NoContent>> UpdateAsync(CourseUpdateDto dto);
        Task<Response<NoContent>> DeleleteAsync(string id);
    }
}
