using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using FLightsApp.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel;

namespace FLightsApp.Pages
{
	public partial class MainTripPage : ContentPage
	{
		int flag = 0, flagg = 0, timeflag=0, travellerflag = 0, TotalPassengerCount=0;
		string adultval = null, childval = null,infantval=null, from =null, to= null, fromshort= null, toshort= null;
		List<MainModel> arrivalitems = new List<MainModel>();
		int monthnumber = 0;
		List<RadioModel> radioflowitems = new List<RadioModel>();
		List<RadioModel> radioflowitems1 = new List<RadioModel>();
		List<RadioModel> radioflowitems2 = new List<RadioModel>();
        string name,userType;
        int userID;
        public MainTripPage(string name,string userType,int userid)
		{
			InitializeComponent();





            this.name = name;
            this.userType = userType;
            this.userID = userid;


            //new user name show welcome //

            username.Text = "Welcome : " + name;

            //
			var roundstacktap = new TapGestureRecognizer();
		
			roundadult.SelectedItem = "1";
			roundcabin.SelectedItem = "Economy";
           
			from = roundfromfullname.Text;
			to = roundtofullname.Text;
			fromshort = roundfromShortName.Text;
			toshort = roundtoShortName.Text;
           
           
          
			var moretap = new TapGestureRecognizer();
			moretap.Tapped += async (s, e) =>
            {
				await Navigation.PushPopupAsync(new MorePopUpPage());
            };
			morepop.GestureRecognizers.Add(moretap);

			var adulttap = new TapGestureRecognizer();
			adulttap.Tapped += async (s, e) =>
			{
				travellerflag = 1;

				TravellerDetail page = new TravellerDetail();
				page.OnSelectedCity += Page_OnSelected;
				await Navigation.PushPopupAsync(page);
			};
			onewayadultcount.GestureRecognizers.Add(adulttap);
			var cabintap = new TapGestureRecognizer();
			cabintap.Tapped += async (s, e) =>
			{
				travellerflag = 1;
				TravellerDetail page = new TravellerDetail();
				page.OnSelectedCity += Page_OnSelected;
				await Navigation.PushPopupAsync(page);
			};
			onewaycabinclass.GestureRecognizers.Add(cabintap);
			var roundadulttap = new TapGestureRecognizer();
			roundadulttap.Tapped += async (s, e) =>
			{
				travellerflag = 0;
				TravellerDetail page = new TravellerDetail();
				page.OnSelectedCity += Page_OnSelected;
				await Navigation.PushPopupAsync(page);
			};
			roundadultcount.GestureRecognizers.Add(roundadulttap);
			var roundcabintap = new TapGestureRecognizer();
			roundcabintap.Tapped += async (s, e) =>
			{
				travellerflag = 0;
				TravellerDetail page = new TravellerDetail();
				page.OnSelectedCity += Page_OnSelected;
				await Navigation.PushPopupAsync(page);
			};
			roundcabinclass.GestureRecognizers.Add(roundcabintap);
			radioflowitems = new List<RadioModel>{
			 new RadioModel {
				   Title = "ARRIVAL"
					

			 } ,
				new RadioModel {
				   Title = "DEPARTURE",
                    IsSelected = false 
				}
		  };
			radioflowitems1 = new List<RadioModel>{
			 new RadioModel {
				   Title = "ARRIVAL"

			 } ,
				new RadioModel {
				   Title = "DEPARTURE",
                    IsSelected = false
				}
			};
			radioflowitems2 = new List<RadioModel>{
			 new RadioModel {
				   Title = "RETURN ARRIVAL"

			 } ,
				new RadioModel {
                    Title = "RETURN DEPARTURE",
                    IsSelected = false
				}
		  };

			radioflow.FlowItemsSource = radioflowitems;
			radioflowreturn.FlowItemsSource = radioflowitems1;
			radioflowreturntime.FlowItemsSource = radioflowitems2;
			var onewaystopfiltertap = new TapGestureRecognizer();
			onewaystopfiltertap.Tapped += async (s, e) =>
			{
				flag = 1;
				ArrivalFiltrationPopUp page = new ArrivalFiltrationPopUp();
				page.OnSelectedCity += Page_OnSelectedCity;
				await Navigation.PushPopupAsync(page);
			};
			fromstack.GestureRecognizers.Add(onewaystopfiltertap);


			var tostacktap = new TapGestureRecognizer();
			tostacktap.Tapped += async (s, e) =>
			{
				flag = 2;
				ArrivalFiltrationPopUp page = new ArrivalFiltrationPopUp();
				page.OnSelectedCity += Page_OnSelectedCity;
				await Navigation.PushPopupAsync(page);
			};
			tostack.GestureRecognizers.Add(tostacktap);


			var roundfromtap = new TapGestureRecognizer();
			roundfromtap.Tapped += async (s, e) =>
			{
				flag = 1;

				ArrivalFiltrationPopUp page = new ArrivalFiltrationPopUp();
				page.OnSelectedCity += Page_OnSelectedCity;
				await Navigation.PushPopupAsync(page);
			};
			roundfrom.GestureRecognizers.Add(roundfromtap);
			var roundtotap = new TapGestureRecognizer();
			roundtotap.Tapped += async (s, e) =>
			{
				flag = 2;

				ArrivalFiltrationPopUp page = new ArrivalFiltrationPopUp();
				page.OnSelectedCity += Page_OnSelectedCity;
				await Navigation.PushPopupAsync(page);
			};
			roundtostack.GestureRecognizers.Add(roundtotap);

			roundstacktap.Tapped += (s, e) =>
			{
				onewaypage.IsVisible = false;
				roundtrippage.IsVisible = true;
				multipage.IsVisible = false;
				roundimage.IsVisible = true;
				roundtrip.BackgroundColor = Color.Red;
				roundstack.BackgroundColor = Color.Red;
				roundtext.TextColor = Color.White;
				oneway.BackgroundColor = Color.White;
				onewaystack.BackgroundColor = Color.White;
				onewayimage.IsVisible = false;
				onewaytext.TextColor = Color.Gray;
				multicity.BackgroundColor = Color.White;
				multistack.BackgroundColor = Color.White;
				multiimage.IsVisible = false;
				multitext.TextColor = Color.Gray;
			};
			roundtrip.GestureRecognizers.Add(roundstacktap);
			var multistacktap = new TapGestureRecognizer();
			multistacktap.Tapped += (s, e) =>
			{
				onewaypage.IsVisible = false;
				roundtrippage.IsVisible = false;
				multipage.IsVisible = true;
				multiimage.IsVisible = true;
				multicity.BackgroundColor = Color.Red;
				multistack.BackgroundColor = Color.Red;
				multitext.TextColor = Color.White;
				oneway.BackgroundColor = Color.White;
				onewaystack.BackgroundColor = Color.White;
				onewayimage.IsVisible = false;
				onewaytext.TextColor = Color.Gray;
				roundtrip.BackgroundColor = Color.White;
				roundstack.BackgroundColor = Color.White;
				roundimage.IsVisible = false;
				roundtext.TextColor = Color.Gray;
			};
			multicity.GestureRecognizers.Add(multistacktap);
			var onewaytap = new TapGestureRecognizer();
			onewaytap.Tapped += (s, e) =>
			{
				onewaypage.IsVisible = true;
				roundtrippage.IsVisible = false;
				multipage.IsVisible = false;
				onewayimage.IsVisible = true;
				multicity.BackgroundColor = Color.White;
				multistack.BackgroundColor = Color.White;
				multiimage.IsVisible = false;
				multitext.TextColor = Color.Gray;
				oneway.BackgroundColor = Color.Red;
				onewaystack.BackgroundColor = Color.Red;
				onewaytext.TextColor = Color.White;
				roundtrip.BackgroundColor = Color.White;
				roundstack.BackgroundColor = Color.White;
				roundimage.IsVisible = false;
				roundtext.TextColor = Color.Gray;
			};
			oneway.GestureRecognizers.Add(onewaytap);


			arrivalitems = new List<MainModel>
			{
				new MainModel{
					title = "All",
					isChecked = false

				},
				new MainModel{
					title = "1 stop",
					isChecked = false
				},
				new MainModel{
					title = "2+ stop",
					isChecked = false
				}
			};
			filteritems.ItemsSource = arrivalitems;

			string date1 = null;

			var datetap1 = new TapGestureRecognizer();
			datetap1.Tapped += (s, e) =>
			{
				datepicker.Focus();
				flagg = 1;
			};
			rounddatelabel.GestureRecognizers.Add(datetap1);

			var datetap2 = new TapGestureRecognizer();
			datetap2.Tapped += (s, e) =>
			{
				datepicker2.Focus();
				flagg = 2;
			};
			roundreturndatelabel.GestureRecognizers.Add(datetap2);

			var datetap = new TapGestureRecognizer();
			datetap.Tapped += (s, e) =>
			{
				datepicker.Focus();
			};
			labrl.GestureRecognizers.Add(datetap);

			monthnumber = datepicker.Date.Month;
			date.Text = Convert.ToString(datepicker.Date.Day);
			year.Text = Convert.ToString(datepicker.Date.Year);

			day.Text = Convert.ToString(datepicker.Date.DayOfWeek);
			rounddate.Text = Convert.ToString(datepicker.Date.Day);
			roundyear.Text = Convert.ToString(datepicker.Date.Year);
			roundday.Text = Convert.ToString(datepicker.Date.DayOfWeek);
			roundreturndate.Text = Convert.ToString(datepicker.Date.Day);
			roundreturnyear.Text = Convert.ToString(datepicker.Date.Year);
			roundreturnday.Text = Convert.ToString(datepicker.Date.DayOfWeek);

			switch (monthnumber)
			{
				case 1:
					month.Text = "Jan";
					roundmonth.Text = "Jan";
					roundreturnmonth.Text = "Jan";
					break;
				case 2:
					month.Text = "Feb";
					roundmonth.Text = "Feb"; 
					roundreturnmonth.Text = "Feb";
					break;
				case 3:
					month.Text = "Mar";
					roundmonth.Text = "Mar";
					roundreturnmonth.Text = "Mar";

					break;
				case 4:
					month.Text = "Apr";
					roundmonth.Text = "Apr";
					roundreturnmonth.Text = "Apr";
					break;
				case 5:
					month.Text = "May";
					roundmonth.Text = "May";
					roundreturnmonth.Text = "May";
					break;
				case 6:
					month.Text = "Jul";
					roundmonth.Text = "Jul"; roundreturnmonth.Text = "Jul";
					break;
				case 7:
					month.Text = "Jun";
					roundmonth.Text = "Jun";
					roundreturnmonth.Text = "Jun";
					break;
				case 8:
					month.Text = "Aug";
					roundmonth.Text = "Aug";
					roundreturnmonth.Text = "Aug";
					break;
				case 9:
					month.Text = "Sep";
					roundmonth.Text = "Sep";
					roundreturnmonth.Text = "Sep";
					break;
				case 10:
					month.Text = "Oct";
					roundmonth.Text = "Oct";
					roundreturnmonth.Text = "Oct";
					break;
				case 11:
					month.Text = "Nov";
					roundmonth.Text = "Nov";
					roundreturnmonth.Text = "Nov";
					break;
				case 12:
					month.Text = "Dec";
					roundmonth.Text = "Dec";
					roundreturnmonth.Text = "Dec";
					break;
			}



            //new code for hide previous dates 
            datepicker.MinimumDate = DateTime.Now;

		}




