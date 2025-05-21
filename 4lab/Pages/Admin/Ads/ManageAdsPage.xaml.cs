using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using _4lab.BD;
using _4lab.Resources;
using System.Linq;
using _4lab.Windows;

namespace _4lab.Pages.Admin.Ads
{
    public partial class ManageAdsPage : Page
    {
        private readonly string _adsImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Ads");

        public ManageAdsPage()
        {
            InitializeComponent();
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

                // Копируем изображение в папку Images/Ads
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagePath)}";
                string destinationPath = Path.Combine(_adsImagePath, fileName);
                File.Copy(imagePath, destinationPath, true);

                // Сохраняем только имя файла в БД
                using (var context = new DBContext())
                {
                    // Удаляем старую рекламу, если она есть
                    var oldAds = context.Advertisements.ToList();
                    context.Advertisements.RemoveRange(oldAds);

                    var newAd = new Advertisement
                    {
                        Image = fileName, // Сохраняем только имя файла
                        Link = link,
                    };

                    context.Advertisements.Add(newAd);
                    context.SaveChanges();

                    MessageBox.Show("Реклама успешно сохранена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ImageUrlTextBox.Text = string.Empty;
                    LinkTextBox.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении рекламы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}