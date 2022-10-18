using Core.Results.Abstract;
using Core.Results.Concrete;
using Data.DataTransferObjects.Request;
using Data.Entities.DataTransferObjects.Response;
using DataAccess.Repositories.Concrete;

namespace Business.Service.Abstract;

public interface IAdvertService
{
    public Task<IDataResult<IList<AdvertGetDto>?>> GetAllAdverts();
    public Task<IDataResult<AdvertGetDto?>> GetAdvertById(int id);
    public Task<IDataResult<AdvertGetDto?>> UpdateAdvert(UpdateAdvertRequestDto model);
    public Task<IDataResult<AdvertGetDto?>> AddAdvert(AddAdvertRequestDto model);
    public Task<IDataResult<bool>> DeleteAdvert(int id);


}