        private void addNoteToDiary_DateSelected(object sender, DateChangedEventArgs e)
		{
			monthnumber = datepicker.Date.Month;
			date.Text = Convert.ToString(datepicker.Date.Day);
			year.Text = Convert.ToString(datepicker.Date.Year);
			day.Text = Convert.ToString(datepicker.Date.DayOfWeek);

			rounddate.Text = Convert.ToString(datepicker.Date.Day);
            roundyear.Text = Convert.ToString(datepicker.Date.Year);
            roundday.Text = Convert.ToString(datepicker.Date.DayOfWeek);
			switch (monthnumber)
			{
				case 1:
					month.Text = "Jan";

					break;
				case 2:
					month.Text = "Feb";

					break;
				case 3:
					month.Text = "Mar";
                	break;
				case 4:
					month.Text = "Apr";

					break;
				case 5:
					month.Text = "May";

					break;
				case 6:
					month.Text = "Jul";

					break;
				case 7:
					month.Text = "Jun";

					break;
				case 8:
					month.Text = "Aug";

					break;
				case 9:
					month.Text = "Sep";

					break;
				case 10:
					month.Text = "Oct";

					break;
				case 11:
					month.Text = "Nov";

					break;
				case 12:
					month.Text = "Dec";

					break;
			}

            switch (monthnumber)
            {
                case 1:
                    roundmonth.Text = "Jan";
                    break;
                case 2:
                    roundmonth.Text = "Feb";
                    break;
                case 3:
                    roundmonth.Text = "Mar";

                    break;
                case 4:
                    roundmonth.Text = "Apr";
                    break;
                case 5:
                    roundmonth.Text = "May";
                    break;
                case 6:
                    roundmonth.Text = "Jul";
                    break;
                case 7:
                    roundmonth.Text = "Jun";
                    break;
                case 8:
                    roundmonth.Text = "Aug";
                    break;
                case 9:

                    roundmonth.Text = "Sep";

                    break;
                case 10:

                    roundmonth.Text = "Oct";

                    break;
                case 11:

                    roundmonth.Text = "Nov";

                    break;
                case 12:

                    roundmonth.Text = "Dec";

                    break;
            }
		}

