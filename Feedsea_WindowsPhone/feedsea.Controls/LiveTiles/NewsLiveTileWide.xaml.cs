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
using System.IO;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using feedsea.Common;

namespace feedsea.Controls.LiveTiles
{
    public enum TileSize
    {
        Wide,
        Medium
    }

    public partial class NewsLiveTileWide : UserControl
    {
        public NewsLiveTileWide(string title, string source, string sourceImage, byte[] image)
        {
            InitializeComponent();

            txtSource.Text = source;
            txtTitle.Text = title;

            
            if (image != null)
            {
                BitmapImage bmp = new BitmapImage();
                using (MemoryStream str = new MemoryStream(image))
                {
                    bmp.SetSource(str);
                    str.Close();
                }
                LayoutRoot.Background = new ImageBrush() { ImageSource = bmp, Stretch = Stretch.UniformToFill };
            }

            if (!string.IsNullOrWhiteSpace(sourceImage))
            {
                BitmapImage bmp = new BitmapImage();
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var str = store.OpenFile(sourceImage, FileMode.Open, FileAccess.Read))
                    {
                        bmp.SetSource(str);
                        imgSource.Source = bmp;
                        str.Close();
                    }
                }
            }
        }

        public void SaveJpeg(TileSize tileSize, string fileName)
        {
            var width = tileSize == TileSize.Wide ? 691 : 336;
            try
            {
                this.UpdateLayout();
                this.Measure(new Size(width, 336));
                this.UpdateLayout();
                this.Arrange(new Rect(0, 0, width, 336));

                var wb = new WriteableBitmap(width, 336);
                wb.Render(LayoutRoot, null);
                wb.Invalidate();

                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                //if (!myStore.DirectoryExists(InternalSettings.FolderShellContent))
                //{
                //    myStore.CreateDirectory(InternalSettings.FolderShellContent);
                //}

                // Delete images from earlier execution
                var filesTodelete = from f in myStore.GetFileNames(fileName).AsQueryable()
                                    //where !f.EndsWith(fileName)
                                    select f;

                foreach (var file in filesTodelete)
                {
                    //myStore.DeleteFile(InternalSettings.FolderShellContent + file);
                }

                using (IsolatedStorageFileStream myFileStream = myStore.CreateFile(fileName))
                {
                    // Encode WriteableBitmap object to a JPEG stream.
                    wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 75);
                    myFileStream.Close();
                }

                // Fire completion event
            }
            catch (Exception ex)
            {
                // Log it
                System.Diagnostics.Debug.WriteLine("Exception in SaveJpeg: " + ex.ToString());
            }
        }
    }
}
