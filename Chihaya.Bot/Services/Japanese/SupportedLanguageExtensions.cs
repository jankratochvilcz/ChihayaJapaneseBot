using System;

namespace Chihaya.Bot.Services
{
    public static class SupportedLanguageExtensions
    {
        public static string ToCode(this SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.English:
                    return "en";
                case SupportedLanguage.Japanese:
                    return "ja";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
