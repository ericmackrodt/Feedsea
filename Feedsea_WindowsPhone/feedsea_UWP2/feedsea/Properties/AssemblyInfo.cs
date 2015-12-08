using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("feedsea")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Eric Mackrodt")]
[assembly: AssemblyProduct("feedsea")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("117e301c-8506-496e-a940-7161f352485b")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.1")]
[assembly: AssemblyFileVersion("1.0.1")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]
//Known Issues
/*
* Articles with base64 images crash the app
*/
//Changelog
//NEXT STEPS
/*
* Edit subscriptions
* Secondary Live Tiles
* Welcome Screen
* Lock screen background
*/
//TODO LIST
/* This is the app's todo list, I think it's gonna work just fine!
* - Pin source to start screen.
* - Add button to go to top on the article list.
* - Add mobilizer button in the article page.
* - Double tap set as read?
* - Maybe allow all sources selection in source list.
* - Animate article list to fade when load.
* - Create First run Tutorial.
* 
*/
//TESTING
/*
* - Investigate unread bug that marks the button read even if it's not read in the article.
*/
//DONE
/*
* - Fix Gif bug on the article list
* - Don't use justified text in the article page.
* - Show unread article number in the source list.
* - Change background agent to use the api.
* - Allow clear selected source.
* - Fix Pagination bug where the comparation takes in consideration ALL ARTICLES in the database.
* - Make unread number disappear if there's none.
* - Create no picture layout for article list
* - Create simple article item for article list
* - Create no picture simple article item for article list
* - Create settings page.
* - Allow changing article layout in settings.
* - Allow changing days to keep article with maximum 7 days.
* - Change add source textbox icon
* - Add button to open in IE in the article page.
* - Finish CSS customization of mobilizer in Article page.
* - Make youtube iframe changes with HTMLAgilityPack instead jquery in the article page and make it smaller.
* - Treat WebExceptions.
* - Verify if there is internet connection before trying to make any requests.
* - Add Mark all as read in the source and the main page.
* - Add swipe context in the article page (if it's a source selected, or favorite, or queue or all articles)
* - Add button to keep article unread in the article page.
* - Add share article in the article page.
* - Add button to add to favorites in the article page.
* - Clear 'Add Source' page when opening it.
* * - Make the Favorites list and allow adding favorites with favorite count.
*  * - add Source cateogories
*  * Fix Connection verify Exception
*  * Finish about popup
*  * - add no sources message on the first page.
* - add no articles with sources on the first page
* - Add 'About' popup.
* * In the AddSource page, I have to put the field validations, connection test, command enabling/disabling depending on the situation, treat exceptions.
* * about popup rate the app
* * Difficult to click on the article action buttons on the main feed.
* * - Add setting to show older articles first
* - Add setting to allow user to choose whether the article should be set to read when opening it
*/
//IDEAS FOR FUTURE VERSIONS
/*
* -Allow changing themes
*/