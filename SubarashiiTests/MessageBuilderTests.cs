using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subarashii.Core;
using System.Linq;

namespace SubarashiiTests
{
    [TestClass]
    public class MessageBuilderTests
    {
        [TestMethod]
        public void BuildsStringMessageCorrectly()
        {
            var payload = "Hello there!";

            var message = new MessageBuilder()
                .PutOperationCode("12")
                .PutPayload(payload)
                .Build();

            var decoded = MessageDecoder.Decode(message.Skip(sizeof(int)).ToArray());

            Assert.IsFalse(decoded.IsResponse);
            Assert.AreEqual("------", decoded.Auth);
            Assert.AreEqual("12", decoded.Code);
            Assert.AreEqual(payload, MessageDecoder.DecodePayload(decoded.Payload));
        }

        [TestMethod]
        public void BuildsObjectMessageCorrectly()
        {
            var user = new User()
            {
                Id = 3,
                Name = "El hijo de Piri"
            };

            var message = new MessageBuilder()
                .PutOperationCode("14")
                .PutPayload(user)
                .Build();

            var decoded = MessageDecoder.Decode(message.Skip(sizeof(int)).ToArray());
            var payload = MessageDecoder.DecodePayload<User>(decoded.Payload);

            Assert.IsFalse(decoded.IsResponse);
            Assert.AreEqual("------", decoded.Auth);
            Assert.AreEqual("14", decoded.Code);
            Assert.AreEqual(user.Id, payload.Id);
            Assert.AreEqual(user.Name, payload.Name);
        }

        [TestMethod]
        public void CanSetAuthInfo()
        {
            var message = new MessageBuilder()
                .PutAuthInfo("220159")
                .PutOperationCode("24")
                .PutPayload("Hello there")
                .Build();

            var decoded = MessageDecoder.Decode(message.Skip(sizeof(int)).ToArray());
            Assert.AreEqual("220159", decoded.Auth);
        }

        [TestMethod]
        public void CanMarkAsResponse()
        {
            var message = new MessageBuilder()
                .MarkAsResponse()
                .PutOperationCode("33")
                .PutPayload("Hello there")
                .Build();

            var decoded = MessageDecoder.Decode(message.Skip(sizeof(int)).ToArray());
            Assert.IsTrue(decoded.IsResponse);
        }
    }
}
