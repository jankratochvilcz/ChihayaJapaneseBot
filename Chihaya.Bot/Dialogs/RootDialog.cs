using Chihaya.Bot.Services;
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
        readonly WordLookUpDialog wordLookupDialog;
        readonly TranslateDialog translateDialog;
        readonly IFallbackIntentRecognitionService x;

        public RootDialog(
            WordLookUpDialog wordLookupDialog,
            TranslateDialog translateDialog,
            IFallbackIntentRecognitionService fallbackIntentRecognitionService)
        {
            this.x = fallbackIntentRecognitionService;
            this.translateDialog = translateDialog;
            this.wordLookupDialog = wordLookupDialog;
        }

        [LuisIntent(KnownIntents.WordLookUp)]
        public async Task LookupWordIntent(IDialogContext context, LuisResult result)
        {
            var word = result.Entities
                .SingleOrDefault(x => x.Type == KnownEntities.WordToLookUp)
                ?.Entity;

            if (word == null)
            {
                await this.UnknownIntent(context, result);
                return;
            }

            this.wordLookupDialog.WordToLookUp = word;

            await context.Forward(
                this.wordLookupDialog,
                new ResumeAfter<IMessageActivity>(this.MessageReceived),
                result,
                CancellationToken.None);
        }

        [LuisIntent(KnownIntents.Translate)]
        public async Task TranslateIntent(IDialogContext context, LuisResult result)
        {
            var phrase = result.Entities
                .SingleOrDefault(x => x.Type == KnownEntities.PhraseToLookUp)
                ?.Entity;

            if (phrase == null)
                phrase = this.x.RecognizePhraseLookup(result.Query);

            if (phrase == null)
            {
                await this.UnknownIntent(context, result);
                return;
            }

            this.translateDialog.PhraseToTranslate = phrase;

            await context.Forward(
                this.translateDialog,
                new ResumeAfter<IMessageActivity>(this.MessageReceived),
                result,
                CancellationToken.None);
        }

        [LuisIntent(KnownIntents.Unknown)]
        public async Task UnknownIntent(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.AddKeyboardCard(string.Empty, new[] { "look up home" });
            message.Text = "Sorry, I didn't get that. Here are some things to try.";

            await context.PostAsync(message);
        }
    }
}
