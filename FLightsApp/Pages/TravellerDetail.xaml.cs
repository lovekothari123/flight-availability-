using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using FLightsApp.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	public partial class TravellerDetail : PopupPage
	{
		public event EventHandler<SelectedItemChangedEventArgs> OnSelectedCity;
		List<RadioModel> cabinclassitmes = new List<RadioModel>();
		int adultcountval = 0, childcountval = 0, infantcountval = 0, totalcount=0;
		string cabinclassval = null;
		public TravellerDetail()
		{

			InitializeComponent();
			var backtap = new TapGestureRecognizer();
			backtap.Tapped += async (s, e) =>
			{
				await Navigation.PopPopupAsync();
			};
			back.GestureRecognizers.Add(backtap);
			adultcountval = Convert.ToInt16(adultcount.Text);
			childcountval = Convert.ToInt16(childcount.Text);
			infantcountval = Convert.ToInt16(infantcount.Text);

			cabinclassitmes = new List<RadioModel>
			{
				new RadioModel{
					Title = "Economy",
					IsSelected=true
          	},
				new RadioModel{
					Title = "Premium Economy"
				},
				new RadioModel{
					Title = "Business"
				}
			};
			cabinclasslist.ItemsSource = cabinclassitmes;

			var adultminustap = new TapGestureRecognizer();
			adultminustap.Tapped += async (s, e) =>
			{

				if (adultcountval == 1)
				{
					adultminus.TextColor = Color.Gray;
					adultplus.TextColor = Color.Red;
				}
				else if (adultcountval == infantcountval)
				{
					adultminus.TextColor = Color.Gray;
					adultplus.TextColor = Color.Red;
					UserDialogs.Instance.Alert("No. Infants cannot exceed no. of Adults", "", "OK");
				}
				else
				{
					adultminus.TextColor = Color.Red;
					adultcountval -= 1;
					adultcount.Text = Convert.ToString(adultcountval);
					if (adultcountval < 9)
					{
						adultplus.TextColor = Color.Red;
					}
					else
					{
						adultplus.TextColor = Color.Gray;
					}



				}
			};
			adultminus.GestureRecognizers.Add(adultminustap);

			var adultplustap = new TapGestureRecognizer();
			adultplustap.Tapped += async (s, e) =>
			{
				 totalcount = adultcountval + childcountval + infantcountval;
				if (totalcount < 9)
				{
					if (adultcountval >= 1 && adultcountval < 9)
					{
						adultminus.TextColor = Color.Red;
						if (adultcountval == 1)
						{

							adultplus.TextColor = Color.Red;
						}
						else
						{
							adultminus.TextColor = Color.Red;
						}
						adultcountval += 1;
						adultcount.Text = Convert.ToString(adultcountval);


					}
					else
					{
						if (adultcountval == 1)
						{
							adultminus.TextColor = Color.Gray;
							adultplus.TextColor = Color.Red;
						}
						else
						{
							adultminus.TextColor = Color.Red;
							if (adultcountval == 9)
							{
								adultplus.TextColor = Color.Gray;
							}
						}

					}
				}
				else
				{
					UserDialogs.Instance.Alert("Upto 9 Travellers can be booked at a time", "", "OK");
				}
			};
			adultplus.GestureRecognizers.Add(adultplustap);


			var childplustap = new TapGestureRecognizer();
			childplustap.Tapped += async (s, e) =>
			{
				 totalcount = adultcountval + childcountval + infantcountval;
				if (totalcount < 9)
				{
					if (childcountval >= 0 && childcountval < 9)
					{
						childminus.TextColor = Color.Red;
						if (childcountval == 0)
						{

							childminus.TextColor = Color.Red;
						}
						else
						{
							childminus.TextColor = Color.Red;
						}
						childcountval += 1;
						childcount.Text = Convert.ToString(childcountval);


					}
					else
					{
						if (childcountval == 0)
						{
							childminus.TextColor = Color.Gray;
							childplus.TextColor = Color.Red;
						}
						else
						{
							childminus.TextColor = Color.Red;
							if (childcountval == 9)
							{
								childplus.TextColor = Color.Gray;
							}
						}

					}
				}
				else
				{
					UserDialogs.Instance.Alert("Upto 9 Travellers ca be booked at a time", "", "OK");
				}
			};
			childplus.GestureRecognizers.Add(childplustap);

			var childminustap = new TapGestureRecognizer();
			childminustap.Tapped += async (s, e) =>
			{
				if (childcountval == 0)
				{
					childminus.TextColor = Color.Gray;
					childplus.TextColor = Color.Red;
				}
				else
				{
					childminus.TextColor = Color.Red;
					childcountval -= 1;
					childcount.Text = Convert.ToString(childcountval);
					if (childcountval < 9)
					{
						childplus.TextColor = Color.Red;
					}
					else
					{
						childplus.TextColor = Color.Gray;
					}



				}
			};
			childminus.GestureRecognizers.Add(childminustap);

			var infantplustap = new TapGestureRecognizer();
			infantplustap.Tapped += async (s, e) =>
			{
			 totalcount = adultcountval + childcountval + infantcountval;
				if (totalcount < 9)
				{
					if (infantcountval >= 0 && infantcountval < 9 && infantcountval < adultcountval)
					{
						infantminus.TextColor = Color.Red;
						if (infantcountval == 0)
						{

							infantminus.TextColor = Color.Red;
						}
						else
						{
							infantminus.TextColor = Color.Red;
						}
						infantcountval += 1;
						infantcount.Text = Convert.ToString(infantcountval);


					}
					else if (infantcountval == adultcountval)
					{
						infantminus.TextColor = Color.Red;
						infantplus.TextColor = Color.Gray;
						UserDialogs.Instance.Alert("No. of Infants cannot exceed no. Adults", "", "OK");
					}
					else
					{
						if (infantcountval == 0)
						{
							infantminus.TextColor = Color.Gray;
							infantplus.TextColor = Color.Red;
						}
						else
						{
							infantminus.TextColor = Color.Red;
							if (infantcountval == 9)
							{
								infantplus.TextColor = Color.Gray;
							}
						}

					}
				}
				else
				{
					UserDialogs.Instance.Alert("Upto 9 Travellers can be booked at a time", "", "OK");
				}
			};
			infantplus.GestureRecognizers.Add(infantplustap);

			var infantminustap = new TapGestureRecognizer();
			infantminustap.Tapped += async (s, e) =>
			{
				if (infantcountval == 0)
				{
					infantminus.TextColor = Color.Gray;
					infantplus.TextColor = Color.Red;
				}
				else
				{
					infantminus.TextColor = Color.Red;
					infantcountval -= 1;
					infantcount.Text = Convert.ToString(infantcountval);
					if (infantcountval < 9)
					{
						infantplus.TextColor = Color.Red;
					}
					else
					{
						infantplus.TextColor = Color.Gray;
					}



				}
			};
			infantminus.GestureRecognizers.Add(infantminustap);
		}
		public TravellerDetail(EventHandler<SelectedItemChangedEventArgs> OnSelectedCity)
		{
			this.OnSelectedCity = OnSelectedCity;
		}

		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			MainModel mainModel = new MainModel();
			mainModel.adult = adultcount.Text;
			mainModel.child = childcount.Text;
			mainModel.infant = infantcount.Text;
			mainModel.cabinclass = cabinclassval;
			mainModel.Totalcount = totalcount+1;
			OnSelectedCity(mainModel, null);
			await Navigation.PopPopupAsync();
		}

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var item = e.Item as RadioModel;

			cabinclassval = item.Title;
			if (item == null)
				return;

			else
			{
				foreach (var group in cabinclassitmes)
				{
					string storedname = null;
					if (group.Title == item.Title)
					{
						foreach (var s in cabinclassitmes)
						{
							if (s.IsSelected)
							{
								storedname = s.Title;
							}
							s.IsSelected = false;

						}
						item.IsSelected = true;



					}
				}
			}
		}
	}
}
