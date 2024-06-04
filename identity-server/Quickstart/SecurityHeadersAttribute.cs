// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace IdentityServer.Quickstart;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SecurityHeadersAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ViewResult)
            return;
        
        var headerDictionary = context.HttpContext.Response.Headers;

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
        if (!headerDictionary.ContainsKey("X-Content-Type-Options"))
            headerDictionary.Append("X-Content-Type-Options", "nosniff");

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        if (!headerDictionary.ContainsKey("X-Frame-Options"))
            headerDictionary.Append("X-Frame-Options", "SAMEORIGIN");

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
        const string csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
        // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
        //csp += "upgrade-insecure-requests;";
        // also an example if you need client images to be displayed from twitter
        // csp += "img-src 'self' https://pbs.twimg.com;";

        // once for standards compliant browsers
        if (!headerDictionary.ContainsKey("Content-Security-Policy"))
            headerDictionary.Append("Content-Security-Policy", csp);

        // and once again for IE
        if (!headerDictionary.ContainsKey("X-Content-Security-Policy"))
            headerDictionary.Append("X-Content-Security-Policy", csp);

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
        const string referrerPolicy = "no-referrer";
        if (!headerDictionary.ContainsKey("Referrer-Policy"))
            headerDictionary.Append("Referrer-Policy", referrerPolicy);
    }
}
