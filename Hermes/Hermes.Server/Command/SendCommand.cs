using Hermes.Model;
using Hermes.Model.Request;
using Hermes.Model.Response;

namespace Hermes.Server.Command
{
    internal sealed class SendCommand
    {
        public static SendResponse Send (SendRequest request)
        {
            Message msg = new Message()
            {
                SourceUserId = request.UserId,
                DestinationUserId = request.DestinationUserId,
                Data = request.Data
            };

            AsyncListener.PendingMessages.Add(msg);

            SendResponse response = new SendResponse();
            // Add server ID:
            response.UserId = "0";

            return response;
        }
    }
}
