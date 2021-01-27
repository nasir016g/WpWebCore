namespace Nsr.Common.Core.Localization
{
    public class LocaleStringResource: Entity
    {
        public string ResourceName { get; set; }       
        public string ResourceValue { get; set; }
        public int LanguageId { get; set; }        
        public virtual Language Language { get; set; }
    }
}
