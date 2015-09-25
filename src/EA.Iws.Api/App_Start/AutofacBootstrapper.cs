﻿namespace EA.Iws.Api
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using DataAccess;
    using DataAccess.Identity;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Autofac;
    using RequestHandlers;
    using RequestHandlers.Feedback;
    using Services;

    public class AutofacBootstrapper
    {
        public static IContainer Initialize(ContainerBuilder builder, HttpConfiguration config)
        {
            // Register all controllers
            builder.RegisterApiControllers(typeof(Startup).Assembly);

            // Register Autofac filter provider
            builder.RegisterWebApiFilterProvider(config);

            // Register model binders
            builder.RegisterWebApiModelBinders(typeof(Startup).Assembly);
            builder.RegisterWebApiModelBinderProvider();

            // Register all Autofac specific IModule implementations
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
            builder.RegisterAssemblyModules(typeof(AutofacMediator).Assembly);

            builder.RegisterModule(new RequestHandlerModule());
            builder.RegisterModule(new EntityFrameworkModule());

            // http://www.talksharp.com/configuring-autofac-to-work-with-the-aspnet-identity-framework-in-mvc-5
            builder.RegisterType<IwsIdentityContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().As<UserManager<ApplicationUser>>().InstancePerRequest();

            builder.Register(c =>
            {
                var componentContext = c.Resolve<IComponentContext>();
                var configFeedback = componentContext.Resolve<AppConfiguration>();
                return new FeedbackInformation(configFeedback.FeedbackEmailTo);
            }).SingleInstance();

            return builder.Build();
        }
    }
}