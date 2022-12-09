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
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Business.Service.Concrete;

public class AdvertService : BaseService, IAdvertService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public AdvertService(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment) : base(unitOfWork)
    {
        this._hostEnvironment = hostEnvironment;

    }

    public async Task<IDataResult<List<AdvertGetDto>>> GetAllAdverts()
    {
        var adverts = await UnitOfWork.Adverts.FindBy().Select(x => x.ToDto()).ToListAsync();
        if (adverts is null)
        {
            return new DataResult<List<AdvertGetDto>>(ResultStatusEnum.Error, "Adverts not found", null);
        }

        return new DataResult<List<AdvertGetDto>>(ResultStatusEnum.Success, adverts);
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
        try
        {
            var advert = new Advert()
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
                ImageFile = new Image()
            };

            //Save image to wwwroot/image
            var withoutExtension = Path.GetFileNameWithoutExtension(model.ImageFile.ImageFile.FileName);
            var uniqueFileName = StringExtensions.GetUniqueFileName(withoutExtension);

            var uploads = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "img", uniqueFileName);

            var filePath = Path.Combine(uploads, model.ImageFile.ImageFile.FileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await model.ImageFile.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

            advert.ImageFile.Title = model.ImageFile.Title;
            advert.ImageFile.ImageName = model.ImageFile.ImageName;
            advert.ImageFile.ImageFile = model.ImageFile.ImageFile;
            advert.ImageFile.ImagePath = filePath;

            await UnitOfWork.Adverts.InsertAsync(advert);
            await UnitOfWork.SaveAsync();
            return new DataResult<Advert>(ResultStatusEnum.Success, "Advert deleted successfully", advert);

        }

        catch (Exception ex)
        {
            throw;
        }



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

    public async Task<List<AdvertGetDto>> GetAdvertsPaginated(SearchAdvertRequest model, PaginationFilter filter,
        int orderBy = 1)
    {
        var data = UnitOfWork.Adverts.FindBy();


        if (!string.IsNullOrEmpty(model.SearchKeyWord))
            data = data.Where(x => x.Title.Contains(model.SearchKeyWord)
                                   || x.Description.Contains(model.SearchKeyWord));
        //AdvertType
        if (!string.IsNullOrEmpty(model.AdvertType))
        {
            int.TryParse(model.AdvertType, out int num);
            AdvertTypeEnum types = (AdvertTypeEnum)num;
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
            HouseTypeEnum types = (HouseTypeEnum)num;
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
            LocationEnum types = (LocationEnum)num;
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

        ////OrderBy
        //if (orderBy > 0)
        //{
        //    data = orderBy switch
        //    {
        //        1 => data.OrderByDescending(x=>x.CreatedDate),
        //        2 => data = data.OrderByDescending(x => x.Price),
        //        3 => data = data.OrderBy(x=>x.Price),
        //        _ => data
        //    };
        //}

        var advertGetDtos = await data.Select(x => new AdvertGetDto()
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
            IsFeatured = x.IsFeatured,
        }).ToListAsync();

        return advertGetDtos;

    }


}