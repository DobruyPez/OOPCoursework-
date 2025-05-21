using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using _4lab.BD;
using _4lab.Resources;

namespace _4lab.Pages.Admin.Ads
{
    public partial class ManageAdsPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly string _adsImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "ads");

        public ManageAdsPage(MainWindow mainWindow = null)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            // Создаем папку для изображений, если она не существует
            Directory.CreateDirectory(_adsImagePath);
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif",
                Title = "Выберите изображение для рекламы"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImageUrlTextBox.Text = openFileDialog.FileName;
            }
        }

        private void ImageUrlTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && IsImageFile(files[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void ImageUrlTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && IsImageFile(files[0]))
                {
                    ImageUrlTextBox.Text = files[0];
                }
            }
        }

        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }

        private void SaveAdButton_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = ImageUrlTextBox.Text.Trim();
            string link = LinkTextBox.Text.Trim();

            if (string.IsNullOrEmpty(imagePath) || string.IsNullOrEmpty(link))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Проверяем валидность URL для ссылки
                if (!Uri.IsWellFormedUriString(link, UriKind.Absolute))
                {
                    MessageBox.Show("Пожалуйста, введите корректный URL для ссылки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверяем существование файла изображения
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show("Выбранный файл изображения не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Копируем изображение в папку wwwroot/images/ads
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagePath)}";
                string destinationPath = Path.Combine(_adsImagePath, fileName);
                File.Copy(imagePath, destinationPath, true);

                // Формируем относительный путь для сохранения в БД
                string relativePath = $"/images/ads/{fileName}";

                using (var context = new DBContext())
                {
                    var newAd = new Advertisement
                    {
                        Image = relativePath,
                        Link = link,
                    };

                    context.Advertisements.Add(newAd);
                    context.SaveChanges();

                    MessageBox.Show("Реклама успешно сохранена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ImageUrlTextBox.Text = string.Empty;
                    LinkTextBox.Text = string.Empty;

                    // Обновляем рекламу в MainWindow
                    _mainWindow?.RefreshAdvertisement();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении рекламы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}