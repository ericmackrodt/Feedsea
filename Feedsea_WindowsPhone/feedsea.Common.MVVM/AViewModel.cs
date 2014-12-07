using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace feedsea.Common.MVVM
{
    public delegate T ExecuteDataAsyncAction<T>();

    [DataContract]
    public abstract class AViewModel<T> : INotifyPropertyChanged, INotifyPropertyChanging where T : AViewModel<T>
    {
        private bool isBusy;
        private bool isDataLoaded;
        protected IConnectionVerify connection;
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        
        [DataMember]
        public virtual bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyChanged(o => o.IsBusy);
            }
        }

        [DataMember]
        public virtual bool IsDataLoaded
        {
            get { return this.isDataLoaded; }
            set
            {
                this.isDataLoaded = value;
                NotifyChanged(o => o.IsDataLoaded);
            }
        }

        public virtual void NotifyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public virtual void NotifyChanged(Expression<Func<T, object>> func)
        {
            //var regexProperty = @"(.+ => )?(Convert\(.+ => )?.+\.(?<Property>\w+)\)?";
            //var property = Regex.Match(func.Body.ToString(), regexProperty).Groups["Property"].Value;
            var property = GetProperty(func);
            
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        
        public virtual void NotifyChanging(Expression<Func<T, object>> func)
        {
            var property = GetProperty(func);

            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(property));
        }

        private string GetProperty(Expression<Func<T, object>> func)
        {
            var body = func.Body;
            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;
            var property = (body as MemberExpression).Member.Name;
            return property;
        }

        public virtual void LoadData()
        {
        }

        public virtual void LoadData(object argument)
        {
        }

        protected async Task<TResult> ConnectionVerifyCall<TResult, TInput>(Func<TInput, Task<TResult>> call, TInput input, Action catchAction = null)
        {
            if (!connection.HasInternetConnection())
                return default(TResult);

            try
            {
                return await call(input);
            }
            catch (Exception ex)
            {
                connection.VerifyConnectionException(ex);
                if (catchAction != null)
                    catchAction();
            }

            return default(TResult);
        }

        protected async Task<T> ConnectionVerifyCall<T>(Func<Task<T>> call, Action catchAction = null)
        {
            if (!connection.HasInternetConnection())
                return default(T);

            try
            {
                return await call();
            }
            catch (Exception ex)
            {
                connection.VerifyConnectionException(ex);
                if (catchAction != null)
                    catchAction();
            }

            return default(T);
        }

        protected async Task ConnectionVerifyCall(Func<Task> call, Action catchAction = null)
        {
            if (!connection.HasInternetConnection())
                return;

            try
            {
                await call();
            }
            catch (Exception ex)
            {
                connection.VerifyConnectionException(ex);
                if (catchAction != null)
                    catchAction();
            }
        }

        protected async Task ConnectionVerifyCall<T>(Func<T, Task> call, T input, Action catchAction = null)
        {
            if (!connection.HasInternetConnection())
                return;

            try
            {
                await call(input);
            }
            catch (Exception ex)
            {
                connection.VerifyConnectionException(ex);
                if (catchAction != null)
                    catchAction();
            }
        }
    }
}