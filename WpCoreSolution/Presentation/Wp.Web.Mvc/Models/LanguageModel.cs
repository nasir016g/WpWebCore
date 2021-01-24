namespace Wp.Web.Mvc.Models
{
    public partial class LanguageModel : BaseModel
    {
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public string UniqueSeoCode { get; set; }
        public string FlagImageFileName { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }

    }

    public partial class LanguageResourceModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }

        //[AllowHtml]
        //[Required(ErrorMessage = "Name is required.")]
        //[WpResourceDisplayName("Common.Name")]
        public string ResourceName { get; set; }

        //[AllowHtml]
        //[Required(ErrorMessage = "Value is required.")]
        //[WpResourceDisplayName("Common.Value")]
        public string ResourceValue { get; set; }

        //[AllowHtml]
        //[DisplayName("Language")]
        public string LanguageName { get; set; }
    }
}
