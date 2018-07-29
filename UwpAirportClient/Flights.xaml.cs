using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UwpAirportClient.Models;
using UwpAirportClient.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpAirportClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Flights : Page
    {
        public ObservableCollection<FlightDTO> flightsList = new ObservableCollection<FlightDTO>();

        public double height => Window.Current.Bounds.Height - 50;
        public Thickness marginCreate => new Thickness(Window.Current.Bounds.Width - 210, 10, 10, 10);

        private GenericService<FlightDTO> service = new GenericService<FlightDTO>(new System.Net.Http.HttpClient(), Url.Value + "Flights");
        private GenericService<TicketDTO> ticketService = new GenericService<TicketDTO>(new System.Net.Http.HttpClient(), Url.Value + "Tickets");
        private ObservableCollection<TicketDTO> ticketsList = new ObservableCollection<TicketDTO>();
        private List<int> selectedTickets = new List<int>();

        public Flights()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Flights_Load;
        }

        private void Flights_Load(object sender, RoutedEventArgs e)
        {
            UpdateList();
            GetTickets();
        }

        private async void GetTickets()
        {
            ticketsList.Clear();
            try
            {
                (await ticketService.getAllAsync()).ForEach(o => ticketsList.Add(o));
            }
            catch (Exception) { };
        }

        private async void UpdateList()
        {
            flightsList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => flightsList.Add(o));
            }
            catch (Exception) { };
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private (Button, TextBox, TextBox, TextBox, TimePicker, TimePicker, ComboBox) RenderCreate()
        {
            SingleItem.Children.Clear();

            var Number = new TextBox
            {
                Header = "Flight number",
                Width = 100,
                PlaceholderText = "Number",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var StartPoint = new TextBox
            {
                Header = "Start point",
                Width = 100,
                PlaceholderText = "Start",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var FinishPoint = new TextBox
            {
                Header = "Finish point",
                Width = 100,
                PlaceholderText = "Finish",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var StartTime = new TimePicker
            {
                Header = "Start time",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var FinishTime = new TimePicker
            {
                Header = "Start time",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var tickets = new ComboBox
            {
                Header = "Tickets",
                PlaceholderText = "ticket",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = ticketsList
            };
            ScrollViewer.SetVerticalScrollMode(tickets, ScrollMode.Enabled);
            selectedTickets.Clear();
            var addticketButton = new Button()
            {
                Content = "Add Ticket",
            };
            addticketButton.Click += (object sendet, RoutedEventArgs e) =>
              {
                  selectedTickets.Add(((TicketDTO)tickets.SelectedItem).Id);
                  ticketsList.Remove((TicketDTO)tickets.SelectedItem);
              };

            var btnCreate = new Button
            {
                Content = "Add item",
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            StackPanel panel1 = new StackPanel();
            panel1.Orientation = Orientation.Horizontal;
            panel1.HorizontalAlignment = HorizontalAlignment.Center;
            panel1.Children.Add(Number);
            panel1.Children.Add(StartPoint);
            panel1.Children.Add(FinishPoint);

            SingleItem.Children.Add(panel1);

            SingleItem.Children.Add(StartTime);
            SingleItem.Children.Add(FinishTime);

       
            SingleItem.Children.Add(tickets);
            SingleItem.Children.Add(addticketButton);
            SingleItem.Children.Add(btnCreate);
            
            return (btnCreate, Number, StartPoint, FinishPoint, StartTime, FinishTime, tickets);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            Button btnCreate = turple.Item1;
            TextBox number = turple.Item2;
            TextBox StartPoint = turple.Item3;
            TextBox FinishPoint = turple.Item4;
            TimePicker StarTime = turple.Item5;
            TimePicker FinishTime = turple.Item6;
            ComboBox SelectedTickets = turple.Item7;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var flight = new FlightDTO()
                {
                    Number = number.Text,
                    StartPoint = StartPoint.Text,
                    FinishPoint = FinishPoint.Text,
                    StartTime = (new DateTime(2018, 2, 2) + StarTime.Time).ToString(),
                    FinishTime = (new DateTime(2018, 2, 2) + FinishTime.Time).ToString(),
                    TicketIds = selectedTickets
                };
                try
                {
                    await service.CreateAsync(flight);
                }
                catch (Exception) { }

                flightsList.Add(flight);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button, Button, TextBox, TextBox, TextBox, TimePicker, TimePicker, FlightDTO) RenderDetail(ItemClickEventArgs e)
        {
            var flight = (FlightDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Flight id:" + flight.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;

            var Number = new TextBox
            {
                Header = "Name",
                Text = flight.Number,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            var StartPoint = new TextBox
            {
                Header = "Start point",
                Text = flight.StartPoint,
                Width = 100,
                PlaceholderText = "Start",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var FinishPoint = new TextBox
            {
                Header = "Finish point",
                Text = flight.FinishPoint,

                Width = 100,
                PlaceholderText = "Finish",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var StartTime = new TimePicker
            {
                Time = DateTime.Parse(flight.StartTime).TimeOfDay,
                Header = "Start time",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var FinishTime = new TimePicker
            {
                Time = DateTime.Parse(flight.FinishTime).TimeOfDay,
                Header = "Start time",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var tickets = new ComboBox
            {
                Header = "Tickets",
                PlaceholderText = "ticket",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                ItemsSource = ticketsList
            };
            ScrollViewer.SetVerticalScrollMode(tickets, ScrollMode.Enabled);
            selectedTickets.Clear();

            var addticketButton = new Button()
            {
                Content = "Add Ticket",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            addticketButton.Click += (object sendet, RoutedEventArgs evArgs) =>
            {
                selectedTickets.Add(((TicketDTO)tickets.SelectedItem).Id);
                ticketsList.Remove((TicketDTO)tickets.SelectedItem);
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
            StackPanel panel1 = new StackPanel();
            panel1.Orientation = Orientation.Horizontal;
            panel1.HorizontalAlignment = HorizontalAlignment.Center;
            panel1.Children.Add(Number);
            panel1.Children.Add(StartPoint);
            panel1.Children.Add(FinishPoint);

            SingleItem.Children.Add(panel1);

            SingleItem.Children.Add(StartTime);
            SingleItem.Children.Add(FinishTime);


            SingleItem.Children.Add(tickets);
            SingleItem.Children.Add(addticketButton);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, Number, StartPoint,FinishPoint,StartTime,FinishTime,flight);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            Button btnUpdate = turple.Item1;
            Button btnDelete = turple.Item2;
            TextBox number = turple.Item3;
            TextBox StartPoint = turple.Item4;
            TextBox FinishPoint = turple.Item5;
            TimePicker StartTime = turple.Item6;
            TimePicker EndTime = turple.Item7;
            FlightDTO flight = turple.Item8;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var flightCreating = new FlightDTO()
                {
                    Id = flight.Id,
                    Number = number.Text,
                    StartTime = (new DateTime(2018, 2, 2) + StartTime.Time).ToString(),
                    FinishTime = (new DateTime(2018, 2, 2) + StartTime.Time).ToString(),
                    StartPoint = StartPoint.Text,
                    FinishPoint = FinishPoint.Text,
                    TicketIds = selectedTickets
                };
                int index = flightsList.ToList().FindIndex(t => t.Id == flight.Id);
                flightsList.RemoveAt(index);
                flightsList.Insert(index, flightCreating);
                try
                {
                    await service.UpdateAsync(flightCreating);
                }
                catch (Exception) { }

                UpdateList();
                SingleItem.Children.Clear();
            };

            btnDelete.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                try
                {
                    await service.DeleteAsync(flight);
                }
                catch (Exception) { }
                flightsList.Remove(flight);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }
    }
}
