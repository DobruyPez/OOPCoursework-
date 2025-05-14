using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using _4lab.DB;
using _4lab.Migrations;
using Roles;

namespace _4lab
{
    public partial class RegisterUserPage : Page
    {
        private bool _isLoginMode = true;
        private readonly LoginControl _loginControl;
        private readonly RegisterControl _registerControl;

        public RegisterUserPage()
        {
            InitializeComponent();
            _loginControl = new LoginControl();
            _registerControl = new RegisterControl();
            UpdateView();
        }

        private void UpdateView()
        {
            if (_isLoginMode)
            {
                TitleTextBlock.Text = Application.Current.Resources["LoginTitle"]?.ToString() ?? "Login";
                AuthContentControl.Content = _loginControl;
                ToggleButton.Content = Application.Current.Resources["GoToRegisterButton"]?.ToString() ?? "Go to Register";
            }
            else
            {
                TitleTextBlock.Text = Application.Current.Resources["RegisterUserTitle"]?.ToString() ?? "Register";
                AuthContentControl.Content = _registerControl;
                ToggleButton.Content = Application.Current.Resources["GoToLoginButton"]?.ToString() ?? "Go to Login";
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _isLoginMode = !_isLoginMode;
            UpdateView();
        }
    }
}