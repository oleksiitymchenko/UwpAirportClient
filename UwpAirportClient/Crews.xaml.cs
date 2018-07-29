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
    public sealed partial class Crews : Page
    {
        public ObservableCollection<CrewDTO> crewsList = new ObservableCollection<CrewDTO>();

        public double height => Window.Current.Bounds.Height - 50;
        public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

        private GenericService<CrewDTO> service = new GenericService<CrewDTO>(new System.Net.Http.HttpClient(), Url.Value + "Flights");

        private GenericService<PilotDTO> pilotService = new GenericService<PilotDTO>(new System.Net.Http.HttpClient(), Url.Value + "Pilots");
        private ObservableCollection<PilotDTO> pilotsList = new ObservableCollection<PilotDTO>();


        private GenericService<StewardessDTO> stewService = new GenericService<StewardessDTO>(new System.Net.Http.HttpClient(), Url.Value + "Stewardesses");
        private ObservableCollection<StewardessDTO> stewsList = new ObservableCollection<StewardessDTO>();
        private List<int> selectedStews = new List<int>();

        public Crews()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Flights_Load;
        }

        private void Flights_Load(object sender, RoutedEventArgs e)
        {
            UpdateList();
            GetStews();
        }

        private async void GetPilots()
        {
            pilotsList.Clear();
            try
            {
                (await pilotService.getAllAsync()).ForEach(o => pilotsList.Add(o));
            }
            catch (Exception) { };
        }

        private async void GetStews()
        {
            stewsList.Clear();
            try
            {
                (await stewService.getAllAsync()).ForEach(o => stewsList.Add(o));
            }
            catch (Exception) { };
        }

        private async void UpdateList()
        {
            crewsList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => crewsList.Add(o));
            }
            catch (Exception) { };
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private (Button, ComboBox) RenderCreate()
        {
            SingleItem.Children.Clear();

            var pilots = new ComboBox
            {
                Header = "Pilots",
                PlaceholderText = "pilot",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = pilotsList
            };


            var stews = new ComboBox
            {
                Header = "Stewardesses",
                PlaceholderText = "stews",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = stewsList
            };
            ScrollViewer.SetVerticalScrollMode(stews, ScrollMode.Enabled);
            selectedStews.Clear();

            var addticketButton = new Button()
            {
                Content = "Add stewardess",
            };
            addticketButton.Click += (object sendet, RoutedEventArgs e) =>
            {
                selectedStews.Add(((StewardessDTO)stews.SelectedItem).Id);
                stewsList.Remove((StewardessDTO)stews.SelectedItem);
            };

            var btnCreate = new Button
            {
                Content = "Add item",
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            SingleItem.Children.Add(pilots);
            SingleItem.Children.Add(stews);

            SingleItem.Children.Add(addticketButton);
            SingleItem.Children.Add(btnCreate);

            return (btnCreate, pilots);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            Button btnCreate = turple.Item1;

            ComboBox Pilot = turple.Item2;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var crew = new CrewDTO()
                {
                    PilotId = ((PilotDTO)Pilot.SelectedItem).Id,
                    StewardressIds = selectedStews
                };
                try
                {
                    await service.CreateAsync(crew);
                }
                catch (Exception) { }

                crewsList.Add(crew);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button, Button, ComboBox, CrewDTO) RenderDetail(ItemClickEventArgs e)
        {
            var crew = (CrewDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Crew id:" + crew.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;
            var pilots = new ComboBox
            {
                Header = "Pilots",
                PlaceholderText = "pilot",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = pilotsList
            };

            var stews = new ComboBox
            {
                Header = "Stewardesses",
                PlaceholderText = "stews",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = stewsList
            };
            selectedStews.Clear();

            var addticketButton = new Button()
            {
                Content = "Add Ticket",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            addticketButton.Click += (object sendet, RoutedEventArgs evArgs) =>
            {
                selectedStews.Add(((StewardessDTO)stews.SelectedItem).Id);
                stewsList.Remove((StewardessDTO)stews.SelectedItem);
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

            SingleItem.Children.Add(pilots);
            SingleItem.Children.Add(stews);
            SingleItem.Children.Add(addticketButton);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, pilots,  crew);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            Button btnUpdate = turple.Item1;
            Button btnDelete = turple.Item2;
            ComboBox pilots = turple.Item3;
            CrewDTO crew = turple.Item4;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var crewCreating = new CrewDTO()
                {
                    Id = crew.Id,
                    PilotId = ((PilotDTO)pilots.SelectedItem).Id,
                    StewardressIds = selectedStews
                };
                int index = crewsList.ToList().FindIndex(t => t.Id == crew.Id);
                crewsList.RemoveAt(index);
                crewsList.Insert(index, crewCreating);
                try
                {
                    await service.UpdateAsync(crewCreating);
                }
                catch (Exception) { }

                UpdateList();
                SingleItem.Children.Clear();
            };

            btnDelete.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                try
                {
                    await service.DeleteAsync(crew);
                }
                catch (Exception) { }
                crewsList.Remove(crew);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }
    }
}
