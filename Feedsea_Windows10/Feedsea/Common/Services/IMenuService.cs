using Feedsea.Models;
using System.Collections.Generic;

namespace Feedsea.Common.Services
{
    public interface IMenuService
    {
        IEnumerable<MenuItem> MainMenuItems();
        IEnumerable<MenuItem> MainMenuSecondaryItems();
    }
}