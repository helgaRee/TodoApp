using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class CategoryService(CategoryRepository categoryRepository)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;



    /// <summary>
    /// Method that creates a category if it doesnt exist
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns>Returns a new Category if it doesnt exist, else return false.</returns>
    public async Task<bool> CreateCategoryAsync(string categoryName)
    {
        try
        {
            if (!await _categoryRepository.ExistingAsync(x => x.CategoryName == categoryName))
            {
                var categoryEntity = await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = categoryName });
                if (categoryEntity != null)
                {
                    return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }


/// <summary>
/// Method gets an existing Category and returns a CategoryDto which contains CategoryName and CategoryId
/// </summary>
/// <param name="predicate"></param>
/// <returns>Returns a Dto of Categorie if not already exists, else return null.</returns>
    public async Task<CategoryDto> GetCategoryAsync(Expression<Func<CategoryEntity, bool>>predicate)
    {
        try
        {
           var categoryEntity = await _categoryRepository.GetAsync(predicate);
            if (categoryEntity != null)
            {         
                var categoryDto = new CategoryDto(categoryEntity.CategoryId, categoryEntity.CategoryName);
                return categoryDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return null!;
    }


/// <summary>
/// Gets all categories
/// </summary>
/// <returns>Returns a list of all categories</returns>
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        try
        {
            var categoryEntities = await _categoryRepository.GetAllAsync();

            if (categoryEntities != null)
            {
                var list = new List<CategoryDto>();
                foreach (var categoryEntity in categoryEntities)
                    list.Add(new CategoryDto(categoryEntity.CategoryId, categoryEntity.CategoryName));
                return list;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return null!;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto dto)
    {
        try
        {
            //Skapar en ny instans av CategoryEntity och stoppar in CategoryId och CategoryName som hämtas från updatedCAtegoryDto.
            //Den nya Categorin ska uppdateras med värden från updatedCategoryDto. "skapar en kopia av entiteten och tjoffar in värden"
            var categoryEntity = new CategoryEntity 
            { 
                CategoryId = dto.Id, 
                CategoryName = dto.CategoryName 
            };
                      
            //Asynkront anrop till metoden UpdateAsync.  Den tar 2 parametrar, ett lambdauttryck för att filtrera vilken kategori som ska uppdateras
            //baserat på CategoryId, samt den nya kategorin som ska uppdateras
            var updatedCategoryEntity = await _categoryRepository.UpdateAsync(x => x.CategoryId == dto.Id, categoryEntity);

            if(updatedCategoryEntity != null)
            {
                var categoryDto = new CategoryDto(updatedCategoryEntity.CategoryId, updatedCategoryEntity.CategoryName);
                return categoryDto;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return null!;
    }


/// <summary>
/// 
/// </summary>
/// <param name="expression"></param>
/// <returns></returns>

    public async Task<bool> DeleteCategoryAsync(Expression<Func<CategoryEntity, bool>> expression)
    {
        try
        {
            var result = await _categoryRepository.DeleteAsync(expression);                     
            return result;           
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return false;
    }

}
