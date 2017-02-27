using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WpfTestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FileInfo> DLLsList = new List<FileInfo>();
        List<Type> TypesList = new List<Type>();
        List<FieldInfo> FieldsList = new List<FieldInfo>();
        List<MethodInfo> MethodsList = new List<MethodInfo>();
        List<PropertyInfo> PropertiesList = new List<PropertyInfo>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtSelectDirectory_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Select Directory button is clicked!");
            LBFiles.Items.Clear();
            LBTypes.Items.Clear();
            LBComponents.Items.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (TBDirectoryPath.Text.Equals(""))
                dialog.SelectedPath = ConfigurationManager.AppSettings["DirectoryPath"];
            else
                dialog.SelectedPath = TBDirectoryPath.Text;
            TBDirectoryPath.Text = "";
            TBDirectoryPath.Text = dialog.SelectedPath;
            while (dialog.SelectedPath == "")
            {
                dialog.ShowDialog();
                TBDirectoryPath.Text = dialog.SelectedPath;
            }
            DirectoryInfo di = new DirectoryInfo(dialog.SelectedPath);
            DLLsList = di.GetFiles().ToList().Where(x => x.Extension.ToLower() == ".dll").ToList();
            try
            {
                foreach (FileInfo file in DLLsList)
                    LBFiles.Items.Add(file.Name);
            }
            catch (NullReferenceException ex) { }
        }

        private void LBFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TBDirectoryPath.Text != "" && LBFiles.Items.Count != 0)
            {
                LBTypes.Items.Clear();
                LBComponents.Items.Clear();
                var item = LBFiles.SelectedItem.ToString();
                var DLL = DLLsList.Where(x => x.Name == item).ToList()[0];
                try
                {
                    Assembly a = Assembly.LoadFile(DLL.FullName);
                    TypesList = a.GetTypes().ToList();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    TypesList = ex.Types.ToList();
                }
                catch (BadImageFormatException ex) {}
                try
                {
                    foreach (var type in TypesList)
                        LBTypes.Items.Add(type.Name);
                }
                catch (NullReferenceException ex) {}
            }
        }

        private void LBTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBTypes.Items.Count != 0)
            {
                LBComponents.Items.Clear();
                CBSelectedComponents.SelectedIndex = 0;
                var item = LBTypes.SelectedItem.ToString();
                var type = TypesList.Where(x => x.Name == item).ToList()[0];
                FieldsList = type.GetFields().ToList();
                MethodsList = type.GetMethods().ToList();
                PropertiesList = type.GetProperties().ToList();
                try
                {
                    AddAll();
                }
                catch (NullReferenceException ex) { }
            }
        }

        private void AddMethods()
        {
            foreach (var method in MethodsList)
                LBComponents.Items.Add(method.Name);
        }

        private void AddProperties()
        {
            foreach (var property in PropertiesList)
                LBComponents.Items.Add(property.Name);
        }

        private void AddFields()
        {
            foreach (var field in FieldsList)
                LBComponents.Items.Add(field.Name);
        }

        private void AddAll()
        {
            AddMethods();
            AddProperties();
            AddFields();            
        }

        private void CBSelectedComponents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBTypes.Items.Count != 0 && LBComponents.Items.Count != 0)
            {
                LBComponents.Items.Clear();
                try
                {
                    switch (CBSelectedComponents.SelectedIndex)
                    {
                        case 0:
                            AddAll();
                            break;
                        case 1:
                            AddMethods();
                            break;
                        case 2:
                            AddProperties();
                            break;
                        case 3:
                            AddFields();
                            break;
                    }
                }
                catch (NullReferenceException ex) { }
            }
        }
    }
}