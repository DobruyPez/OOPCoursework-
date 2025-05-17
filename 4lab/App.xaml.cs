using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace _4lab
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                //Console.WriteLine($"MergedDictionaries count: {Resources.MergedDictionaries.Count}");
                //for (int i = 0; i < Resources.MergedDictionaries.Count; i++)
                //{
                //    Console.WriteLine($"Dictionary {i} Source: {Resources.MergedDictionaries[i].Source}");
                //    if (Resources.MergedDictionaries[i].Contains("HeaderTextBlockStyle"))
                //    {
                //        Console.WriteLine($"HeaderTextBlockStyle found in Dictionary {i}");
                //    }
                //}

                //var styleDict = Resources.MergedDictionaries[1]; // GeneralStyle.xaml
                //if (styleDict.Contains("HeaderTextBlockStyle"))
                //{
                //    Console.WriteLine("HeaderTextBlockStyle found in GeneralStyle.xaml");
                //}
                //else
                //{
                //    Console.WriteLine("HeaderTextBlockStyle NOT found in GeneralStyle.xaml");
                //    throw new Exception("HeaderTextBlockStyle is missing in GeneralStyle.xaml");
                //}

                //// Проверка через Application.Current.Resources
                //var style = Application.Current.Resources["HeaderTextBlockStyle"];
                //if (style != null)
                //{
                //    Console.WriteLine("HeaderTextBlockStyle found in Application.Current.Resources");
                //}
                //else
                //{
                //    Console.WriteLine("HeaderTextBlockStyle NOT found in Application.Current.Resources");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading resources: {ex.Message}", "Resource Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public App()
        {
            _4lab.DB.DataBaseInteractor.CreateDatabase();
        }
    }
}
