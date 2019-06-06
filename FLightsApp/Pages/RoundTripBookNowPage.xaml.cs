using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FLightsApp.Models;
using FLightsApp.Webservice;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
    public partial class RoundTripBookNowPage : ContentPage 
	{ 
		int listcount = 0, recurringflag =0, flag=0, flag1=0;
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
	

		public RoundTripBookNowPage( MainModel model,string flightno1,string flightno2, string track1, string track2, int flag, int flag1)
        {
			InitializeComponent();
			flightNo1 = flightno1;
            flightNo2 = flightno2;
			this.flag = flag;
			this.flag1 = flag1;

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
        }

		public async void GetMealDetails(string trackno, string track2)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    UserDialogs.Instance.ShowLoading("Loading");
                    var client = new HttpClient();
					string request = @"{GoTrackNo  :'" + trackno + "',BackTrackNo :'" + track2 + "'}";
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync(WebserviceUrls.ROUNDPASSENGERDETAILS, content);
                    if(response.IsSuccessStatusCode)
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



						for (int k = 0;k<bookingDetails.golstFlightDetail.Count; k++){

						
							flightcode.Text = bookingDetails.golstFlightDetail[k].AirlineCode + " " + bookingDetails.golstFlightDetail[k].FlightNo;
							flightimage.Source = bookingDetails.golstFlightDetail[k].IconPath;

							from.Text = bookingDetails.golstFlightDetail[k].FromAirportCode;
							to.Text = bookingDetails.golstFlightDetail[k].ToAirportCode;


                                        DateTime dateTime = new DateTime();
							            string date = split(bookingDetails.golstFlightDetail[k].DepDate);
                                        dateTime = Convert.ToDateTime(date);
							            deptdate.Text = dateTime.ToString("dd MMM") + ", " + bookingDetails.golstFlightDetail[k].DepTime;

                                        DateTime dateTime1 = new DateTime();
							             string date1 = split(bookingDetails.golstFlightDetail[k].ArrDate);
                                        dateTime1 = Convert.ToDateTime(date1);
							             arrdate.Text = dateTime1.ToString("dd MMM") + ", " + bookingDetails.golstFlightDetail[k].ArrTime;

							fare.Text =bookingDetails.golstFlightDetail[k].TotalAmount;
							flightclass.Text = bookingDetails.golstFlightDetail[k].FlightClass;
                                     //   airlinename.Text = flightlistitems[i].AirlineFullName;
                                    
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

								if(passengerDetails[j].AdditionalServiceSelect.MealValueLst!=null){
									for (int i = 0; i < passengerDetails[j].AdditionalServiceSelect.MealValueLst.Count; i++)
                                    {
                                        passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealServiceName = passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealServiceName + " - " + Convert.ToString(Math.Round(Convert.ToDecimal(passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealAmount), 0));
                                        
                                    }
								}else{
									//UserDialogs.Instance.Alert("Meals not Available", "", "Ok");
								}
								if(passengerDetails[j].AdditionalServiceSelect.MealValueLst != null){
									for (int k = 0; k < passengerDetails[j].AdditionalServiceSelect.BaggageValueLst.Count; k++)
                                    {
                                        passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageServiceName = passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageServiceName + " - " + Convert.ToString(Math.Round(Convert.ToDecimal(passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageAmount), 0));

                                    }
								}
								else{
									//UserDialogs.Instance.Alert("baggage not Available", "", "Ok");
								}
                            }
                            else
                            {
                                UserDialogs.Instance.Alert("Additional Service not Available", "", "Ok");
                            }
                        }
                        if (listcount > 1)
                        {
							BookingPassengerDetails bookingPassengerDetails = new BookingPassengerDetails();
							for (int l = 1; l < listcount; l++)
                            {
								passengerDetails.Add(new BookingPassengerDetails { PaxNo = "Passenger" + Convert.ToString(l + 1), AdditionalServiceSelect = additionalServices});

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

        public async void GetValues(string flightNo,string flightNo1, MainModel modelvalue, int flagg, int flg)
        {
			UserDialogs.Instance.ShowLoading("Loading");
            try
            {
                if (NetworkCheck.IsInternet())
				{      

                    var client = new HttpClient();
                    string request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound + "',TripType:'" + "1" + "',FlightClass:'" + modelvalue.FlightClassrequest + "'}}";
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
					var response = client.PostAsync(WebserviceUrls.GETROUNDTRIPAVAILABILITY, content).Result;
                    if (response.IsSuccessStatusCode)
                    {

                        var result = await response.Content.ReadAsStringAsync();
                        PlaceList placeList = new PlaceList();
                        if (result != "")
                        {
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
						flightlistitems = new List<MainModel>(placeList.GoFilterFlightList);
						mainFareDetails = new List<MainFareDetail>(placeList.GoSameFlightList);
						Backflightlistitems = new List<MainModel>(placeList.BackFilterFlightList);
                        // fareDetails = new List<FareDetail>(placeList.SameFlightList.samefarelist);
                        for (int i = 0; i < flightlistitems.Count; i++)
                        {
							
                            if (flightlistitems[i].FlightNo == flightNo)
                            {
								trackvalue = flightlistitems[i].TrackNo;
                                flightlistitems[i].view = "VIEW MORE";
                                if (flightlistitems[i].FareType == "R")
                                {
                                    flightlistitems[i].FareType = "Refundable";
                                }
                                else
                                {
                                    flightlistitems[i].FareType = "Non-Refundable";
                                }
                                TimeSpan tspan = TimeSpan.FromMinutes(Convert.ToDouble(flightlistitems[i].FlightTime));
                                flightlistitems[i].FlightTime = (int)tspan.TotalHours + "hr " + tspan.Minutes + "m";
                                if (flightlistitems[i].Stops == 0)
                                {
                                    flightlistitems[i].stop = "Non-Stop";
                                }
                                else
                                {
                                    flightlistitems[i].stop = flightlistitems[i].Stops + " Stop";
                                }
                                if (flightlistitems[i].IconPath == "")
                                {

                                }
                                else
                                {


                                    if (flightlistitems[i].IconPath.Contains("SG"))
                                    {
                                        flightlistitems[i].IconPath = "SG.png";
                                    }
                                    else if (flightlistitems[i].IconPath.Contains("AI"))
                                    {
                                        flightlistitems[i].IconPath = "AI.png";
                                    }
                                    else if (flightlistitems[i].IconPath.Contains("6E"))
                                    {
                                        flightlistitems[i].IconPath = "EE.png";
                                    }
                                    else if (flightlistitems[i].IconPath.Contains("9W"))
                                    {
                                        flightlistitems[i].IconPath = "WW.png";
                                    }
                                    else if (flightlistitems[i].IconPath.Contains("UK"))
                                    {
                                        flightlistitems[i].IconPath = "UK.png";
                                    }
                                    else
                                    {
                                        flightlistitems[i].IconPath = "GG.png";

                                    }

                                }

                                for (int j = 0; j < mainFareDetails.Count; j++)
                                {
                                    if (mainFareDetails[j].FlightNo == flightlistitems[i].FlightNo)
                                    {

									

                                        if (mainFareDetails[j].samefareDetail.Count == 1)
                                        {
                                            for (int k = 0; k < mainFareDetails[j].samefareDetail.Count; k++)
                                            {
                                                if (mainFareDetails[j].samefareDetail[k].IsCorporate == "0")
                                                {
                                                    flightlistitems[i].CorporateFare = "NA";
                                                    flightlistitems[i].CorporateFareType = "NA";
                                                    flightlistitems[i].CorporateClass = "NA";
                                                    flightlistitems[i].CheapFare2 = "NA";
                                                    flightlistitems[i].CheapFare2Class = "NA";
                                                    flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    flightlistitems[i].CheapFare1Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                }
                                                else
                                                {
                                                    flightlistitems[i].CorporateFare = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    flightlistitems[i].CorporateClass = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    flightlistitems[i].CheapFare1 = "NA";
                                                    flightlistitems[i].CheapFare2 = "NA";
                                                    flightlistitems[i].CheapFare2Class = "NA";
                                                    flightlistitems[i].CheapFare1Class = "NA";
                                                }

                                            }
                                        }
                                        else if (mainFareDetails[j].samefareDetail.Count == 2)
                                        {
                                            for (int k = 0; k < mainFareDetails[j].samefareDetail.Count; k++)
                                            {
                                                if (mainFareDetails[j].samefareDetail[k].IsCorporate == "0")
                                                {
                                                    flightlistitems[i].CorporateFare = "NA";
                                                    flightlistitems[i].CorporateFareType = "NA";
                                                    flightlistitems[i].CorporateClass = "NA";
                                                    if (k == 0)
                                                    {
                                                        flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                        flightlistitems[i].CheapFare1Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                        flightlistitems[i].CheapFare2Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }
                                                }
                                                else
                                                {
                                                    if (k == 0)
                                                    {
                                                        flightlistitems[i].CorporateFare = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));

                                                        flightlistitems[i].CorporateClass = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                        flightlistitems[i].CheapFare1Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }

                                                    flightlistitems[i].CheapFare2 = "NA";
                                                    flightlistitems[i].CheapFare2Class = "NA";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int k = 0; k < mainFareDetails[j].samefareDetail.Count; k++)
                                            {
                                                if (mainFareDetails[j].samefareDetail[k].IsCorporate == "0")
                                                {
                                                    flightlistitems[i].CorporateFare = "NA";
                                                    flightlistitems[i].CorporateFareType = "NA";
                                                    flightlistitems[i].CorporateClass = "NA";
                                                    if (k == 0)
                                                    {
                                                        flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                        flightlistitems[i].CheapFare1Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }
                                                    else if (k == 1)
                                                    {
                                                        flightlistitems[i].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                        flightlistitems[i].CheapFare2Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }
                                                    else
                                                    {

                                                    }
                                                }
                                                else
                                                {
                                                    if (k == 0)
                                                    {
                                                        flightlistitems[i].CorporateFare = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                        flightlistitems[i].CorporateClass = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                    }
                                                    else if (k == 1)
                                                    {
                                                        flightlistitems[i].CheapFare1Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                        flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].CheapFare2Class = mainFareDetails[j].samefareDetail[k].FlightClass;
                                                        flightlistitems[i].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    }
                                                }
                                            }
                                        }
									
                                        flightcode.Text = flightlistitems[i].AirlineCode + " " + flightlistitems[i].FlightNo;
                                        flightimage.Source = flightlistitems[i].IconPath;

                                        from.Text = flightlistitems[i].FromAirportCode;
                                        to.Text = flightlistitems[i].ToAirportCode;


                                        DateTime dateTime = new DateTime();
                                        string date = split(flightlistitems[i].DepDate);
                                        dateTime = Convert.ToDateTime(date);
                                        deptdate.Text = dateTime.ToString("dd MMM") + ", " + flightlistitems[i].DepTime;

                                        DateTime dateTime1 = new DateTime();
                                        string date1 = split(flightlistitems[i].ArrDate);
                                        dateTime1 = Convert.ToDateTime(date1);
                                        arrdate.Text = dateTime1.ToString("dd MMM") + ", " + flightlistitems[i].ArrTime;
                                     //   airlinename.Text = flightlistitems[i].AirlineFullName;
                                        if (flagg == 0)
                                        {
                                            fare.Text = flightlistitems[i].CorporateFare;

                                            totalticketAmount = splitamount(flightlistitems[i].CorporateFare);
											bookingDetails.goFinalTotalAmount = Convert.ToDouble(totalticketAmount);
											NetAmount = Convert.ToInt16(totalticketAmount);
                                            totalticketamount.Text = "Total Ticket Amount: " + flightlistitems[i].CorporateFare;
                                            flightclass.Text = flightlistitems[i].CorporateClass;
                                            trackamount = flightlistitems[i].CorporateFare;
                                        }
                                        else if (flagg == 1)
                                        {
                                            fare.Text = flightlistitems[i].CheapFare1;
									
											totalticketAmount = splitamount(flightlistitems[i].CheapFare1); 
											bookingDetails.goFinalTotalAmount = Convert.ToDouble(totalticketAmount);
											NetAmount = Convert.ToInt16(totalticketAmount);
                                            totalticketamount.Text = "Total Ticket Amount: " + flightlistitems[i].CheapFare1;
                                            flightclass.Text = flightlistitems[i].CheapFare1Class;
                                            trackamount = flightlistitems[i].CheapFare1;
                                        }
                                        else
                                        {
                                            fare.Text = flightlistitems[i].CheapFare2;
									
                                            flightclass.Text = flightlistitems[i].CheapFare2Class;

                                            totalticketAmount = splitamount(flightlistitems[i].CheapFare2);
											bookingDetails.goFinalTotalAmount = Convert.ToDouble(totalticketAmount);
											NetAmount = Convert.ToInt16(totalticketAmount);
                                            totalticketamount.Text = "Total Ticket Amount: " + flightlistitems[i].CheapFare2;
                                            trackamount = flightlistitems[i].CheapFare2;

                                        }
									
                                      
                                      

                                    }

                                }


                            }

                        }
					
                        BackmainFareDetails = new List<MainFareDetail>(placeList.BackSameFlightList);

						for (int m = 0; m < Backflightlistitems.Count; m++)
						{

							if (Backflightlistitems[m].FlightNo == flightNo1)
							{

								Backflightlistitems[m].view = "VIEW MORE";
							if (Backflightlistitems[m].FareType == "R")
							{
								Backflightlistitems[m].FareType = "Refundable";
							}
							else
							{
								Backflightlistitems[m].FareType = "Non-Refundable";
							}
							TimeSpan tspan1 = TimeSpan.FromMinutes(Convert.ToDouble(Backflightlistitems[m].FlightTime)); //converting minutes to timespan
							Backflightlistitems[m].FlightTime = (int)tspan1.TotalHours + "hr " + tspan1.Minutes + "m";
							if (Backflightlistitems[m].Stops == 0)
							{
								Backflightlistitems[m].stop = "Non-Stop";
							}
							else
							{
								Backflightlistitems[m].stop = Backflightlistitems[m].Stops + " Stop";
							}
							if (Backflightlistitems[m].IconPath == "")
							{
							}
							else
							{


								if (Backflightlistitems[m].IconPath.Contains("SG"))
								{
									Backflightlistitems[m].IconPath = "SG.png";
								}
								else if (Backflightlistitems[m].IconPath.Contains("AI"))
								{
									Backflightlistitems[m].IconPath = "AI.png";
								}
								else if (Backflightlistitems[m].IconPath.Contains("6E"))
								{
									Backflightlistitems[m].IconPath = "EE.png";
								}
								else if (Backflightlistitems[m].IconPath.Contains("9W"))
								{
									Backflightlistitems[m].IconPath = "WW.png";
								}
								else if (Backflightlistitems[m].IconPath.Contains("UK"))
								{
									Backflightlistitems[m].IconPath = "UK.png";
								}
								else
								{
									Backflightlistitems[m].IconPath = "GG.png";
								}
							}

								for (int n = 0; n < BackmainFareDetails.Count; n++)
								{
									
										if (BackmainFareDetails[n].FlightNo == Backflightlistitems[m].FlightNo)
										{

											Application.Current.Properties["myList"] = BackmainFareDetails[n].fltDetails;
											if (BackmainFareDetails[n].samefareDetail.Count == 1)
											{
												for (int p = 0; p < BackmainFareDetails[n].samefareDetail.Count; p++)
												{
													if (BackmainFareDetails[n].samefareDetail[p].IsCorporate == "0")
													{
														Backflightlistitems[m].CorporateFare = "NA";
														Backflightlistitems[m].CorporateFareType = "NA";
														Backflightlistitems[m].CheapFare2 = "NA";
														Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
													}
													else
													{
														Backflightlistitems[m].CorporateFare = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														Backflightlistitems[m].CorporateTrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CheapFare1 = "NA";
														Backflightlistitems[m].CheapFare2 = "NA";
													}

												}
											}
											else if (BackmainFareDetails[n].samefareDetail.Count == 2)
											{
												for (int p = 0; p < BackmainFareDetails[n].samefareDetail.Count; p++)
												{
													if (BackmainFareDetails[n].samefareDetail[p].IsCorporate == "0")
													{
														Backflightlistitems[m].CorporateFare = "NA";
														Backflightlistitems[m].CorporateFareType = "NA";
														if (p == 0)
														{
															Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
															Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														}
														else
														{
															Backflightlistitems[m].Cheap2TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
													}
													else
													{
														if (p == 0)
														{
															Backflightlistitems[m].CorporateTrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CorporateFare = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
														else
														{
															Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}

														Backflightlistitems[m].CheapFare2 = "NA";
													}
												}
											}
											else
											{
												for (int p = 0; p < BackmainFareDetails[n].samefareDetail.Count; p++)
												{
													if (BackmainFareDetails[n].samefareDetail[p].IsCorporate == "0")
													{
														Backflightlistitems[m].CorporateFare = "NA";
														Backflightlistitems[m].CorporateFareType = "NA";
														if (p == 0)
														{
															Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
														else if (p == 1)
														{
															Backflightlistitems[m].Cheap2TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
														else
														{

														}
													}
													else
													{
														if (p == 0)
														{
															Backflightlistitems[m].CorporateTrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CorporateFare = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
														else if (p == 1)
														{
															Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
														else
														{
															Backflightlistitems[m].Cheap2TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
															Backflightlistitems[m].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														}
													}
												}
											}

										returnflightcode.Text = Backflightlistitems[m].AirlineCode + " " + Backflightlistitems[m].FlightNo;
                                        returnflightimage.Source = Backflightlistitems[m].IconPath;



                                        DateTime dateTime = new DateTime();
                                        string date = split(Backflightlistitems[m].DepDate);
                                        dateTime = Convert.ToDateTime(date);
										returndeptdate.Text = dateTime.ToString("dd MMM") + ", " + Backflightlistitems[m].DepTime;

                                        DateTime dateTime1 = new DateTime();
                                        string date1 = split(Backflightlistitems[m].ArrDate);
                                        dateTime1 = Convert.ToDateTime(date1);
										returnarrdate.Text = dateTime1.ToString("dd MMM") + ", " + Backflightlistitems[m].ArrTime;
                                        //   airlinename.Text = flightlistitems[i].AirlineFullName;
                                        if (flg == 3)
                                        {
                                            returnfare.Text = Backflightlistitems[m].CorporateFare;
										
                                            totalticketAmount = splitamount(Backflightlistitems[m].CorporateFare);
											bookingDetails.backFinalTotalAmount = Convert.ToDouble(totalticketAmount);
                                            NetAmount += Convert.ToInt16(totalticketAmount);
                                         // totalticketamount.Text = "Total Ticket Amount: " + Backflightlistitems[m].CorporateFare;
                                            returnflightclass.Text = Backflightlistitems[m].CorporateClass;
                                            trackamount = Backflightlistitems[m].CorporateFare;
                                        }
                                        else if (flg == 4)
                                        {
                                            returnfare.Text = Backflightlistitems[m].CheapFare1;
										
                                            totalticketAmount = splitamount(Backflightlistitems[m].CheapFare1);
											bookingDetails.backFinalTotalAmount = Convert.ToDouble(totalticketAmount);
                                            NetAmount += Convert.ToInt16(totalticketAmount);
                                         // totalticketamount.Text = "Total Ticket Amount: " + Backflightlistitems[m].CheapFare1;
                                            returnflightclass.Text = Backflightlistitems[m].CheapFare1Class;
                                            trackamount = Backflightlistitems[m].CheapFare1;
                                        }
                                        else
                                        {
                                            returnfare.Text = Backflightlistitems[m].CheapFare2;
										
                                            returnflightclass.Text = Backflightlistitems[m].CheapFare2Class;
                                            totalticketAmount = splitamount(Backflightlistitems[m].CheapFare2);
											bookingDetails.backFinalTotalAmount = Convert.ToDouble(totalticketAmount);
                                            NetAmount += Convert.ToInt16(totalticketAmount);
                                        //    totalticketamount.Text = "Total Ticket Amount: " + Backflightlistitems[m].CheapFare2;
                                            trackamount = Backflightlistitems[m].CheapFare2;

                                        }
										totalticketamount.Text = "Total Ticket Amount: " + Convert.ToString(NetAmount);
                                        wholetotalamount.Text = "Total Amount : " + Convert.ToString(NetAmount);
									
										}

								}

							
						}
                        }
					
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Error while fetching data", "", "OK");
                    }
                }

                else
                {
                    await DisplayAlert("JSONParsing", "No network is available.", "Ok");
                }
			
            }
            catch (Exception e)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Error Occured", "", "OK"); ;
            }
			GetMealDetails(tracknumber1, tracknumber2);
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

		public async void ConfirmBooking()
        {
            if (NetworkCheck.IsInternet())
            {
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(bookingDetails);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
				HttpResponseMessage response = await client.PostAsync(WebserviceUrls.ROUNDBOOKING, content);

                if (response.IsSuccessStatusCode)
                {
                    var response1 = response.Content.ReadAsStringAsync().Result;
                    PlaceList data = new PlaceList();
                    if (!string.IsNullOrEmpty(response1))
                    {
                        data = JsonConvert.DeserializeObject<PlaceList>(response1);
                    }
                    
                    bookTicketResponse = data.goBookTicketResponse;
                    string status1 = data.goErrorMessage;
					string status2 = data.backErrorMessage;
					if (status1 == "Success" && status2 == "Success")
                    {
                        UserDialogs.Instance.Alert("Booking Successful", "", "Ok");
                    }
                    else
                    {
						UserDialogs.Instance.Alert(data.goErrorMessage +"  "+ data.backErrorMessage, "", "Ok");
						//UserDialogs.Instance.Alert(data.backErrorMessage, "", "Ok");
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Booking failed", "", "OK");
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please Check Your Internet Connection", "", "OK");
            }
            UserDialogs.Instance.HideLoading();
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

    }
}
