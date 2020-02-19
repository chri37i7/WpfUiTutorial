using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WpfTreeViewsAndValueConverters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Constructor

        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        /// 
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region On Loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get every drive on the machine
            foreach(string drive in Directory.GetLogicalDrives())
            {
                // Create object
                TreeViewItem item = new TreeViewItem()
                {
                    // Set header and path
                    Header = drive,
                    Tag = drive
                };

                // Add dummy item
                item.Items.Add(null);

                // Listen out for item being expanded
                item.Expanded += Folder_Expanded;

                // Add item to FolderView
                FolderView.Items.Add(item);
            }
        }

        #endregion

        #region Folder Expanded

        /// <summary>
        /// When a folder is expanded, find the subfolders/files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks

            TreeViewItem item = (TreeViewItem)sender;

            // If the item only contains the dummy item
            if(item.Items.Count != 1 || item.Items[0] != null)
            {
                return;
            }

            // Clear dummy data
            item.Items.Clear();

            // Get folder name
            string fullPath = (string)item.Tag;

            #endregion

            #region Get Folders

            // List for storing directories
            List<string> directories = new List<string>();

            // Try and get directories from the folder
            // Ignore any exception(Bad practice)
            try
            {
                string[] dirs = Directory.GetDirectories(fullPath);

                if(dirs.Length > 0)
                {
                    directories.AddRange(dirs);
                }
            }
            catch { }

            // For each directory
            directories.ForEach(directoryPath =>
            {
                // Create directory object
                TreeViewItem subItem = new TreeViewItem()
                {
                    // Set header as foldername
                    Header = GetFileFolderName(directoryPath),
                    // Set tag as full path
                    Tag = directoryPath
                };

                // Add dummy item so we can expand folder
                subItem.Items.Add(null);

                // Handle expanding
                subItem.Expanded += Folder_Expanded;

                // Add this to the parent
                item.Items.Add(subItem);
            });

            #endregion

            #region Get Files

            // List for storing files
            List<string> files = new List<string>();

            // Try and get files from the folder
            // Ignore any exceptions(Bad practice)
            try
            {
                string[] file = Directory.GetFiles(fullPath);

                if(file.Length > 0)
                {
                    files.AddRange(file);
                }
            }
            catch { }

            // For each file...
            files.ForEach(filePath =>
            {
                // Create file item
                TreeViewItem subItem = new TreeViewItem()
                {
                    // Set header as file name
                    Header = GetFileFolderName(filePath),
                    // Set tag as full path
                    Tag = filePath
                };

                // Add item to the parent
                item.Items.Add(subItem);
            });

            #endregion
        }

        #endregion

        #region Helpers

        public static string GetFileFolderName(string path)
        {
            // C:\Something\a folder
            // C:\Something\a file.png

            // If we have no path, return empty
            if(string.IsNullOrEmpty(path))
            { 
                return string.Empty; 
            }

            // Make all slashes back slashes
            string normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            int lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if(lastIndex <= 0)
            {
                return path;
            }

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }

        #endregion
    }
}