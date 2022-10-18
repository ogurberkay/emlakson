namespace Business.Utilities;

public static class Messages
{
    public static class Advert
    {
        public static string NotFoundById(int advertId) => $"{advertId} koduna ait bir ilan bulanamadı";
        
        public static string NotFound(bool isPlural)
        {
            if (isPlural) return "Hiç bir ilan bulunamadı.";
            return "Böyle bir ilan bulunamadı.";
        }
    }
}