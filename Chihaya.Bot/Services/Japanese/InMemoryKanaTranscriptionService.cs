using Chihaya.Bot.Services.Japanese;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class InMemoryKanaTranscriptionService
        : IKanaTranscriptionService,
        ILanguageDetectionService
    {
        private readonly Dictionary<string, string> hiraganaToKatakanaTranscriptions;
        private readonly IConversationSettingsService conversationSettingsService;

        public InMemoryKanaTranscriptionService(
            IConversationSettingsService conversationSettingsService)
        {
            this.conversationSettingsService = conversationSettingsService;
            this.hiraganaToKatakanaTranscriptions = new Dictionary<string, string>
            {
                {"あ", "ア"},
                {"い", "イ"},
                {"う", "ウ"},
                {"え", "エ"},
                {"お", "オ"},

                {"か", "カ"},
                {"き", "キ"},
                {"く", "ク"},
                {"け", "ケ"},
                {"こ", "コ"},
                {"が", "ガ"},
                {"ぎ", "ギ"},
                {"ぐ", "グ"},
                {"げ", "ゲ"},
                {"ご", "ゴ"},

                {"さ", "サ"},
                {"し", "シ"},
                {"す", "ス"},
                {"せ", "セ"},
                {"そ", "ソ"},
                {"ざ", "ザ"},
                {"じ", "ジ"},
                {"ず", "ズ"},
                {"ぜ", "ゼ"},
                {"ぞ", "ゾ"},

                {"た", "タ"},
                {"ち", "チ"},
                {"つ", "ツ"},
                {"て", "テ"},
                {"と", "ト"},
                {"だ", "ダ"},
                {"で", "デ"},
                {"ど", "ド"},

                {"な", "ナ"},
                {"に", "ニ"},
                {"ぬ", "ヌ"},
                {"ね", "ネ"},
                {"の", "ノ"},

                {"は", "ハ"},
                {"ひ", "ヒ"},
                {"ふ", "フ"},
                {"へ", "ヘ"},
                {"ほ", "ホ"},
                {"ば", "バ"},
                {"び", "ビ"},
                {"ぶ", "ブ"},
                {"べ", "ベ"},
                {"ぼ", "ボ"},
                {"ぱ", "パ"},
                {"ぴ", "ピ"},
                {"ぷ", "プ"},
                {"ぺ", "ペ"},
                {"ぽ", "ポ"},

                {"ま", "マ"},
                {"み", "ミ"},
                {"む", "ム"},
                {"め", "メ"},
                {"も", "モ"},

                {"ら", "ラ"},
                {"り", "リ"},
                {"る", "ル"},
                {"れ", "レ"},
                {"ろ", "ロ"},

                {"わ", "ワ"},
                {"を", "ヲ"},
                {"や", "ヤ"},
                {"ゆ", "ユ"},
                {"よ", "ヨ"},
                {"ん", "ン"},


                {"ゃ", "ャ"},
                {"ゅ", "ュ"},
                {"ょ", "ョ"},
            };
        }

        public SupportedLanguage GetLanguage(string utterance)
            => utterance.Any(x => this.hiraganaToKatakanaTranscriptions.Any(y => y.Key == x.ToString() || y.Value == x.ToString()))
                ? SupportedLanguage.Japanese
                : SupportedLanguage.English;

        public string Transcribe(string text, KanaType toKanaType)
        {
            if (toKanaType == KanaType.Romanji) throw new NotSupportedException();

            var transcriptions = toKanaType == KanaType.Katakana
                ? this.hiraganaToKatakanaTranscriptions
                : this.hiraganaToKatakanaTranscriptions.ToDictionary(x => x.Value, x => x.Key);

            return text
                .Select(x =>
                    transcriptions.TryGetValue(x.ToString(), out string result)
                        ? result
                        : x.ToString())
                .Aggregate(string.Empty, (acc, curr) => acc + curr);
        }

        public string TranscribeToPreferredKana(string text, IDialogContext context)
            => this.Transcribe(text, this.conversationSettingsService.GetPreferredKanaType(context));
    }
}
