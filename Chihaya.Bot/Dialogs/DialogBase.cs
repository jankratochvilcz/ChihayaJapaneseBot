using Chihaya.Bot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Chihaya.Bot.Dialogs
{
    [Serializable]
    public abstract class DialogBase : IDialog<IMessageActivity>
    {
        readonly MetaMessagingService metaMessagingService;

        public DialogBase(
            MetaMessagingService metaMessagingService)
        {
            this.metaMessagingService = metaMessagingService;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await this.metaMessagingService.SendTypingIndicator(context);

            context.Wait(new ResumeAfter<object>(this.MessageRecievedAsyncInternal));
        }

        private Task MessageRecievedAsyncInternal(IDialogContext context, IAwaitable<object> result)
        {
            return this.MessageRecievedAsync(context, result);
        }

        protected abstract Task MessageRecievedAsync(IDialogContext context, IAwaitable<object> result);
    }
}
