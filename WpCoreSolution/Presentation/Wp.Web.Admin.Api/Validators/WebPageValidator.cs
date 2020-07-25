using FluentValidation;
using Wp.Web.Admin.Api.Models.Admin;

namespace Wp.Web.Admin.Api.Validators.Admin
{
    public class WebPageValidator : AbstractValidator<WebPageModel>
    {
        public WebPageValidator()
        {
            RuleFor(x => x.NavigationName).MinimumLength(4);
        }
    }
}
