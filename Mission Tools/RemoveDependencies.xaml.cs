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
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Thunder
{
    /// <summary>
    /// Interaction logic for RemoveDependencies.xaml
    /// </summary>
    public partial class RemoveDependencies : UserControl
    {
        private string folder;
        private string pattern = @"addons\[\] = {[""a-zA-Z0-9_,]*};";
        public RemoveDependencies()
        {
            InitializeComponent();
        }

        private void ISelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.Instance.SelectFolder();
            if (string.IsNullOrEmpty(path))
            {
                ISubFolderCheckbox.IsChecked = false;
                IMissionList.Items.Clear();
                folder = "";
                return;
            }
            folder = path;
            ISubFolderCheckbox.IsChecked = false;
            IFolderDisplay.Content = path;
            IMissionList.Items.Clear();
        }

        private void IMissionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ISubFolderCheckbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == false || string.IsNullOrEmpty(folder))
            {
                IMissionList.Items.Clear();
                return;
            }
            updateMissions();
        }

        private void updateMissions()
        {
            List<string> newMissions = new List<string>();
            IMissionList.Items.Clear();
            if (Directory.Exists(folder))
            {
                foreach (string mission in Directory.GetDirectories(folder))
                {
                    newMissions.Add(Path.GetFileName(mission));
                }
                foreach (var mission in newMissions)
                {
                    IMissionList.Items.Add(new CheckBox { Content = mission, IsChecked = true });
                }
            }
        }

        private void IRemoveDependenciesButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.ToolsPath))
            {
                IErrorDialog.Title = "Error: Setting Missing";
                IErrorText.Text = "The Tools path is empty";
                IErrorDialog.IsOpen = true;
                return;
            }
            if (string.IsNullOrEmpty(folder))
            {
                IErrorDialog.Title = "Error: No mission selected";
                IErrorText.Text = "Please select a mission or folder before removing dependencies.";
                IErrorDialog.IsOpen = true;
                return;
            }
            if (ISubFolderCheckbox.IsChecked == true)
            {
                foreach (CheckBox mission in IMissionList.Items.Cast<CheckBox>().Where(i => i.IsChecked == true))
                {
                    string path = Path.Combine(folder, mission.Content.ToString(), "mission.sqm");
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = Path.Combine(Properties.Settings.Default.ToolsPath, "DeRapDos.exe");
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.Arguments = "-p " + path;
                    Process.Start(processStartInfo);
                    string missionTxt = File.ReadAllText(path + ".txt", Encoding.UTF8);
                    var options = RegexOptions.Multiline;
                    Regex regex = new Regex(pattern, options);
                    var matches = regex.Replace(missionTxt, "addons[] = {};");
                    File.Delete(path);
                    File.Create(path).Close();
                    File.WriteAllText(path, matches);
                    File.Delete(path + ".txt");
                    Debug.WriteLine(File.Exists(Path.Combine(folder,mission.Content.ToString(), "mission.sqm")));
                }
            } else
            {
                string path = Path.Combine(folder, "mission.sqm");
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = Path.Combine(Properties.Settings.Default.ToolsPath, "DeRapDos.exe");
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.Arguments = "-p " + path;
                Process.Start(processStartInfo).WaitForExit();
                string missionTxt = File.ReadAllText(path+".txt",Encoding.UTF8);
                var options = RegexOptions.Multiline;
                Regex regex = new Regex(pattern, options);
                var matches = regex.Replace(missionTxt, "addons[] = {};");
                File.Delete(path);
                File.Create(path).Close();
                File.WriteAllText(path, matches);
                File.Delete(path + ".txt");
                Debug.WriteLine(File.Exists(Path.Combine(folder, "mission.sqm")));
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
