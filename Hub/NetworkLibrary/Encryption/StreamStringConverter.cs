using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary.Encryption
{
    //이후에 file 도 정상처리 가능하게 미리 처리
    public class StreamStringConverter : JsonConverter
    {
        private static Type AllowedType = typeof(Stream);

        public override bool CanConvert(Type objectType)
            => objectType == AllowedType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var objectContents = (string)reader.Value;
            var base64Decoded = Convert.FromBase64String(objectContents);

            var memoryStream = new MemoryStream(base64Decoded);

            return memoryStream;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueStream = (FileStream)value;
            var fileBytes = new byte[valueStream.Length];

            valueStream.Read(fileBytes, 0, (int)valueStream.Length);

            var bytesAsString = Convert.ToBase64String(fileBytes);

            writer.WriteValue(bytesAsString);
        }
    }
}
