using Autofac;
using Feedsea.Common.Api.Feedly;
using Feedsea.Common.Components;
using Feedsea.Common.Services;
using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Feedly;
using Feedsea.Settings;
using Feedsea.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea
{
    public class ViewModelLocator
    {
        IContainer _container;
        
        public ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FeedlySettings>().As<IFeedlySettings>();
            builder.RegisterType<FeedlySettings>().As<IFeedlyApiSettings>();
            builder.RegisterType<GeneralSettings>().As<IGeneralSettings>();

            builder.RegisterType<OAuthLogin>().As<IOAuthLogin>();
            builder.RegisterType<FeedlyWebClient>().As<IFeedlyClient>();
            builder.RegisterType<FeedlyProvider>().As<INewsProvider>();
            builder.RegisterType<ProviderStorage>().As<IProviderStorage>();
            builder.RegisterType<MenuService>().As<IMenuService>();
            builder.RegisterType<ShareService>().As<IShareService>();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>();

            builder.RegisterType<WelcomeViewModel>();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<MenusViewModel>();

            _container = builder.Build();
        }

        public WelcomeViewModel Welcome
        {
            get { return _container.Resolve<WelcomeViewModel>(); }
        }

        public MainViewModel Main
        {
            get { return _container.Resolve<MainViewModel>(); }
        }

        public MenusViewModel Menus
        {
            get { return _container.Resolve<MenusViewModel>(); }
        }
    }
}
