using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper,IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(Category => true).ToListAsync(); 

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories),200); 
        }
        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categorydto)
        {
            var categorys = _mapper.Map<Category>(categorydto);

            await _categoryCollection.InsertOneAsync(categorys);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(categorys),200);
        }

        public async Task<Response<CategoryDto>>GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find<Category>(x=>x.Id== id).FirstOrDefaultAsync();
            if (category == null)
            {
                return Response<CategoryDto>.Fail("category not found", 404);
            }
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);
        }
    }
}
