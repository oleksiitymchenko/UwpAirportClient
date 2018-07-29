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
    public sealed partial class Tickets : Page
    {
        public ObservableCollection<TicketDTO> ticketList = new ObservableCollection<TicketDTO>();

        public double height =>Window.Current.Bounds.Height;

        private TicketService service = new TicketService();

        public Tickets()
        {
            UpdateList();
            this.InitializeComponent();
            this.Loaded += Tickets_Loaded;
        }

        private void Tickets_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
           // ticketsList.ItemsSource = this.ticketList;
        }

        private async void UpdateList()
        {
            ticketList.Clear();
            (await service.getAllAsync()).ForEach(o=>ticketList.Add(o));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(ticketList);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void itemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ticket = (TicketDTO)e.ClickedItem;
            SingleItem.Children.Clear();
            var id = new TextBlock();
            id.Text = "Ticket id:" + ticket.Id;
            id.FontWeight = Windows.UI.Text.FontWeights.Bold;
            id.Margin = new Windows.UI.Xaml.Thickness(0,100,10,10);
            id.Width = 200;

            var number = new TextBox();
            number.Header = "Flight number";
            number.Text = ticket.FlightNumber;
            number.Width = 200;

            var price = new TextBox();
            price.Header = "Price ";
            price.Text = ticket.Price.ToString()+"$";
            price.Width = 200;

            var btnUpdate = new Button();
            btnUpdate.Content = "Update item";
            btnUpdate.Width = 95;

            var btnDelete = new Button();
            btnDelete.Content = "Delete item";
            btnDelete.Width = 95;

            var buttonsStack = new StackPanel();
            buttonsStack.Orientation = Orientation.Horizontal;

            btnDelete.Click += async (object sen, RoutedEventArgs eva)=>
            {
                await service.DeleteAsync(ticket);
                ticketList.Remove(ticket);
                UpdateList();
                SingleItem.Children.Clear();

            };

            SingleItem.Children.Add(id);
            SingleItem.Children.Add(number);
            SingleItem.Children.Add(price);
            buttonsStack.Children.Add(btnUpdate);
            buttonsStack.Children.Add(btnDelete);
            SingleItem.Children.Add(buttonsStack);
        }
    }
}
