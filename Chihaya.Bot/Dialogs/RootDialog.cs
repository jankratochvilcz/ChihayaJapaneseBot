using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chihaya.Bot.Dialogs
{
    [Serializable]
    [LuisModel("96aa24ab-e3f6-46df-9ef2-81445a4f2dcb", "96dcf98e02974c08b0178ef88c9c4c9a", verbose: true)]
    public class RootDialog : LuisDialog<object>
    {
        private const string WordToLookUpEntityName = "WordToLookUp";

        readonly WordLookUpDialog wordLookupDialog;

        public RootDialog(WordLookUpDialog wordLookupDialog)
        {
            this.wordLookupDialog = wordLookupDialog;
        }

        [LuisIntent("WordLookUp")]
        public async Task LookupWordIntent(IDialogContext context, LuisResult result)
        {
            var wordToLookUp = result.Entities
                .SingleOrDefault(x => x.Type == RootDialog.WordToLookUpEntityName)
                ?.Entity;

            this.wordLookupDialog.WordToLookUp = wordToLookUp;

            await context.Forward(
                this.wordLookupDialog,
                new ResumeAfter<IMessageActivity>(this.MessageReceived),
                result,
                CancellationToken.None);
        }
    }
}
