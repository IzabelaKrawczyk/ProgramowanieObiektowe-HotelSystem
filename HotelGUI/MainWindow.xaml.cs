﻿using HotelSystem;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HotelGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<HotelRoom> observable;
        public Hotel hotel = new Hotel();
        public Reservation reservation;

        public MainWindow()
        {
            hotel = Hotel.ReadXML("hotel.xml");
            InitializeComponent();
            observable = new ObservableCollection<HotelRoom>(hotel.RoomList);
            listbox_rooms.ItemsSource = observable;
            ComboBox comboBox = new ComboBox();
            comboBox = cb_roomType;
            comboBox.Items.Add("SINGLE");
            comboBox.Items.Add("DOUBLE");
            comboBox.Items.Add("FAMILY");
        }


        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            textBox_firstName.Text = null;
            textBox_lastName.Text = null;
            textBox_street.Text = null;
            textBox_streetNumber.Text = null;
            textBox_flatNumber.Text = null;
            textBox_postalCode.Text = null;
            textBox_city.Text = null;
            textBox_telNo.Text = null;
            textBox_mail.Text = null;
            radiobutton_male.IsChecked = false;
            radiobutton_female.IsChecked = false;
            datepicker_dateofBirth.Text = null;
            cb_roomType.Text = null;
            datePicker_checkInDate.Text = null;
            datepicker_checkOutDate.Text = null;
            observable = new ObservableCollection<HotelRoom>(hotel.RoomList);
            listbox_rooms.ItemsSource = observable;
        }

        private void Button_Check_Click(object sender, RoutedEventArgs e)
        {
            HotelRoom room = new HotelRoom();
            Address address = new Address();
            Client client = new Client();
            string temp = textBox_firstName.Text;
            if (temp == null)
                MessageBox.Show("First name is null", "Important Message");
            else
                client.FirstName = temp;

            temp = textBox_lastName.Text;
            if (temp == null)
                MessageBox.Show("Last name is null", "Important Message");
            else
                client.LastName = temp;

            if (radiobutton_male == null && radiobutton_female == null)
                MessageBox.Show("Select gender", "Important Message");
            else
                client.Gender1 = (bool)radiobutton_male.IsChecked ? "M" : "F";


            if (datepicker_dateofBirth.SelectedDate == null)
                MessageBox.Show("Date of birth is not selected", "Important Message");
            else
                client.DateOfBirth = datepicker_dateofBirth.SelectedDate.ToString();

            temp = textBox_telNo.Text;
            if (temp == null)
                MessageBox.Show("Telephone number is null", "Important Message");
            else
                client.TelNo = temp;

            temp = textBox_mail.Text;
            if (temp == null)
                MessageBox.Show("Email address is null", "Important Message");
            else
                client.MailAddress = temp;

            temp = textBox_street.Text;
            if (temp == null)
                MessageBox.Show("Street name is null", "Important Message");
            else
                address.Street1 = temp;

            temp = textBox_streetNumber.Text;
            if (temp == null)
                MessageBox.Show("Street number is null", "Important Message");
            else
                address.StreetNumber1 = temp;

            temp = textBox_flatNumber.Text;
            address.FlatNumber = temp;

            temp = textBox_postalCode.Text;
            if (temp == null)
                MessageBox.Show("Postal code is null", "Important Message");
            else
                address.PostalCode = temp;

            temp = textBox_city.Text;
            if (temp == null)
                MessageBox.Show("City name is null", "Important Message");
            else
                address.City = temp;

            client.Address = address;

            room.RoomType1 = cb_roomType.Text;
            var roomData = listbox_rooms.SelectedItem.ToString().Split(' ');
            room.Name = roomData[1];
            room.Price = Double.Parse(roomData[3]);

            reservation = new Reservation(client, room, datePicker_checkInDate.SelectedDate.ToString(), datepicker_checkOutDate.SelectedDate.ToString());

            ReservationWindow reservationWindow = new ReservationWindow(reservation, hotel);
            reservationWindow.ShowDialog();
        }


        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            if (cb_roomType.SelectedItem != null && datePicker_checkInDate.SelectedDate != null && datepicker_checkOutDate.SelectedDate != null)
            {
                string checkindate = datePicker_checkInDate.SelectedDate.ToString();
                string checkoutdate = datepicker_checkOutDate.SelectedDate.ToString();
                string roomType = cb_roomType.SelectedItem.ToString();
                observable = new ObservableCollection<HotelRoom>();

                for (int i = 0; i < hotel.RoomList.Count; i++)
                {
                    if (hotel.RoomList[i].RoomType1.Equals(roomType))
                        if (hotel.IsRoomFree(checkindate, checkoutdate, hotel.RoomList[i]))
                            observable.Add(hotel.RoomList[i]);
                }
                listbox_rooms.ItemsSource = observable;
            }
            else MessageBox.Show("Select room type, check-in date, check-out date.", "Important Message");
        }

        private void Button_AllReservations_Click(object sender, RoutedEventArgs e)
        {
            ReservationsWindow okno = new ReservationsWindow(hotel);
            okno.ShowDialog();
        }
        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {


            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                hotel = Hotel.ReadXML(filename);

                listbox_rooms.ClearValue(ItemsControl.ItemsSourceProperty);
                observable = new ObservableCollection<HotelRoom>(hotel.RoomList);
                listbox_rooms.ItemsSource = observable;
            }


        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                Hotel.WriteXML(filename, hotel);
            }

        }
    }
}
