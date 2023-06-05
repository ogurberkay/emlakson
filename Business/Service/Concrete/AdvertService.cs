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
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Data.DataTransferObjects.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Business.Service.Concrete;

public class AdvertService : BaseService, IAdvertService
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SmtpClient smtp;

    public AdvertService(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment) : base(unitOfWork)
    {
        smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("rizamertyagci@gmail.com", "myiodpjkqpddwbss")
        };

        _unitOfWork = unitOfWork;
        this._hostEnvironment = hostEnvironment;
        this.smtp = smtp;
    }

    public async Task<bool> SendEmailToCustomers(Advert? advert, Customer? customer)
    {
        try
        {
            if (customer is not null)
            {
                var adverts = await _unitOfWork.Repository<Advert>().FindBy()
                    .Include(x => x.AdvertExtraAttributes)
                    .ToListAsync();

                foreach (var item in adverts)
                {
                    if (item.Location != customer.Location)
                        continue;
                    if (item.AdvertType != customer.AdvertType)
                        continue;
                    if (item.HouseType != customer.HouseType)
                        continue;
                    if (item.Price > customer.Price)
                        continue;

                    string itemDistrict = ReplaceTurkishCharacters(item.District);
                    string customerDistrict = ReplaceTurkishCharacters(customer.District);

                    if (!string.Equals(itemDistrict, customerDistrict, StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (item.BathroomNumber < customer.BathroomNumber)
                        continue;
                    if (item.BedroomNumber < customer.BedroomNumber)
                        continue;

                    // Ekstra özelliklerin kontrolü
                    bool hasMissingExtraAttribute = false;
                    foreach (var customerAttribute in customer.CustomerExtraAttributes)
                    {
                        bool hasMatch = false;
                        foreach (var itemAttribute in item.AdvertExtraAttributes)
                        {
                            if (customerAttribute.ExtraAttributeId == itemAttribute.ExtraAttributeId)
                            {
                                hasMatch = true;
                                break;
                            }
                        }

                        if (!hasMatch)
                        {
                            hasMissingExtraAttribute = true;
                            break;
                        }
                    }

                    if (hasMissingExtraAttribute)
                        continue;
                    if (item.Meters < customer.Meters)
                        continue;
                    SendEmail("rizamertyagci@gmail.com", "Mert",
                        new List<string>() {"rizamertyagci@gmail.com"}, null,
                        null, $"Müşteri bulundu", $"<p> <bold>{customer.Name} {customer.Surname}</bold>" +
                                                  $" isimli müşteri </br> {item.Location} {item.District} lokasyonunda bulunan {item.Price}" +
                                                  $" fiyatındaki {item.HouseType} tipindeki {item.AdvertType} ev için uygun bulunmuştur.</br></br> İlanın adı: {item.Title}" +
                                                  $"</br> </br>  İlanın açıklaması: {item.Description} </p>", null);
                }
            }
            else if (advert is not null)
            {
                var customers = await _unitOfWork.Repository<Customer>().FindBy()
                    .Include(x => x.CustomerExtraAttributes)
                    .ToListAsync();

                 foreach (var item in customers)
                {
                    if (advert.Location != item.Location)
                        continue;
                    if (advert.AdvertType != item.AdvertType)
                        continue;
                    if (advert.HouseType != item.HouseType)
                        continue;
                    if (advert.Price > item.Price)
                        continue;

                    string itemDistrict = ReplaceTurkishCharacters(advert.District);
                    string customerDistrict = ReplaceTurkishCharacters(item.District);

                    if (!string.Equals(itemDistrict, customerDistrict, StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (advert.BathroomNumber < item.BathroomNumber)
                        continue;
                    if (advert.BedroomNumber < item.BedroomNumber)
                        continue;

                    // Ekstra özelliklerin kontrolü
                    bool hasMissingExtraAttribute = false;
                    foreach (var customerAttribute in item.CustomerExtraAttributes)
                    {
                        bool hasMatch = false;
                        foreach (var itemAttribute in advert.AdvertExtraAttributes)
                        {
                            if (customerAttribute.ExtraAttributeId == itemAttribute.ExtraAttributeId)
                            {
                                hasMatch = true;
                                break;
                            }
                        }

                        if (!hasMatch)
                        {
                            hasMissingExtraAttribute = true;
                            break;
                        }
                    }

                    if (hasMissingExtraAttribute)
                        continue;
                    if (advert.Meters < customer.Meters)
                        continue;
                    SendEmail("rizamertyagci@gmail.com", "Mert",
                        new List<string>() {"rizamertyagci@gmail.com"}, null,
                        null, $"Müşteri bulundu", $"<p> <bold>{item.Name} {item.Surname}</bold>" +
                                                  $" isimli müşteri </br> {advert.Location} {advert.District} lokasyonunda bulunan {advert.Price}" +
                                                  $" fiyatındaki {advert.HouseType} tipindeki {advert.AdvertType} ev için uygun bulunmuştur.</br></br> İlanın adı: {advert.Title}" +
                                                  $"</br> </br>  İlanın açıklaması: {advert.Description} </p>", null);
                }
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return false;
    }

    private string ReplaceTurkishCharacters(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var oldChars = new[] {'ç', 'ğ', 'ı', 'ö', 'ş', 'ü', 'Ç', 'Ğ', 'İ', 'Ö', 'Ş', 'Ü'};
        var newChars = new[] {'c', 'g', 'i', 'o', 's', 'u', 'C', 'G', 'I', 'O', 'S', 'U'};

        for (int i = 0; i < oldChars.Length; i++)
        {
            text = text.Replace(oldChars[i], newChars[i]);
        }

        return text;
    }

    public void SendEmail(
        string fromEmail,
        string fromName,
        List<string> toEmails,
        List<string> ccEmails,
        List<string> bccEmails,
        string subject,
        string htmlBody,
        List<string> attachmentPaths)
    {
        try
        {
            var fromAddress = new MailAddress(fromEmail, fromName);
            var message = new MailMessage()
            {
                From = fromAddress,
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
            };

            toEmails?.ForEach(email => message.To.Add(email));
            ccEmails?.ForEach(email => message.CC.Add(email));
            bccEmails?.ForEach(email => message.Bcc.Add(email));

            attachmentPaths?.ForEach(path =>
            {
                if (File.Exists(path))
                {
                    var attachment = new Attachment(path, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(path);
                    disposition.ModificationDate = File.GetLastWriteTime(path);
                    disposition.ReadDate = File.GetLastAccessTime(path);
                    message.Attachments.Add(attachment);
                }
            });

            smtp.Send(message);
        }
        catch (Exception ex)
        {
            // Hata işleme - bu durumda, hatayı yakalayıp konsola yazdırıyoruz,
            // Ancak gerçek bir uygulamada daha gelişmiş hata işlemeye ihtiyaç duyabilirsiniz.
            Console.WriteLine("E-posta gönderilirken bir hata oluştu: " + ex.ToString());
        }
    }

    public async Task<IDataResult<List<AdvertGetDto>>> GetAllAdverts()
    {
        try
        {
            var adverts = await UnitOfWork.Adverts.FindBy().Select(x => x.ToDto()).ToListAsync();
            //adverts.ForEach(x => {);
            if (adverts is null)
            {
                return new DataResult<List<AdvertGetDto>>(ResultStatusEnum.Error, "Adverts not found", null);
            }

            return new DataResult<List<AdvertGetDto>>(ResultStatusEnum.Success, adverts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IDataResult<Advert>> UpdateAdvert(Advert model)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x => x.Id == model.Id)
            .Include(x => x.AdvertExtraAttributes)
            .Include(x => x.ImageFile)
            .FirstOrDefaultAsync();

        try
        {
            advert.AdvertExtraAttributes = model.AdvertExtraAttributes;
            advert.AdvertType = model.AdvertType;
            advert.IsFeatured = model.IsFeatured;
            advert.BathroomNumber = model.BathroomNumber;
            advert.BedroomNumber = model.BedroomNumber;
            advert.IsFeatured = model.IsFeatured;
            advert.Description = model.Description;
            advert.Location = model.Location;
            advert.Price = model.Price;
            advert.Meters = model.Meters;
            advert.District = model.District;
            advert.HouseType = model.HouseType;
            advert.ImageFile.ImageName = model.ImageFile?.ImageName;
            advert.ImageFile.ImagePath = model.ImageFile?.ImagePath ?? advert.ImageFile.ImagePath;
            advert.ImageFile.ImageFile = model.ImageFile?.ImageFile;
            advert.Title = model.Title;
            if (advert is null)
            {
                return new DataResult<Advert>(ResultStatusEnum.Error, "Advert not found", null);
            }

            if (model.ImageFile?.ImageFile is not null)
            {
                var withoutExtension = Path.GetFileNameWithoutExtension(model.ImageFile?.ImageFile?.FileName);
                var uniqueFileName = StringExtensions.GetUniqueFileName(withoutExtension) +
                                     Path.GetExtension(model.ImageFile?.ImageFile?.FileName);

                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "img", uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                await model.ImageFile.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                advert.ImageFile.ImageName = withoutExtension;
                advert.ImageFile.ImageFile = model.ImageFile?.ImageFile;
                advert.ImageFile.ImagePath = filePath;
            }

            //advert.AdvertDescription = model.AdvertDescription;
            UnitOfWork.Adverts.Update(advert);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
        }

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
                AdvertExtraAttributes = model.AdvertExtraAttributes,
                District = model.District,
                Description = model.Description,
                BedroomNumber = model.BedroomNumber,
                BathroomNumber = model.BathroomNumber,
                AdvertType = model.AdvertType,
                IsFeatured = model.IsFeatured,
                ImageFile = new Image()
            };

            //Save image to wwwroot/image
            if (model.ImageFile?.ImageFile is not null)
            {
                var withoutExtension = Path.GetFileNameWithoutExtension(model.ImageFile?.ImageFile?.FileName);
                var uniqueFileName = StringExtensions.GetUniqueFileName(withoutExtension) +
                                     Path.GetExtension(model.ImageFile?.ImageFile?.FileName);

                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "img", uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                await model.ImageFile.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                advert.ImageFile.ImageName = withoutExtension;
                advert.ImageFile.ImageFile = model.ImageFile?.ImageFile;
                advert.ImageFile.ImagePath = filePath;
            }

            await UnitOfWork.Adverts.InsertAsync(advert);
            await UnitOfWork.SaveChangesAsync();
            var isSuccess = await SendEmailToCustomers(advert, null);
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

        UnitOfWork.Adverts.HardDelete(advert);
        await UnitOfWork.SaveChangesAsync();
        return new DataResult<bool>(ResultStatusEnum.Success, "Advert deleted successfully", true);
    }

    public async Task<IDataResult<Advert>> GetAdvertById(int id)
    {
        var advert = await UnitOfWork.Adverts.FindBy(x => x.Id == id)
            .Include(x => x.ImageFile)
            .Include(x => x.AdvertExtraAttributes)
            .FirstOrDefaultAsync();

        if (advert is null)
        {
            return new DataResult<Advert>(ResultStatusEnum.Error, "Advert not found", null);
        }

        return new DataResult<Advert>(ResultStatusEnum.Success, advert);
    }

    public async Task<List<AdvertGetDto>> GetAdvertsPaginated(SearchAdvertRequest model, PaginationFilter filter)
    {
        var data = UnitOfWork.Adverts.FindBy()
            .Include(x => x.ImageFile)
            .AsQueryable();

        if (!string.IsNullOrEmpty(model.SearchKeyWord))
            data = data.Where(x => x.Title.Contains(model.SearchKeyWord)
                                   || x.Description.Contains(model.SearchKeyWord));
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

        //TODO extraAttributes model should be okay
        //extraAttributesModel.ForEach(item =>
        //{
        //    data = data.Where(x => x.ExtraAttributes != null && x.ExtraAttributes.Contains(item));
        //});

        data = model.OrderBy switch
        {
            1 => data.OrderByDescending(x => x.CreatedDate),
            2 => data = data.OrderByDescending(x => x.Price),
            3 => data = data.OrderBy(x => x.Price),
            _ => data
        };

        var dataObjects = await data.ToListAsync();

        var advertGetDtos = dataObjects.Select(x => new AdvertGetDto
        {
            Title = x.Title,
            AdvertType = x.AdvertType,
            BathroomNumber = x.BathroomNumber,
            BedroomNumber = x.BedroomNumber,
            Description = x.Description,
            District = x.District,
            HouseType = x.HouseType,
            Location = x.Location,
            Meters = x.Meters,
            Price = x.Price,
            IsFeatured = x.IsFeatured,
            ImageWebPath = GetImagePath(x.ImageFile?.ImagePath),
            CreatedDate = x.CreatedDate,
        }).ToList();


        return advertGetDtos;
    }

    public async Task<IDataResult<List<CustomerGetDto>>> GetAllCustomers()
    {
        try
        {
            var customers = await _unitOfWork.Repository<Customer>().FindBy().Select(x => x.ToDto()).ToListAsync();
            if (customers is null)
            {
                return new DataResult<List<CustomerGetDto>>(ResultStatusEnum.Error, "Customers not found", null);
            }

            return new DataResult<List<CustomerGetDto>>(ResultStatusEnum.Success, customers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IDataResult<Customer>> UpdateCustomer(Customer model)
    {
        var customer = await _unitOfWork.Repository<Customer>().FindBy(x => x.Id == model.Id)
            .Include(x => x.CustomerExtraAttributes)
            .FirstOrDefaultAsync();

        try
        {
            if (customer is null)
            {
                return new DataResult<Customer>(ResultStatusEnum.Error, "Customer not found", null);
            }

            customer.Name = model.Name;
            customer.Surname = model.Surname;
            customer.HouseType = model.HouseType;
            customer.Location = model.Location;
            customer.District = model.District;
            customer.AdvertType = model.AdvertType;
            customer.BedroomNumber = model.BedroomNumber;
            customer.BathroomNumber = model.BathroomNumber;
            customer.Meters = model.Meters;
            customer.Price = model.Price;
            customer.CustomerExtraAttributes = model.CustomerExtraAttributes;

            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return new DataResult<Customer>(ResultStatusEnum.Success, "Customer updated successfully", customer);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new DataResult<Customer>(ResultStatusEnum.Error, "Customer couldn't updated", null);
    }


    public async Task<IDataResult<Customer>> AddCustomer(Customer model)
    {
        if (model is null)
        {
            return new DataResult<Customer>(ResultStatusEnum.Error, "Customer cannot be found", null);
        }

        try
        {
            var customer = new Customer()
            {
                Name = model.Name,
                Surname = model.Surname,
                HouseType = model.HouseType,
                Location = model.Location,
                District = model.District,
                AdvertType = model.AdvertType,
                BedroomNumber = model.BedroomNumber,
                BathroomNumber = model.BathroomNumber,
                Meters = model.Meters,
                Price = model.Price,
                CustomerExtraAttributes = model.CustomerExtraAttributes,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Repository<Customer>().InsertAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            var isSuccess = await SendEmailToCustomers(null,customer);
            return new DataResult<Customer>(ResultStatusEnum.Success, "Customer added successfully", customer);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }


    public async Task<IDataResult<bool>> DeleteCustomer(int id)
    {
        var customer = await _unitOfWork.Repository<Customer>().FindBy(x => x.Id == id)
            .Include(x => x.CustomerExtraAttributes)
            .FirstOrDefaultAsync();
        if (customer is null)
        {
            return new DataResult<bool>(ResultStatusEnum.Error, "Customer not found", false);
        }

        _unitOfWork.Repository<Customer>().HardDelete(customer);
        await _unitOfWork.SaveChangesAsync();
        return new DataResult<bool>(ResultStatusEnum.Success, "Customer deleted successfully", true);
    }

    public async Task<IDataResult<Customer>> GetCustomerById(int id)
    {
        var customer = await _unitOfWork.Repository<Customer>().FindBy(x => x.Id == id)
            .Include(x => x.CustomerExtraAttributes)
            .FirstOrDefaultAsync();
        if (customer is null)
            return new DataResult<Customer>(ResultStatusEnum.Error, "Customer not found", null);

        return new DataResult<Customer>(ResultStatusEnum.Success, "Customer found", customer);
    }


    string? GetImagePath(string? imagePath)
    {
        if (imagePath == null)
        {
            var defaultImagePath = "uploads/img/defaultImage.jpg";
            return defaultImagePath;
        }

        var splitPath = imagePath.Split(new string[] {"wwwroot\\"}, StringSplitOptions.None);
        return splitPath.Length > 1 ? splitPath[1] : null;
    }
}