		private void addNoteToDiary_DateSelected1(object sender, DateChangedEventArgs e)
		{
			monthnumber = datepicker.Date.Month;
            
			rounddate.Text = Convert.ToString(datepicker.Date.Day);
			roundyear.Text = Convert.ToString(datepicker.Date.Year);
			roundday.Text = Convert.ToString(datepicker.Date.DayOfWeek);


			switch (monthnumber)
			{
				case 1:
					roundmonth.Text = "Jan";
					break;
				case 2:
					roundmonth.Text = "Feb";
					break;
				case 3:
					roundmonth.Text = "Mar";

					break;
				case 4:
					roundmonth.Text = "Apr";
					break;
				case 5:
					roundmonth.Text = "May";
					break;
				case 6:
					roundmonth.Text = "Jul";
					break;
				case 7:
					roundmonth.Text = "Jun";
					break;
				case 8:
					roundmonth.Text = "Aug";
					break;
				case 9:

					roundmonth.Text = "Sep";

					break;
				case 10:

					roundmonth.Text = "Oct";

					break;
				case 11:

					roundmonth.Text = "Nov";

					break;
				case 12:

					roundmonth.Text = "Dec";

					break;
			}
		}
		private void addNoteToDiary_DateSelected2(object sender, DateChangedEventArgs e)
		{
			monthnumber = datepicker2.Date.Month;

			roundreturndate.Text = Convert.ToString(datepicker2.Date.Day);
			roundreturnyear.Text = Convert.ToString(datepicker2.Date.Year);
			roundreturnday.Text = Convert.ToString(datepicker2.Date.DayOfWeek);

			switch (monthnumber)
			{
				case 1:

					roundreturnmonth.Text = "Jan";
					break;
				case 2:
					roundreturnmonth.Text = "Feb";
					break;
				case 3:

					roundreturnmonth.Text = "Mar";

					break;
				case 4:

					roundreturnmonth.Text = "Apr";
					break;
				case 5:

					roundreturnmonth.Text = "May";
					break;
				case 6:
					roundreturnmonth.Text = "Jul";
					break;
				case 7:

					roundreturnmonth.Text = "Jun";
					break;
				case 8:

					roundreturnmonth.Text = "Aug";
					break;
				case 9:

					roundreturnmonth.Text = "Sep";
					break;
				case 10:

					roundreturnmonth.Text = "Oct";
					break;
				case 11:

					roundreturnmonth.Text = "Nov";
					break;
				case 12:

					roundreturnmonth.Text = "Dec";
					break;
			}
		}


