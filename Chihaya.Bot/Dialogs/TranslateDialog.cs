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
    public class TranslateDialog : IDialog<IMessageActivity>
    {
        readonly IJapaneseTranslationService japaneseTranslationService;
        readonly IJapaneseTokenizationService japaneseTokenizationService;
        readonly MetaMessagingService typingIndicatorService;
        readonly IKanaTranscriptionService kanaTranscriptionService;
        readonly ILanguageDetectionService languageDetectionService;

        public string PhraseToTranslate { get; set; }

        private bool InitialLookupPerformed { get; set; }

        public List<JapaneseLanguageToken> Tokens { get; set; }

        public TranslateDialog(
            IJapaneseTranslationService japaneseTranslationService,
            IJapaneseTokenizationService japaneseTokenizationService,
            ILanguageDetectionService languageDetectionService,
            MetaMessagingService typingIndicatorService,
            IKanaTranscriptionService kanaTranscriptionService)
        {
            this.languageDetectionService = languageDetectionService;
            this.kanaTranscriptionService = kanaTranscriptionService;
            this.typingIndicatorService = typingIndicatorService;
            this.japaneseTokenizationService = japaneseTokenizationService;
            this.japaneseTranslationService = japaneseTranslationService;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await this.typingIndicatorService.SendTypingIndicator(context);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (!this.InitialLookupPerformed)
            {
                await HandleTranslation(this.PhraseToTranslate, context);
                this.InitialLookupPerformed = true;

                return;
            }

            var currentActivity = await result;
            var currentUtterance = currentActivity.Text.NormalizeUtterance();

            var supportedPostInitialLookupIntents = new Dictionary<Func<string, bool>, Func<IDialogContext, Task>>
            {
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

        private async Task HandleTranslation(string utterance, IDialogContext context)
        {
            var language = this.languageDetectionService.GetLanguage(utterance) == SupportedLanguage.Japanese
                ? SupportedLanguage.English
                : SupportedLanguage.Japanese;

            var translationResult = await this.japaneseTranslationService.Translate(utterance, language);

            this.Tokens = await this.japaneseTokenizationService.Tokenize(translationResult);
            var kanaReading = language == SupportedLanguage.Japanese
                ? this.kanaTranscriptionService.TranscribeToPreferredKana(
                    this.Tokens.Aggregate(string.Empty, (acc, current) => acc + current.Reading),
                    context)
                : null;

            var card = new HeroCard(translationResult, text: kanaReading);
            var message = context.MakeMessage();
            message.Attachments.Add(card.ToAttachment());

            await context.PostAsync(message);
        }
    }
}
