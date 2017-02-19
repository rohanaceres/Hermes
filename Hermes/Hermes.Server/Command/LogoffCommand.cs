﻿using Hermes.Model.Request;
using Hermes.Model.Response;

namespace Hermes.Server.Command
{
    internal sealed class LogoffCommand
    {
        public static LogoffResponse LogUserOff(LogoffRequest request)
        {
            // Unregister user from the container:
            if (AsyncListener.ConnectedClientIds.Contains(request.UserId)
                == true)
            {
                AsyncListener.ConnectedClientIds.Remove(request.UserId);
            }

            LogoffResponse logoffResponse = new LogoffResponse();
            logoffResponse.UserId = request.UserId;

            return logoffResponse;
        }
    }
}
