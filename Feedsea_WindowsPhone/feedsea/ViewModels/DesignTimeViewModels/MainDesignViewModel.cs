using feedsea.Common;
using feedsea.Common.Providers;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.ViewModels.DesignTimeViewModels
{
    public class MainDesignViewModel
    {
        public bool IsBusy { get; set; }
        public bool PaginationBusy { get; set; }
        public bool SourceLoadingBusy { get; set; }
        //public System.Collections.ObjectModel.ObservableCollection<Article> Articles { get; set; }
        //public System.Collections.ObjectModel.ObservableCollection<Category> Sources { get; set; }

        public ArticleTemplateType ArticleTemplateType { get { return Common.ArticleTemplateType.NormalTemplate; } }

        public MainDesignViewModel()
        {
            IsBusy = true;
            PaginationBusy = true;
            SourceLoadingBusy = true;
//            Articles = new System.Collections.ObjectModel.ObservableCollection<Common.Data.Article>();
//            Articles.Add(new Article()
//            {
//                Title = "Mussum ipsum cacilds, vidis litro abertis. Consetis adipiscings elitis. Pra lá , depois divoltis porris, paradis. Paisis, filhis, espiritis santis. Mé faiz elementum girarzis, nisi eros vermeio, in elementis mé pra quem é amistosis quis leo. Manduma pindureta quium dia nois paga. Sapien in monti palavris qui num significa nadis i pareci latim. Interessantiss quisso pudia ce receita de bolis, mais bolis eu num gostis.",
//                Summary = "Mussum ipsum cacilds, vidis litro abertis. Consetis adipiscings elitis. Pra lá , depois divoltis porris, paradis. Paisis, filhis, espiritis santis. Mé faiz elementum girarzis, nisi eros vermeio, in elementis mé pra quem é amistosis quis leo. Manduma pindureta quium dia nois paga. Sapien in monti palavris qui num significa nadis i pareci latim. Interessantiss quisso pudia ce receita de bolis, mais bolis eu num gostis.",
//                Date = DateTime.Now,
//                Author = "Mussum Ipsum",
//                URL = "http://mussumipsum.com/",
//                ArticleContents = new System.Data.Linq.EntitySet<ArticleContent>()
//                {
//                    new ArticleContent() {
//                        ArticleContentID = 1,
//                        Content = @"<p>Mussum ipsum cacilds, vidis litro abertis. Consetis adipiscings elitis. Pra lá , depois divoltis porris, paradis. Paisis, filhis, espiritis santis. Mé faiz elementum girarzis, nisi eros vermeio, in elementis mé pra quem é amistosis quis leo. Manduma pindureta quium dia nois paga. Sapien in monti palavris qui num significa nadis i pareci latim. Interessantiss quisso pudia ce receita de bolis, mais bolis eu num gostis.
//                                    Suco de cevadiss, é um leite divinis, qui tem lupuliz, matis, aguis e fermentis. Interagi no mé, cursus quis, vehicula ac nisi. Aenean vel dui dui. Nullam leo erat, aliquet quis tempus a, posuere ut mi. Ut scelerisque neque et turpis posuere pulvinar pellentesque nibh ullamcorper. Pharetra in mattis molestie, volutpat elementum justo. Aenean ut ante turpis. Pellentesque laoreet mé vel lectus scelerisque interdum cursus velit auctor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam ac mauris lectus, non scelerisque augue. Aenean justo massa.</p>"
//                    },
//                }
//            });

            //Sources = new System.Collections.ObjectModel.ObservableCollection<Category>();
            //Sources.Add(new Category()
            //{
            //    Name = "Teste Cat",
            //    NewsSources = new EntitySet<NewsSource>() { new NewsSource()
            //{
            //    Name = "Teste Source",
            //},
            //new NewsSource()
            //{
            //    Name = "Teste Source",
            //}}
            //});
        }
    }
}
