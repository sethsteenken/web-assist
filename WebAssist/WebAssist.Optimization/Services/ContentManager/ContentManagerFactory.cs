using Newtonsoft.Json;
using System.Configuration;
using System.Web;

namespace WebAssist.Optimization
{
    internal static class ContentManagerFactory
    {
        public static IContentManager GetInstance(HttpContextBase httpContext)
        {
            ContentManager manager;
            if (TryGetInstanceFromHttpContext(httpContext, out manager))
                return manager;

            manager = BuildManager(httpContext);

            SetToHttpContext(httpContext, manager);
            return manager;
        }

        private static bool TryGetInstanceFromHttpContext(HttpContextBase context, out ContentManager manager)
        {
            if (context == null)
            {
                manager = null;
                return false;
            }
                
            manager = (ContentManager)context.Items[ContentManager.ContextKey];
            return manager != null;
        }

        private static ContentManager BuildManager(HttpContextBase httpContext)
        {
            IBundleConfigurationBuilder configBuilder = new BundleConfigurationBuilder(new JsonSerializer());

            IBundlingSettings bundlingSettings = GetBundlingSettings();            

            string configsDirectory = httpContext.Server.MapPath(bundlingSettings.DefinitionsDirectory);
            IBundleResolver bundleResolver = new BundleResolver(
                bundlingSettings,
                new FileWriteTimeVersionResolver(httpContext.Server),
                configBuilder.GetConfigurations(configsDirectory));

            return new ContentManager(
                new BundleInterpreter(bundleResolver, new UrlPathFormatter(httpContext.Request), bundlingSettings)
            );
        }

        private static IBundlingSettings GetBundlingSettings()
        {
            IBundlingSettings bundlingSettings = BundlingSettings.Instance;
            if (bundlingSettings != null)
                return bundlingSettings;

            var configuration = ConfigurationManager.GetSection(BundlingConfigurationSection.SectionName) as BundlingConfigurationSection;
            if (configuration == null)
                throw new ConfigurationErrorsException($"Configuration section '{BundlingConfigurationSection.SectionName}' of type {typeof(BundlingConfigurationSection).FullName} was not found. Either establish this section in your application's configuration or call BundlingSettings.ApplySettings on application startup.");

            BundlingSettings.ApplySettings(configuration);
            return configuration;
        }

        private static void SetToHttpContext(HttpContextBase context, ContentManager manager)
        {
            if (context == null || manager == null)
                return;

            context.Items[ContentManager.ContextKey] = manager;
        }
    }
}
