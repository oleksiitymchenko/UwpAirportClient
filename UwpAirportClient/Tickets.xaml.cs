using System;
using System.Collections.ObjectModel;
using System.Linq;
using UwpAirportClient.Models;
using UwpAirportClient.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpAirportClient
{
    public sealed partial class Tickets : Page
    {
        public ObservableCollection<TicketDTO> ticketList = new ObservableCollection<TicketDTO>();

        public double height =>Window.Current.Bounds.Height-50;
        public Thickness marginCreate => new  Thickness(Window.Current.Bounds.Width-210,10,10,10);

        private GenericService<TicketDTO> service = new GenericService<TicketDTO>(new System.Net.Http.HttpClient(),Url.Value+"Tickets");

        public Tickets()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Tickets_Loaded;
        }

        private void Tickets_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private async void UpdateList()
        {
            ticketList.Clear();
            try
            {
                (await service.getAllAsync()).ForEach(o => ticketList.Add(o));
            }
            catch(Exception){};
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private (Button, TextBox, TextBox) RenderCreate()
        {
            SingleItem.Children.Clear();

            var number = new TextBox
            {
                Header = "Flight number",
                Width = 200,
                PlaceholderText = "Flight number",
                Margin = new Thickness(0, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var price = new TextBox
            {
                Header = "Price ",
                PlaceholderText = "Price",
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            price.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
            {
                if (evArgs.Key.ToString().Equals("Back"))
                {
                    evArgs.Handled = false;
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (evArgs.Key.ToString() == string.Format("Number{0}", i))
                    {
                        evArgs.Handled = false;
                        return;
                    }
                }
                evArgs.Handled = true;
            };


            var btnCreate = new Button
            {
                Content = "Add item",
                Width = 200,
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            SingleItem.Children.Add(number);
            SingleItem.Children.Add(price);
            SingleItem.Children.Add(btnCreate);
            return (btnCreate,number,price);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var turple = RenderCreate();
            var btnCreate = turple.Item1;
            var number = turple.Item2;
            var price = turple.Item3;

            btnCreate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var ticketCreating = new TicketDTO()
                { FlightNumber = number.Text, Price = double.Parse(price.Text) };
                try
                {
                    await service.CreateAsync(ticketCreating);
                }
                catch (Exception) { }

                ticketList.Add(ticketCreating);
                UpdateList();
                SingleItem.Children.Clear();
            };
        }

        private (Button,Button,TextBox,TextBox,TicketDTO) RenderDetail(ItemClickEventArgs e)
        {
            var ticket = (TicketDTO)e.ClickedItem;
            SingleItem.Children.Clear();

            var id = new TextBlock();
            id.Text = "Ticket id:" + ticket.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Thickness(0, 100, 10, 10);
            id.Width = 200;

            var number = new TextBox
            {
                Header = "Flight number",
                Text = ticket.FlightNumber,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            var price = new TextBox
            {
                Header = "Price ",
                Text = ticket.Price.ToString(),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            price.KeyDown += (object obj, KeyRoutedEventArgs evArgs) =>
            {
                if (evArgs.Key.ToString().Equals("Back"))
                {
                    evArgs.Handled = false;
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (evArgs.Key.ToString() == string.Format("Number{0}", i))
                    {
                        evArgs.Handled = false;
                        return;
                    }
                }
                evArgs.Handled = true;
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
            SingleItem.Children.Add(number);
            SingleItem.Children.Add(price);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);

            return (btnUpdate, btnDelete, number, price, ticket);
        }

        private void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var turple = RenderDetail(e);
            var btnUpdate = turple.Item1;
            var btnDelete = turple.Item2;
            var number = turple.Item3;
            var price = turple.Item4;
            var ticket = turple.Item5;

            btnUpdate.Click += async (object sen, RoutedEventArgs evArgs) =>
            {
                var ticketUpdating = new TicketDTO()
                { Id = ticket.Id, FlightNumber = number.Text, Price = double.Parse(price.Text) };

                int index = ticketList.ToList().FindIndex(t => t.Id == ticket.Id);
                ticketList.Insert(index, ticketUpdating);
              
                try
                 {
                     await service.UpdateAsync(ticketUpdating);
                    }
                 catch (Exception) { }

                 UpdateList();
                 SingleItem.Children.Clear();
             };
          
            btnDelete.Click += async (object sen, RoutedEventArgs evArgs)=>
            {
                try
                {
                    await service.DeleteAsync(ticket);
                }
                catch (Exception) { }
                ticketList.Remove(ticket);
                UpdateList();
                SingleItem.Children.Clear();
            }; 
        }
    }
}