		void Page_OnSelectedCity(object sender, SelectedItemChangedEventArgs e)
		{
			var stop = (MainModel)(sender);
			if (flag == 1)
			{
				fromentryfullName.Text = stop.AirportName;
				fromentryShortName.Text = stop.AirportCode;
				roundfromfullname.Text = stop.AirportName;
                roundfromShortName.Text = stop.AirportCode;
				from = stop.AirportName;
				fromshort = stop.AirportCode;
			}
			else if (flag == 2)
			{
				TofullName.Text = stop.AirportName;
				ToShortName.Text = stop.AirportCode;
				roundtofullname.Text = stop.AirportName;
                roundtoShortName.Text = stop.AirportCode;
				to = stop.AirportName;
				toshort = stop.AirportCode;
			}
			else if (flag == 3)
			{
				roundfromfullname.Text = stop.AirportName;
				roundfromShortName.Text = stop.AirportCode;
			}
			else
			{
				roundtofullname.Text = stop.AirportName;
				roundtoShortName.Text = stop.AirportCode;
			}

		}



		void Page_OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var stop = (MainModel)(sender);
			TotalPassengerCount = stop.Totalcount;
			adultval = stop.adult;
			childval = stop.child;
			infantval = stop.infant;
			Application.Current.Properties["count"] = TotalPassengerCount;
			if (string.IsNullOrEmpty(stop.cabinclass))
			{
				onewaycabinclass.Text = "Economy";
			}
			else
			{
				onewaycabinclass.Text = stop.cabinclass;
			}

