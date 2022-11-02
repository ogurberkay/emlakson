namespace Data.DataTransferObjects.Request;

public class SearchAdvertRequest
{
    public string? SearchKeyWord { get; set; }
    public string? HouseType { get; set; }
    public string? Lokasyon { get; set; }
    public string? AdvertType { get; set; }
    public int BedroomNumber { get; set; }
    public int BathroomNumber { get; set; }
    public int metresStartValue { get; set; }
    public int metresEndValue { get; set; }
    public int pricesStartValue { get; set; }
    public int pricesEndValue { get; set; }
}