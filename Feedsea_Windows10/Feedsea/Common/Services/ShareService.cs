using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedsea.Common.Providers.Data;
using Windows.ApplicationModel.DataTransfer;

namespace Feedsea.Common.Services
{
    public class ShareService : IShareService
    {
        private ArticleData _article;

        private void ShareService_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (_article != null)
            {
                var msg = _article.Title.Length > 140 ? _article.Title.Substring(0, 125) : _article.Title;
                args.Request.Data.SetText(string.Format("{0} (via @feedsea)", msg));
                args.Request.Data.SetWebLink(new Uri(_article.URL));
                args.Request.Data.SetHtmlFormat(_article.Summary);

                args.Request.Data.Properties.ContentSourceWebLink = new Uri(_article.URL);
                args.Request.Data.Properties.ContentSourceApplicationLink = new Uri(_article.URL);
                args.Request.Data.Properties.ApplicationName = Windows.ApplicationModel.Package.Current.DisplayName;
                args.Request.Data.Properties.Description = (string)new Converters.LimitTextLengthConverter().Convert(_article.Summary, null, null, null);
                args.Request.Data.Properties.Title = _article.Title;
            }
            else
            {
                args.Request.FailWithDisplayText("Nothing to share");
            }

            DataTransferManager.GetForCurrentView().DataRequested -= ShareService_DataRequested;
            _article = null;
        }

        public void Share(ArticleData article)
        {
            _article = article;
            DataTransferManager.GetForCurrentView().DataRequested += ShareService_DataRequested;
            DataTransferManager.ShowShareUI();
        }
    }
}

