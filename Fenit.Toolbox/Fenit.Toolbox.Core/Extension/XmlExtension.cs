using System;
using System.IO;
using System.Xml.Serialization;
using Fenit.Toolbox.Core.Answers;

namespace Fenit.Toolbox.Core.Extension
{
    public static class XmlExtension
    {
        public static Response XmlSerialize<T>(this T obj, Stream fileName)
            where T : class
        {
            var res = new Response();
            try
            {
                if (fileName != null)
                {
                    var xSer = new XmlSerializer(typeof(T));
                    xSer.Serialize(fileName, obj);
                    fileName.Dispose();
                }

                res.AddError("Null Stream");
            }
            catch (Exception e)
            {
                res.AddError(e.Message);
            }

            return res;
        }

        public static Response<T> DeserializeFromString<T>(this string input)
            where T : class
        {
            var res = new Response<T>();
            using (var file = new StringReader(input))
            {
                res.AddValue(DeserializeXml<T>(file));
            }

            return res;
        }

        public static Response<T> DeserializeFromFile<T>(this string path)
            where T : class
        {
            var res = new Response<T>();

            using (var file = new StreamReader(path))
            {
                res.AddValue(DeserializeXml<T>(file));
            }

            return res;
        }

        private static T DeserializeXml<T>(TextReader stream)
        {
            var xml = new XmlSerializer(typeof(T));
            return (T) xml.Deserialize(stream);
        }
    }
}