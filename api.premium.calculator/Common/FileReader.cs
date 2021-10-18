using Newtonsoft.Json;
using System;
using System.IO;

namespace api.premium.calculator.Common
{
    public interface IFileReader
    {
        T LoadJson<T>(string fileName) where T : class;
    }

    public class FileReader : IFileReader
    {
        public T LoadJson<T>(string fileName) where T : class
        {
            T result = null;
            try
            {
                using (var r = new StreamReader($"./Configuration/StaticData/{fileName}"))
                {
                    string json = r.ReadToEnd();
                    result = JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error reading the static file {fileName}", ex);
            }            
            return result;
        }
    }
}
