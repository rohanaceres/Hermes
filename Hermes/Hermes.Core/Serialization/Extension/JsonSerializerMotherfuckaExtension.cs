using System;
using System.IO;

namespace Hermes.Core.Serialization
{
    public static class JsonSerializerMotherfuckaExtension
    {
        /// <summary>
		/// Serialize an object of type T to a string on JSON format.
		/// </summary>
		/// <typeparam name="T">Object type.</typeparam>
		/// <param name="obj">Object to serialize.</param>
		/// <returns>Serialized object in JSON format.</returns>
		public static string SerializeToJson<T>(this T obj)
        {
            try
            {
                JsonSerializerMotherfucka serializer = new JsonSerializerMotherfucka();
                return serializer.Serialize<T>(obj);
            }
            catch (Exception ex)
            {
                throw new InvalidSerializationException(string.Format(
                    @"Could not serialize {0} into a JSON. See inner exception for 
                    more detailed information.", obj.GetType().ToString()), ex);
            }
        }
        public static T DeserializeFromJson<T>(this string json)
        {
            try
            {
                JsonSerializerMotherfucka serializer = new JsonSerializerMotherfucka();
                return serializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidSerializationException(string.Format(
                    @"Could not deserialize {0} into an object. See inner exception for 
                    more detailed information.", json), ex);
            }
        }
    }
}
