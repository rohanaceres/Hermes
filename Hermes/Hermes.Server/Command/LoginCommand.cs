using Hermes.Model.Request;
using Hermes.Model.Response;
using System.Linq;
using System.Collections.Generic;
using Hermes.Server.Extension;

namespace Hermes.Server.Command
{
    // TODO: Doc.
    internal sealed class LoginCommand
    {
        public static LoginResponse LogUserIn (LoginRequest loginRequest)
        {
            // Register this user into the container:
            if (AsyncListener.ConnectedClientIds.Contains(loginRequest.UserId) 
                == false)
            {
                AsyncListener.ConnectedClientIds.Add(loginRequest.UserId);
            }

            LoginResponse loginResponse = new LoginResponse();
            loginResponse.Data = new PendingMessages();
            loginResponse.Data.Messages = new List<PendingMessage>();
            loginResponse.Data.Messages.AddRange(
                AsyncListener.PendingMessages
                    .Where(m => m.DestinationUserId == loginRequest.UserId)
                    .ToPendingMessages()
                );
            loginResponse.UserId = "0";

            return loginResponse;
        }
    }
}
