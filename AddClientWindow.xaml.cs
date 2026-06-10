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
using System.Text.RegularExpressions;


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
            bool hasError = false;

            string phoneInput = PhoneNumTB.Text.Trim();
            string phonePattern = @"^\+?\d{10,15}$";

            if (string.IsNullOrEmpty(FCsTB.Text))
            {
                FCsTB.BorderBrush = Brushes.Red;
                hasError = true;
            }
            if (string.IsNullOrEmpty(PhoneNumTB.Text))
            {
                PhoneNumTB.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (!Regex.IsMatch(phoneInput, phonePattern))
            {
                PhoneNumTB.BorderBrush = Brushes.Red;
                MessageBox.Show("Пожалуйста, введите корректный номер телефона (10-15 цифр, может начинаться с '+').", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (hasError)
            {
                MessageBox.Show("ФИО и Телефон обязательны для заполнения!");
                return;
            }
            if (!DateOnly.TryParse(PurchaseDateTB.Text, out DateOnly purchaseDate))
            {
                PurchaseDateTB.BorderBrush = Brushes.Red;
                MessageBox.Show("Пожалуйста, введите корректные значения для полей", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.ClearValue(Border.BorderBrushProperty);
            }
        }
    }
}
