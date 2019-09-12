using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subarashii.Core
{
    internal class MessageDecoder
    {
        public static DecodedMessage<byte[]> Decode(byte[] message)
        {
            string header = Encoding.UTF8.GetString(message, 0, Constants.HEADER_LENGTH);

            bool isResponse = header.Substring(0, 3) == "RES";
            string code = header.Substring(3, 2);
            bool isFile = header.Substring(5, 1) == "F";
            string auth = header.Substring(6, 6);

            byte[] payload = message.Skip(Constants.HEADER_LENGTH).ToArray();

            return new DecodedMessage<byte[]>()
            {
                IsResponse = isResponse,
                Code = code,
                IsFile = isFile,
                Auth = auth,
                Payload = payload
            };
        }

        public static string DecodePayload(byte[] payload)
        {
            return Encoding.UTF8.GetString(payload, 0, payload.Length);
        }

        public static T DecodePayload<T>(byte[] payload) where T : class
        {
            var decoded = DecodePayload(payload);
            var entries = decoded.Split('&');
            var map = new Dictionary<string, string>();

            foreach (var entry in entries)
            {
                var keyValue = entry.Split('=');
                map.Add(keyValue[0], keyValue[1]);
            }

            var props = typeof(T).GetProperties();
            var ret = Activator.CreateInstance<T>();

            foreach (var prop in props)
            {
                string value;
                map.TryGetValue(prop.Name, out value);
                prop.SetValue(ret, Convert.ChangeType(value, prop.PropertyType));
            }

            return ret;
        }

        public static object DecodePayload(byte[] payload, Type t)
        {
            var decoded = DecodePayload(payload);
            var entries = decoded.Split('&');
            var map = new Dictionary<string, string>();

            foreach (var entry in entries)
            {
                var keyValue = entry.Split('=');
                map.Add(keyValue[0], keyValue[1]);
            }

            var props = t.GetProperties();
            var ret = Activator.CreateInstance(t);

            foreach (var prop in props)
            {
                string value;
                map.TryGetValue(prop.Name, out value);
                prop.SetValue(ret, Convert.ChangeType(value, prop.PropertyType));
            }

            return ret;
        }
    }
}
