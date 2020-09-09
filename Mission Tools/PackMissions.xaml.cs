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
using System.IO;
using System.Linq;
using System.Diagnostics;
using Thunder.Models;

namespace Thunder
{
    /// <summary>
    /// Interaction logic for PackMissions.xaml
    /// </summary>
    public partial class PackMissions : UserControl
    {
        string path;
        public PackMissions()
        {
            InitializeComponent();
        }

        private void ISelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            path = MainWindow.Instance.SelectFolder();
            if (string.IsNullOrEmpty(path)) return;
            //Do mission finding logic
            updateMissions();
        }

        private void updateMissions()
        {
            List<string> newMissions = new List<string>();
            IMissionList.Items.Clear();
            if (Directory.Exists(path))
            {
                foreach (string mission in Directory.GetDirectories(path))
                {
                    if(Path.GetFileName(mission) == "temp")
                    {
                        break;
                    }
                    newMissions.Add(Path.GetFileName(mission));
                }
                foreach (var mission in newMissions)
                {
                    IMissionList.Items.Add(new CheckBox { Content = mission, IsChecked = true });
                }
            }
        }

        private void IPackMissionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.ToolsPath) || string.IsNullOrEmpty(Properties.Settings.Default.ScriptsPath))
            {
                IErrorDialog.Title = "Error: Setting Missing";
                IErrorText.Text = "The Tools path is empty";
                IErrorDialog.IsOpen = true;
                return;
            }
            var missions = IMissionList.Items.Cast<CheckBox>().Where(i => i.IsChecked == true);
            string tmpDir = Path.Combine(path, "temp");
            foreach(var mission in missions)
            {
                string tmpPath = Path.Combine(path, mission.Content.ToString());
                Functions.DirectoryCopy(tmpPath, Path.Combine(tmpDir, mission.Content.ToString()));
                tmpPath = Path.Combine(tmpDir, mission.Content.ToString());
                Functions.DirectoryCopy(Properties.Settings.Default.ScriptsPath, tmpPath, true);
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = Path.Combine(Properties.Settings.Default.ToolsPath, "makepbo.exe");
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.Arguments = "-p " + tmpPath;
                Process.Start(processStartInfo).WaitForExit();
                Directory.Delete(tmpPath, true);
                Debug.WriteLine(tmpPath);
            }
        }

        private void IErrorOKButton_Click(object sender, RoutedEventArgs e)
        {
            IErrorDialog.IsOpen = false;
        }

        private void IMissionsRefresh_Click(object sender, RoutedEventArgs e)
        {
            updateMissions();
        }
    }
}
