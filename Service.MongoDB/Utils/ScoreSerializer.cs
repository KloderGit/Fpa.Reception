using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.MongoDB.Utils
{
    public class SystemTypeSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(Type);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value is Type item)
            {
                context.Writer.WriteString(item.FullName);
            }
            else
            {
                throw new NotSupportedException("This is not a System.Type");
            }
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return Type.GetType(value);
        }
    }

}
