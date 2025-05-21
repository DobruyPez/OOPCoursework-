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
                // Инициализация базы данных и проверка администратора
                DB.DataBaseInteractor.CreateDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
        public App()
        {
            _4lab.DB.DataBaseInteractor.CreateDatabase();
        }
    }
}