			if (childval == "0" && infantval == "0" && adultval != "1")
			{
				onewayadultcount.Text = adultval + " Adults";
			}
			else if (childval != "0" && infantval == "0" && adultval != "1")
			{
				if (childval == "1")
				{
					onewayadultcount.Text = adultval + " Adults, " + childval + " Child";
				}
				else
				{
					onewayadultcount.Text = adultval + " Adults, " + childval + " Children";
				}

			}
			else if (childval == "0" && infantval != "0" && adultval != "1")
			{
				if (infantval == "1")
				{
					onewayadultcount.Text = adultval + " Adults, " + infantval + " Infant";
				}
				else
				{
					onewayadultcount.Text = adultval + " Adults, " + infantval + " Infants";
				}

			}
			else if (childval != "0" && infantval != "0" && adultval != "1")
			{
				if (infantval == "1" && childval == "1")
				{
					onewayadultcount.Text = adultval + " Adults, " + childval + " Child, " + infantval + " Infant";
				}
				else if (infantval == "1" && childval != "1")
				{
					onewayadultcount.Text = adultval + " Adults, " + childval + " Children, " + infantval + " Infant";
				}
				else if (infantval != "1" && childval == "1")
				{
					onewayadultcount.Text = adultval + " Adults, " + childval + " Child, " + infantval + " Infants";
				}
				else
				{
					onewayadultcount.Text = adultval + " Adults, " + childval + " Children, " + infantval + " Infants";
				}

			}
			else if (childval == "0" && infantval == "0" && adultval == "1")
			{
				onewayadultcount.Text = adultval + " Adult";
			}
			else if (childval != "0" && infantval == "0" && adultval == "1")
			{
				if (childval == "1")
				{
					onewayadultcount.Text = adultval + " Adult, " + childval + " Child";
				}
				else
				{
					onewayadultcount.Text = adultval + " Adult, " + childval + " Children";
				}

			}
			else if (childval == "0" && infantval != "0" && adultval == "1")
			{
				if (infantval == "1")
				{
					onewayadultcount.Text = adultval + " Adult, " + infantval + " Infant";
				}
				else
				{
					onewayadultcount.Text = adultval + " Adult, " + infantval + " Infants";
				}

			}
			else if (childval != "0" && infantval != "0" && adultval == "1")
			{
				if (infantval == "1" && childval == "1")
				{
					onewayadultcount.Text = adultval + " Adult, " + childval + " Child, " + infantval + "Infant";
				}
				else if (infantval == "1" && childval != "1")
				{
					onewayadultcount.Text = adultval + " Adult, " + childval + " Children, " + infantval + "Infant";
				}
				else if (infantval != "1" && childval == "1")
				{
					onewayadultcount.Text = adultval + " Adult, " + childval + " Child, " + infantval + "Infants";
				}
				else
				{
					onewayadultcount.Text = adultval + " Adult, " + childval + "Children, " + infantval + "Infants";
				}

			}

			if (string.IsNullOrEmpty(stop.cabinclass))
			{
				roundcabinclass.Text = "Economy";
			}
			else
			{
				roundcabinclass.Text = stop.cabinclass;
			}

			if (childval == "0" && infantval == "0" && adultval != "1")
			{
				roundadultcount.Text = adultval + " Adults";
			}
			else if (childval != "0" && infantval == "0" && adultval != "1")
			{
				if (childval == "1")
				{
					roundadultcount.Text = adultval + " Adults, " + childval + " Child";
				}
				else
				{
					roundadultcount.Text = adultval + " Adults, " + childval + " Children";
				}

			}
			else if (childval == "0" && infantval != "0" && adultval != "1")
			{
				if (infantval == "1")
				{
					roundadultcount.Text = adultval + " Adults, " + infantval + " Infant";
				}
				else
				{
					roundadultcount.Text = adultval + " Adults, " + infantval + " Infants";
				}

			}
			else if (childval != "0" && infantval != "0" && adultval != "1")
			{
				if (infantval == "1" && childval == "1")
				{
					roundadultcount.Text = adultval + " Adults, " + childval + " Child, " + infantval + " Infant";
				}
				else if (infantval == "1" && childval != "1")
				{
					roundadultcount.Text = adultval + " Adults, " + childval + " Children, " + infantval + " Infant";
				}
				else if (infantval != "1" && childval == "1")
				{
					roundadultcount.Text = adultval + " Adults, " + childval + " Child, " + infantval + " Infants";
				}
				else
				{
					roundadultcount.Text = adultval + " Adults, " + childval + " Children, " + infantval + " Infants";
				}

			}
			else if (childval == "0" && infantval == "0" && adultval == "1")
			{
				roundadultcount.Text = adultval + " Adult";
			}
			else if (childval != "0" && infantval == "0" && adultval == "1")
			{
				if (childval == "1")
				{
					roundadultcount.Text = adultval + " Adult, " + childval + " Child";
				}
				else
				{
					roundadultcount.Text = adultval + " Adult, " + childval + " Children";
				}

			}
			else if (childval == "0" && infantval != "0" && adultval == "1")
			{
				if (infantval == "1")
				{
					roundadultcount.Text = adultval + " Adult, " + infantval + " Infant";
				}
				else
				{
					roundadultcount.Text = adultval + " Adult, " + infantval + " Infants";
				}

			}
			else if (childval != "0" && infantval != "0" && adultval == "1")
			{
				if (infantval == "1" && childval == "1")
				{
					roundadultcount.Text = adultval + " Adult, " + childval + " Child, " + infantval + " Infant";
				}
				else if (infantval == "1" && childval != "1")
				{
					roundadultcount.Text = adultval + " Adult, " + childval + " Children, " + infantval + " Infant";
				}
				else if (infantval != "1" && childval == "1")
				{
					roundadultcount.Text = adultval + " Adult, " + childval + " Child, " + infantval + " Infants";
				}
				else
				{
					roundadultcount.Text = adultval + " Adult, " + childval + " Children, " + infantval + " Infants";
				}

			}

		}


