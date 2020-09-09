using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Diagnostics;
using Thunder.Models;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Thunder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static MainWindow _instance;
        public MainWindow()
        {
            InitializeComponent();
            IRemoveDependenciesTabSelect.Selected += MenuItem_Selected;
            IPackMissionsTabSelect.Selected += MenuItem_Selected;
            ISettingsTabSelect.Selected += MenuItem_Selected;
            
            
        }

        public static MainWindow Instance => _instance ??= new MainWindow();

        #region WindowEvents

        private void MetroWindow_Closing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        private void OpenGithub(object sender, RoutedEventArgs e)
        {
            Functions.OpenBrowser("https://github.com/501stLegionA3/501st-Missions/");
        }

        public string SelectFolder(string default_folder = "")
        {
            var dlg = new CommonOpenFileDialog
            {
                Title = "Select the folder",
                IsFolderPicker = true,
                AddToMostRecentlyUsedList = false,
                InitialDirectory = default_folder,
                DefaultDirectory = default_folder,
                AllowNonFileSystemItems = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            return dlg.ShowDialog() == CommonFileDialogResult.Ok
                ? dlg.FileName
                : null;
        }

        private void MenuItem_Selected(object sender, RoutedEventArgs e)
        {
            var menus = new List<ListBox> { IMainMenuItems, IOtherMenuItems };
            ListBoxItem lbItem = sender as ListBoxItem;
            foreach (var list in menus)
            {
                foreach (ListBoxItem item in list.Items)
                {
                    if (item.Name != lbItem?.Name)
                    {
                        item.IsSelected = false;
                    }
                }

                foreach (TabItem item in IMainContent.Items)
                {
                    if (item.Name == lbItem?.Name.Replace("Select", ""))
                    {
                        IMainContent.SelectedItem = item;
                    }
                }
            }
        }
    }
}
