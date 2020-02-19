using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfTreeViewsAndValueConverters
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder, or file
    /// </summary>
    /// 
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the full path
            string path = (string)value;

            // IF the path is null, ignore
            if(path == null)
            {
                return null;
            }

            // Get the name of the file/folder
            string name = MainWindow.GetFileFolderName(path);

            // Default image
            string image = "Images/file.png";

            // If the image is blank, it is a drive, as there cannot be a bland file, or folder name
            if(string.IsNullOrEmpty(name))
            {
                image = "Images/drive.png";
            }
            else if(new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                image = "Images/folder-closed.png";
            }

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
