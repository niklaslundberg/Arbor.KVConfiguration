using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.SystemConfiguration;
using Autofac;
using Autofac.Integration.Mvc;

namespace Arbor.KVConfiguration.Samples.Web
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            RegisterConfiguration(builder);
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var keyValueConfiguration = container.Resolve<IKeyValueConfiguration>();

            KeyValueConfigurationManager.Initialize(keyValueConfiguration);
        }

        private static void RegisterConfiguration(ContainerBuilder builder)
        {
            IKeyValueConfiguration keyValueConfiguration = KeyValueConfigurationManager
                .Add(new ReflectionKeyValueConfiguration(typeof(Bootstrapper).Assembly))
                .Add(new AppSettingsKeyValueConfiguration())
                .Add(new UserConfiguration.UserConfiguration())
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .DecorateWith(new AddSuffixDecorator("!"))
                .Build(message => Debug.WriteLine(message));

            builder.RegisterInstance(keyValueConfiguration)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }



    internal class AddSuffixDecorator : DecoratorBase
    {
        private readonly string _suffix;

        public AddSuffixDecorator(string suffix)
        {
            _suffix = suffix;
        }
        

        public override string GetValue(string value)
        {
            return $"{value}{_suffix}";
        }
        
    }
}
