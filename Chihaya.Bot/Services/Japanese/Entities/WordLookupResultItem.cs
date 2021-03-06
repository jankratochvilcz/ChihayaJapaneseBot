﻿using System;
using System.Collections.Generic;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class WordLookupResultItem
    {
        public string ReadingWithKanji { get; set; }
        public string ReadingWithKana { get; set; }
        public List<string> EnglishDefinitions { get; set; }
    }
}
