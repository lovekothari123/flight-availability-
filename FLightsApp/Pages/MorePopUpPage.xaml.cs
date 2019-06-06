using System;
using System.Collections.Generic;
using FLightsApp.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
    public partial class MorePopUpPage : PopupPage
    {
		int id = 0;
		BookTicketResponse bookTicketResponse = new BookTicketResponse();
        public MorePopUpPage()
        {
            InitializeComponent();
			if (Application.Current.Properties.ContainsKey("id"))
            {
                id = Convert.ToInt16(Application.Current.Properties["id"]);

            }
			var gethistorytap = new TapGestureRecognizer();
			gethistorytap.Tapped += async(s, e) =>
			{
				gethistory.TextColor = Color.Gray;
				await Navigation.PushAsync(new BookingDetails(bookTicketResponse, id));
				await Navigation.PopPopupAsync();
			};
			gethistory.GestureRecognizers.Add(gethistorytap);

			var closetap = new TapGestureRecognizer();
			closetap.Tapped += async (s, e) =>
            {
				await Navigation.PopPopupAsync();
            };
			close.GestureRecognizers.Add(closetap);

			var logouttap = new TapGestureRecognizer();
			logouttap.Tapped += async (s, e) =>
            {
				logout.TextColor = Color.Gray;
				await Navigation.PushAsync(new LoginPage());
                await Navigation.PopPopupAsync();
            };
		 logout.GestureRecognizers.Add(logouttap);
        }
    }
}
