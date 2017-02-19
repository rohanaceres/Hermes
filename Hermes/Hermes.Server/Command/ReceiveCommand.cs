using Hermes.Model.Request;
using Hermes.Model.Response;
using System.Collections.Generic;
using System.Linq;
using Hermes.Server.Extension;

namespace Hermes.Server.Command
{
    internal sealed class ReceiveCommand
    {
        public static ReceiveResponse GetPendingMessages (ReceiveRequest request)
        {
            ReceiveResponse response = new ReceiveResponse();

            // Get pending messages for this user:
            response.Data = new PendingMessages();
            response.Data.Messages = new List<PendingMessage>();
            response.Data.Messages.AddRange(AsyncListener.PendingMessages
                .Where(m => m.DestinationUserId == request.UserId)
                .ToPendingMessages());
            response.UserId = "0";

            // Remove pending messages from buffer:
            AsyncListener.PendingMessages.RemoveAll(m => m.DestinationUserId == request.UserId);

            return response;
        }
    }
}
