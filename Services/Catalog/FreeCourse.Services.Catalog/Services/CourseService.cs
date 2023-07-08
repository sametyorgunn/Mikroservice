using Amazon.Runtime.Internal.Util;
using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CategoryCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var Courses = await _courseCollection.Find(Course => true).ToListAsync();
            if (Courses.Any())
            {
                foreach(var course in Courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x=>x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                Courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(Courses), 200);
        }
      

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var courses = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
            if (courses == null)
            {
                return Response<CourseDto>.Fail("course not found", 404);
            }
            courses.Category = await _categoryCollection.Find<Category>(x=>x.Id==courses.Id).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByUserIdAsync(string id)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == id).FirstOrDefaultAsync();
            if (courses == null)
            {
                return Response<CourseDto>.Fail("course not found", 404);
            }
            courses.Category = await _categoryCollection.Find<Category>(x => x.Id == courses.Id).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(courses), 200);
        }
        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto dto)
        {
            var newcourse = _mapper.Map<Course>(dto);
            newcourse.Created = DateTime.Now;

            await _courseCollection.InsertOneAsync(newcourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newcourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto dto)
        {
            var updatecourse = _mapper.Map<Course>(dto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == dto.Id, updatecourse);

            if(result == null)
            {
                return Response<NoContent>.Fail("course not founded", 404);
            }
            
            return Response<NoContent>.Success(204);
            
        }
        
        public async Task<Response<NoContent>> DeleleteAsync(string id)
        { 
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if(result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("course not found", 404);
            }
            
        }
    }
}
