using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thunder
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            Initialized += Settings_Initialized;
            InitializeComponent();
        }

        private void Settings_Initialized(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            IFolderScripts.Text = settings.ScriptsPath ?? string.Empty;
            IFolderTools.Text = settings.ToolsPath ?? string.Empty;
        }

        private void ISaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.ToolsPath = IFolderTools.Text;
            settings.ScriptsPath = IFolderScripts.Text;
            settings.Save();
        }

        private void ISelectFolderScriptsButton_Click(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.Instance.SelectFolder();
            if (string.IsNullOrEmpty(path)) return;
            IFolderScripts.Text = path;
        }

        private void ISelectFolderToolsButton_Click(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.Instance.SelectFolder();
            if (string.IsNullOrEmpty(path)) return;
            IFolderTools.Text = path;
        }
    }
}
