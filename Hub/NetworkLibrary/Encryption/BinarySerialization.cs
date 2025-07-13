using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NetworkLibrary.Encryption
{
    public class BinarySerialization<T>
    {        
        public byte[]? Serialize(T data)
        {
            byte[]? bytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {                
                try
                {
                    using (BsonWriter bsonDataWriter = new BsonWriter(memoryStream))
                    {
                        new JsonSerializer().Serialize(bsonDataWriter, data);
                        bytes =  memoryStream.ToArray();
                    }
                }
               
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Flush();
                        memoryStream.Close();
                    }
                }
            }
            return bytes;
        }
        
        public T Deserialize(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            T? result = default ;
            try
            {
                using (BsonReader bsonDataReader = new BsonReader(memoryStream))
                    result =  new JsonSerializer().Deserialize<T>(bsonDataReader);
            }
            
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Flush();
                    memoryStream.Close();
                }
            }
            return result?? default;
        }
    }
}
