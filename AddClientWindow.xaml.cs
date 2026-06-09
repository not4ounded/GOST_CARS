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
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();

        public AddClientWindow()
        {
            InitializeComponent();
            PurchaseDateTB.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private async void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FCsTB.Text) || string.IsNullOrEmpty(PhoneNumTB.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (ФИО, телефон)", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DateOnly.TryParse(PurchaseDateTB.Text, out DateOnly purchaseDate))
            {
                MessageBox.Show("Пожалуйста, введите корректную дату покупки (формат: ГГГГ-ММ-ДД)", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CreateBtn.IsEnabled = false;

            try
            {
                var response = await _apiService.AddClientAsync(
                    FCsTB.Text.Trim(),
                    PhoneNumTB.Text.Trim(),
                    AddressTB.Text.Trim(),
                    purchaseDate
                );

                if (response.IsSuccess)
                {
                    MessageBox.Show("Клиент успешно добавлен в базу!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Ошибка при добавлении клиента: {response.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    CreateBtn.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при попытке связаться с сервером. Пожалуйста, попробуйте позже.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                CreateBtn.IsEnabled = true;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
