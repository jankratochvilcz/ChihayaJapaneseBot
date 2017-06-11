using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace Chihaya.Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageRecieved);

            return Task.CompletedTask;
        }

        private async Task MessageRecieved(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("hello world");
        }
    }
}
