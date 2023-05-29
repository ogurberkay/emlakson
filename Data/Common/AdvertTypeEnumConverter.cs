using System.Text.Json;
using System.Text.Json.Serialization;
using Data.Entities.Models;

public class AdvertTypeEnumConverter : JsonConverter<AdvertTypeEnum>
{
    public override AdvertTypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();

        if (Enum.TryParse(value, true, out AdvertTypeEnum advertType))
        {
            return advertType;
        }

        throw new JsonException($"Value '{value}' is not a valid AdvertTypeEnum");
    }

    public override void Write(Utf8JsonWriter writer, AdvertTypeEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}