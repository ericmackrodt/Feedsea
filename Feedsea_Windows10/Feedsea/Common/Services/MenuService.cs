using System;
using System.Collections.Generic;
using Feedsea.Models;
using Feedsea.Common.Api.Feedly;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Feedsea.Common.Services
{
    public class MenuService : IMenuService
    {
        public IEnumerable<MenuItem> MainMenuItems()
        {
            var resourceLoader = new ResourceLoader();
            var items = new List<MenuItem>();
            items.Add(new MenuItem() { Name = resourceLoader.GetString("MainMenu_Today/Text"), Symbol = Symbol.Home, UrlID = ApiConstants.GlobalCategory_All, IsMostEngaging = true });
            items.Add(new MenuItem() { Name = resourceLoader.GetString("MainMenu_AllArticles/Text"), Symbol = Symbol.Bullets, UrlID = ApiConstants.GlobalCategory_All });
            items.Add(new MenuItem() { Name = resourceLoader.GetString("MainMenu_SavedForLater/Text"), Symbol = Symbol.SolidStar, UrlID = ApiConstants.GlobalTag_Saved });
            return items;
        }

        public IEnumerable<MenuItem> MainMenuSecondaryItems()
        {
            throw new NotImplementedException();
        }
    }
}