		void Handle_PropertyChanged(object sender, SelectedItemChangedEventArgs e)
		{
			var data = (XLabs.Forms.Controls.CheckBox)sender;

			string s = data.DefaultText;

			filteritems.IsVisible = false;
			if (data.DefaultText == "All" && data.Checked)
			{
				for (int i = 0; i < arrivalitems.Count; i++)
				{
					arrivalitems[i].isChecked = true;

				}
				filteritems.ItemsSource = null;
				filteritems.ItemsSource = arrivalitems;


				onewaystopfilter.Text = "All";
			}
			else if (data.DefaultText == "All" && !data.Checked)
			{
				for (int i = 0; i < arrivalitems.Count; i++)
				{
					arrivalitems[i].isChecked = false;

				}
				filteritems.ItemsSource = null;
				filteritems.ItemsSource = arrivalitems;

				onewaystopfilter.Text = "Select here";
			}
			else
			{
				if (data.Checked)
				{

					if (data.DefaultText == "1 stop" || data.DefaultText == "2+ stop")
					{
						for (int i = 0; i < arrivalitems.Count; i++)
						{
							if (arrivalitems[i].title != data.DefaultText && arrivalitems[i].title != "All" && arrivalitems[i].isChecked)
							{
								for (int j = 0; j < arrivalitems.Count; j++)
								{
									arrivalitems[j].isChecked = true;

									onewaystopfilter.Text = "All";

								}
								filteritems.ItemsSource = null;
								filteritems.ItemsSource = arrivalitems;
							}
							else
							{
								onewaystopfilter.Text = data.DefaultText;
							}


						}
					}



				}
				else
				{
					string previousval = null;
					for (int i = 0; i < arrivalitems.Count; i++)
					{
						if (arrivalitems[i].title == "All" && arrivalitems[i].isChecked)
						{
							arrivalitems[i].isChecked = false;
							filteritems.ItemsSource = null;
							filteritems.ItemsSource = arrivalitems;
						}


						else if (arrivalitems[i].title != data.DefaultText && arrivalitems[i].title != "All" && arrivalitems[i].isChecked)
						{

							previousval = arrivalitems[i].title;
							//onewaystopfilter.Text = onewaystopfilter.Text.Replace(data.DefaultText, "");
							onewaystopfilter.Text = previousval;
						}
						else
						{

							onewaystopfilter.Text = "Select here";
						}

					}

				}
			}

		}
       


		async void Search_Clicked(object sender, System.EventArgs e)
		{
			MainModel mainModel = new MainModel();
          
            // user type new code 
            mainModel.usersType = userType;
            mainModel.usersId = userID;
            // emd 

			TimeSpan time = onewaytime.Time;
			if(timeflag==0){
				mainModel.onewayarrivaltime = Convert.ToString(time);
				mainModel.onewaydeparturetime = null;
			}
			else{
				mainModel.onewaydeparturetime = Convert.ToString(time);
				mainModel.onewayarrivaltime = null;
			}
			//string fulltime = time.ToString("HH:mm:ss");
			if (fromentryfullName.Text != "" && TofullName.Text != "")
			{
				if (fromentryfullName.Text == TofullName.Text)
				{
                    
					UserDialogs.Instance.Alert("Source and destination cannot be same", "", "OK");

				}
				else
				{
				    mainModel.FromOneway = fromentryShortName.Text;
                 	mainModel.Cabinclass = onewaycabinclass.Text;
					mainModel.SpecialFare = 0;
					if(adultval==null){
						mainModel.NoOfAdult = 1;
					}else{
						mainModel.NoOfAdult = Convert.ToInt32(adultval);
					}
                 	if(string.IsNullOrEmpty(childval)){
						mainModel.NoOfChild = 0;	
					}else{
						mainModel.NoOfChild = Convert.ToInt32(childval);
					}
					if(string.IsNullOrEmpty(infantval)){
						mainModel.NoOfInfant = 0;
					}else{
						mainModel.NoOfInfant = Convert.ToInt32(infantval);
                  	}
					mainModel.ToOneway = ToShortName.Text;
					if (mainModel.Cabinclass == "Economy")
					{
						mainModel.FlightClassrequest = "Y";
					}
					else if (mainModel.Cabinclass =="Premium Economy"){
						mainModel.FlightClassrequest = "C";
					}else{
						mainModel.FlightClassrequest = "F";
					}
					mainModel.DateOneway = date.Text + month.Text + year.Text + day.Text;
					DateTime dateTime = new DateTime();
					dateTime = Convert.ToDateTime(mainModel.DateOneway);
					string orgdate = mainModel.DateOneway;
					mainModel.DateOneway = dateTime.ToString("dd/MM/yyyy");

			
					mainModel.ReturnDateRound = roundreturndate.Text + roundreturnmonth.Text + roundreturnyear.Text + roundreturnday.Text;
					dateTime = Convert.ToDateTime(mainModel.ReturnDateRound);
					//mainModel.ReturnDateRound = dateTime.ToString("dd/MM/yyyy");
					mainModel.ReturnDateRound = "";
					mainModel.TripType = 1;

					await Navigation.PushAsync(new FlightsListPage(mainModel, orgdate, null));
				}
			}
			else if (fromentryfullName.Text == "" && TofullName.Text != "")
			{
				UserDialogs.Instance.Alert("Please Select Source", "", "OK");
				Console.WriteLine("Please Select Source");
			}
			else if (fromentryfullName.Text != "" && TofullName.Text == "")
			{
				UserDialogs.Instance.Alert("Please Select destination", "", "OK");
				Console.WriteLine("Please Select destination");
			}
			else
			{
				UserDialogs.Instance.Alert("Please Select  source and destination", "", "OK");
				Console.WriteLine("please select source and destination");
			}

		}
		/*async void Multi_SearchClicked(object sender, System.EventArgs e)
			{
				if (multifrom.SelectedIndex != -1 && multito.SelectedIndex != -1)
				{
					if (multifrom.Items[multifrom.SelectedIndex] == multito.Items[multito.SelectedIndex])
					{
						UserDialogs.Instance.Alert("Source and destination cannot be same", "", "OK");
					}else{
						await Navigation.PushModalAsync(new FlightsListPage());
					}

				}
				else if (multifrom.SelectedIndex == -1 && multito.SelectedIndex != -1)
				{
					UserDialogs.Instance.Alert("Please Select Source", "", "OK");
					Console.WriteLine("Please Select Source");
				}
				else if (multifrom.SelectedIndex != -1 && multito.SelectedIndex == -1)
				{
					UserDialogs.Instance.Alert("Please Select destination", "", "OK");
					Console.WriteLine("Please Select destination");
				}
				else
				{
					UserDialogs.Instance.Alert("Please Select  source and destination", "", "OK");
					Console.WriteLine("please select source and destination");

				}
			}*/

