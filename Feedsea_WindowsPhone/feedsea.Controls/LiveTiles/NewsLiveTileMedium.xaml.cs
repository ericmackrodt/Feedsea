using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.IO;
using feedsea.Common;

namespace feedsea.Controls.LiveTiles
{
    public partial class NewsLiveTileMedium : UserControl
    {
        public NewsLiveTileMedium(string title, string source, string sourceImage, byte[] image)
        {
            InitializeComponent();

            txtSource.Text = source;
            txtTitle.Text = title;

            BitmapImage bmp = new BitmapImage();
            using (MemoryStream str = new MemoryStream(image))
            {
                bmp.SetSource(str);
                str.Close();
            }

            LayoutRoot.Background = new ImageBrush() { ImageSource = bmp };

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var favicon = string.Concat(InternalSettings.FolderFavicons, "/", sourceImage);
                using (var str = new IsolatedStorageFileStream(favicon, FileMode.Open, store))
                {
                    bmp.SetSource(str);
                    imgSource.Source = bmp;
                    str.Close();
                }
            }
        }

        public string SaveJpeg()
        {
            try
            {
                this.UpdateLayout();
                this.Measure(new Size(691, 336));
                this.UpdateLayout();
                this.Arrange(new Rect(0, 0, 691, 336));

                var wb = new WriteableBitmap(691, 336);
                wb.Render(LayoutRoot, null);
                wb.Invalidate();

                // Create a filename for JPEG file in isolated storage
                // Tile images *must* be in shared/shellcontent.
                String fileName = "Tile_" + Guid.NewGuid().ToString() + ".jpg";

                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                //if (!myStore.DirectoryExists(InternalSettings.FolderShellContent))
                //{
                //    myStore.CreateDirectory(InternalSettings.FolderShellContent);
                //}

                //using (IsolatedStorageFileStream myFileStream = myStore.CreateFile(InternalSettings.FolderShellContent + fileName))
                //{
                //    // Encode WriteableBitmap object to a JPEG stream.
                //    wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 75);
                //    myFileStream.Close();
                //}

                //// Delete images from earlier execution
                //var filesTodelete = from f in myStore.GetFileNames(InternalSettings.FolderShellContent + "/Tile_*").AsQueryable()
                //                    where !f.EndsWith(fileName)
                //                    select f;
                //foreach (var file in filesTodelete)
                //{
                //    myStore.DeleteFile(InternalSettings.FolderShellContent + file);
                //}

                // Fire completion event
                return fileName;
            }
            catch (Exception ex)
            {
                // Log it
                System.Diagnostics.Debug.WriteLine("Exception in SaveJpeg: " + ex.ToString());
                return "";
            }
        }
    }
}
