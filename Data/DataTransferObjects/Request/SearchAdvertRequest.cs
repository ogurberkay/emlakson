namespace Data.DataTransferObjects.Request;

public class SearchAdvertRequest
{
    public string? SearchKeyWord { get; set; }
    public string? HouseType { get; set; }
    public string? Location { get; set; }
    public string? AdvertType { get; set; }
    public int? BedroomNumber { get; set; }
    public int? BathroomNumber { get; set; }
    public int MetresStartValue { get; set; }
    public int MetresEndValue { get; set; } = 5000;
    public int PricesStartValue { get; set; }
    public int PricesEndValue { get; set; } = 1000000000;
    public string? ExtraAttributes { get; set; }
    public int OrderBy { get; set; } = 1;

}