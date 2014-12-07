using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace feedsea.Common.Data.Helpers
{
    public interface IIncluder 
    {
        void Include(System.Data.Linq.DataLoadOptions opt);
    }

    public class Includer<T> : IIncluder
    {
        private Expression<Func<T, object>> expression;

        public Includer(Expression<Func<T, object>> expression)
        {
            this.expression = expression;
        }

        public void Include(System.Data.Linq.DataLoadOptions opt) 
        {
            opt.LoadWith<T>(expression);
        }
    }

    public static class DataHelpers
    {
        public static void LoadIncluded<T>(this Data.RssReaderDataContext cn, params Expression<Func<T, object>>[] expressions)
        {
            System.Data.Linq.DataLoadOptions opt = new System.Data.Linq.DataLoadOptions();
            foreach (var exp in expressions)
                opt.LoadWith<T>(exp);
            cn.LoadOptions = opt;
        }

        public static void LoadIncluded(this Data.RssReaderDataContext cn, params IIncluder[] includers)
        {
            System.Data.Linq.DataLoadOptions opt = new System.Data.Linq.DataLoadOptions();
            foreach (var exp in includers)
                exp.Include(opt);
            cn.LoadOptions = opt;
        }
    }
}
