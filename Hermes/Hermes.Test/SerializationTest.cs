using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermes.Model.Request;
using Hermes.Core.Serialization;

namespace Hermes.Test
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void SerializeToJson_ShouldReturnCorrectLoginJson_IfSerializationIsCorrect()
        {
            // Arrange
            LoginRequest request = new LoginRequest()
            {
                MessageIndex = 1,
                UserId = 666
            };

            // Act
            string serializedRequest = request.SerializeToJson();

            // Assert
            Assert.AreEqual(serializedRequest, "{\"cmd\":\"login\",\"id\":666,\"msgNr\":1}");
        }
        [TestMethod]
        public void DeserializeFromJson_ShouldReturnCorrectLoginJson_IfSerializationIsCorrect()
        {
            // Arrange
            LoginRequest expectedRequest = new LoginRequest()
            {
                MessageIndex = 1,
                UserId = 666
            };
            string json = expectedRequest.SerializeToJson();

            // Act
            LoginRequest deserializedRequest = json.DeserializeFromJson<LoginRequest>();

            // Assert
            Assert.AreEqual(deserializedRequest.CommandName, expectedRequest.CommandName);
            Assert.AreEqual(deserializedRequest.MessageIndex, expectedRequest.MessageIndex);
            Assert.AreEqual(deserializedRequest.UserId, expectedRequest.UserId);
        }

        [TestMethod]
        public void SerializeToJson_ShouldReturnCorrectReceiveJson_IfSerializationIsCorrect()
        {
            // Arrange
            ReceiveRequest request = new ReceiveRequest()
            {
                MessageIndex = 2,
                UserId = 777
            };

            // Act
            string serializedRequest = request.SerializeToJson();

            // Assert
            Assert.AreEqual(serializedRequest, "{\"cmd\":\"receber\",\"id\":777,\"msgNr\":2}");
        }
        [TestMethod]
        public void DeserializeFromJson_ShouldReturnCorrectReceiveJson_IfSerializationIsCorrect()
        {
            // Arrange
            ReceiveRequest expectedRequest = new ReceiveRequest()
            {
                MessageIndex = 1,
                UserId = 666
            };
            string json = expectedRequest.SerializeToJson();

            // Act
            ReceiveRequest deserializedRequest = json.DeserializeFromJson<ReceiveRequest>();

            // Assert
            Assert.AreEqual(deserializedRequest.CommandName, expectedRequest.CommandName);
            Assert.AreEqual(deserializedRequest.MessageIndex, expectedRequest.MessageIndex);
            Assert.AreEqual(deserializedRequest.UserId, expectedRequest.UserId);
        }

        [TestMethod]
        public void SerializeToJson_ShouldReturnCorrectSendJson_IfSerializationIsCorrect()
        {
            // Arrange
            SendRequest request = new SendRequest()
            {
                MessageIndex = 3,
                UserId = 888,
                Data = "oieeee",
                DestinationUserId = 3
            };

            // Act
            string serializedRequest = request.SerializeToJson();

            // Assert
            Assert.AreEqual(serializedRequest, "{\"dst\":3,\"data\":\"oieeee\",\"cmd\":\"enviar\",\"id\":888,\"msgNr\":3}");
        }
        [TestMethod]
        public void DeserializeFromJson_ShouldReturnCorrectSendJson_IfSerializationIsCorrect()
        {
            // Arrange
            SendRequest expectedRequest = new SendRequest()
            {
                MessageIndex = 1,
                UserId = 666,
                Data = "oieeee",
                DestinationUserId = 3
            };
            string json = expectedRequest.SerializeToJson();

            // Act
            SendRequest deserializedRequest = json.DeserializeFromJson<SendRequest>();

            // Assert
            Assert.AreEqual(deserializedRequest.CommandName, expectedRequest.CommandName);
            Assert.AreEqual(deserializedRequest.MessageIndex, expectedRequest.MessageIndex);
            Assert.AreEqual(deserializedRequest.UserId, expectedRequest.UserId);
            Assert.AreEqual(deserializedRequest.Data, expectedRequest.Data);
            Assert.AreEqual(deserializedRequest.DestinationUserId, expectedRequest.DestinationUserId);
        }
    }
}
