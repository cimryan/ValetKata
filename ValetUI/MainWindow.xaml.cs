using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Valet valet = new Valet();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void getTicketNumberButton_Click(object sender, RoutedEventArgs e)
        {
            ticketNumberBlock.Text = this.valet.RecordCarAccepted(makeModelColorBox.Text).ToString();
            makeModelColorBox.Text = "";
        }

        private void recordCarParkedButton_Click(object sender, RoutedEventArgs e)
        {
            this.valet.RecordCarParked(ticketNumberRowColumnBox.Text);

            ticketNumberRowColumnBox.Text = "";

            spotConfirmedBlock.Text = "Okay, spot confirmed";
        }

        private void ticketNumberRowColumnBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            spotConfirmedBlock.Text = "";
        }

        private void getInfoButton_Click(object sender, RoutedEventArgs e)
        {
            carInformationBlock.Text = this.valet.RetrieveParkedCarInformation(int.Parse(ticketNumberBox.Text));
        }

        private void makeModelColorBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ticketNumberBlock.Text = "";
        }

        private void ticketNumberBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            carInformationBlock.Text = "";
        }

        private void carReturnedTicketNumberBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            carReturnedBlock.Text = "";
        }

        private void returnCar_Click(object sender, RoutedEventArgs e)
        {
            this.valet.RecordCarReturned(int.Parse(carReturnedTicketNumberBox.Text));
            carReturnedTicketNumberBox.Text = "";
        }

        //private void lostTicketMakeModelColorBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    lostTicketCarInfoBlock.Text = "";
        //}

        //private void findCarInfoButton_Click(object sender, RoutedEventArgs e)
        //{
        //    lostTicketCarInfoBlock.Text = String.Join("\n", this.valet.FindCarInfo(lostTicketMakeModelColorBox.Text).ToArray());

        //    this.lostTicketMakeModelColorBox.Text = "";
        //}
    }
}
