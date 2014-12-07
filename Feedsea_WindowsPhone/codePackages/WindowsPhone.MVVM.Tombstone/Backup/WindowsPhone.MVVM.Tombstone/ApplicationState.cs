using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

namespace WindowsPhone.MVVM.Tombstone
{
    internal class ApplicationState
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        
        /// <summary>
        /// Runs over all the properties in the page's viewmodel and saves the state of those that have the tombstone attribute applied
        /// </summary>
        static internal void Save(PhoneApplicationPage page)
        {
            foreach (PropertyInfo tombstoneProperty in GetTombstoneProperties(page.DataContext))
            {
                string key = GetKey(tombstoneProperty);
                
                object value = tombstoneProperty.GetValue(page.DataContext, null);
                page.State[key] = JsonConvert.SerializeObject(value, Formatting.None, settings);
            }
        }

        /// <summary>
        /// Runs over all the properties in the page's viewmodel and restores the state of those that have the tombstone attribute applied and have no value
        /// </summary>
        static internal void Restore(PhoneApplicationPage page)
        {
            foreach (PropertyInfo tombstoneProperty in GetTombstoneProperties(page.DataContext))
            {
                if (tombstoneProperty.GetValue(page.DataContext, null) == null)
                {
                    string key = GetKey(tombstoneProperty);
                    if (page.State.ContainsKey(key))
                    {
                        tombstoneProperty.SetValue(page.DataContext, JsonConvert.DeserializeObject((string)page.State[key], tombstoneProperty.PropertyType, settings), null);
                    }
                }
            }
        }

        /// <summary>
        /// Defines the key we are saving in the page's state bag
        /// </summary>
        private static string GetKey(PropertyInfo Prop)
        {
            return "tshelper.viewmodel." + Prop.Name;
        }

        /// <summary>
        /// Obtains all the propertyinfos that have the TombstoneAttribute applied
        /// </summary>
        /// <param name="ViewModel">The viewmodel to check. This is set to the page's DataContext</param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetTombstoneProperties(object ViewModel)
	    {
		    if (ViewModel != null) {
			    IEnumerable<PropertyInfo> tsProps = from p in ViewModel.GetType().GetProperties() 
                                                where p.GetCustomAttributes(typeof(TombstoneAttribute), false).Length > 0
                                                select p;

			    foreach (PropertyInfo tsProp in tsProps) {
				    if (!tsProp.CanRead || !tsProp.CanWrite) {
    					throw new TombstoneException(string.Format("Cannot restore value of property {0}. Make sure the getter and setter are public", tsProp.Name));
				    }
			    }
			    return tsProps;
		    } else {
			    return new List<PropertyInfo>();
		    }
	    }
    }

}
