using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Acr.UserDialogs;
using FLightsApp.Models;
using FLightsApp.Webservice;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();

		}

		async void Submit_Clicked(object sender, System.EventArgs e)
		{
			UserDialogs.Instance.ShowLoading("please wait", MaskType.Gradient);
			if (string.IsNullOrWhiteSpace(userid.Text) || string.IsNullOrWhiteSpace(password.Text))
			{
				UserDialogs.Instance.HideLoading();
				Console.WriteLine("Please Enter All Details");
			}
			else
			{
				GetLogin(userid.Text, password.Text);
			}
		}


		public async void GetLogin(string username, string password)
		{

			if (NetworkCheck.IsInternet())
			{
				var client = new HttpClient();
				string request = @"{'AdminName' :'" + username + "','Password' :'" + password + "'}";
				string status = null, message = null;
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				HttpResponseMessage result = await client.PostAsync(WebserviceUrls.LOGINURL, content);

				if (result.IsSuccessStatusCode)
				{
					string response = result.Content.ReadAsStringAsync().Result;
					var msg = JsonConvert.DeserializeObject<LoginData>(response);

					status = msg.errorStatus.Status;
					message = msg.errorStatus.Message; 

					if (status == "0")
					{
						Application.Current.Properties["id"] = Convert.ToInt16(msg.UserId);
						UserDialogs.Instance.HideLoading();
                        string user = msg.UserName;
                        string userType = msg.UserType;
                        int userID = msg.UserId;
                        await Navigation.PushAsync(new MainTripPage(user,userType,userID));

					}
					else
					{
						UserDialogs.Instance.Alert(message, "", "Ok");
						UserDialogs.Instance.HideLoading();
					}
				}

				else
				{
					await DisplayAlert("JSONParsing", "No network is available.", "Ok");
					UserDialogs.Instance.HideLoading();
				}
			}
		}
	}
}
