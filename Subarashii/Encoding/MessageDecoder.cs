using Gestion.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace Subarashii.Core
{
    public class MessageDecoder
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
            var ret = Encoding.UTF8.GetString(payload, 0, payload.Length);

            if (ret == "ERROR")
            {
                throw new OperationFailedException();
            }

            return ret;
        }

        public static T DecodePayload<T>(byte[] payload) where T : class
        {
            var decoded = DecodePayload(payload);
            var ret = JsonConvert.DeserializeObject<T>(decoded);
            return ret;
        }

        public static object DecodePayload(byte[] payload, Type t)
        {
            var decoded = DecodePayload(payload);
            var ret = JsonConvert.DeserializeObject(decoded, t);
            return ret;
        }
    }
}
