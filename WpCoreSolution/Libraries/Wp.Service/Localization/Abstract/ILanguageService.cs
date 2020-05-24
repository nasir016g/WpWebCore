using Wp.Core;
using Wp.Core.Domain.Localization;

namespace Wp.Services.Localization
{
    public partial interface ILanguageService : IEntityService<Language>
    {
        //Language GetById(int languageId);
    }

}