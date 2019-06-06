using System;
using System.Collections.Generic;
using System.Linq;
using FLightsApp.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	public partial class FilterByPage : PopupPage
	{
		int height = 40;
		MainModel model = new MainModel();
 		List<string> filterlist = new List<string>();
		List<int> stopfilterlist = new List<int>();
        
		List<MainModel> flightfilteritems = new List<MainModel>();
		List<MainModel> Stopfilteritems = new List<MainModel>();
		public event EventHandler<SelectedItemChangedEventArgs> OnSelectedCity;
		public FilterByPage(List<string> filter, List<int> stopfilters)
		{
			model.Flightfilterlst = new List<string>(0);
			model.Stopfilterlst = new List<int>(0);
			InitializeComponent();

			flightfilteritems = new List<MainModel>
			{
				new MainModel{
					AirlineName= "Jet Airways"
					
				},
				new MainModel{
                    AirlineName= "Spice Jet"

                },
				new MainModel{
                    AirlineName= "Go Air"

                },
				new MainModel{
                    AirlineName= "Air India"

                },
				new MainModel{
                    AirlineName= "Indigo"
                        
                }
			};
			Stopfilteritems = new List<MainModel>
			{
				new MainModel{
					Stops= 0

				},
				new MainModel{
					Stops= 1
                        
				},
				new MainModel{
					Stops= 2

				}
			};
			var data = flightfilteritems.Where(X => filter.Contains(X.AirlineName)).ToList();
			flightfilters.ItemsSource = data;
			flightfilters.HeightRequest = height * data.Count;

			var Stopdata = Stopfilteritems.Where(X => stopfilters.Contains(X.Stops)).ToList();
			stopfilterslist.ItemsSource = Stopdata;
			stopfilterslist.HeightRequest = height * Stopdata.Count;


			var donetap = new TapGestureRecognizer();
			donetap.Tapped += async (s, e) =>
			 {

				OnSelectedCity(model, null);
                 await Navigation.PopPopupAsync();
	         };
			done.GestureRecognizers.Add(donetap);
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
			model.Flightfilterlst = filterlist;
        }
		void Handle_PropertyChanged1(object sender, EventArgs e)
        {
            var data = (XLabs.Forms.Controls.CheckBox)sender;
            //checkvalue += data.DefaultText + " ";
            if (data.Checked)
            {
				stopfilterlist.Add(Convert.ToInt16(data.DefaultText));


            }
            else
            {
				if (stopfilterlist.Contains(Convert.ToInt16(data.DefaultText)))
                {
					stopfilterlist.Remove(Convert.ToInt16(data.DefaultText));
                }
            }
			model.Stopfilterlst = stopfilterlist;
        }
		public FilterByPage(EventHandler<SelectedItemChangedEventArgs> OnSelectedCity)
		{
			this.OnSelectedCity = OnSelectedCity;
		}
	}
}
