using System;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace feedsea.Common
{

   //Source: http://www.scottlogic.co.uk/blog/colin/2011/05/metro-in-motion-part-4-tilt-effect/
   /// <summary>
   /// Adapts a DependencyObject to provide methods required for generate
   /// a Linq To Tree API
   /// </summary>
   public class VisualTreeAdapter
      : ILinqTree<Windows.UI.Xaml.DependencyObject>
   {
      private Windows.UI.Xaml.DependencyObject _item;

      public VisualTreeAdapter(Windows.UI.Xaml.DependencyObject item)
      {
         _item = item;
      }

      public IEnumerable<Windows.UI.Xaml.DependencyObject> Children()
      {
         int childrenCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(_item);
         for ( int i = 0; i < childrenCount; i++ )
         {
            yield return Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(_item, i);
         }
      }

      public Windows.UI.Xaml.DependencyObject Parent
      {
         get
         {
            return Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(_item);
         }
      }

   }

}
namespace feedsea.Common
{

   /// <summary>
   /// Defines an interface that must be implemented to generate the LinqToTree methods
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface ILinqTree<T>
   {

      IEnumerable<T> Children();

      T Parent { get; }

   }

   public static class TreeExtensions
   {

      /// <summary>
      /// Returns a collection of descendant elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Descendants(this Windows.UI.Xaml.DependencyObject item)
      {
         ILinqTree<Windows.UI.Xaml.DependencyObject> adapter = new VisualTreeAdapter(item);
         foreach ( var child in adapter.Children() )
         {
            yield return child;
            foreach ( var grandChild in child.Descendants() )
            {
               yield return grandChild;
            }
         }
      }

      /// <summary>
      /// Returns a collection containing this element and all descendant elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> DescendantsAndSelf(this Windows.UI.Xaml.DependencyObject item)
      {
         yield return item;
         foreach ( var child in item.Descendants() )
         {
            yield return child;
         }
      }

      /// <summary>
      /// Returns a collection of ancestor elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Ancestors(this Windows.UI.Xaml.DependencyObject item)
      {
         ILinqTree<Windows.UI.Xaml.DependencyObject> adapter = new VisualTreeAdapter(item);
         var parent = adapter.Parent;
         while ( parent != null )
         {
            yield return parent;
            adapter = new VisualTreeAdapter(parent);
            parent = adapter.Parent;
         }
      }

      /// <summary>
      /// Returns a collection containing this element and all ancestor elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> AncestorsAndSelf(this Windows.UI.Xaml.DependencyObject item)
      {
         yield return item;
         foreach ( var ancestor in item.Ancestors() )
         {
            yield return ancestor;
         }
      }

      /// <summary>
      /// Returns a collection of child elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Elements(this Windows.UI.Xaml.DependencyObject item)
      {
         ILinqTree<Windows.UI.Xaml.DependencyObject> adapter = new VisualTreeAdapter(item);
         foreach ( var child in adapter.Children() )
         {
            yield return child;
         }
      }

      /// <summary>
      /// Returns a collection of the sibling elements before this node, in document order.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsBeforeSelf(this Windows.UI.Xaml.DependencyObject item)
      {
         if ( item.Ancestors().FirstOrDefault() == null )
            yield break;
         foreach ( var child in item.Ancestors().First().Elements() )
         {
            if ( child.Equals(item) )
               break;
            yield return child;
         }
      }

      /// <summary>
      /// Returns a collection of the after elements after this node, in document order.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsAfterSelf(this Windows.UI.Xaml.DependencyObject item)
      {
         if ( item.Ancestors().FirstOrDefault() == null )
            yield break;
         bool afterSelf = false;
         foreach ( var child in item.Ancestors().First().Elements() )
         {
            if ( afterSelf )
               yield return child;
            if ( child.Equals(item) )
               afterSelf = true;
         }
      }

      /// <summary>
      /// Returns a collection containing this element and all child elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsAndSelf(this Windows.UI.Xaml.DependencyObject item)
      {
         yield return item;
         foreach ( var child in item.Elements() )
         {
            yield return child;
         }
      }

