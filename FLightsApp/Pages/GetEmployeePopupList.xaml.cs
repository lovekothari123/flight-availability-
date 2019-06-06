using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using FLightsApp.Models;
using FLightsApp.Webservice;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
    public partial class GetEmployeePopupList : PopupPage
    {
        List<BookingPassengerDetails> emplist = new List<BookingPassengerDetails>();
        public event EventHandler<SelectedItemChangedEventArgs> OnSelected;
        string paxno = null;
        public GetEmployeePopupList(string paxno)
        {
            InitializeComponent();
            this.paxno = paxno;
            var backtap = new TapGestureRecognizer();
            Console.WriteLine("aya" + paxno);
            backtap.Tapped += async (s, e) =>
            {
                await Navigation.PopPopupAsync();
            };
            back.GestureRecognizers.Add(backtap);

            GetEmployeeList();






        }




        public async void GetEmployeeList()
        {
            UserDialogs.Instance.ShowLoading("please wait", MaskType.Gradient);
            if (NetworkCheck.IsInternet())
            {

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(WebserviceUrls.GETEMPLOYEE);
                string getemploye = await response.Content.ReadAsStringAsync();
                PlaceList empdata = new PlaceList();
                if (getemploye != "")
                {
                    empdata = JsonConvert.DeserializeObject<PlaceList>(getemploye);
                }
                emplist = empdata.lstempmaster;
                UserDialogs.Instance.HideLoading();
                //lst = new List<GetEmployeResponse>(placeList.lstFlightAirport); 

                getEmpList.ItemsSource = emplist;
            }
            else
            {
                UserDialogs.Instance.Alert("Please Check Your Internet Connection", "", "OK");
            }
        }



        public GetEmployeePopupList(EventHandler<SelectedItemChangedEventArgs> OnSelectedCity)
        {
            this.OnSelected = OnSelectedCity;
        }

        async void itemselected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            else
            {


                ((ListView)sender).SelectedItem = null;
                if (OnSelected != null)

                {
                    OnSelected((BookingPassengerDetails)e.SelectedItem, null);
                }
                await Navigation.PopPopupAsync();

            }


        }

    }
}
