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
            if(!int.TryParse(YearTB.Text, out int year) || !decimal.TryParse(PriceTB.Text, out decimal price))
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для полей.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updateResponse = await _apiService.UpdateCarAsync(
                _carToEdit.Id,
                BrandTB.Text.Trim(),
                ModelTB.Text.Trim(),
                year,
                _carToEdit.Color ?? "Не указан",
                price,
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
    }
}
