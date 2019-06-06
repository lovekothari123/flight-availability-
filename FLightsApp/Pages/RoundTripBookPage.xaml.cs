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
    public partial class RoundTripBookPage : ContentPage
    {
		int listcount = 0, recurringflag = 0;
        string trackamount = null, mainclass = null, tracknumber1 = null, tracknumber2 = null;
        string totalticketAmount = null, flightNo1 = null, flightNo2 = null, trackvalue = null;
		int mealamount = 0, baggageamount = 0, totalprice = 0, NetAmount = 0;
        List<FlightDetail> flightDetailsitems = new List<FlightDetail>();
        List<FlightDetail> GoflightDetailsitems = new List<FlightDetail>();
        List<FlightDetail> BackflightDetailsitems = new List<FlightDetail>();
        BookingDetailsData bookingDetails = new BookingDetailsData();
        AdditionalService additionalServices = new AdditionalService();
        List<BookingPassengerDetails> passengerDetails = new List<BookingPassengerDetails>();
        MainModel modelproperty = new MainModel();
        BookTicketResponse bookTicketResponse = new BookTicketResponse();
        List<MainModel> flightlistitems = new List<MainModel>();
        List<MainFareDetail> mainFareDetails = new List<MainFareDetail>();
        List<MainModel> Backflightlistitems = new List<MainModel>();
        List<MainFareDetail> BackmainFareDetails = new List<MainFareDetail>();
        List<FareDetail> fareDetails = new List<FareDetail>();
		public RoundTripBookPage(MainModel model, string flightno1, string flightno2, string track1, string track2, int flag, int flag1)
        {
            InitializeComponent();

            flightNo1 = flightno1;
            flightNo2 = flightno2;
            mainclass = model.FlightClassrequest;
            modelproperty = model;
            tracknumber1 = track1;
            tracknumber2 = track2;

            if (Application.Current.Properties.ContainsKey("id"))
            {
                bookingDetails.UserId = Convert.ToInt16(Application.Current.Properties["id"]);

            }
            if (Application.Current.Properties.ContainsKey("count"))
            {
                listcount = Convert.ToInt16(Application.Current.Properties["count"]);

            }



            // GetFlightDetails(flightNo, modelproperty);

            var gobacktap = new TapGestureRecognizer();
            gobacktap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            backarrow.GestureRecognizers.Add(gobacktap);
			GetMealDetails(tracknumber1, tracknumber2);
			//GetValues(flightno1, flightno2, model, flag, flag1);
        }
        public string changetype(string date, string time)
        {



            string[] arr = date.Split('/');
            string dayc = arr[1].TrimStart("0".ToCharArray());
            string mont = arr[0];
            string[] monthc = { " ", "jan", "feb", "mar", "apr", "may", "jun", "july", "aug", "sep", "oct", "nov", "dec" };
            for (int i = 1; i < monthc.Length; i++)
            {


                if (dayc.Contains(Convert.ToString(i)))
                {
                    date = mont + " " + monthc[i] + " " + time;

                }


            }

            return date;
        }

		public async void GetMealDetails(string trackno, string track2)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                    var client = new HttpClient();
                    string request = @"{GoTrackNo  :'" + trackno + "',BackTrackNo :'" + track2 + "'}";
                    Console.WriteLine("Request" + request);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(WebserviceUrls.ROUNDPASSENGERDETAILS, content);
                    if (response.IsSuccessStatusCode)
                    {

                        var result = await response.Content.ReadAsStringAsync();
                        PlaceList placeList = new PlaceList();
                        PlaceList placeList1 = new PlaceList();
                        if (result != "")
                        {
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
                        flightDetailsitems = placeList.golstFlightDetail;
                        bookingDetails.golstFlightDetail = flightDetailsitems;
                        bookingDetails.backlstFlightDetail = placeList.backlstFlightDetail;
                        passengerDetails = new List<BookingPassengerDetails>(placeList.LstbookingPassengerDetails);
                        bookingDetails.LstbookingPassengerDetails = passengerDetails;
						for (int k = 0; k < bookingDetails.golstFlightDetail.Count; k++)
                        {


                            flightcode.Text = bookingDetails.golstFlightDetail[k].AirlineCode + " " + bookingDetails.golstFlightDetail[k].FlightNo;

                
							if (bookingDetails.golstFlightDetail[k].AirlineCode.Contains("SG"))
                            {
								flightimage.Source = "SG.png";
                            }
							else if (bookingDetails.golstFlightDetail[k].AirlineCode.Contains("AI"))
                            {
								flightimage.Source = "AI.png";
                            }
							else if (bookingDetails.golstFlightDetail[k].AirlineCode.Contains("6E"))
                            {
								flightimage.Source = "EE.png";
                            }
							else if (bookingDetails.golstFlightDetail[k].AirlineCode.Contains("9W"))
                            {
								flightimage.Source = "WW.png";
                            }
							else if (bookingDetails.golstFlightDetail[k].AirlineCode.Contains("UK"))
                            {
								flightimage.Source= "UK.png";
                            }
                            else
                            {
								flightimage.Source = "GG.png";

                            }

                            //to.Text = bookingDetails.golstFlightDetail[k].FromAirportCode;
                            //from.Text = bookingDetails.backlstFlightDetail[k].ToAirportCode;

                            // new demo code for cheque//

                            from.Text = bookingDetails.golstFlightDetail[0].FromAirportCode;
                            to.Text = bookingDetails.backlstFlightDetail[0].FromAirportCode;

                            //code end

                            DateTime dateTime = new DateTime();
                            string date = split(bookingDetails.golstFlightDetail[k].DepDate);
                            dateTime = Convert.ToDateTime(date);
                            deptdate.Text = dateTime.ToString("dd MMM") + ", " + bookingDetails.golstFlightDetail[k].DepTime;

                            DateTime dateTime1 = new DateTime();
                            string date1 = split(bookingDetails.golstFlightDetail[k].ArrDate);
                            dateTime1 = Convert.ToDateTime(date1);
                            arrdate.Text = dateTime1.ToString("dd MMM") + ", " + bookingDetails.golstFlightDetail[k].ArrTime;

							fare.Text = Convert.ToString(bookingDetails.golstFlightDetail[k].TotalAmount);
                            flightclass.Text = bookingDetails.golstFlightDetail[k].AirlineName;
                            //airlinename.Text = flightlistitems[i].AirlineFullName;

                        }

						for (int l = 0; l < bookingDetails.backlstFlightDetail.Count; l++)
                        {
							if (l == 0)
							{


								returnflightcode.Text = bookingDetails.backlstFlightDetail[l].AirlineCode + " " + bookingDetails.backlstFlightDetail[l].FlightNo;
								if (bookingDetails.backlstFlightDetail[l].AirlineCode.Contains("SG"))
								{
									returnflightimage.Source = "SG.png";
								}
								else if (bookingDetails.backlstFlightDetail[l].AirlineCode.Contains("AI"))
								{
									returnflightimage.Source = "AI.png";
								}
								else if (bookingDetails.backlstFlightDetail[l].AirlineCode.Contains("6E"))
								{
									returnflightimage.Source = "EE.png";
								}
								else if (bookingDetails.backlstFlightDetail[l].AirlineCode.Contains("9W"))
								{
									returnflightimage.Source = "WW.png";
								}
								else if (bookingDetails.backlstFlightDetail[l].AirlineCode.Contains("UK"))
								{
									returnflightimage.Source = "UK.png";
								}
								else
								{
									returnflightimage.Source = "GG.png";

								}




								DateTime dateTime = new DateTime();
								string date = split(bookingDetails.backlstFlightDetail[l].DepDate);
								dateTime = Convert.ToDateTime(date);
								returndeptdate.Text = dateTime.ToString("dd MMM") + ", " + bookingDetails.backlstFlightDetail[l].DepTime;

								DateTime dateTime1 = new DateTime();
								string date1 = split(bookingDetails.backlstFlightDetail[l].ArrDate);
								dateTime1 = Convert.ToDateTime(date1);
								returnarrdate.Text = dateTime1.ToString("dd MMM") + ", " + bookingDetails.backlstFlightDetail[l].ArrTime;

								returnfare.Text = Convert.ToString(bookingDetails.backlstFlightDetail[l].TotalAmount);
                                returnflightclass.Text = bookingDetails.backlstFlightDetail[l].AirlineName;

								//   airlinename.Text = flightlistitems[i].AirlineFullName;
							}
                        }



                        

                        //bookingDetails.lstAdditionServiceDetails = additionalServices.BaggageValueLst;
                        for (int j = 0; j < passengerDetails.Count; j++)
                        {

                            if (j == 0)
                            {
                                passengerDetails[j].PaxNo = "Passenger" + Convert.ToString(j + 1);
                                passengerDetails[j].Title = "Mr";

                            }

                            if (additionalServices != null)
                            {


                                passengerDetails[j].AdditionalServiceSelect = additionalServices;

                                if (passengerDetails[j].AdditionalServiceSelect.MealValueLst != null)
                                {
                                    for (int i = 0; i < passengerDetails[j].AdditionalServiceSelect.MealValueLst.Count; i++)
                                    {
                                        passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealServiceName = passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealServiceName + " - " + Convert.ToString(Math.Round(Convert.ToDecimal(passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealAmount), 0));

                                    }
                                }
                                else
                                {
                                    //UserDialogs.Instance.Alert("Meals not Available", "", "Ok");
                                }
                                if (passengerDetails[j].AdditionalServiceSelect.MealValueLst != null)
                                {
                                    for (int k = 0; k < passengerDetails[j].AdditionalServiceSelect.BaggageValueLst.Count; k++)
                                    {
                                        passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageServiceName = passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageServiceName + " - " + Convert.ToString(Math.Round(Convert.ToDecimal(passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageAmount), 0));

                                    }
                                }
                                else
                                {
                                    //UserDialogs.Instance.Alert("baggage not Available", "", "Ok");
                                }
                            }
                            else
                            {
                                UserDialogs.Instance.Alert("Additional Service not Available", "", "Ok");
                            }
                        }

						Console.WriteLine(totalticketamount.Text + " asdhjklqweuio");
						totalticketamount.Text = "Total Ticket Amount: " + Convert.ToString(Convert.ToDouble(fare.Text)+Convert.ToDouble(returnfare.Text));
						Console.WriteLine(totalticketamount.Text + " asdhjklqweuio");
						wholetotalamount.Text = "Total Amount : " + Convert.ToString(Convert.ToDouble(fare.Text) + Convert.ToDouble(returnfare.Text));
                        if (listcount > 1)
                        {
                            BookingPassengerDetails bookingPassengerDetails = new BookingPassengerDetails();
                            for (int l = 1; l < listcount; l++)
                            {
                                passengerDetails.Add(new BookingPassengerDetails { PaxNo = "Passenger" + Convert.ToString(l + 1), AdditionalServiceSelect = additionalServices });
                            }

                        }
                        pasengerlist.HeightRequest = 180 * passengerDetails.Count;
                        pasengerlist.ItemsSource = passengerDetails;
                        UserDialogs.Instance.HideLoading();


                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.HideLoading();
                pasengerlist.IsVisible = false;
                dnf.IsVisible = true;

            }
        }

      
		void Handle_PickerChanged(object sender, EventArgs e)
        {
        }
        void Handle_BagPickerChanged(object sender, EventArgs e)
        {
        }
        void Confirm_Clicked(object sender, System.EventArgs e)
        {
            if (terms.Checked)
            {
                //ConfirmBooking();

            }
            else { UserDialogs.Instance.Alert("Please Accept the terms and Conditions", "", "Ok"); }
        }
        async void History_Clicked(object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new BookingDetails(bookTicketResponse, bookingDetails.UserId));
        }
		public string split(string date)
        {
            string[] parts = date.Split('/');
            string part1 = null, part2 = null, part3 = null;
            part1 = parts[1];
            part2 = parts[0];
            part3 = parts[2];
            string actualdate = part1 + "/" + part2 + "/" + part3;
            return actualdate;
        }

        public string splitamount(string amount)
        {
            string[] parts = amount.Split(' ');
            string part1 = null, part2 = null;
            part1 = parts[1];
            part2 = parts[0];

            string amountval = part1;
            return amountval;
        }
    }

}
