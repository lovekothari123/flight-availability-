using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using FLightsApp.Models;
using FLightsApp.Webservice;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	public partial class ArrivalFiltrationPopUp : PopupPage
	{
		List<MainModel> arrivalitems = new List<MainModel>();
		List<MainModel> lst = new List<MainModel>();
		public event EventHandler<SelectedItemChangedEventArgs> OnSelectedCity;
		public ArrivalFiltrationPopUp()
		{
			InitializeComponent();
			BindingContext = this;
			GetPlacesList();
			/*	arrivalitems = new List<MainModel>
				{
					new MainModel{
						PlaceName = "Delhi",
						ShortPlaceName = "DEL"

					},
					new MainModel{
						PlaceName = "Mumbai",
						ShortPlaceName = "BOM"
					},
					new MainModel{
						PlaceName = "Indore",
						ShortPlaceName = "IDR"
					}
					,
					new MainModel{
						PlaceName = "Hyderabad",
						ShortPlaceName = "HYD"
					}
					,
					new MainModel{
						PlaceName = "Kolkata",
						ShortPlaceName = "CCU"

					}
				};
				filteritems.ItemsSource = arrivalitems;*/

			var backtap = new TapGestureRecognizer();
			backtap.Tapped += async (s, e) =>
			{
				await Navigation.PopPopupAsync();
			};
			back.GestureRecognizers.Add(backtap);


			var cleartap = new TapGestureRecognizer();
			cleartap.Tapped += async (s, e) =>
			{
				fromentry.Text = "";

			};
			clear.GestureRecognizers.Add(cleartap);

		}

		void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{

            //filteritems.ItemsSource = lst.Where(x => x.AirportName.ToLower().Contains(fromentry.Text.ToString().ToLower()) ).ToList();
           

            var data = lst.Where(X => X.CountryName == "India").ToList();

            filteritems.ItemsSource = data.Where(x =>( x.AirportName.ToLower().Contains(fromentry.Text.ToString().ToLower())|| x.AirportCode.ToLower().Contains(fromentry.Text.ToString().ToLower()))).ToList();

           // filteritems.ItemsSource = data.Where((i) => i.AirlineName.ToLower().Contains(fromentry.Text.ToString().ToLower()) == i.AirlineCode.ToLower().Contains(fromentry.Text.ToString().ToLower())).ToList();

            //filteritems.ItemsSource = data.Where(x => x.AirportName.ToLower().Contains(fromentry.Text.ToString().ToLower()) || x.AirlineCode.ToLower().Contains(fromentry.Text.ToString().ToLower())).ToList();
        
        }
       

		public ArrivalFiltrationPopUp(EventHandler<SelectedItemChangedEventArgs> OnSelectedCity)
		{
			this.OnSelectedCity = OnSelectedCity;
		}
		async void Handle_PropertyChanged(object sender, SelectedItemChangedEventArgs e)
		{
			var data = (XLabs.Forms.Controls.CheckBox)sender;

			string s = data.DefaultText;

			if (e.SelectedItem == null)
			{
				return;
			}
			else
			{
				//((XLabs.Forms.Controls.CheckBox)sender)e.SelectedItem = null;
				if (OnSelectedCity != null)
				{
					OnSelectedCity((MainModel)e.SelectedItem, null);
				}
				await Navigation.PopPopupAsync();

			}

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
				if (OnSelectedCity != null)
				{
					OnSelectedCity((MainModel)e.SelectedItem, null);
				}
				await Navigation.PopPopupAsync();

			}


		}

		public async void GetPlacesList()
		{
			UserDialogs.Instance.ShowLoading("please wait", MaskType.Gradient);
			if (NetworkCheck.IsInternet())
			{

				var client = new System.Net.Http.HttpClient();
				var response = await client.GetAsync(WebserviceUrls.PLACELIST);
				string places = await response.Content.ReadAsStringAsync();
				PlaceList placeList = new PlaceList();
				if (places != "")
				{
					placeList = JsonConvert.DeserializeObject<PlaceList>(places);
				}
				UserDialogs.Instance.HideLoading();
				lst = new List<MainModel>(placeList.lstFlightAirport);
				var data = lst.Where(X => X.CountryName == "India").ToList();
				filteritems.ItemsSource = data;
			}
			else
			{
				UserDialogs.Instance.Alert("Please Check Your Internet Connection", "", "OK");
			}
		}

	}
}
