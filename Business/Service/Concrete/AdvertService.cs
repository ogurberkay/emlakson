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
using Core;
using Core.Extensions;
using Core.Results.Filter;
using Core.Utilities;
using Data.DataTransferObjects;
using Data.DataTransferObjects.Request;

namespace Business.Service.Concrete;

public class AdvertService : BaseService, IAdvertService
{
    public AdvertService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<IDataResult<IList<Advert>>> GetAllAdverts()
    {
        var adverts = await UnitOfWork.Adverts.FindBy().ToListAsync();
        if (adverts is null)
        {
            return new DataResult<IList<Advert>>(ResultStatusEnum.Error, "Adverts not found", null);
        }

        return new DataResult<IList<Advert>>(ResultStatusEnum.Success, adverts);
    }

    public async Task<IDataResult<Advert>> UpdateAdvert(Advert model)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x => x.Id == model.Id).FirstOrDefaultAsync();
        if (advert is null)
        {
            return new DataResult<Advert>(ResultStatusEnum.Error, "Advert not found", null);
        }

        //advert.AdvertDescription = model.AdvertDescription;
        UnitOfWork.Adverts.Update(advert);
        await UnitOfWork.SaveAsync();
        return new DataResult<Advert>(ResultStatusEnum.Success, "Advert deleted successfully", advert);
    }

    public async Task<IDataResult<Advert>> AddAdvert(Advert model)
    {
        if (model is null)
        {
            return new DataResult<Advert>(ResultStatusEnum.Error, "Advert cannot be found", null);
        }

        var advert = await UnitOfWork.Adverts.InsertAsync(new Advert()
        {
            Title = model.Title,
            Price = model.Price,
            Meters = model.Meters,
            Location = model.Location,
            HouseType = model.HouseType,
            ExtraAttributes = model.ExtraAttributes,
            District = model.District,
            Description = model.Description,
            BedroomNumber = model.BedroomNumber,
            BathroomNumber = model.BathroomNumber,
            AdvertType = model.AdvertType,
        });
        await UnitOfWork.SaveAsync();
        return new DataResult<Advert>(ResultStatusEnum.Success, "Advert deleted successfully", advert);
    }

    public async Task<IDataResult<bool>> DeleteAdvert(int id)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x => x.Id == id).FirstOrDefaultAsync();

        if (advert is null)
        {
            return new DataResult<bool>(ResultStatusEnum.Error, "Advert not found", false);
        }

        UnitOfWork.Adverts.Delete(advert);
        await UnitOfWork.SaveAsync();
        return new DataResult<bool>(ResultStatusEnum.Success, "Advert deleted successfully", true);
    }

    public async Task<IDataResult<Advert>> GetAdvertById(int id)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x => x.Id == id).FirstOrDefaultAsync();

        if (advert is null)
        {
            return new DataResult<Advert>(ResultStatusEnum.Error, "Advert not found", null);
        }

        return new DataResult<Advert>(ResultStatusEnum.Success, advert);
    }

    public async Task<PagedList<AdvertGetDto>> GetAdvertsPaginated(SearchAdvertRequest model, PaginationFilter filter,
        string orderBy = "Desc")
    {
        var data = UnitOfWork.Adverts.FindBy();


        if (!string.IsNullOrEmpty(model.SearchKeyWord))
            data = data.Where(x => string.Equals(x.Title, model.SearchKeyWord, StringComparison.OrdinalIgnoreCase)
                                   || string.Equals(x.Description, model.SearchKeyWord,
                                       StringComparison.OrdinalIgnoreCase));
        //AdvertType
        if (!string.IsNullOrEmpty(model.AdvertType))
        {
            int.TryParse(model.AdvertType, out int num);
            AdvertTypeEnum types = (AdvertTypeEnum) num;
            data = types switch
            {
                AdvertTypeEnum.Satilik => data.Where(x => x.AdvertType == AdvertTypeEnum.Satilik),
                AdvertTypeEnum.Kiralik => data = data.Where(x => x.AdvertType == AdvertTypeEnum.Kiralik),
                _ => data
            };
        }

        //HouseType
        if (!string.IsNullOrEmpty(model.HouseType))
        {
            int.TryParse(model.HouseType, out int num);
            HouseTypeEnum types = (HouseTypeEnum) num;
            data = types switch
            {
                HouseTypeEnum.Apartman => data.Where(x => x.HouseType == HouseTypeEnum.Apartman),
                HouseTypeEnum.Mustakil => data = data.Where(x => x.HouseType == HouseTypeEnum.Mustakil),
                _ => data
            };
        }

        //Lokasyon
        if (!string.IsNullOrEmpty(model.Location))
        {
            int.TryParse(model.Location, out int num);
            LocationEnum types = (LocationEnum) num;
            data = types switch
            {
                LocationEnum.Istanbul => data = data.Where(x => x.Location == LocationEnum.Istanbul),
                LocationEnum.Izmir => data = data.Where(x => x.Location == LocationEnum.Izmir),
                LocationEnum.Mersin => data = data.Where(x => x.Location == LocationEnum.Mersin),
                _ => data
            };
        }

        //Yatak odası
        if (model.BedroomNumber > 0)
        {
            data = data.Where(x => x.BedroomNumber == model.BedroomNumber);
        }

        //Banyo 
        if (model.BathroomNumber > 0)
        {
            data = data.Where(x => x.BathroomNumber == model.BathroomNumber);
        }

        //metrekare
        //TODO check.isnotnull model
        data = data.Where(x => x.Meters >= model.MetresStartValue && x.Meters <= model.MetresEndValue);

        //Fiyat aralığı
        data = data.Where(x => x.Price >= model.PricesStartValue && x.Meters <= model.PricesEndValue);

        var extraAttributesModel = new List<ExtraAttributeEnum>();
        var extraAttributesModelStrings = model?.ExtraAttributes?.Split(',');
        // var extraAttributes = data.Intersect(model.ex)
        if (extraAttributesModelStrings != null)
        {
            foreach (var item in extraAttributesModelStrings)
            {
                var englishStr = item.TurkishChrToEnglishChr();
                var strWithoutSpace = englishStr.Replace(" ", "");
                ExtraAttributeEnum attribute =
                    Enum.Parse<ExtraAttributeEnum>(strWithoutSpace.TurkishChrToEnglishChr(), true);
                extraAttributesModel.Add(attribute);
            }
        }

        extraAttributesModel.ForEach(item =>
        {
            data = data.Where(x => x.ExtraAttributes != null && x.ExtraAttributes.Contains(item));
        });

        var advertGetDtos = data.Select(x => new AdvertGetDto()
        {
            AdvertType = x.AdvertType,
            BathroomNumber = x.BathroomNumber,
            BedroomNumber = x.BedroomNumber,
            Description = x.Description,
            District = x.District,
            ExtraAttributes = x.ExtraAttributes,
            HouseType = x.HouseType,
            Location = x.Location,
            Meters = x.Meters,
            Price = x.Price,
            Title = x.Title,
            IsFeatured = x.IsFeatured,
        });


        // .OrderBy(x=);
        return await PagedList<AdvertGetDto>.ToPagedList(advertGetDtos, filter.PageNumber, filter.PageSize);
    }
}