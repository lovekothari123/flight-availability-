using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using FLightsApp.Models;
using FLightsApp.Webservice;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;

namespace FLightsApp.Pages
{
	public partial class BookingDetails : ContentPage
    
	{
		string refno = null; int uid = 0;
		BookTicketResponse bookTicket = new BookTicketResponse();

		List<HistoryDetail> historyitems = new List<HistoryDetail>();
		public BookingDetails(BookTicketResponse bookTicketResponse, int userid)
        {
			InitializeComponent();
			uid = userid;
			var backtap = new TapGestureRecognizer();
			backtap.Tapped += async (s, e) =>
			{
				await Navigation.PopAsync();
			};
			back.GestureRecognizers.Add(backtap);
            
			bookTicketResponse.IsNoShow = true;
	
	
	/*		for (int i = 0; i < bookTicketResponse.PassengerDetails.Count;i++){
				for (int j=0; j < bookTicketResponse.TicketDetails.Count;j++){
			     	bookTicketResponse.PassengerDetails[i].Name = bookTicketResponse.PassengerDetails[i].Title +" "+ bookTicketResponse.PassengerDetails[i].FirstName + " " +bookTicketResponse.PassengerDetails[i].LastName;
					bookTicketResponse.PassengerDetails[i].TotalAmount = bookTicketResponse.TicketDetails[j].TotalAmount; 	
					bookTicketResponse.PassengerDetails[i].RefNo = bookTicketResponse.TicketDetails[j].RefNo;
					bookTicketResponse.RefNo = bookTicketResponse.PassengerDetails[i].RefNo;
				}
				for (int k=0; k< bookTicketResponse.FlightFareDetails.Count; k++)
                {
					bookTicketResponse.PassengerDetails[i].FromAirportCode = bookTicketResponse.FlightFareDetails[k].FromAirportCode; 
					bookTicketResponse.PassengerDetails[i].ToAirportCode = bookTicketResponse.FlightFareDetails[k].ToAirportCode; 
					bookTicketResponse.PassengerDetails[i].AirlineCode = bookTicketResponse.FlightFareDetails[k].AirlineCode;
					if (bookTicketResponse.PassengerDetails[i].AirlineCode=="SG")
                    {
						bookTicketResponse.PassengerDetails[i].IconPath= "SG.png";
                    }
					else if (bookTicketResponse.PassengerDetails[i].AirlineCode =="AI")
                    {
						bookTicketResponse.PassengerDetails[i].IconPath = "AI.png";
                    }
					else if (bookTicketResponse.PassengerDetails[i].AirlineCode =="6E")
                    {
						bookTicketResponse.PassengerDetails[i].IconPath = "EE.png";
                    }
					else if (bookTicketResponse.PassengerDetails[i].AirlineCode == "9W")
                    {
						bookTicketResponse.PassengerDetails[i].IconPath = "WW.png";
                    }
					else if (bookTicketResponse.PassengerDetails[i].AirlineCode == "UK")
                    {
						bookTicketResponse.PassengerDetails[i].IconPath = "UK.png";
                    }
                    else
                    {
						bookTicketResponse.PassengerDetails[i].IconPath = "GG.png";
                    } 
                }
			}
			bookTicket = bookTicketResponse; 
			history_list.ItemsSource = bookTicketResponse.PassengerDetails;*/
        }
		protected override void OnAppearing()
        {
            base.OnAppearing();
			GetHistory(uid);
       

        }

		public void CancelRequest(BookTicketResponse bookTicketResponse){
			if (NetworkCheck.IsInternet())
			{
				var client = new HttpClient();
				var json = JsonConvert.SerializeObject(bookTicketResponse);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
			}else{
				UserDialogs.Instance.Alert("No Internet Connection", "", "Ok");
			}
			UserDialogs.Instance.HideLoading();
		}
		public async void GetHistory(int Id)
        {
			UserDialogs.Instance.ShowLoading("loading",MaskType.Clear);
            if (NetworkCheck.IsInternet())
            {
				try{
                string request = null;
                var client = new HttpClient(); 
				request = @"{PNRNo :" + "" + ",AgentId :" + ""+ ",AirlinePNR : " + "" + ",FromDate:" + "" + ",ToDate:" + "" + ",FirstName:" + "" + ",LastName:" + "" + ",TravelDate:" + "" + ",UserId:'" + Id  + "'}";

                    Console.WriteLine("request From History   " + request);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    
					var response =  client.PostAsync(WebserviceUrls.GETHISTORY, content).Result;
					var result = await response.Content.ReadAsStringAsync();

				if(response.IsSuccessStatusCode){
					
					//var result = await response.Content.ReadAsStringAsync();
                    PlaceList data = new PlaceList();
                    if (result != "")
                    {
                        data = JsonConvert.DeserializeObject<PlaceList>(result);
                    }

                        historyitems = new List<HistoryDetail>(data.bookingadminLst);
						for (int i = 0; i < historyitems.Count; i++)
						{
							if (historyitems[i].AirlineCode == "SG")
							{
								historyitems[i].IconPath = "SG.png";
							}
							else if (historyitems[i].AirlineCode == "AI")
							{
								historyitems[i].IconPath = "AI.png";
							}
							else if (historyitems[i].AirlineCode == "6E")
							{
								historyitems[i].IconPath = "EE.png";
							}
							else if (historyitems[i].AirlineCode == "9W")
							{
								historyitems[i].IconPath = "WW.png";
							}
							else if (historyitems[i].AirlineCode == "UK")
							{
								historyitems[i].IconPath = "UK.png";
							}
							else
							{
								historyitems[i].IconPath = "GG.png";
							}
						}
						history_list.ItemsSource = historyitems;
					}
				}
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                UserDialogs.Instance.Alert("No Internet Connection", "", "Ok");
            }
            UserDialogs.Instance.HideLoading();
        }
		public async void OnframeTapped2(object sender, EventArgs args)
        {
            StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
           
                refno = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;
		
          }
            await Navigation.PushPopupAsync(new CancelPopUp(refno));
        }

        public async   void Handle_Tapped(object sender, System.EventArgs e)
        {
            StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;

                refno = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;

            }
            await Navigation.PushPopupAsync(new CancelPopUp(refno));
        }
    }
}
