using Autofac;
using Autofac.Core.Activators.Reflection;
using Cimbalino.Phone.Toolkit.Services;
using feedsea.Common;
using feedsea.Common.Controls;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Services;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Feedly;
using feedsea.Common.Providers.Instapaper;
using feedsea.Common.Providers.MobilizerProvider;
using feedsea.Common.Providers.OneNote;
using feedsea.Common.Providers.Pocket;
using feedsea.Services;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.ViewModels
{

   public class ViewModelLocator
   {
      private readonly Autofac.IContainer container;

      public ViewModelLocator()
      {
         var containerBuilder = new ContainerBuilder();
         //Services
         containerBuilder.RegisterType<MessageBoxService>().As<IMessageBoxService>();
         containerBuilder.RegisterType<ConnectionVerify>().As<IConnectionVerify>();
         containerBuilder.RegisterType<ProviderStorage>().As<IProviderStorage>();
         containerBuilder.RegisterType<TilePin>().As<ITilePin>();
         containerBuilder.RegisterType<OAuthLogin>().As<IOAuthLogin>();
         containerBuilder.RegisterType<XAuthLogin>().As<IXAuthLogin>();
         containerBuilder.RegisterType<ToastService>().As<IToastService>();
         containerBuilder.RegisterType<ShareService>().As<IShareService>();
         containerBuilder.RegisterType<ShareProxy>().As<IShareProxy>();
         containerBuilder.RegisterType<FullLoadingService>().As<IFullLoadingService>();
         containerBuilder.RegisterType<LoadingService>().As<ILoadingService>();
         containerBuilder.RegisterType<PaidFeatures>().As<IPaidFeatures>();
         containerBuilder.RegisterType<ArticleHtmlBuilder>().As<IArticleHtmlBuilder>();
         //Settings
         containerBuilder.RegisterType<GeneralSettings>().As<IProviderSettings>();
         containerBuilder.RegisterType<GeneralSettings>().As<IGeneralSettings>();
         containerBuilder.RegisterType<LiveTileSettings>().As<ILiveTileSettings>();
         containerBuilder.RegisterType<AppearanceSettings>().As<IAppearanceSettings>();
         containerBuilder.RegisterType<ThirdPartySettings>().As<IThirdPartySettings>();
         containerBuilder.RegisterType<OneNoteSettings>().As<IOneNoteSettings>();
         containerBuilder.RegisterType<PocketSettings>().As<IPocketSettings>();
         containerBuilder.RegisterType<InstapaperSettings>().As<IInstapaperSettings>();
         //Providers
         containerBuilder.RegisterType<FeedlyProvider>().As<INewsProvider>();
         containerBuilder.RegisterType<OneNoteProvider>().As<IOneNoteProvider>();
         containerBuilder.RegisterType<MobilizerProvider>().As<IMobilizerProvider>();
         containerBuilder.RegisterType<PocketProvider>().As<IPocketProvider>();
         containerBuilder.RegisterType<InstapaperProvider>().As<IInstapaperProvider>();
         //NEWS SERVICE
         containerBuilder.RegisterType<NewsService>().As<INewsService>().SingleInstance();
         //ViewModels
         containerBuilder.RegisterType<MainViewModel>().SingleInstance().UsingConstructor(new DefaultConstructorSelector());
         containerBuilder.RegisterType<ArticleViewModel>().UsingConstructor(new DefaultConstructorSelector());
         containerBuilder.RegisterType<AddSourceViewModel>().UsingConstructor(new DefaultConstructorSelector());
         containerBuilder.RegisterType<SettingsViewModel>(); //.UsingConstructor(new DefaultConstructorSelector());

         containerBuilder.RegisterType<SettingsGeneralViewModel>(); //.UsingConstructor(new DefaultConstructorSelector());

         containerBuilder.RegisterType<SettingsLiveTilesViewModel>(); //.UsingConstructor(new DefaultConstructorSelector());

         containerBuilder.RegisterType<SettingsAppearanceViewModel>(); //.UsingConstructor(new DefaultConstructorSelector());

         containerBuilder.RegisterType<SettingsThirdPartyViewModel>(); //.UsingConstructor(new DefaultConstructorSelector());

         containerBuilder.RegisterType<LoginViewModel>().UsingConstructor(new DefaultConstructorSelector());
         containerBuilder.RegisterType<ShareArticleViewModel>().UsingConstructor(new DefaultConstructorSelector());
         containerBuilder.RegisterType<SelectedSourceViewModel>();
         containerBuilder.RegisterType<LoggedUserViewModel>();
         containerBuilder.RegisterType<ApplicationFrameViewModel>().SingleInstance().UsingConstructor(new DefaultConstructorSelector());
         container = containerBuilder.Build();
      }

      public MainViewModel MainPage
      {
         get
         {
            return container.Resolve<MainViewModel>();
         }
      }

      public AddSourceViewModel AddSourcePage
      {
         get
         {
            return container.Resolve<AddSourceViewModel>();
         }
      }

      public ArticleViewModel ArticlePage
      {
         get
         {
            return container.Resolve<ArticleViewModel>();
         }
      }

      public SettingsViewModel SettingsPage
      {
         get
         {
            return container.Resolve<SettingsViewModel>();
         }
      }

      public SettingsGeneralViewModel SettingsGeneralPage
      {
         get
         {
            return container.Resolve<SettingsGeneralViewModel>();
         }
      }

      public SettingsAppearanceViewModel SettingsAppearancePage
      {
         get
         {
            return container.Resolve<SettingsAppearanceViewModel>();
         }
      }

      public SettingsLiveTilesViewModel SettingsLiveTilesPage
      {
         get
         {
            return container.Resolve<SettingsLiveTilesViewModel>();
         }
      }

      public SettingsThirdPartyViewModel SettingsThirdPartyPage
      {
         get
         {
            return container.Resolve<SettingsThirdPartyViewModel>();
         }
      }

      public LoginViewModel LoginPage
      {
         get
         {
            return container.Resolve<LoginViewModel>();
         }
      }

      public ShareArticleViewModel ShareArticle
      {
         get
         {
            return container.Resolve<ShareArticleViewModel>();
         }
      }

      public ApplicationFrameViewModel ApplicationFrame
      {
         get
         {
            return container.Resolve<ApplicationFrameViewModel>();
         }
      }

      public SelectedSourceViewModel SelectedSource
      {
         get
         {
            return container.Resolve<SelectedSourceViewModel>();
         }
      }

      public LoggedUserViewModel LoggedUser
      {
         get
         {
            return container.Resolve<LoggedUserViewModel>();
         }
      }

   }

   public class DefaultConstructorSelector
      : IConstructorSelector
   {

      public ConstructorParameterBinding SelectConstructorBinding(ConstructorParameterBinding[] constructorBindings)
      {
         var defaultConstructor = constructorBindings.SingleOrDefault(c => c.TargetConstructor.GetParameters().Length > 0);
         if ( defaultConstructor == null )
            //handle the case when there is no default constructor
            throw new InvalidOperationException();
         return defaultConstructor;
      }

   }

}