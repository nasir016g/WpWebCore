using FluentValidation;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Validators.Admin
{
    public class WebPageValidator : AbstractValidator<WebPageModel>
    {
        public WebPageValidator()
        {
            RuleFor(x => x.NavigationName).MinimumLength(4);
        }
    }
}