		async void Round_Search_Clicked(object sender, System.EventArgs e)
		{
			MainModel mainModel = new MainModel();
			TimeSpan time = roundtime.Time;
			TimeSpan time1 = roundreturntime.Time;
			if(timeflag==2){
				mainModel.roundreturnarrivaltime = Convert.ToString(time1);
			}else if(timeflag==3){
				mainModel.roundreturndeparturetime = Convert.ToString(time1);
			}else if(timeflag == 4){
				mainModel.roundarrivaltime = Convert.ToString(time);
			}else{
				mainModel.rounddeparturetime = Convert.ToString(time);
			}
			if (roundfromfullname.Text != "" && roundtofullname.Text != "")
			{
				if (roundfromfullname.Text == roundtofullname.Text)
				{
					UserDialogs.Instance.Alert("Source and destination cannot be same", "", "OK");
				}
				else
				{
					
					mainModel.FromOneway = roundfromShortName.Text;


					mainModel.Cabinclass = roundcabinclass.Text;
                    mainModel.SpecialFare = 0;
                    if (adultval == null)
                    {
                        mainModel.NoOfAdult = 1;
                    }
                    else
                    {
                        mainModel.NoOfAdult = Convert.ToInt32(adultval);
                    }

                    if (string.IsNullOrEmpty(childval))
                    {
                        mainModel.NoOfChild = 0;
                    }
                    else
                    {
                        mainModel.NoOfChild = Convert.ToInt32(childval);
                    }
                    if (string.IsNullOrEmpty(infantval))
                    {
                        mainModel.NoOfInfant = 0;
                    }
                    else
                    {
                        mainModel.NoOfInfant = Convert.ToInt32(infantval);

                    }
					mainModel.ToOneway = roundtoShortName.Text;
                    if (mainModel.Cabinclass == "Economy")
                    {
                        mainModel.FlightClassrequest = "Y";
                    }
                    else if (mainModel.Cabinclass == "Premium Economy")
                    {
                        mainModel.FlightClassrequest = "C";
                    }
                    else
                    {
                        mainModel.FlightClassrequest = "F";
                    }
                    mainModel.DateOneway = date.Text + month.Text + year.Text + day.Text;
                    DateTime dateTime = new DateTime();
                    dateTime = Convert.ToDateTime(mainModel.DateOneway);
                    string orgdate = mainModel.DateOneway;

                    mainModel.DateOneway = dateTime.ToString("dd/MM/yyyy");


                    mainModel.ReturnDateRound = roundreturndate.Text + roundreturnmonth.Text + roundreturnyear.Text + roundreturnday.Text;
                    dateTime = Convert.ToDateTime(mainModel.ReturnDateRound);
					string returnorgdate = mainModel.ReturnDateRound;
                    mainModel.ReturnDateRound = dateTime.ToString("dd/MM/yyyy");
                    //mainModel.ReturnDateRound = "";
                    mainModel.TripType = 2;
					await Navigation.PushAsync(new RoundFlightListPage(mainModel, orgdate, returnorgdate));
				}
			}
			else if (roundfromfullname.Text == "" && roundtofullname.Text != "")
			{
				UserDialogs.Instance.Alert("Please Select Source", "", "OK");
				Console.WriteLine("Please Select Source");
			}
			else if (roundfromfullname.Text != "" && roundtofullname.Text == "")
			{
				UserDialogs.Instance.Alert("Please Select destination", "", "OK");
				Console.WriteLine("Please Select destination");
			}
			else
			{
				UserDialogs.Instance.Alert("Please Select  source and destination", "", "OK");
				Console.WriteLine("please select source and destination");

			}
		}
         
