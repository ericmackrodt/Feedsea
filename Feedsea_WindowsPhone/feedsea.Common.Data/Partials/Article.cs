using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    public partial class Article
    {
        public string MainImageUrl
        {
            get
            {
                ArticleImage image = null;

                image = ArticleImages.FirstOrDefault(o => o.ImageScope == "Thumbnail");

                if (image == null)
                    image = ArticleImages.FirstOrDefault(o => o.ImageScope == "SeparateContent");

                if (image == null)
                    image = ArticleImages.FirstOrDefault();

                return image == null ? null : image.ImageUrl;
            }
        }

        //partial void OnIsReadChanged()
        //{
        //    NewsSource.UnreadNumber = NewsSource.Articles.Count(o => !o.IsRead);
        //}
    }
}
