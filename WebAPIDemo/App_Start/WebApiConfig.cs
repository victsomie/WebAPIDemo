using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;
using System.Web.Http.Cors;

namespace WebAPIDemo
{
    //public class CustomJsonFormatter : JsonMediaTypeFormatter
    //{
    //    public CustomJsonFormatter()
    //    {
    //        this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
    //    }

    //    // override 
    //    public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
    //    {
    //        base.SetDefaultContentHeaders(type, headers, mediaType);
    //        headers.ContentType = new MediaTypeHeaderValue("application/json");

    //    }
    //}

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Registering the HTTPS redirect when HTTP is made
            config.Filters.Add(new RequireHttpsAttribute());



            // Creating a custom formatter
            // config.Formatters.Add(new CustomJsonFormatter());

            // Removing the XML formatter
            // config.Formatters.Remove(config.Formatters.XmlFormatter);

            // -------------------JSONP CONFIGURATION -----------------------------------
            //Removing error when other external aplications are accessing the API
            // Removing the error
            // var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0, jsonpFormatter);
            // ------------------- End of JSONP  ---------------------------------

            // EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();


            // When MEDIATYPE HEADER VALUE IS TEXT/HTML user JsonFormetter to format the value
            // config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Indenting
            // config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            //Camel case
            // config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
