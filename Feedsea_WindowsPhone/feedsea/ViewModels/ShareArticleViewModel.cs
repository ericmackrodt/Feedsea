using feedsea.Common;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.MVVM.Services;
using feedsea.Common.Providers;
using feedsea.Common.Providers.OneNote;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using feedsea.Common.Helpers;
using feedsea.Models;
using System.Collections.ObjectModel;
using feedsea.Resources;
using feedsea.Common.Providers.Pocket;
using feedsea.Common.Providers.Instapaper;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.MobilizerProvider;
using feedsea.Common.Controls;

namespace feedsea.ViewModels
{
    public class ShareArticleViewModel : AViewModel<ShareArticleViewModel>
    {
        public event EventHandler Done;

        private ArticleData _article;
        private IOneNoteSettings _oneNoteSettings;
        private IShareProxy _share;
        private IOneNoteProvider _oneNote;
        private IThirdPartySettings _thirdPartySettings;
        private IGeneralSettings _generalSettings;
        private IPocketSettings _pocketSettings;
        private IPocketProvider _pocket;
        private IInstapaperSettings _instapaperSettings;
        private IInstapaperProvider _instapaper;
        private ILoadingService _loadingService;
        private IMobilizerProvider _contentDownloader;
        private IArticleHtmlBuilder _articleHtml;

        public bool IsOneNoteEnabled { get { return _oneNoteSettings.IsLoggedInSetting; } }

        private ObservableCollection<ShareButtonModel> buttons;
        public ObservableCollection<ShareButtonModel> Buttons
        {
            get { return buttons; }
            set
            {
                buttons = value;
                NotifyChanged(o => o.Buttons);
            }
        }

        public ShareArticleViewModel()
        {

        }
        
        public ShareArticleViewModel(
            IShareProxy shareProxy, 
            IOneNoteProvider oneNoteProvider, 
            IOneNoteSettings oneNoteSettings,  
            IGeneralSettings generalSettings,
            IThirdPartySettings thirdPartySettings,
            IConnectionVerify connectionVerify,
            IPocketSettings pocketSettings,
            IPocketProvider pocketProvider,
            IInstapaperSettings instapaperSettings,
            IInstapaperProvider instapaperProvider,
            ILoadingService loadingService,
            IMobilizerProvider mobilizerProvider,
            IArticleHtmlBuilder htmlBuilder)
        {
            _share = shareProxy;
            _oneNote = oneNoteProvider;
            _oneNoteSettings = oneNoteSettings;
            _thirdPartySettings = thirdPartySettings;
            _generalSettings = generalSettings;
            connection = connectionVerify;
            _pocketSettings = pocketSettings;
            _pocket = pocketProvider;
            _instapaper = instapaperProvider;
            _instapaperSettings = instapaperSettings;
            _loadingService = loadingService;
            _contentDownloader = mobilizerProvider;
            _articleHtml = htmlBuilder;

            BuildShareButtons();
        }

