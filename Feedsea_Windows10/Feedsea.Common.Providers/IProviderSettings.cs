namespace Feedsea.Common.Providers
{
    public interface IProviderSettings
    {
        bool ShowRead { get; set; }
        bool ArticlesFromOldestToNewest { get; set; }
        bool ShowReadIfNoUnread { get; set; }
        string ProfilePicture { get; set; }
        string LoginEmail { get; set; }
        string UserName { get; set; }
        string LoggedInService { get; set; }
    }
}