		void Handle_FlowItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{

			var item = e.Item as RadioModel;
			string resultstring1 = null; 			if (item.Title == "ARRIVAL")
			{
				timeflag = 0;
				arrivallabel.IsVisible = true;
				departurelabel.IsVisible = false;
      	    }
			else
			{
				timeflag = 1;
				arrivallabel.IsVisible = false;
				departurelabel.IsVisible = true;
			}
			//string resultString = Regex.Match(item.Name, @"\d+").Value;
			if (item == null)
				return;

			else
			{
				foreach (var group in radioflowitems)
				{
					string storedname = null;
					if (group.Title == item.Title)
					{
						foreach (var s in radioflowitems)
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
		} 		void returntime_Handle_FlowItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{

			var item = e.Item as RadioModel;

			if (item.Title == "RETURN ARRIVAL")
			{
				timeflag = 2;
				returntimearrivallabel.IsVisible = true;
				returntimedeparturelabel.IsVisible = false;
			}
			else
			{
				timeflag = 3;
				returntimedeparturelabel.IsVisible = true;
				returntimearrivallabel.IsVisible = false;
			}
			//string resultString = Regex.Match(item.Name, @"\d+").Value;
			if (item == null)
				return;

			else
			{
				foreach (var group in radioflowitems2)
				{
					string storedname = null;
					if (group.Title == item.Title)
					{
						foreach (var s in radioflowitems2)
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
		void return_Handle_FlowItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{

			var item = e.Item as RadioModel;

			if (item.Title == "ARRIVAL")
			{
				timeflag = 4;
				returnarrivallabel.IsVisible = true;
				returndeparturelabel.IsVisible = false;

			}
			else
			{
				timeflag = 5;
				returnarrivallabel.IsVisible = false;
				returndeparturelabel.IsVisible = true;
			}
			if (item == null)
				return;

			else
			{
				foreach (var group in radioflowitems1)
				{
					string storedname = null;
					if (group.Title == item.Title)
					{
						foreach (var s in radioflowitems1)
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



	async	void  OnImageNameTapped(object sender, EventArgs args)
        {
            try
            {
				if(imageflip.Rotation == 360){
					await imageflip.RotateTo(-360, 150); 
					roundfromfullname.Text = from;
					roundfromShortName.Text = fromshort;
					fromentryfullName.Text = from;
					fromentryShortName.Text = fromshort;
					TofullName.Text = to;
					ToShortName.Text = toshort;
                    roundtofullname.Text = to;
					roundtoShortName.Text = toshort;
				}else{
					await imageflip.RotateTo(360, 150);
					roundfromfullname.Text = to;
					roundfromShortName.Text = toshort;
					roundtofullname.Text = from;
					roundtoShortName.Text = fromshort;
                    

					fromentryfullName.Text = to;
					fromentryShortName.Text = toshort;
					TofullName.Text = from;
					ToShortName.Text = fromshort;
                    
				}
			
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		async void OnImageNameTapped1(object sender, EventArgs args)
        {
            try
            {
				if (imageflip1.Rotation == 360)
                {
                    await imageflip1.RotateTo(-360, 150);
                    roundfromfullname.Text = from;
                    roundfromShortName.Text = fromshort;
                    fromentryfullName.Text = from;
                    fromentryShortName.Text = fromshort;
                    TofullName.Text = to;
                    ToShortName.Text = toshort;
                    roundtofullname.Text = to;
                    roundtoShortName.Text = toshort;
                }
                else
                {
                    await imageflip1.RotateTo(360, 150);
                    roundfromfullname.Text = to;
                    roundfromShortName.Text = toshort;
                    roundtofullname.Text = from;
                    roundtoShortName.Text = fromshort;


                    fromentryfullName.Text = to;
                    fromentryShortName.Text = toshort;
                    TofullName.Text = from;
                    ToShortName.Text = fromshort;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
}
}
