using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace WebAPIDemo
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {

        // The parameter actionContext provides us with both the request and the response object
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            // Check if the request was issued using HTTPS
            // If not redirect to HTTPS
            if(actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent("<p> Use HTTPS instead of HTTP</p>", System.Text.Encoding.UTF8, "text/html");

                // Redirecting the request using HTTPS
                UriBuilder uriBuilder = new UriBuilder(actionContext.Request.RequestUri); // Builds the URI from the request object
                uriBuilder.Scheme = Uri.UriSchemeHttps; // Changing the scheme to HTTPS
                uriBuilder.Port = 44362; // Change the port to the one for HTTPS (F4 on WepApi)

                // Set the URI within the location header
                actionContext.Response.Headers.Location = uriBuilder.Uri;
            }
            else
            {
                // The request was issued using HTTPS
                // Execute the class the base class on authorization

                base.OnAuthorization(actionContext);
            }
            
        }
    }
}