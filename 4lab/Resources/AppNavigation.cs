using _4lab;
using System.Windows;        
using System.Windows.Controls; 
public static class AppNavigation
{
    public static void NavigateTo(Page page)
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.MainFrame.Navigate(page);
        }
    }
}