        private void BuildShareButtons()
        {
            Buttons = new ObservableCollection<ShareButtonModel>();
            Buttons.Add(new ShareButtonModel()
            {
                Title = AppResources.Share_SocialNetworks,
                Image = "../Assets/Icons/share.social.png",
                ShareCommand = new RelayCommand(SocialNetworkShare)
            });
            Buttons.Add(new ShareButtonModel()
            {
                Title = AppResources.Share_Email,
                Image = "../Assets/Icons/share.email.png",
                ShareCommand = new RelayCommand(EmailShare)
            });
            Buttons.Add(new ShareButtonModel()
            {
                Title = AppResources.Share_TextMessage,
                Image = "../Assets/Icons/share.sms.png",
                ShareCommand = new RelayCommand(TextMessageShare)
            });
            Buttons.Add(new ShareButtonModel()
            {
                Title = AppResources.Share_Clipboard,
                Image = "../Assets/Icons/share.clipboard.png",
                ShareCommand = new RelayCommand(ClipboardShare)
            });

            //if (false)
            //    Buttons.Add(new ShareButtonModel()
            //    {
            //        Title = AppResources.Share_NFC,
            //        Image = "",
            //        ShareCommand = new RelayCommand(NfcShare)
            //    });

            if (_oneNoteSettings.IsLoggedInSetting)
                Buttons.Add(new ShareButtonModel()
                {
                    Title = AppResources.Share_OneNote,
                    Image = "../Assets/Icons/share.onenote.png",
                    ShareCommand = new AsyncDelegateCommand(OneNoteShare)
                });

            if (_thirdPartySettings.IsPocketEnabledSetting)
            {
                var model = new ShareButtonModel()
                {
                    Title = AppResources.Share_Pocket,
                    Image = "../Assets/Icons/share.pocket.png"
                };

                if (_thirdPartySettings.PocketShareEnabledSetting)
                    model.ShareCommand = new RelayCommand(PocketThirdPartyShare);
                else if (_pocketSettings.IsEnabledSetting)
                    model.ShareCommand = new AsyncDelegateCommand(PocketShare);

                Buttons.Add(model);
            }

            if (_instapaperSettings.IsEnabledSetting)
                Buttons.Add(new ShareButtonModel()
                {
                    Title = AppResources.Share_Instapaper,
                    Image = "../Assets/Icons/share.instapaper.png",
                    ShareCommand = new AsyncDelegateCommand(InstapaperShare)
                });
        }

        private async Task InstapaperShare(object arg)
        {
            SendDone();
            await ConnectionVerifyCall(async () =>
            {
                _loadingService.StartLoading(AppResources.Msg_SendingTo_Instapaper);
                await _instapaper.Add(_article);
                _loadingService.EndLoading(AppResources.Msg_SendingTo_Instapaper_Done);
            }, EndLoading);
        }

        private async Task PocketShare(object arg)
        {
            SendDone();
            await ConnectionVerifyCall(async () =>
            {
                _loadingService.StartLoading(AppResources.Msg_SendingTo_Pocket);
                await _pocket.Add(_article);
                _loadingService.EndLoading(AppResources.Msg_SendingTo_Pocket_Done);
            }, EndLoading);
        }

        private void PocketThirdPartyShare()
        {
            SendDone();
            PocketWP.PocketHelper.AddItemToPocket(_article.URL, null, _article.Title, null);
        }

        private async Task OneNoteShare(object arg)
        {
            SendDone();
            _loadingService.StartLoading(AppResources.Msg_SendingTo_OneNote);

            await ConnectionVerifyCall(async () =>
            {
                if (string.IsNullOrWhiteSpace(_article.Content) && _generalSettings.DownloadArticleIfNoContentSetting)
                {
                    _article.Content = await _contentDownloader.GetMobilized(_article.URL);
                }

                if (!_article.IsDataLoaded)
                {
                    _article.Content = await _articleHtml.BuildHtml(_article, _thirdPartySettings.YoutubeClientSetting);
                    _article.IsDataLoaded = true;
                }

                await _oneNote.AddPage(_article.Content);

                _loadingService.EndLoading(AppResources.Msg_SendingTo_OneNote_Done);
            }, EndLoading);
        }

        private void NfcShare()
        {
            _share.SendToNFC(_article.URL);
            SendDone();
        }

        private void ClipboardShare()
        {
            _share.SendToClipboard(_article.URL);
            SendDone();
        }

        private void TextMessageShare()
        {
            _share.SendToTextMessage(_article.Title, _article.URL);
            SendDone();
        }

        private void EmailShare()
        {
            _share.SendToEmail(_article.Title, _article.URL);
            SendDone();
        }

        private void SocialNetworkShare()
        {
            _share.SendToSocialNetworks(_article.Title, _article.URL);
            SendDone();
        }

        public override void LoadData(object argument)
        {
            _article = (ArticleData)argument;
        }
                
        private void SendDone() 
        {
            if (Done != null)
                Done(this, new EventArgs());
        }

        private void EndLoading()
        {
            _loadingService.EndLoading();
        }
    }
}
