using FluentValidation;
using Wp.Web.Framework.Models.Admin;

namespace Wp.Web.Api.Admin.Validators.Admin
{
    public class WebPageValidator : AbstractValidator<WebPageModel>
    {
        public WebPageValidator()
        {
            RuleFor(x => x.NavigationName).MinimumLength(4);
        }
    }
}
