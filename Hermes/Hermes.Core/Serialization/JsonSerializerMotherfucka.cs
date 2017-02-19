using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Hermes.Core.Serialization
{
    public sealed class JsonSerializerMotherfucka
    {
        /// <summary>
		/// Verify if a byte array containing JSON data could be deserialized into a
		/// specified object.
		/// </summary>
		/// <typeparam name="T">Object to be returned from the byte array.</typeparam>
		/// <param name="data">String containing an object in JSON format.</param>
		/// <param name="obj">Object to parse into JSON.</param>
		/// <returns>Whether the byte array could be parsed as JSON into the object or not.</returns>
        public bool TryParse<T>(string data, out T obj)
        {
            try
            {
                obj = JsonConvert.DeserializeObject<T>(data);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                obj = default(T);
                return false;
            }
        }
        /// <summary>
        /// Deserialize a string containing data in JSON format, into a generic object.
        /// </summary>
        /// <typeparam name="T">Object to be returned from the string in JSON format.</typeparam>
        /// <param name="data">String in JSON format containing an object.</param>
        /// <returns>The object extracted from the string, which contains data as JSON.</returns>
        public T Deserialize<T>(string data)
        {
            T request = default(T);

            try
            {
                request = JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return request;
        }
        /// <summary>
        /// Serializes an object into a string.
        /// </summary>
        /// <typeparam name="T">Generic object type.</typeparam>
        /// <param name="request">Object to be serialized.</param>
        /// <returns>The string in JSON format of the serialized object.</returns>
        public string Serialize<T>(T request)
        {
            string serializedRequest = JsonConvert.SerializeObject(request);
            return serializedRequest;
        }
    }
}
