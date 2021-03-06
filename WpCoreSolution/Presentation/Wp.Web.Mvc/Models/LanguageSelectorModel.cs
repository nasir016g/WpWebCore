﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Web.Mvc.Models
{
    public class LanguageSelectorModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }

        public bool UseImages { get; set; }

        // nested classes
        public partial class LanguageModel
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string FlagImageFileName { get; set; }

        }
    }
    
}
