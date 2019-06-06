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
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	
    public partial class RoundFlightListPage : ContentPage
    {
		int flag = 0, flag2=0; string trackno1 = null,amount= null, trackno2=null, flightno1 = null, flightno2= null;
        MainModel modelproperties = new MainModel();
        List<MainModel> flightlistitems = new List<MainModel>();
		List<MainModel> Backflightlistitems = new List<MainModel>();
		List<MainModel> flightlistitems1 = new List<MainModel>();
        List<MainFareDetail> mainFareDetails = new List<MainFareDetail>();
		List<MainFareDetail> BackmainFareDetails = new List<MainFareDetail>();
        List<FareDetail> fareDetails = new List<FareDetail>();
        List<string> vs = new List<string>();
        List<int> stopfilterlist = new List<int>();
		DateTime dateTime = new DateTime();
		public RoundFlightListPage(MainModel model, string date, string returndate)
        {
            InitializeComponent();
			//trackno = "dsfbsdf";


			from.Text = model.FromOneway;
            To.Text = model.ToOneway;
			modelproperties = model;
            Adultno.Text = "Adult : " + Convert.ToString(model.NoOfAdult);
            childno.Text = "Child : " + Convert.ToString(model.NoOfChild);
            infantno.Text = "Infant : " + Convert.ToString(model.NoOfInfant);
	
       
            dateTime = Convert.ToDateTime(date);
            titlecode.Text = model.FromOneway + " : " + model.ToOneway + ", " + dateTime.ToString("dd MMM");
            //toolbardate.Text = dateTime.ToString("dd MMM");

			dateTime = Convert.ToDateTime(returndate);
            titlecode1.Text =model.ToOneway + " : " + model.FromOneway + ", " + dateTime.ToString("dd MMM");
      

				GetFlightList(model);
			var gobacktap = new TapGestureRecognizer();
            gobacktap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
			goback.GestureRecognizers.Add(gobacktap);
			var returnTapgest = new TapGestureRecognizer();
			returnTapgest.Tapped += (s, e) =>
			{
				//titlecode.IsVisible = false;
				//titlecode1.IsVisible = true;
				returnlistpage.IsVisible = true;
				flightlist.IsVisible = false;
				departlist.IsVisible = true;
				returnlist.IsVisible = false;
				firstheader.IsVisible = false;
				secondheader.IsVisible = true;
			};
			returnTap.GestureRecognizers.Add(returnTapgest);
            
			var departTapgest = new TapGestureRecognizer();
			departTapgest.Tapped += (s, e) =>
            {
				//titlecode.IsVisible = true;
				//titlecode1.IsVisible = false;
                returnlistpage.IsVisible = false;
                flightlist.IsVisible = true;
                departlist.IsVisible = false;
                returnlist.IsVisible = true;
                firstheader.IsVisible = true;
                secondheader.IsVisible = false;
            };
			departTap.GestureRecognizers.Add(departTapgest);
            
			var filterTap = new TapGestureRecognizer();
			filterTap.Tapped += async(s, e) =>
			{
                FilterByPage page = new FilterByPage(vs, stopfilterlist);
                page.OnSelectedCity += Page_OnSelected;
                await Navigation.PushPopupAsync(page);
			};
			filter.GestureRecognizers.Add(filterTap);


        
        }

       
		internal void Page_OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var filteritem = (MainModel)(sender);


            if (filteritem.Flightfilterlst.Count != 0 && filteritem.Stopfilterlst.Count != 0)
            {
                //var data = flightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName)).ToList();
                var data = flightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName) && filteritem.Stopfilterlst.Contains(X.Stops)).ToList();
				var data1 = Backflightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName) && filteritem.Stopfilterlst.Contains(X.Stops)).ToList();

				if (data.Count != 0 && data1.Count != 0)
                {
                    flightlist.ItemsSource = data;
                    departlist.ItemsSource = data;
                    returnlist.ItemsSource = data1;
                    returnlistpage.ItemsSource = data1;
                }
                else if (data.Count != 0 && data1.Count == 0)
                {
                    flightlist.ItemsSource = data;
                    departlist.ItemsSource = data;
                    UserDialogs.Instance.Alert("No such Flights found for Returning");
                }
                else if (data.Count == 0 && data1.Count != 0)
                {
                    returnlist.ItemsSource = data1;
                    returnlistpage.ItemsSource = data1;
                    UserDialogs.Instance.Alert("No such Flights found for Departure");
                }
                else
                {
                    UserDialogs.Instance.Alert("No Flights Found", "", "OK");
                }
            }
            else if (filteritem.Flightfilterlst.Count != 0 && filteritem.Stopfilterlst.Count == 0)
            {
                var data = flightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName)).ToList();
				var data1 = Backflightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName)).ToList();

				if (data.Count != 0 && data1.Count!=0) {
					flightlist.ItemsSource = data; 
					departlist.ItemsSource = data;
					returnlist.ItemsSource = data1;
					returnlistpage.ItemsSource = data1;
				}else if(data.Count != 0 && data1.Count == 0){
					flightlist.ItemsSource = data;
                    departlist.ItemsSource = data;
					UserDialogs.Instance.Alert("No such Flights found for Returning");
				}
				else if (data.Count == 0 && data1.Count != 0)
                {
					returnlist.ItemsSource = data1;
                    returnlistpage.ItemsSource = data1;
					UserDialogs.Instance.Alert("No such Flights found for Departure");
                }
                else
                {
                    UserDialogs.Instance.Alert("No Flights Found", "", "OK");
                }
            }
            else if (filteritem.Flightfilterlst.Count == 0 && filteritem.Stopfilterlst.Count != 0)
            {
                var data = flightlistitems.Where(X => filteritem.Stopfilterlst.Contains(X.Stops)).ToList();
				var data1 = Backflightlistitems.Where(X => filteritem.Stopfilterlst.Contains(X.Stops)).ToList();
				if (data.Count != 0 && data1.Count != 0)
                {
                    flightlist.ItemsSource = data;
                    departlist.ItemsSource = data;
                    returnlist.ItemsSource = data1;
                    returnlistpage.ItemsSource = data1;
                }
                else if (data.Count != 0 && data1.Count == 0)
                {
                    flightlist.ItemsSource = data;
                    departlist.ItemsSource = data;
                    UserDialogs.Instance.Alert("No such Flights found for Returning");
                }
                else if (data.Count == 0 && data1.Count != 0)
                {
                    returnlist.ItemsSource = data1;
                    returnlistpage.ItemsSource = data1;
                    UserDialogs.Instance.Alert("No such Flights found for Departure");
                }
                else
                {
                    UserDialogs.Instance.Alert("No Flights Found", "", "OK");
                }
            }
            else
            {
                flightlist.ItemsSource = flightlistitems;
				departlist.ItemsSource = flightlistitems;
				returnlist.ItemsSource = Backflightlistitems;
				returnlistpage.ItemsSource = Backflightlistitems;
            }


        }






		private void Cell_OnTapped(object sender, EventArgs e)
        {
			//sohil @forexbazaar.com
			var viewCell = (ViewCell)sender ;
			//var data = viewCell.CheapFare1;
            if (viewCell.View != null)
            {
				//dateTime1 = Convert.ToDateTime(returndate);
                //titlecode1.Text = model.ToOneway + " : " + model.FromOneway + ", " + dateTime1.ToString("dd MMM");
				returnlistpage.IsVisible = true;
                flightlist.IsVisible = false;
                departlist.IsVisible = true;
                returnlist.IsVisible = false;
                firstheader.IsVisible = false;
                secondheader.IsVisible = true;
                Console.WriteLine("Click cell");
            }
        }


        private void Cell_OnTapped1(object sender, EventArgs e)
        {

            var viewCell = (ViewCell)sender;
        
            if (viewCell.View != null)
            {
				returnlistpage.IsVisible = false;
                flightlist.IsVisible = true;
                departlist.IsVisible = false;
                returnlist.IsVisible = true;
                firstheader.IsVisible = true;
                secondheader.IsVisible = false;
                Console.WriteLine("Click cellsecond");




            }
        }


		public async void GetFlightList(MainModel modelvalue)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                    var client = new HttpClient();
                    string request = null;
					if (modelvalue.roundarrivaltime != null)
                    {
						request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound + "',TripType:'" + "1" + "',FlightClass:'" + modelvalue.FlightClassrequest + "'}";
						Console.WriteLine(request);
                    }
                    else
                    {
						request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound  + "',TripType:'" + "1" + "',FlightClass:'" + modelvalue.FlightClassrequest +  "'}";
						Console.WriteLine(request);
                    }


                    var content = new StringContent(request, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync(WebserviceUrls.GETROUNDTRIPAVAILABILITY, content);
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        var result = await response.Content.ReadAsStringAsync();
                        PlaceList placeList = new PlaceList();
                        if (result != "")
                        {
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
						flightlistitems = new List<MainModel>(placeList.GoFilterFlightList);
						Backflightlistitems = new List<MainModel>(placeList.BackFilterFlightList);

						bool spicejetfound = (flightlistitems.Any(s => s.AirlineFullName == "Spice Jet")||Backflightlistitems.Any(s => s.AirlineFullName == "Spice Jet"));
						bool jetfound = (flightlistitems.Any(s => s.AirlineFullName == "Jet Airways")||Backflightlistitems.Any(s => s.AirlineFullName == "Jet Airways"));
						bool airfound = (flightlistitems.Any(s => s.AirlineFullName == "Air India")|| Backflightlistitems.Any(s => s.AirlineFullName == "Air India"));
						bool goairfound = (flightlistitems.Any(s => s.AirlineFullName == "Go Air")||Backflightlistitems.Any(s => s.AirlineFullName == "Go Air"));
						bool indigofound = (flightlistitems.Any(s => s.AirlineFullName == "IndiGo")||Backflightlistitems.Any(s => s.AirlineFullName == "IndiGo"));
						bool zerostopfound = (flightlistitems.Any(s => s.Stops == 0)||Backflightlistitems.Any(s => s.Stops == 0));
						bool onestopfound = (flightlistitems.Any(s => s.Stops == 1)||Backflightlistitems.Any(s => s.Stops == 1));
						bool twostopfound = (flightlistitems.Any(s => s.Stops == 2)||Backflightlistitems.Any(s => s.Stops == 2));

                        if (zerostopfound)
                        {
                            stopfilterlist.Add(0); 
                        }
                        if (onestopfound)
                        {
                            stopfilterlist.Add(1);
                        }
                        if (twostopfound)
                        {
                            stopfilterlist.Add(2);
                        }

                        if (spicejetfound)
                        {
                            vs.Add("Spice Jet");
                        }
                        if (jetfound)
                        {
                            vs.Add("Jet Airways");
                        }
                        if (airfound)
                        {
                            vs.Add("Air India");
                        }
                        if (goairfound)
                        {
                            vs.Add("Go Air");
                        }
                        if (indigofound)
                        {
                            vs.Add("IndiGo");
                        }


						mainFareDetails = new List<MainFareDetail>(placeList.GoSameFlightList);

						for (int i = 0; i < flightlistitems.Count; i++)
						{





                            //new code for next day
                            string dateone = flightlistitems[i].DepDate;
                            string datesecond = flightlistitems[i].ArrDate;
                           

                            if (dateone == datesecond)
                            {

                            }
                            else
                            {
                                flightlistitems[i].NextDay = "NA";
                            }



                            //end code ///


							flightlistitems[i].view = "VIEW MORE";
						
							TimeSpan tspan = TimeSpan.FromMinutes(Convert.ToDouble(flightlistitems[i].FlightTime)); //converting minutes to timespan
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
								mainFareDetails[j].samefareDetail = mainFareDetails[j].samefareDetail.OrderBy(x => x.TotalAmount).ToList();
								List<FareDetail> fareDetails1 = new List<FareDetail>(mainFareDetails[j].samefareDetail);
                                //  fareDetails1 = mainFareDetails[j].samefareDetail;
                                fareDetails1 = fareDetails1.Where(X => X.FareType == "R").ToList();
                                var data1 = mainFareDetails[j].samefareDetail.Where(X => X.FareType == "N").ToList();
                                mainFareDetails[j].samefareDetail.Clear();
                                for (int p = 0; p < fareDetails1.Count; p++)
								{int count = 1;

                                    mainFareDetails[j].samefareDetail.Add(fareDetails1[p]);
                                    for (int q = 0; q < data1.Count; q++)
                                    {
										
                                        bool isFound = mainFareDetails[j].samefareDetail.Intersect(data1).Any();
                                        if (!isFound)
										{
											if(count ==1){
												mainFareDetails[j].samefareDetail.Add(data1[q]);
												count = 0;
											}
										
                                        }
                                    }
                                }
								if (mainFareDetails[j].FlightNo == flightlistitems[i].FlightNo)
                                {

                                    Application.Current.Properties["myList"] = mainFareDetails[j].fltDetails;
                                  


                                    if (mainFareDetails[j].samefareDetail.Count == 1)
                                    {
                                        for (int k = 0; k < mainFareDetails[j].samefareDetail.Count; k++)
                                        {
                                            if (mainFareDetails[j].samefareDetail[k].IsCorporate == "0")
                                            {
                                                flightlistitems[i].CorporateFare = "NA";
                                                flightlistitems[i].CorporateFareType = "NA";
                                                flightlistitems[i].CheapFare2 = "NA";
                                                flightlistitems[i].Cheap2FareType = "NA";
                                                flightlistitems[i].Cheap2MealIcon = "NA.png";
                                                flightlistitems[i].Cheap2BaggageIcon = "NA.png";
                                                flightlistitems[i].MealIcon = "NA.png";
                                                flightlistitems[i].BaggageIcon = "NA.png";
                                                flightlistitems[i].Cheap1TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                {
                                                    if (mainFareDetails[j].samefareDetail[k].TotalAmount <= Convert.ToDecimal(3500.0))
                                                    {
                                                        flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1FareType = "Refundable";
                                                    }

                                                }
                                                else
                                                {
                                                    flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                }
                                                if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "meal.png";
                                                }
                                                else
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                }
                                                if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                {
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                    } else
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                    }

                                                }
                                                else
                                                {
                                                    flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                }



                                            }
                                            else
                                            {
                                                flightlistitems[i].CorporateFare = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                flightlistitems[i].CorporateTrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;

                                                if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                {
                                                    flightlistitems[i].CorporateFareType = "Refundable";
                                                }
                                                else
                                                {
                                                    flightlistitems[i].CorporateFareType = "Non-Refundable";
                                                }
                                                if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                {
                                                    flightlistitems[i].MealIcon = "meal.png";
                                                }
                                                else
                                                {
                                                    flightlistitems[i].MealIcon = "nomeal.png";
                                                }
                                                if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                {
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                    {
                                                        flightlistitems[i].BaggageIcon = "b15.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                    {
                                                        flightlistitems[i].BaggageIcon = "checkin15kg.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                    }else  if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                    {
                                                        flightlistitems[i].BaggageIcon = "b15.png";
                                                    }
                                                    else 
                                                    {
                                                        flightlistitems[i].BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                    }

                                                }
                                                else
                                                {
                                                    flightlistitems[i].BaggageIcon = "checkin15kg.png";
                                                }

                                                flightlistitems[i].CheapFare1 = "NA";
                                                flightlistitems[i].Cheap1FareType = "NA";
                                                flightlistitems[i].CheapFare1 = "NA";
                                                flightlistitems[i].Cheap2FareType = "NA";
                                                flightlistitems[i].Cheap2MealIcon = "NA.png";
                                                flightlistitems[i].Cheap2BaggageIcon = "NA.png";
                                                flightlistitems[i].Cheap1MealIcon = "NA.png";
                                                flightlistitems[i].Cheap1BaggageIcon = "NA.png";
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
                                                flightlistitems[i].MealIcon = "NA.png";
                                                flightlistitems[i].BaggageIcon = "NA.png";
                                                if (flightlistitems[i].CheapFare1 == "" || flightlistitems[i].CheapFare1 == null)
                                                {

                                                    flightlistitems[i].CheapFare2 = "NA";
                                                    flightlistitems[i].Cheap2FareType = "NA";
                                                    flightlistitems[i].Cheap2MealIcon = "NA.png";
                                                    flightlistitems[i].Cheap2BaggageIcon = "NA.png";
                                                    flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    flightlistitems[i].Cheap1TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].TotalAmount <= Convert.ToDecimal(3500.0))
                                                        {
                                                            flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap1FareType = "Refundable";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                        }  else
                                                       if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                    }


                                                }
                                                else
                                                {
                                                    flightlistitems[i].Cheap2TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    flightlistitems[i].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].TotalAmount <= Convert.ToDecimal(3500.0))
                                                        {
                                                            flightlistitems[i].Cheap2FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap2FareType = "Refundable";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap2FareType = "Non-Refundable";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    {
                                                        flightlistitems[i].Cheap2MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap2MealIcon = "nomeal.png";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = "b15.png";
                                                        }  else
                                                       if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap2BaggageIcon = "checkin15kg.png";
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                if (flightlistitems[i].CorporateFare == "NA")
                                                {
                                                    flightlistitems[i].CorporateTrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    flightlistitems[i].CorporateFare = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    {
                                                        flightlistitems[i].CorporateFareType = "Refundable";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].CorporateFareType = "Non-Refundable";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    {
                                                        flightlistitems[i].MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].MealIcon = "nomeal.png";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                        {
                                                            flightlistitems[i].BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                        {
                                                            flightlistitems[i].BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                        {
                                                            flightlistitems[i].BaggageIcon = "b15.png";
                                                        }  else
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].BaggageIcon = "checkin15kg.png";
                                                    }

                                                }
                                                else
                                                {
                                                    //flightlistitems[i].Cheap1TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    //flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    //if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    //                                       {
                                                    //                                           flightlistitems[i].Cheap1FareType = "Refundable";
                                                    //                                       }
                                                    //                                       else
                                                    //                                       {
                                                    //                                           flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                    //                                       }
                                                    //if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    //                                      {
                                                    //                                          flightlistitems[i].Cheap1MealIcon = "meal.png";
                                                    //                                      }
                                                    //                                      else
                                                    //                                      {
                                                    //                                          flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    //                                      }
                                                    //                                      if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    //                                      {
                                                    //                                          if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                    //                                          {
                                                    //                                              flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                    //                                          }
                                                    //                                          else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                    //                                          {
                                                    //                                              flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                    //                                          }
                                                    //else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces"))
                                                    //                                         {
                                                    //flightlistitems[i].Cheap1BaggageIcon = "onepc.png";
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //     flightlistitems[i].Cheap1BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                    //    }

                                                    //}
                                                    //else
                                                    //{
                                                    //    flightlistitems[i].Cheap1BaggageIcon = "NA.png";
                                                    //}

                                                }

                                                flightlistitems[i].CheapFare2 = "NA";
                                                flightlistitems[i].Cheap2FareType = "NA";
                                                flightlistitems[i].Cheap2MealIcon = "NA.png";
                                                flightlistitems[i].Cheap2BaggageIcon = "NA.png";

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

                                                flightlistitems[i].MealIcon = "NA.png";
                                                flightlistitems[i].BaggageIcon = "NA.png";
                                                if (flightlistitems[i].CheapFare1 == "" || flightlistitems[i].CheapFare1 == null)
                                                {

                                                    flightlistitems[i].Cheap1TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].TotalAmount <= Convert.ToDecimal(3500.0))
                                                        {
                                                            flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap1FareType = "Refundable";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                        }  else
                                                         if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                    }

                                                }
                                                else if (flightlistitems[i].CheapFare2 == "" || flightlistitems[i].CheapFare2 == null)
                                                {
                                                    flightlistitems[i].Cheap2TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    flightlistitems[i].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].TotalAmount <= Convert.ToDecimal(3500.0))
                                                        {
                                                            flightlistitems[i].Cheap2FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap2FareType = "Refundable";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap2FareType = "Non-Refundable";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    {
                                                        flightlistitems[i].Cheap2MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap2MealIcon = "nomeal.png";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = "b15.png";
                                                        }  else
                                                       if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].Cheap2BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].Cheap2BaggageIcon = "checkin15kg.png";
                                                    }
                                                }
                                                else
                                                {

                                                }
                                            }
                                            else
                                            {
                                                if (flightlistitems[i].CorporateFare == "NA")
                                                {
                                                    flightlistitems[i].CorporateTrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    flightlistitems[i].CorporateFare = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    {
                                                        flightlistitems[i].CorporateFareType = "Refundable";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].CorporateFareType = "Non-Refundable";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    {
                                                        flightlistitems[i].MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].MealIcon = "nomeal.png";
                                                    }
                                                    if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    {
                                                        if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                        {
                                                            flightlistitems[i].BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                        {
                                                            flightlistitems[i].BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("1PC") || mainFareDetails[j].samefareDetail[k].Baggage.Contains("2PC"))
                                                        {
                                                            flightlistitems[i].BaggageIcon = "b15.png";
                                                        }  else
                                                       if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("30"))
                                                        {
                                                            flightlistitems[i].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            flightlistitems[i].BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        flightlistitems[i].BaggageIcon = "NA.png";
                                                    }

                                                }
                                                //else if (k == 1)
                                                //{
                                                //  flightlistitems[i].Cheap1TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                //  flightlistitems[i].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                //  if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                //                                        {
                                                //                                            flightlistitems[i].Cheap1FareType = "Refundable";
                                                //                                        }
                                                //                                        else
                                                //                                        {
                                                //                                            flightlistitems[i].Cheap1FareType = "Non-Refundable";
                                                //                                        }
                                                //  if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                //                                        {
                                                //                                            flightlistitems[i].Cheap1MealIcon = "meal.png";
                                                //                                        }
                                                //                                        else
                                                //                                        {
                                                //                                            flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                //                                        }
                                                //                                        if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                //                                        {
                                                //                                            if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                //                                            {
                                                //                                                flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                //                                            }
                                                //                                            else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                //                                            {
                                                //                                                flightlistitems[i].Cheap1BaggageIcon = "checkin15kg.png";
                                                //                                            }
                                                //      else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces"))
                                                //                                            {
                                                //          flightlistitems[i].Cheap1BaggageIcon= "onepc.png";
                                                //                                            }
                                                //                                            else
                                                //                                            {
                                                //                                              flightlistitems[i].Cheap1BaggageIcon = mainFareDetails[j].samefareDetail[k].Baggage;
                                                //                                            }

                                                //                                        }
                                                //                                        else
                                                //                                        {
                                                //                                           flightlistitems[i].Cheap1BaggageIcon = "NA.png";
                                                //                                        }

                                                //}
                                                else
                                                {
                                                    //flightlistitems[i].Cheap2TrackNo = mainFareDetails[j].samefareDetail[k].TrackNo;
                                                    //flightlistitems[i].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(mainFareDetails[j].samefareDetail[k].TotalAmount, 0));
                                                    //if (mainFareDetails[j].samefareDetail[k].FareType == "R")
                                                    //                                       {
                                                    //                                           flightlistitems[i].Cheap2FareType = "Refundable";
                                                    //                                       }
                                                    //                                       else
                                                    //                                       {
                                                    //                                           flightlistitems[i].Cheap2FareType = "Non-Refundable";
                                                    //                                       }
                                                    //if (mainFareDetails[j].samefareDetail[k].FlightRemarks != "")
                                                    //                                      {
                                                    //                                          flightlistitems[i].Cheap2MealIcon = "meal.png";
                                                    //                                      }
                                                    //                                      else
                                                    //                                      {
                                                    //                                          flightlistitems[i].Cheap2MealIcon = "nomeal.png";
                                                    //                                      }
                                                    //                                      if (mainFareDetails[j].samefareDetail[k].Baggage != "" && !string.IsNullOrEmpty(mainFareDetails[j].samefareDetail[k].Baggage))
                                                    //                                      {
                                                    //                                          if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("25"))
                                                    //                                          {
                                                    //                                              flightlistitems[i].Cheap2BaggageIcon = "b15.png";
                                                    //                                          }
                                                    //                                          else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("15"))
                                                    //                                          {
                                                    //                                              flightlistitems[i].Cheap2BaggageIcon = "checkin15kg.png";
                                                    //                                          }
                                                    //else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("1Pieces"))
                                                    //                                         {
                                                    //flightlistitems[i].Cheap2BaggageIcon = "onepc.png";
                                                    //                                         }
                                                    //                                         else
                                                    //                                         {
                                                    //flightlistitems[i].Cheap2BaggageIcon =mainFareDetails[j].samefareDetail[k].Baggage;;
                                                    //    }

                                                    //}
                                                    //else
                                                    //{
                                                    //   flightlistitems[i].Cheap2BaggageIcon = "NA.png";
                                                    //}

                                                }
                                            }
                                        }
                                    }
                                }

                            }

						}
							BackmainFareDetails = new List<MainFareDetail>(placeList.BackSameFlightList);

                            for (int m = 0; m < Backflightlistitems.Count; m++)
                            {

                                Backflightlistitems[m].view = "VIEW MORE";
                            
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
								BackmainFareDetails[n].samefareDetail = BackmainFareDetails[n].samefareDetail.OrderBy(x => x.TotalAmount).ToList();    
								List<FareDetail> fareDetails1 = new List<FareDetail>(BackmainFareDetails[n].samefareDetail);
                                //  fareDetails1 = mainFareDetails[j].samefareDetail;
                                fareDetails1 = fareDetails1.Where(X => X.FareType == "R").ToList();
								var data1 =BackmainFareDetails[n].samefareDetail.Where(X => X.FareType == "N").ToList();
								BackmainFareDetails[n].samefareDetail.Clear();
                                for (int p = 0; p < fareDetails1.Count; p++)
								{
									int count = 1;

									BackmainFareDetails[n].samefareDetail.Add(fareDetails1[p]);
                                    for (int q = 0; q < data1.Count; q++)
                                    {
										bool isFound =BackmainFareDetails[n].samefareDetail.Intersect(data1).Any();
                                        if (!isFound)
                                        {
											if (count==1){
												BackmainFareDetails[n].samefareDetail.Add(data1[q]);
												count = 0;
											}

                                        }

                                    }
                                }
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
												Backflightlistitems[m].Cheap2FareType = "NA";
												Backflightlistitems[m].MealIcon = "NA.png";
												Backflightlistitems[m].BaggageIcon = "NA.png";
												Backflightlistitems[m].Cheap2MealIcon = "NA.png";
												Backflightlistitems[m].Cheap2BaggageIcon = "NA.png";

													Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
													Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
												if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                {
													if (BackmainFareDetails[n].samefareDetail[p].TotalAmount <= Convert.ToDecimal(3500))
                                                    {
														Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
                                                    }
                                                    else
                                                    {
														Backflightlistitems[m].Cheap1FareType = "Refundable";
                                                    }
                                                }
                                                else
                                                {
													Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
                                                }
												if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                {
													Backflightlistitems[m].Cheap1MealIcon = "meal.png";
                                                }
                                                else
                                                {
													Backflightlistitems[m].Cheap1MealIcon = "nomeal.png";
                                                }
												if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                {
													if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                    {
														Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                    }
													else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                    {
														Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
                                                    }
                                                    else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                    } else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                    }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                    }
                                                    else
                                                    {
														Backflightlistitems[m].Cheap1BaggageIcon = BackmainFareDetails[n].samefareDetail[p] .Baggage;
                                                    }

                                                }
                                                else
                                                {
                                                    Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
                                                }

											
											}
                                                else
                                                {
													Backflightlistitems[m].CorporateFare = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
													Backflightlistitems[m].CorporateTrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
												if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                {

														Backflightlistitems[m].CorporateFareType = "Refundable";

                                                    
                                                }
                                                else
                                                {
                                                    Backflightlistitems[m].CorporateFareType = "Non-Refundable";
                                                }
                                                if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                {
                                                    Backflightlistitems[m].MealIcon = "meal.png";
                                                }
                                                else
                                                {
                                                    Backflightlistitems[m].MealIcon = "nomeal.png";
                                                }
                                                if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                {
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                    {
                                                        Backflightlistitems[m].BaggageIcon = "b15.png";
                                                    }
                                                    else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                    {
                                                        Backflightlistitems[m].BaggageIcon = "checkin15kg.png";
                                                    }
                                                    else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                    }
                                                    else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                    }
                                                    else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                    }
                                                    else
                                                    {
														Backflightlistitems[m].BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                    }

                                                }
                                                else
                                                {
                                                    Backflightlistitems[m].BaggageIcon = "checkin15kg.png";
                                                }
													Backflightlistitems[m].CheapFare1 = "NA";
													Backflightlistitems[m].CheapFare2 = "NA";
												Backflightlistitems[m].Cheap1FareType = "NA";
												Backflightlistitems[m].Cheap2FareType = "NA";
												Backflightlistitems[m].Cheap2MealIcon = "NA.png";
												Backflightlistitems[m].Cheap2BaggageIcon = "NA.png";
												Backflightlistitems[m].Cheap1MealIcon = "NA.png";
                                                Backflightlistitems[m].Cheap1BaggageIcon = "NA.png";

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
												Backflightlistitems[m].MealIcon = "NA.png";
												Backflightlistitems[m].BaggageIcon = "NA.png";
												if (Backflightlistitems[m].CheapFare1 == "" || Backflightlistitems[m].CheapFare1 == null)
                                                    {
														Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
														Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
													if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                    {
														if (BackmainFareDetails[n].samefareDetail[p].TotalAmount <= Convert.ToDecimal(3500))
                                                        {
															Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
															Backflightlistitems[m].Cheap1FareType = "Refundable";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                    {
                                                        Backflightlistitems[m].Cheap1MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                    {
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
                                                    }
                                                    }
                                                    else
                                                    {
														Backflightlistitems[m].Cheap2TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
													if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                    {
														if (BackmainFareDetails[n].samefareDetail[p].TotalAmount <= Convert.ToDecimal(3500))
                                                        {
                                                            Backflightlistitems[m].Cheap2FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].Cheap2FareType = "Refundable";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap2FareType = "Non-Refundable";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                    {
                                                        Backflightlistitems[m].Cheap2MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap2MealIcon = "nomeal.png";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                    {
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                        {
                                                            Backflightlistitems[m].Cheap2BaggageIcon = "b15.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                        {
                                                            Backflightlistitems[m].Cheap2BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else
                                                       if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else 
                                                        {
                                                            Backflightlistitems[m].Cheap2BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap2BaggageIcon = "checkin15kg.png";
                                                    }
												}
                                                }
                                                else
                                                {
												if (Backflightlistitems[m].CorporateFare=="NA")
                                                    {
														Backflightlistitems[m].CorporateTrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CorporateFare = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
                                                   
													if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                    {
                                                        Backflightlistitems[m].FareType = "Refundable";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].FareType = "Non-Refundable";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                    {
                                                        Backflightlistitems[m].MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].MealIcon = "nomeal.png";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                    {
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                        {
                                                            Backflightlistitems[m].BaggageIcon = "b15.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                        {
                                                            Backflightlistitems[m].BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].BaggageIcon = "checkin15kg.png";
                                                    }
												
												
												}
                                                    else
                                                    {
													//	Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
													//	Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
													//if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
              //                                      {
              //                                          Backflightlistitems[m].Cheap1FareType = "Refundable";
              //                                      }
              //                                      else
              //                                      {
              //                                          Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
              //                                      }
              //                                      if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
              //                                      {
              //                                          Backflightlistitems[m].Cheap1MealIcon = "meal.png";
              //                                      }
              //                                      else
              //                                      {
              //                                          Backflightlistitems[m].Cheap1MealIcon = "nomeal.png";
              //                                      }
              //                                      if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
              //                                      {
              //                                          if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
              //                                          {
              //                                              Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
              //                                          }
              //                                          else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
              //                                          {
              //                                              Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
              //                                          }
														//else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces"))
                                                    //    {
                                                    //        Backflightlistitems[m].Cheap1BaggageIcon = "onepc.png";
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        Backflightlistitems[m].Cheap1BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                    //    }

                                                    //}
                                                    //else
                                                    //{
                                                    //    Backflightlistitems[m].Cheap1BaggageIcon = "NA.png";
                                                    //}
												}

													Backflightlistitems[m].CheapFare2 = "NA";
												Backflightlistitems[m].Cheap2FareType = "NA";
												Backflightlistitems[m].Cheap2MealIcon = "NA.png";
												Backflightlistitems[m].Cheap2BaggageIcon = "NA.png";
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
												Backflightlistitems[m].MealIcon = "NA.png";
												Backflightlistitems[m].BaggageIcon = "NA.png";

												if (Backflightlistitems[m].CheapFare1 == "" || Backflightlistitems[m].CheapFare1 == null)
                                                    {
														Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
                                                   
													if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                    {
														if (BackmainFareDetails[n].samefareDetail[p].TotalAmount <= Convert.ToDecimal(3500))
                                                        {
                                                            Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].Cheap1FareType = "Refundable";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                    {
                                                        Backflightlistitems[m].Cheap1MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                    {
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
                                                    }
												
												}
												else if (Backflightlistitems[m].CheapFare2 == "" || Backflightlistitems[m].CheapFare2 == null)
                                                    {
														Backflightlistitems[m].Cheap2TrackNo =BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
													if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                    {
														if (BackmainFareDetails[n].samefareDetail[p].TotalAmount <= Convert.ToDecimal(3500))
                                                        {
                                                            Backflightlistitems[m].Cheap2FareType = "Non-Refundable";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].Cheap2FareType = "Refundable";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap2FareType = "Non-Refundable";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                    {
                                                        Backflightlistitems[m].Cheap2MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap2MealIcon = "nomeal.png";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                    {
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                        {
                                                            Backflightlistitems[m].Cheap2BaggageIcon = "b15.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                        {
                                                            Backflightlistitems[m].Cheap2BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("2PC"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].Cheap2BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].Cheap2BaggageIcon = "checkin15kg.png";
                                                    }
												
												
												}
                                                    else
                                                    {

                                                    }
                                                }
                                                else
                                                {
												if (Backflightlistitems[m].CorporateFare=="NA")
                                                    {
														Backflightlistitems[m].CorporateTrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
														Backflightlistitems[m].CorporateFare = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
													if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
                                                    {
                                                        Backflightlistitems[m].FareType = "Refundable";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].FareType = "Non-Refundable";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
                                                    {
                                                        Backflightlistitems[m].MealIcon = "meal.png";
                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].MealIcon = "nomeal.png";
                                                    }
                                                    if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
                                                    {
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
                                                        {
                                                            Backflightlistitems[m].BaggageIcon = "b15.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
                                                        {
                                                            Backflightlistitems[m].BaggageIcon = "checkin15kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces") || BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1PC"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else
                                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("35"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin35kg.png";
                                                        }
                                                        else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("30"))
                                                        {
                                                            Backflightlistitems[m].Cheap1BaggageIcon = "checkin30kg.png";
                                                        }
                                                        else
                                                        {
                                                            Backflightlistitems[m].BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Backflightlistitems[m].BaggageIcon = "checkin15kg.png";
                                                    }
												
												
												
												
												}
            //                                        else if (p == 1)
            //                                        {
												//		Backflightlistitems[m].Cheap1TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
												//		Backflightlistitems[m].CheapFare1 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
												//	if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
            //                                        {
            //                                            Backflightlistitems[m].Cheap1FareType = "Refundable";
            //                                        }
            //                                        else
            //                                        {
            //                                            Backflightlistitems[m].Cheap1FareType = "Non-Refundable";
            //                                        }
            //                                        if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
            //                                        {
            //                                            Backflightlistitems[m].Cheap1MealIcon = "meal.png";
            //                                        }
            //                                        else
            //                                        {
            //                                            Backflightlistitems[m].Cheap1MealIcon = "nomeal.png";
            //                                        }
            //                                        if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
            //                                        {
            //                                            if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
            //                                            {
            //                                                Backflightlistitems[m].Cheap1BaggageIcon = "b15.png";
            //                                            }
            //                                            else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
            //                                            {
            //                                                Backflightlistitems[m].Cheap1BaggageIcon = "checkin15kg.png";
            //                                            }
												//		else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces"))
            //                                            {
            //                                                Backflightlistitems[m].Cheap1BaggageIcon = "onepc.png";
            //                                            }
            //                                            else
            //                                            {
            //                                                Backflightlistitems[m].Cheap1BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
            //                                            }

            //                                        }
            //                                        else
            //                                        {
            //                                            Backflightlistitems[m].Cheap1BaggageIcon = "NA.png";
            //                                        }
												//}
                                                    else
                                                    {
													//	Backflightlistitems[m].Cheap2TrackNo = BackmainFareDetails[n].samefareDetail[p].TrackNo;
													//	Backflightlistitems[m].CheapFare2 = "Rs. " + Convert.ToString(Math.Round(BackmainFareDetails[n].samefareDetail[p].TotalAmount, 0));
                                                   
													//if (BackmainFareDetails[n].samefareDetail[p].FareType == "R")
              //                                      {
              //                                          Backflightlistitems[m].Cheap2FareType = "Refundable";
              //                                      }
              //                                      else
              //                                      {
              //                                          Backflightlistitems[m].Cheap2FareType = "Non-Refundable";
              //                                      }
              //                                      if (BackmainFareDetails[n].samefareDetail[p].FlightRemarks != "")
              //                                      {
              //                                          Backflightlistitems[m].Cheap2MealIcon = "meal.png";
              //                                      }
              //                                      else
              //                                      {
              //                                          Backflightlistitems[m].Cheap2MealIcon = "nomeal.png";
              //                                      }
              //                                      if (BackmainFareDetails[n].samefareDetail[p].Baggage != "" && !string.IsNullOrEmpty(BackmainFareDetails[n].samefareDetail[p].Baggage))
              //                                      {
              //                                          if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("25"))
              //                                          {
              //                                              Backflightlistitems[m].Cheap2BaggageIcon = "b15.png";
              //                                          }
              //                                          else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("15"))
              //                                          {
              //                                              Backflightlistitems[m].Cheap2BaggageIcon = "checkin15kg.png";
              //                                          }
														//else if (BackmainFareDetails[n].samefareDetail[p].Baggage.Contains("1Pieces"))
                                                    //    {
                                                    //        Backflightlistitems[m].Cheap2BaggageIcon = "onepc.png";
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        Backflightlistitems[m].Cheap2BaggageIcon = BackmainFareDetails[n].samefareDetail[p].Baggage;
                                                    //    }

                                                    //}
                                                    //else
                                                    //{
                                                    //    Backflightlistitems[m].Cheap2BaggageIcon = "NA.png";
                                                    //}
												}
                                                }
                                            }
                                        }
                                    }

                                }
                            
						}
						 flightlist.ItemsSource = flightlistitems;
						 	returnlist.ItemsSource = Backflightlistitems;
							returnlistpage.ItemsSource = Backflightlistitems;
							departlist.ItemsSource = flightlistitems;
                        
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Error Occured", "", "OK");
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
        }
        
		public  void Handle_Tapped(object sender, EventArgs args)
		{ StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];
            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag = 0;
                flightno1 = emailLabel.Text;

            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();


            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;

                flag = 0;
                trackno1 = emailLabel.Text;

            }
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
                departprice.Text = emailLabel.Text;
				if (!string.IsNullOrEmpty(returnprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(departprice.Text)));

                }
            }
        
        
        
        
        
        }

        public  void Handle_Tapped2(object sender, EventArgs args)
		{StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag = 2;
                flightno1 = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;
                
                flag = 2;
                trackno1 = emailLabel.Text;

            }
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
                departprice.Text = emailLabel.Text;
				if (!string.IsNullOrEmpty(returnprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(departprice.Text)));

                }
            } }

		public  void Handle_Tapped1(object sender, EventArgs args)
		{
			StackLayout label = (StackLayout)sender;
			var label1 = label.Children;
			var reqLabel = label1[0];

			var theLabel = reqLabel.GetType();

			if (theLabel == typeof(Label))
			{
				Label emailLabel = (Label)reqLabel;
				flag = 1;
				flightno1 = emailLabel.Text;
			}
			var reqLabel1 = label1[1];

			var theLabel1 = reqLabel1.GetType();

			if (theLabel1 == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel1;

				flag = 1;
				trackno1 = emailLabel.Text;
                
			}
			var reqLabel2 = label1[2];
            
            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {
                
                Label emailLabel = (Label)reqLabel2;
				pricetag.IsVisible = true;
				departprice.Text = emailLabel.Text;
				if (!string.IsNullOrEmpty(returnprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
					totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(departprice.Text)));

                }
            }
		}
		public void Handle_Tapped3(object sender, EventArgs args)
        {
            StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag2 = 4;
                flightno2 = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;

                flag2 = 4;
                trackno2 = emailLabel.Text;

            }
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
				returnprice.Text = emailLabel.Text;
				if(!string.IsNullOrEmpty(departprice.Text)){
					totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

				}else{
					totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(returnprice.Text)));

				}
            }
        }

		public void Handle_Tapped4(object sender, EventArgs args)
        {
            StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag2 = 3;
                flightno2 = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;
                
                flag2 = 3;
                trackno2 = emailLabel.Text;

            }
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
                returnprice.Text = emailLabel.Text;
                if (!string.IsNullOrEmpty(departprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
            }
        }
		public void Handle_Tapped5(object sender, EventArgs args)
        {
            StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag2 = 4;
                flightno2 = emailLabel.Text;
            }
            var reqLabel1 = label1[1];
            
            var theLabel1 = reqLabel1.GetType();
            
            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;

                flag2 = 4;
                trackno2 = emailLabel.Text;

            }
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
                returnprice.Text = emailLabel.Text;
                if (!string.IsNullOrEmpty(departprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
            }
        }
		public void Handle_Tapped6(object sender, EventArgs args)
        {
            StackLayout label = (StackLayout)sender;
			label.BackgroundColor = Color.Gray;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag2 = 5;
                flightno2 = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;

                flag2 = 5;
                trackno2 = emailLabel.Text;

            }
			var reqLabel3 = label1[3];
            
            var theLabel3 = reqLabel3.GetType();
            
			if (theLabel3 == typeof(Label))
			{
				Label emailLabel = (Label)reqLabel1;

			}
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
				emailLabel.BackgroundColor = Color.Gray;
				returnprice.Text = emailLabel.Text;
				if (!string.IsNullOrEmpty(departprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
					totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
            }
        }
		public void Handle_Tapped7(object sender, EventArgs args)
        {
            StackLayout label = (StackLayout)sender;
            var label1 = label.Children;
            var reqLabel = label1[0];

            var theLabel = reqLabel.GetType();

            if (theLabel == typeof(Label))
            {
                Label emailLabel = (Label)reqLabel;
                flag = 1;
                flightno1 = emailLabel.Text;
            }
            var reqLabel1 = label1[1];

            var theLabel1 = reqLabel1.GetType();

            if (theLabel1 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel1;
                
                flag = 1;
				trackno1 = emailLabel.Text;

            }
            var reqLabel2 = label1[2];

            var theLabel2 = reqLabel2.GetType();

            if (theLabel2 == typeof(Label))
            {

                Label emailLabel = (Label)reqLabel2;
                pricetag.IsVisible = true;
				departprice.Text = emailLabel.Text;
				if (!string.IsNullOrEmpty(returnprice.Text))
                {
                    totalflightprice.Text = "Rs. " + Convert.ToString(Convert.ToInt16(Splitmethod(departprice.Text)) + Convert.ToInt16(Splitmethod(returnprice.Text)));

                }
                else
                {
					totalflightprice.Text = "Rs. " + Convert.ToString(0 + Convert.ToInt16(Splitmethod(departprice.Text)));

                }
            }
        }
		public async void Ondetaillabeltapped(object sender, EventArgs args)
		{
			string flightno = null;
			int detailflag = 2;
			StackLayout label = (StackLayout)sender;
			var labelarray = label.Children;
			var reqLabel = labelarray[0];

			var Label1 = reqLabel.GetType();

			if (Label1 == typeof(Label))
			{
				Label label1 = (Label)reqLabel;
				flightno = label1.Text;
			}
			var reqLabel1 = labelarray[1];

			var Label2 = reqLabel1.GetType();
            int stops = 0;
			if (Label2 == typeof(Label))
			{
				Label label2 = (Label)reqLabel1;
                await Navigation.PushPopupAsync(new DetailsPopUp(stops,flightno,detailflag,0, modelproperties));
			}
		}
		public async void Ondetaillabeltapped1(object sender, EventArgs args)
        {
            string flightno = null;
			int detailflag = 2;
			int detailflagreturn = 3;
            StackLayout label = (StackLayout)sender;
            var labelarray = label.Children;
            var reqLabel = labelarray[0];
            
            var Label1 = reqLabel.GetType();

            if (Label1 == typeof(Label))
            {
                Label label1 = (Label)reqLabel;
                flightno = label1.Text;
            }
            var reqLabel1 = labelarray[1];

            var Label2 = reqLabel1.GetType();
            int stops = 0;
            if (Label2 == typeof(Label))
            {
                Label label2 = (Label)reqLabel1;
				await Navigation.PushPopupAsync(new DetailsPopUp(stops,flightno, detailflag,detailflagreturn, modelproperties));
            }
        }
      public string Splitmethod(string   value){

			string[] parts = value.Split('.');
					string val1 = parts[1].Trim();
			return val1;
				}                                                                  

		public	async void Book_Clicked(object sender, System.EventArgs e)
		{
			
            
				if (!string.IsNullOrEmpty(returnprice.Text) && !string.IsNullOrEmpty(departprice.Text))
				{
				Console.WriteLine(trackno1 + "  " + trackno2);

			
			
				await Navigation.PushAsync(new RoundTripBookPage(modelproperties, flightno1, flightno2, trackno1, trackno2, flag,flag2));
				//this.Content = MainLayout;
			    }
				else if (string.IsNullOrEmpty(returnprice.Text) && !string.IsNullOrEmpty(departprice.Text))
				{
				  UserDialogs.Instance.Alert("Please Select your returning flight");
				}
				else if (!string.IsNullOrEmpty(returnprice.Text) && string.IsNullOrEmpty(departprice.Text))
				{
					UserDialogs.Instance.Alert("Please Select your departure flight", "", "Ok");
				}
				else
				{
					UserDialogs.Instance.Alert("Please Select your departure  and returning flight", "", "Ok");
				}

		}
    }
}
