using Business.Service.Abstract;
using Business.Utilities;
using Core.Results.Abstract;
using Core.Results.Concrete;
using Data.Entities.DataTransferObjects.Response;
using Data.Entities.Models;
using Data.Enum;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Business.Extensions;
using Data.DataTransferObjects.Request;

namespace Business.Service.Concrete;

public class AdvertService:BaseService, IAdvertService
{
    public AdvertService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<IDataResult<IList<AdvertGetDto>?>> GetAllAdverts()
    {
        var adverts = await UnitOfWork.Adverts.FindBy().Select(x=>x.ToDto()).ToListAsync();
        if (adverts is null)
        {
            return new DataResult<IList<AdvertGetDto>?>(ResultStatusEnum.Error, "Adverts not found",null);
            
        }
        return new DataResult<IList<AdvertGetDto>?>(ResultStatusEnum.Success, adverts);
        
    }

    public async Task<IDataResult<AdvertGetDto?>> UpdateAdvert(UpdateAdvertRequestDto model)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x => x.Id == model.Id).FirstOrDefaultAsync();
        if (advert is null)
        {
            return new DataResult<AdvertGetDto?>(ResultStatusEnum.Error, "Advert not found",null);
            
        }

        advert.AdvertDescription = model.AdvertDescription;
        UnitOfWork.Adverts.Update(advert);
        await UnitOfWork.SaveAsync();
        return new DataResult<AdvertGetDto>(ResultStatusEnum.Success, "Advert deleted successfully",advert.ToDto());


    }

    public async Task<IDataResult<AdvertGetDto?>> AddAdvert(AddAdvertRequestDto model)
    {
        if (model is null)
        {
            return new DataResult<AdvertGetDto?>(ResultStatusEnum.Error, "Advert cannot be found",null);
        }

        var advert = await UnitOfWork.Adverts.InsertAsync(new Advert()
        {
            AdvertDescription = model.AdvertDescription
        });
        await UnitOfWork.SaveAsync();
        return new DataResult<AdvertGetDto>(ResultStatusEnum.Success, "Advert deleted successfully",advert.ToDto());

    }

    public async Task<IDataResult<bool>> DeleteAdvert(int id)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x=>x.Id == id).FirstOrDefaultAsync();
        
        if (advert is null)
        {
            return new DataResult<bool>(ResultStatusEnum.Error, "Advert not found",false);
        }

        UnitOfWork.Adverts.Delete(advert);
        await UnitOfWork.SaveAsync();
        return new DataResult<bool>(ResultStatusEnum.Success, "Advert deleted successfully",true);

    }

    public async Task<IDataResult<AdvertGetDto?>> GetAdvertById(int id)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x=>x.Id == id).FirstOrDefaultAsync();
        
        if (advert is null)
        {
            return new DataResult<AdvertGetDto?>(ResultStatusEnum.Error, "Advert not found",null);
            
        }
        return new DataResult<AdvertGetDto?>(ResultStatusEnum.Success, advert.ToDto());
    }


}