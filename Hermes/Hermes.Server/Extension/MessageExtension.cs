using Hermes.Model;
using Hermes.Model.Response;
using System.Collections;
using System.Collections.Generic;

namespace Hermes.Server.Extension
{
    internal static class MessageExtension
    {
        public static PendingMessage ToPendingMessage (this Message self)
        {
            PendingMessage message = new PendingMessage();

            message.Message = self.Data;
            message.Sender = self.SourceUserId;

            return message;
        }
        public static IEnumerable<PendingMessage> ToPendingMessages (this IEnumerable<Message> self)
        {
            List<PendingMessage> messages = new List<PendingMessage>();

            foreach (Message currentMsg in self)
            {
                messages.Add(currentMsg.ToPendingMessage());
            }

            return messages;
        }
    }
}
