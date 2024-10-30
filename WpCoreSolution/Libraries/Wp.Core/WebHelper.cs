using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Wp.Core
{
    public class WebHelper : IWebHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        public WebHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        /// <summary>
        /// Get the raw path and full query of request
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Raw URL</returns>
        public virtual string GetRawUrl(HttpRequest request)
        {
            //first try to get the raw target from request feature
            //note: value has not been UrlDecoded
            var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            //or compose raw URL manually
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";

            return rawUrl;
        }

        public virtual string ApplicationPath
        {
            get
            {
                if (_httpContext.HttpContext.Request.Path == "/")
                    return _httpContext.HttpContext.Request.Path;
                else
                    return _httpContext.HttpContext.Request.Path + "/";
            }
        }
    }
}
