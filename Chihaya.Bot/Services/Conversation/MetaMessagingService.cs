using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class MetaMessagingService
    {
        public async Task SendTypingIndicator(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Type = ActivityTypes.Typing;
            message.ServiceUrl = null;
            await context.PostAsync(message);
        }
    }
}
