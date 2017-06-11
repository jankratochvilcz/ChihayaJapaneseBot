using Chihaya.Bot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chihaya.Bot.Dialogs
{
    [Serializable]
    public class WordLookUpDialog : IDialog<IMessageActivity>
    {
        private readonly IWordLookupService wordLookupService;

        private const int MaxOverflowResultsCount = 5;

        public string WordToLookUp { get; set; }

        private bool InitialLookupPerformed { get; set; }

        private WordLookupResult LookupResults { get; set; }

        public WordLookUpDialog(
            IWordLookupService wordLookupService)
        {
            this.wordLookupService = wordLookupService;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageRecievedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (!this.InitialLookupPerformed)
            {
                await this.HandleWordLookUp(this.WordToLookUp, context);
                this.InitialLookupPerformed = true;

                return;
            }

            var currentActivity = await result;
            var currentUtterance = currentActivity.Text.NormalizeUtterance();

            var supportedPostInitialLookupIntents = new Dictionary<Func<string, bool>, Func<IDialogContext, Task>>
            {
                {
                    x => new [] { "more", "more translations", "expand" }.Any(y => y == x),
                    this.ShowOverflowLookupItems
                }
            };

            if (!supportedPostInitialLookupIntents.Any(x => x.Key(currentUtterance)))
            {
                this.InitialLookupPerformed = false;
                context.Done(currentActivity);
                return;
            }
            else
            {
                var selectedIntent = supportedPostInitialLookupIntents.First(x => x.Key(currentUtterance));
                await selectedIntent.Value(context);
            }
        }

        private Task ShowOverflowLookupItems(IDialogContext context)
            => this.PostLookupResultItems(context, this.LookupResults.Results.Skip(1));

        private async Task HandleWordLookUp(string wordToLookUp, IDialogContext context)
        {
            this.LookupResults = await this.wordLookupService.Lookup(wordToLookUp);

            await this.PostLookupResultItems(context, this.LookupResults.Results.Take(1));
        }

        private async Task PostLookupResultItems(
            IDialogContext context,
            IEnumerable<WordLookupResultItem> lookupResults)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = "carousel";

            var cards = lookupResults
                .Select(x => new HeroCard(
                    x.ReadingWithKanji,
                    x.ReadingWithKanji != x.ReadingWithKana
                        ? x.ReadingWithKana
                        : null,
                    x.EnglishDefinitionsToString()))
                .Select(x => x.ToAttachment());

            foreach (var card in cards) message.Attachments.Add(card);

            await context.PostAsync(message);
        }
    }
}
