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
        public App()
        {
            try
            {
                var assembly = System.Reflection.Assembly.Load("Npgsql.EntityFramework, Version=2.2.7.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7");
                System.Diagnostics.Debug.WriteLine("Npgsql.EntityFramework загружен успешно.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
            }

            _4lab.DB.DataBaseInteractor.CreateDatabase();
        }
    }
}
