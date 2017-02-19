using System.Linq;
using Hermes.Model;
using Hermes.Model.Request;
using Hermes.Model.Response;
using Hermes.Server.Command.Joke;
using System;

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

            if (request.DestinationUserId == "0")
            {
                // Create new joke!
                if (request.Data == "piada")
                {
                    SendCommand.HandleNewJoke(request);
                }
                // Continue new joke!
                else
                {
                    SendCommand.HandlePreviousJoke(request);
                }
            }
            else
            {
                AsyncListener.PendingMessages.Add(msg);
            }

            // Add server ID:
            SendResponse response = new SendResponse();
            response.UserId = "0";

            return response;
        }

        private static void HandlePreviousJoke(SendRequest request)
        {
            TocTocJoke joke = AsyncListener.PendingJokes
                .Where(j => j.UserId == request.UserId)
                .FirstOrDefault();

            if (joke != null)
            {
                // If the interaction was correct:
                if (joke.ClientInteractions[joke.ClientInteractionIndex]
                    .Equals(request.Data, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Increments interaction from both client and server, and adds
                    // the new joke interaction to the pending messages container.
                    joke.ClientInteractionIndex++;

                    Message jokeMsg = new Message()
                    {
                        Data = joke.ServerInteractions[joke.ServerInteractionIndex],
                        DestinationUserId = request.UserId
                    };

                    joke.ServerInteractionIndex++;

                    AsyncListener.PendingMessages.Add(jokeMsg);

                    // If the joke is over, remove it from the bunch:
                    if (joke.ServerInteractionIndex >= joke.ServerInteractions.Length)
                    {
                        AsyncListener.PendingJokes.Remove(joke);
                    }
                }
                else
                {
                    // Return error message and remove the joke fromthe bunch:
                    Message errorMsg = new Message()
                    {
                        Data = "(Piada removida)",
                        DestinationUserId = request.UserId
                    };

                    AsyncListener.PendingMessages.Add(errorMsg);
                    AsyncListener.PendingJokes.Remove(joke);
                }
            }
        }
        private static void HandleNewJoke(SendRequest request)
        {
            // Create joke for the request user:
            TocTocJoke joke = JokeProvider.GetNew();
            joke.UserId = request.UserId;

            // Add a joke message into the container of pending messages.
            // In this case, the next time a receive request is sent by this user,
            // It'll return the joke message.
            Message jokeMsg = new Message()
            {
                Data = joke.ServerInteractions[joke.ServerInteractionIndex],
                DestinationUserId = request.UserId
            };

            AsyncListener.PendingMessages.Add(jokeMsg);

            // Increment the server interaction index, so the next
            // interaction is called.
            joke.ServerInteractionIndex++;

            // Add this joke to the container of pending jokes:
            AsyncListener.PendingJokes.Add(joke);
        }
    }
}
