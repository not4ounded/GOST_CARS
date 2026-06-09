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
using System.Windows.Shapes;
using TestDealershipApi.Api;

namespace GOST_CARS_FRONT
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();
        public AuthWindow()
        {
            InitializeComponent();

            loginBtn.Click += LoginButton_Click;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTB.Text.Trim();
            string password = passBox.Password;
            
            if (string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            loginBtn.IsEnabled = false;

            try
            {
                var loginResponse = await _apiService.LoginAsync(login, password);

                if (loginResponse.IsSuccess && loginResponse.Data != null)
                {
                    MessageBox.Show($"Успешный вход, {loginResponse.Data.Login}!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show(loginResponse.ErrorMessage ?? "Ошибка авторизации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    loginBtn.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось связаться с сервером: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                loginBtn.IsEnabled = true;
            }
            finally
            {
                if (this.IsEnabled)
                {
                    loginBtn.IsEnabled = true;
                }
            }
        }


    }
}
