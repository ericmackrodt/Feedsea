using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace feedsea.Common.MVVM.Tombstone
{
    internal class ApplicationState
    {
        //static JsonSerializerSettings settings = new JsonSerializerSettings
        //{
        //    TypeNameHandling = TypeNameHandling.All
        //};
        
        /// <summary>
        /// Runs over all the properties in the page's viewmodel and saves the state of those that have the tombstone attribute applied
        /// </summary>
        static internal void Save(PhoneApplicationPage page)
        {
            var obj = page.DataContext;

            if (obj == null) return;
            var properties = obj.GetType().GetProperties().Where(o => o.GetCustomAttributes<DataMemberAttribute>().Any());

            if (properties == null || !properties.Any()) return;

            var pageName = page.GetType().Name;

            foreach (var property in properties)
            {
                var key = GetKey(pageName, property.Name);
                page.State[key] = property.GetValue(obj);
            }
        }

        /// <summary>
        /// Runs over all the properties in the page's viewmodel and restores the state of those that have the tombstone attribute applied and have no value
        /// </summary>
        static internal void Restore(PhoneApplicationPage page)
        {
            var obj = page.DataContext;

            if (obj == null) return;
            var properties = obj.GetType().GetProperties().Where(o => o.GetCustomAttributes<DataMemberAttribute>().Any());

            if (properties == null || !properties.Any()) return;

            var pageName = page.GetType().Name;

            foreach (var property in properties)
            {
                var key = GetKey(pageName, property.Name);

                if (page.State.ContainsKey(key))
                {
                    property.SetValue(obj, page.State[key]);
                }
            }
        }

        /// <summary>
        /// Defines the key we are saving in the page's state bag
        /// </summary>
        private static string GetKey(string obj, string property)
        {
            return string.Format("tshelper.viewmodel.{0}.{1}", obj, property);
        }

        private static void SetProperty(object from, object to)
        {
            var toProperties = to.GetType().GetProperties().Where(o => o.GetCustomAttributes<DataMemberAttribute>().Any());

            foreach (var prop in toProperties)
            {
                prop.SetValue(to, prop.GetValue(from));
            }
        }
    }

}
