using Autofac;
using Autofac.Extensions.DependencyInjection;
using Chihaya.Bot.Dialogs;
using Chihaya.Bot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Chihaya.Bot
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            containerBuilder
                .RegisterType<MicrosoftCognitiveTranslationService>()
                .Keyed<IJapaneseTranslationService>(FiberModule.Key_DoNotSerialize)
                .As<IJapaneseTranslationService>();

            containerBuilder
                .RegisterType<MicrosoftCognitiveAuthenticationService>()
                .Keyed<IMicrosoftCognitiveAuthenticationService>(FiberModule.Key_DoNotSerialize)
                .As<IMicrosoftCognitiveAuthenticationService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<HttpJishoOrgWordLookupService>()
                .Keyed<IWordLookupService>(FiberModule.Key_DoNotSerialize)
                .As<IWordLookupService>();

            containerBuilder
                .RegisterType<FallbackIntentRecognitionService>()
                .Keyed<IFallbackIntentRecognitionService>(FiberModule.Key_DoNotSerialize)
                .As<IFallbackIntentRecognitionService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<KuromojiJapaneseTokenizationService>()
                .Keyed<IJapaneseTokenizationService>(FiberModule.Key_DoNotSerialize)
                .As<IJapaneseTokenizationService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<RootDialog>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<WordLookUpDialog>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<TranslateDialog>()
                .InstancePerDependency();

            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
