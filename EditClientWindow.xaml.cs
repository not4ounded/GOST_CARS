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
using TestDealershipApi.Models;

namespace GOST_CARS_FRONT
{
    /// <summary>
    /// Логика взаимодействия для EditClientWindow.xaml
    /// </summary>
    public partial class EditClientWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly Client _clientToEdit;
        public EditClientWindow(Client client)
        {
            InitializeComponent();
            _clientToEdit = client;

            FCsTB.Text = client.FCs;
            PhoneNumTB.Text = client.PhoneNumber;
            AddressTB.Text = client.Address;
            PurchaseDateTB.Text = client.PurchaseDate.ToString("yyyy-MM-dd");
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FCsTB.Text) || string.IsNullOrEmpty(PhoneNumTB.Text))
            {
                MessageBox.Show("ФИО и Телефон обязательны для заполнения!");
                return;
            }
            if (!DateOnly.TryParse(PurchaseDateTB.Text, out DateOnly purchaseDate))
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для полей", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var updateResponse = await _apiService.UpdateClientAsync(
                _clientToEdit.Id,
                FCsTB.Text.Trim(),
                PhoneNumTB.Text.Trim(),
                AddressTB.Text.Trim(),
                purchaseDate
            );
            if (updateResponse.IsSuccess)
            {
                MessageBox.Show($"Клиент успешно обновлен!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Ошибка сохранения: {updateResponse.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
