using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpAirportClient.Models;
using UwpAirportClient.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpAirportClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Departures : Page
    {
        public ObservableCollection<DepartureDTO> departuresList = new ObservableCollection<DepartureDTO>();

        public double height => Window.Current.Bounds.Height - 50;
        public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

        private GenericService<DepartureDTO> service = new GenericService<DepartureDTO>(new System.Net.Http.HttpClient(), Url.Value + "Departures");
        private GenericService<CrewDTO> crewService = new GenericService<CrewDTO>(new System.Net.Http.HttpClient(), Url.Value + "Crews");
        private ObservableCollection<CrewDTO> crewsList = new ObservableCollection<CrewDTO>();
        private GenericService<PlaneDTO> planeService = new GenericService<PlaneDTO>(new System.Net.Http.HttpClient(), Url.Value + "Planes");
        private ObservableCollection<PlaneDTO> planesList = new ObservableCollection<PlaneDTO>();

        public Departures()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Departures_Load;
        }

        private void Departures_Load(object sender, RoutedEventArgs e)
        {
            UpdateList();
            GetCrews();
        }

        private async void GetCrews()
        {
            crewsList.Clear();
            try
            {
                (await crewService.getAllAsync()).ForEach(o => crewsList.Add(o));
            }
            catch (Exception) { };
        }

        private async void GetPlanes()
        {
            planesList.Clear();
            try
            {
                (await planeService.getAllAsync()).ForEach(o => planesList.Add(o));
            }
            catch (Exception) { };
        }


        private async void UpdateList()
        {
            departuresList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => departuresList.Add(o));
            }
            catch (Exception) { };
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private (Button, TextBox, TimePicker, ComboBox, ComboBox) RenderCreate()
        {
            SingleItem.Children.Clear();

            var Number = new TextBox
            {
                Header = "Flight number",
                Width = 200,
                PlaceholderText = "Number",
                Margin = new Thickness(0, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var time = new TimePicker
            {
                Header = "Departure time",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var planes = new ComboBox
            {
                Header = "Planes",
                PlaceholderText = "plane",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                ItemsSource = planesList
            };

            var crews = new ComboBox
            {
                Header = "Crews",
                PlaceholderText = "crew",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                ItemsSource = crewsList
            };

            var btnCreate = new Button
            {
                Content = "Add item",
                Width = 200,
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            SingleItem.Children.Add(Number);
            SingleItem.Children.Add(time);
            SingleItem.Children.Add(planes);
            SingleItem.Children.Add(crews);
            SingleItem.Children.Add(btnCreate);
            return (btnCreate, Number, time, planes, crews);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            var btnCreate = turple.Item1;
            var number = turple.Item2;
            var time = turple.Item3;
            var planes = turple.Item4;
            var crews = turple.Item5;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var plane = new DepartureDTO()
                { FlightNumber = number.Text, DepartureTime = (new DateTime(2018,2,2)+time.Time).ToString(),
                    PlaneId = ((PlaneDTO)planes.SelectedItem).Id, CrewId = ((CrewDTO)crews.SelectedItem).Id };
                try
                {
                    await service.CreateAsync(plane);
                }
                catch (Exception) { }

                departuresList.Add(plane);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button, Button, TextBox, TimePicker, ComboBox, ComboBox, DepartureDTO) RenderDetail(ItemClickEventArgs e)
        {
            var departure = (DepartureDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Departure id:" + departure.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;

            var name = new TextBox
            {
                Header = "Name",
                Text = departure.FlightNumber,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            var timespan = new TimePicker
            {
                Header = "Departure time",
                Time = DateTime.Parse(departure.DepartureTime).TimeOfDay,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var planes = new ComboBox
            {
                Header = "Planes",
                PlaceholderText = "plane",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = planesList
            };

            var crews = new ComboBox
            {
                Header = "Crews",
                PlaceholderText = "crew",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = crewsList
            };

            var btnUpdate = new Button
            {
                Content = "Update",
                Width = 100
            };
            var btnDelete = new Button
            {
                Content = "Delete",
                Width = 100
            };

            var buttonsStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };
            SingleItem.Children.Add(id);
            SingleItem.Children.Add(name);
            SingleItem.Children.Add(timespan);
            SingleItem.Children.Add(planes);
            SingleItem.Children.Add(crews);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, name, timespan, planes, crews, departure);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            var btnUpdate = turple.Item1;
            var btnDelete = turple.Item2;
            var number = turple.Item3;
            var time = turple.Item4;
            var planes = turple.Item5;
            var crews = turple.Item6;
            var departure = turple.Item7;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var stewardessCreating = new DepartureDTO()
                {
                    Id = departure.Id,
                    FlightNumber = number.Text,
                    DepartureTime = (new DateTime(2018, 2, 2) + time.Time).ToString(),
                    PlaneId = ((PlaneDTO)planes.SelectedItem).Id,
                    CrewId = ((CrewDTO)crews.SelectedItem).Id
                };
                int index = departuresList.ToList().FindIndex(t => t.Id == departure.Id);
                departuresList.Insert(index, stewardessCreating);

                try
                {
                    await service.UpdateAsync(stewardessCreating);
                }
                catch (Exception) { }

                UpdateList();
                SingleItem.Children.Clear();
            };

            btnDelete.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                try
                {
                    await service.DeleteAsync(departure);
                }
                catch (Exception) { }
                departuresList.Remove(departure);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }
    }
}
