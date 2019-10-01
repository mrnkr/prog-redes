using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core
{
    public class MessageBuilder
    {
        private Regex authRegex = new Regex(@"^([0-9]{6}|[-]{6})$");

        private int Length { get; set; }
        private bool IsResponse { get; set; }
        private string Code { get; set; }
        private bool IsFile { get; set; }
        private string Auth { get; set; }
        private byte[] Payload { get; set; }

        public MessageBuilder() {
            Length = Constants.HEADER_LENGTH;
            IsResponse = false;
            Code = "00";
            IsFile = false;
            Auth = "------";
        }

        public MessageBuilder PutPayload(string payload)
        {
            var p = Encoding.UTF8.GetBytes(payload);

            if (p.Length >= Constants.MAX_PAYLOAD_SIZE)
            {
                throw new PayloadTooLargeException();
            }

            Payload = p;
            Length = Constants.HEADER_LENGTH + Payload.Length;

            return this;
        }

        public MessageBuilder PutPayload<T>(T payload) where T : class
        {
            var p = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

            if (p.Length >= Constants.MAX_PAYLOAD_SIZE)
            {
                throw new PayloadTooLargeException();
            }

            Payload = p;
            Length = Constants.HEADER_LENGTH + Payload.Length;

            return this;
        }

        public MessageBuilder PutPayload(byte[] payload)
        {
            if (payload.Length >= Constants.MAX_PAYLOAD_SIZE)
            {
              throw new PayloadTooLargeException();
            }

            Payload = payload;
            Length = Constants.HEADER_LENGTH + Payload.Length;

            return this;
        }

        public MessageBuilder MarkAsFile()
        {
            IsFile = true;
            return this;
        }

        public MessageBuilder MarkAsText()
        {
            IsFile = false;
            return this;
        }

        public MessageBuilder PutOperationCode(string code)
        {
            if (!Constants.codeRegex.IsMatch(code))
            {
                throw new InvalidCodeException();
            }

            Code = code;
            return this;
        }

        public MessageBuilder PutAuthInfo(string auth)
        {
            if (!authRegex.IsMatch(auth))
            {
                throw new InvalidAuthException();
            }

            Auth = auth;
            return this;
        }

        public MessageBuilder MarkAsResponse()
        {
            IsResponse = true;
            return this;
        }

        public byte[] Build()
        {
            byte[] len = BitConverter.GetBytes(Length);

            string head = $"{(IsResponse ? "RES" : "REQ")}{Code}{(IsFile ? "F" : "T")}{Auth}";
            byte[] headers = Encoding.UTF8.GetBytes(head);

            byte[] message = new byte[len.Length + headers.Length + (Payload != null ? Payload.Length : 0)];
            len.CopyTo(message, 0);
            headers.CopyTo(message, len.Length);

            if (Payload != null)
                Payload.CopyTo(message, len.Length + headers.Length);

            return message;
        }
    }
}