      /// <summary>
      /// Returns a collection of descendant elements which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Descendants<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.Descendants().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection of the sibling elements before this node, in document order
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsBeforeSelf<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.ElementsBeforeSelf().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection of the after elements after this node, in document order
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsAfterSelf<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.ElementsAfterSelf().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection containing this element and all descendant elements
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> DescendantsAndSelf<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.DescendantsAndSelf().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection of ancestor elements which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Ancestors<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.Ancestors().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection containing this element and all ancestor elements
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> AncestorsAndSelf<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.AncestorsAndSelf().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection of child elements which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Elements<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.Elements().Where(i => i is T).Cast<DependencyObject>();
      }

      /// <summary>
      /// Returns a collection containing this element and all child elements.
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsAndSelf<T>(this Windows.UI.Xaml.DependencyObject item)
      {
         return item.ElementsAndSelf().Where(i => i is T).Cast<DependencyObject>();
      }

   }

   public static class EnumerableTreeExtensions
   {

      /// <summary>
      /// Applies the given function to each of the items in the supplied
      /// IEnumerable.
      /// </summary>
      private static IEnumerable<Windows.UI.Xaml.DependencyObject> DrillDown(this IEnumerable<Windows.UI.Xaml.DependencyObject> items, Func<Windows.UI.Xaml.DependencyObject, IEnumerable<Windows.UI.Xaml.DependencyObject>> function)
      {
         foreach ( var item in items )
         {
            foreach ( var itemChild in function(item) )
            {
               yield return itemChild;
            }
         }
      }

      /// <summary>
      /// Applies the given function to each of the items in the supplied
      /// IEnumerable, which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> DrillDown<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items, Func<Windows.UI.Xaml.DependencyObject, IEnumerable<Windows.UI.Xaml.DependencyObject>> function)
         where T : Windows.UI.Xaml.DependencyObject
      {
         foreach ( var item in items )
         {
            foreach ( var itemChild in function(item) )
            {
               if ( itemChild is T )
               {
                  yield return (T)itemChild;
               }
            }
         }
      }

      /// <summary>
      /// Returns a collection of descendant elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Descendants(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
      {
         return items.DrillDown(i => i.Descendants());
      }

      /// <summary>
      /// Returns a collection containing this element and all descendant elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> DescendantsAndSelf(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
      {
         return items.DrillDown(i => i.DescendantsAndSelf());
      }

      /// <summary>
      /// Returns a collection of ancestor elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Ancestors(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
      {
         return items.DrillDown(i => i.Ancestors());
      }

      /// <summary>
      /// Returns a collection containing this element and all ancestor elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> AncestorsAndSelf(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
      {
         return items.DrillDown(i => i.AncestorsAndSelf());
      }

      /// <summary>
      /// Returns a collection of child elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Elements(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
      {
         return items.DrillDown(i => i.Elements());
      }

      /// <summary>
      /// Returns a collection containing this element and all child elements.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsAndSelf(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
      {
         return items.DrillDown(i => i.ElementsAndSelf());
      }

      /// <summary>
      /// Returns a collection of descendant elements which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Descendants<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
         where T : Windows.UI.Xaml.DependencyObject
      {
         return items.DrillDown<T>(i => i.Descendants());
      }

      /// <summary>
      /// Returns a collection containing this element and all descendant elements.
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> DescendantsAndSelf<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
         where T : Windows.UI.Xaml.DependencyObject
      {
         return items.DrillDown<T>(i => i.DescendantsAndSelf());
      }

      /// <summary>
      /// Returns a collection of ancestor elements which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Ancestors<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
         where T : Windows.UI.Xaml.DependencyObject
      {
         return items.DrillDown<T>(i => i.Ancestors());
      }

      /// <summary>
      /// Returns a collection containing this element and all ancestor elements.
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> AncestorsAndSelf<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
         where T : Windows.UI.Xaml.DependencyObject
      {
         return items.DrillDown<T>(i => i.AncestorsAndSelf());
      }

      /// <summary>
      /// Returns a collection of child elements which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> Elements<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
         where T : Windows.UI.Xaml.DependencyObject
      {
         return items.DrillDown<T>(i => i.Elements());
      }

      /// <summary>
      /// Returns a collection containing this element and all child elements.
      /// which match the given type.
      /// </summary>
      public static IEnumerable<Windows.UI.Xaml.DependencyObject> ElementsAndSelf<T>(this IEnumerable<Windows.UI.Xaml.DependencyObject> items)
         where T : Windows.UI.Xaml.DependencyObject
      {
         return items.DrillDown<T>(i => i.ElementsAndSelf());
      }

   }

}