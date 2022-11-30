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
    public Task<IDataResult<IList<Advert>>> GetAllAdverts();
    public Task<IDataResult<Advert>> GetAdvertById(int id);
    public Task<IDataResult<Advert>> UpdateAdvert(Advert model);
    public Task<IDataResult<Advert>> AddAdvert(Advert model);
    public Task<IDataResult<bool>> DeleteAdvert(int id);
    public Task<PagedList<AdvertGetDto>> GetAdvertsPaginated(SearchAdvertRequest model,PaginationFilter filter,string orderBy);


}