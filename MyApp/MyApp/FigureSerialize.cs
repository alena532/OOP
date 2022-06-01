using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyApp
{
    public class FigureSerializer
    {
        private List<Figure>? data;
        public List<Figure> DeserializedFigures { get { return data; } }
        public static Result SerializeToJson(List<Figure> figures, string fileName)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(figures, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
            }
            catch (Exception ex)
            {
                return Result.SERIALIZE_ERROR;
            }
            return Result.OK;
        }
        public Result DeserializeFromJson(string fileName)
        {
            try
            {
                data = (List<Figure>)JsonConvert.DeserializeObject(File.ReadAllText(fileName), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                return Result.OK;
            }
            catch
            {
                return Result.DESERIALIZE_ERROR;
            }
        }
    }
    public enum Result
    {
        OK = 0,
        SERIALIZE_ERROR = 1,
        DESERIALIZE_ERROR = 1,
    }
}

