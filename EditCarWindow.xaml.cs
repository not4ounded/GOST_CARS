using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestDealershipApi.Api;
using TestDealershipApi.Models;
using System.Text.RegularExpressions;
using System.Globalization;


namespace GOST_CARS_FRONT
{
    /// <summary>
    /// Логика взаимодействия для EditCarWindow.xaml
    /// </summary>
    public partial class EditCarWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly Car _carToEdit;
        public EditCarWindow(Car car)
        {
            InitializeComponent();
            _carToEdit = car;

            BrandTB.Text = car.Brand;
            ModelTB.Text = car.Model;
            YearTB.Text = car.ReleaseYear.ToString();
            PriceTB.Text = car.Price.ToString();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            int parsedYear = 0;
            decimal parsedPrice = 0;

            string yearInput = YearTB.Text.Trim();
            string yearPattern = @"^\d{4}$";

            string priceInput = PriceTB.Text.Trim().Replace(',', '.');
            string pricePattern = @"^\d+(\.\d{1,2})?$";

            if (string.IsNullOrEmpty(yearInput))
            {
                YearTB.BorderBrush = Brushes.Red;
                hasError = true;
            } 
            else if (!Regex.IsMatch(yearInput, yearPattern) || 
                     !int.TryParse(yearInput, out parsedYear) ||
                     parsedYear < 1900 || parsedYear > 2026)
            {
                YearTB.BorderBrush = Brushes.Red;
                MessageBox.Show("Пожалуйста, введите корректный год выпуска (4 цифры от 1900 до 2026).", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(priceInput))
            {
                PriceTB.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (!Regex.IsMatch(priceInput, pricePattern) ||
                     !decimal.TryParse(priceInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsedPrice) ||
                     parsedPrice <= 0)
            {
                PriceTB.BorderBrush = Brushes.Red;
                MessageBox.Show("Пожалуйста, введите корректную цену (положительное число, допускается до 2 знаков после запятой).", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updateResponse = await _apiService.UpdateCarAsync(
                _carToEdit.Id,
                BrandTB.Text.Trim(),
                ModelTB.Text.Trim(),
                parsedYear,
                _carToEdit.Color ?? "Не указан",
                parsedPrice,
                _carToEdit.Condition ?? "Б/У"
            );

            if (updateResponse.IsSuccess)
            {
                MessageBox.Show($"Авто успешно обновлено!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Ошибка созранения: {updateResponse.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                textBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }
    }
}
