using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PosTech.Contacts.ApplicationCore.Serialization
{
    public static class JsonSerializerHelper
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, _options);
        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);
    }
}
