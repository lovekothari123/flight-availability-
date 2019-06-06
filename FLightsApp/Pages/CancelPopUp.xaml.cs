using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Acr.UserDialogs;
using FLightsApp.Models;
using FLightsApp.Webservice;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages; 
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	public partial class CancelPopUp : PopupPage
	{
		CancelTicketRequest cancelTicketRequest = new CancelTicketRequest();
		string checkvalue = null;
		List<FlightDetail> flightDetails = new List<FlightDetail>();
		List<PassengerDetail> passengerDetails = new List<PassengerDetail>();
		List<string> filterlist = new List<string>();
		public CancelPopUp(string RefNo)
		{
			InitializeComponent();
			GetCancelResponse(RefNo);

			var closetap = new TapGestureRecognizer();
			closetap.Tapped += async (s, e) =>
			{
				await Navigation.PopPopupAsync();
			};
			close.GestureRecognizers.Add(closetap);



		}
		public async void GetCancelResponse(string RefNumber)
		{
			if (NetworkCheck.IsInternet())
			{
				string request = null;
				var client = new HttpClient();
				request = @"{RefNo :'" + RefNumber + "'}";
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var response = client.PostAsync(WebserviceUrls.GETCANCELDETAILS, content).Result;
				var result = await response.Content.ReadAsStringAsync();
				PlaceList data = new PlaceList();
				if (result != null)
				{
					data = JsonConvert.DeserializeObject<PlaceList>(result);
				}
				passengerDetails = data.lstCancelFlightPassenger;
				for (int j = 0; j < passengerDetails.Count; j++)
				{
					if (j == 0)
					{
						refno.Text = passengerDetails[j].RefNo;
						cancelTicketRequest.RefNo = passengerDetails[j].RefNo;
					}
					flightDetails.Add(new FlightDetail { FromAirportCode = passengerDetails[j].FromAirportCode, 
						ToAirportCode = passengerDetails[j].ToAirportCode, AirlineCode=passengerDetails[j].AirlineCode,
						FlightClass= passengerDetails[j].FlightClass,FlightNo= passengerDetails[j].FlightNo,SegmentSeqNo= passengerDetails[j].SegmentSeqNo});
				}
				passengerlist.ItemsSource = passengerDetails;
				int i = passengerDetails.Count;
				passengerlist.HeightRequest = i * 31;

			}
			else
			{

			}
		}

			void Handle_PropertyChanged(object sender, EventArgs e)
			{
				var data = (XLabs.Forms.Controls.CheckBox)sender;
				//checkvalue += data.DefaultText + " ";
				if (data.Checked)
				{
					filterlist.Add(data.DefaultText);


				}
				else
				{
					if (filterlist.Contains(data.DefaultText))
					{
						filterlist.Remove(data.DefaultText);
					}
				}
			}

			void Cancel_Clicked(object sender, System.EventArgs e)
			{
				var data = passengerDetails.Where(X => filterlist.Contains(X.FirstName)).ToList();
				cancelTicketRequest.Passengers = data;
				cancelTicketRequest.Segments = flightDetails;
			cancelTicketRequest.IsNoShow = true;
				CancelTicketRequest(cancelTicketRequest);

			}
			async void CancelTicketRequest(CancelTicketRequest requestcancel)
			{
				if (NetworkCheck.IsInternet())
				{

					var client = new HttpClient();
					var json = JsonConvert.SerializeObject(requestcancel);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					var response = client.PostAsync(WebserviceUrls.CANCELREQUEST, content).Result;
					var result = await response.Content.ReadAsStringAsync();
				}
				else
				{
					UserDialogs.Instance.Alert("Please Check your internet Connection", "", "OK");
				}
			}
		}   
}
