using Core.Results.Abstract;
using Core.Results.Concrete;
using Core.Results.Filter;
using Core.Utilities;
using Data.DataTransferObjects.Request;
using Data.Entities.DataTransferObjects.Response;
using Data.Entities.Models;
using DataAccess.Repositories.Concrete;

namespace Business.Service.Abstract;

public interface IAdvertService
{
    public Task<IDataResult<IList<AdvertGetDto>?>> GetAllAdverts();
    public Task<IDataResult<AdvertGetDto?>> GetAdvertById(int id);
    public Task<IDataResult<AdvertGetDto?>> UpdateAdvert(UpdateAdvertRequestDto model);
    public Task<IDataResult<AdvertGetDto?>> AddAdvert(AddAdvertRequestDto model);
    public Task<IDataResult<bool>> DeleteAdvert(int id);
    public Task<PagedList<AdvertGetDto>> GetAdvertsPaginated(SearchAdvertRequest model,PaginationFilter filter,string orderBy);


}