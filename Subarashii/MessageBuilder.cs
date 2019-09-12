using System;
using System.Text;
using System.Text.RegularExpressions;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core
{
    internal class MessageBuilder
    {
        private Regex authRegex = new Regex(@"^[0-9]{6}$");

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
            Payload = Encoding.UTF8.GetBytes(payload);
            Length = Constants.HEADER_LENGTH + Payload.Length;
            IsFile = false;

            return this;
        }

        public MessageBuilder PutPayload<T>(T payload) where T : class
        {
            var props = typeof(T).GetProperties();
            string payloadString = "";

            foreach (var prop in props)
            {
                if (payloadString.Length == 0)
                    payloadString += String.Format("{0}={1}", prop.Name, prop.GetValue(payload));
                else
                    payloadString += String.Format("&{0}={1}", prop.Name, prop.GetValue(payload));
            }

            Payload = Encoding.UTF8.GetBytes(payloadString);
            Length = Constants.HEADER_LENGTH + Payload.Length;
            IsFile = false;

            return this;
        }

        public MessageBuilder PutPayload(byte[] payload)
        {
            Payload = payload;
            Length = Constants.HEADER_LENGTH + Payload.Length;
            IsFile = true;

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

            string head = String.Format("{0}{1}{2}{3}", IsResponse ? "RES" : "REQ", Code, IsFile ? "F" : "T", Auth);
            byte[] headers = Encoding.UTF8.GetBytes(head);

            byte[] message = new byte[len.Length + headers.Length + Payload.Length];
            len.CopyTo(message, 0);
            headers.CopyTo(message, len.Length);
            Payload.CopyTo(message, len.Length + headers.Length);

            return message;
        }
    }
}
