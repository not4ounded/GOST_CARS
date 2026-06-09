using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestDealershipApi.Api;
using TestDealershipApi.Models;

namespace GOST_CARS_FRONT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDataFromApi();
        }

        private async Task LoadDataFromApi()
        {
            try
            {
                var carsResponse = await _apiService.GetAllCarsAsync();
                if (carsResponse.IsSuccess && carsResponse.Data != null)
                {
                    CarsListView.ItemsSource = carsResponse.Data;
                }
                else
                {
                    MessageBox.Show($"Ошибка загрузки машин: {carsResponse.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                var clientsResponse = await _apiService.GetAllClientsAsync();
                if (clientsResponse.IsSuccess && clientsResponse.Data != null)
                {
                    ClientsListView.ItemsSource = clientsResponse.Data;
                }
                else
                {
                    MessageBox.Show($"Ошибка загрузки клиентов: {clientsResponse.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке данных: {ex.Message}");
            }
        }

        private async void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.DataContext is Car car)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить машину {car.Brand} {car.Model}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var deleteResponse = await _apiService.DeleteCarAsync(car.Id);
                    if (deleteResponse.IsSuccess)
                    {
                        MessageBox.Show("Машина успешно удалена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadDataFromApi();
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка при удалении машины: {deleteResponse.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.DataContext is Client client)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {client.FCs}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var deleteResponse = await _apiService.DeleteClientAsync(client.Id);
                    if (deleteResponse.IsSuccess)
                    {
                        MessageBox.Show("Клиент успешно удалён!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadDataFromApi();
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка при удалении клиента: {deleteResponse.ErrorMessage}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void ClientsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ClientsListView.SelectedItem is Client selectedClient)
            {
                EditClientWindow editClientWindow = new EditClientWindow(selectedClient);
                editClientWindow.Owner = this;

                if (editClientWindow.ShowDialog() == true)
                {
                    await LoadDataFromApi();
                }
            }
        }

        private async void CarsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CarsListView.SelectedItem is Car selectedCar)
            {
                EditCarWindow editCarWindow = new EditCarWindow(selectedCar);
                editCarWindow.Owner = this;

                if (editCarWindow.ShowDialog() == true)
                {
                    await LoadDataFromApi();
                }
            }
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?",
                                      "Выход",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Question
                                      );
            if (res == MessageBoxResult.Yes)
            {
                AuthWindow authWindow = new AuthWindow();
                authWindow.Show();
                this.Close();
            }
        }

        private async void addClientBtn_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.Owner = this;

            if (addClientWindow.ShowDialog() == true)
            {
                await LoadDataFromApi();
            }
        }
    }
}