using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace ProjetCESI.Web.Outils
{
    public class ViewToStringRendererService : ViewExecutor
    {
        private readonly IActionContextAccessor _ActionContextAccessor;
        private ITempDataProvider _TempDataProvider;

        public ViewToStringRendererService(
            IActionContextAccessor actionContextAccessor,
            IOptions<MvcViewOptions> viewOptions,
            IHttpResponseStreamWriterFactory writerFactory,
            ICompositeViewEngine viewEngine,
            ITempDataDictionaryFactory tempDataFactory,
            DiagnosticListener diagnosticListener,
            IModelMetadataProvider modelMetadataProvider,
            ITempDataProvider tempDataProvider)
            : base(viewOptions, writerFactory, viewEngine, tempDataFactory, diagnosticListener, modelMetadataProvider)
        {
            _ActionContextAccessor = actionContextAccessor;
            _TempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var context = GetActionContext();

            if (context == null) throw new ArgumentNullException(nameof(context));

            var result = new ViewResult()
            {
                ViewData = new ViewDataDictionary(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                {
                    Model = model,
                },
                TempData = new TempDataDictionary(
                        context.HttpContext,
                        _TempDataProvider),
                ViewName = viewName,
            };

            var viewEngineResult = FindView(context, result);
            viewEngineResult.EnsureSuccessful(originalLocations: null);

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    context,
                    view,
                    new ViewDataDictionary(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        context.HttpContext,
                        _TempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
        private ActionContext GetActionContext()
        {
            return _ActionContextAccessor.ActionContext;
            //// Modified to get the global request context.
            //var httpContext = _httpContextAccessor.HttpContext;
            //if (httpContext == null)
            //{
            //    httpContext = new DefaultHttpContext();
            //    httpContext.RequestServices = _serviceProvider;
            //}
            //return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }

        /// 
        /// Attempts to find the  associated with .
        /// 
        /// The  associated with the current request.
        /// The .
        /// A .
        ViewEngineResult FindView(ActionContext actionContext, ViewResult viewResult)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (viewResult == null)
            {
                throw new ArgumentNullException(nameof(viewResult));
            }

            var viewEngine = viewResult.ViewEngine ?? ViewEngine;

            var viewName = viewResult.ViewName ?? GetActionName(actionContext);

            var result = viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            var originalResult = result;
            if (!result.Success)
            {
                result = viewEngine.FindView(actionContext, viewName, isMainPage: true);
            }

            if (!result.Success)
            {
                if (originalResult.SearchedLocations.Any())
                {
                    if (result.SearchedLocations.Any())
                    {
                        // Return a new ViewEngineResult listing all searched locations.
                        var locations = originalResult.SearchedLocations.ToList();
                        locations.AddRange(result.SearchedLocations);
                        result = ViewEngineResult.NotFound(viewName, locations);
                    }
                    else
                    {
                        // GetView() searched locations but FindView() did not. Use first ViewEngineResult.
                        result = originalResult;
                    }
                }
            }

            if (!result.Success)
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", viewName));

            return result;
        }


        private const string ActionNameKey = "action";
        private static string GetActionName(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.RouteData.Values.TryGetValue(ActionNameKey, out var routeValue))
            {
                return null;
            }

            var actionDescriptor = context.ActionDescriptor;
            string normalizedValue = null;
            if (actionDescriptor.RouteValues.TryGetValue(ActionNameKey, out var value) &&
                !string.IsNullOrEmpty(value))
            {
                normalizedValue = value;
            }

            var stringRouteValue = routeValue?.ToString();
            if (string.Equals(normalizedValue, stringRouteValue, StringComparison.OrdinalIgnoreCase))
            {
                return normalizedValue;
            }

            return stringRouteValue;
        }
    }

    public static class VTSRExts
    {
        // Use this extension method to register the service in your `Startup.cs`, to avoid missing some service dependencies
        public static void AddViewToStringRendererService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<ViewToStringRendererService, ViewToStringRendererService>();
        }
    }
}
