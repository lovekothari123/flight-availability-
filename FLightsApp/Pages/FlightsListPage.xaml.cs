using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FLightsApp.Models;
using FLightsApp.Webservice;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace FLightsApp.Pages
{

	public partial class FlightsListPage : ContentPage
	{
		int flag = 0; string trackno = null, flightno = null, amount = null;
		MainModel modelproperties = new MainModel();
		List<MainModel> flightlistitems = new List<MainModel>();
		List<MainFareDetail> mainFareDetails = new List<MainFareDetail>();
		List<FareDetail> fareDetails = new List<FareDetail>();
		List<string> vs = new List<string>();
		PlaceList placeList = new PlaceList();
		List<int> stopfilterlist = new List<int>();
        string fromt, tot;
		public FlightsListPage(MainModel model, string date, string returndate)
		{

			InitializeComponent();
			modelproperties = model;
			from.Text = model.FromOneway;
			To.Text = model.ToOneway;
			Adultno.Text = "Adult : " + Convert.ToString(model.NoOfAdult);
			childno.Text = "Child : " + Convert.ToString(model.NoOfChild);
			infantno.Text = "Infant : " + Convert.ToString(model.NoOfInfant);
			DateTime dateTime = new DateTime();
			//	dateObject.ToString("dd MMM");
			dateTime = Convert.ToDateTime(date);
			titlecode.Text = model.FromOneway + " : " + model.ToOneway + ", " + dateTime.ToString("dd MMM");
			//toolbardate.Text = dateTime.ToString("dd MMM");
			GetFlightList(model);

			var arrowtap = new TapGestureRecognizer();
			arrowtap.Tapped += async (s, e) =>
			{
				//await roundarrow.RotateTo(360, 200);
			};
			roundarrow.GestureRecognizers.Add(arrowtap);

			var filtertap = new TapGestureRecognizer();
			filtertap.Tapped += async (s, e) =>
			{


				FilterByPage page = new FilterByPage(vs, stopfilterlist);
				page.OnSelectedCity += Page_OnSelected;
				await Navigation.PushPopupAsync(page);
			};
			filter.GestureRecognizers.Add(filtertap);

			var gobacktap = new TapGestureRecognizer();
			gobacktap.Tapped += async (s, e) =>
			{
				await Navigation.PopAsync();
			};
			goback.GestureRecognizers.Add(gobacktap);

		}
		internal void Page_OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var filteritem = (MainModel)(sender);


			if (filteritem.Flightfilterlst.Count != 0 && filteritem.Stopfilterlst.Count != 0)
			{
				//var data = flightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName)).ToList();
				var data = flightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName) && filteritem.Stopfilterlst.Contains(X.Stops)).ToList();
				if (data.Count != 0) { flightlist.ItemsSource = data; }
				else
				{
					UserDialogs.Instance.Alert("No Flights Found", "", "OK");
				}
			}
			else if (filteritem.Flightfilterlst.Count != 0 && filteritem.Stopfilterlst.Count == 0)
			{
				var data = flightlistitems.Where(X => filteritem.Flightfilterlst.Contains(X.AirlineFullName)).ToList();
				if (data.Count != 0) { flightlist.ItemsSource = data; }
				else
				{
					UserDialogs.Instance.Alert("No Flights Found", "", "OK");
				}
			}
			else if (filteritem.Flightfilterlst.Count == 0 && filteritem.Stopfilterlst.Count != 0)
			{
				var data = flightlistitems.Where(X => filteritem.Stopfilterlst.Contains(X.Stops)).ToList();
				if (data.Count != 0) { flightlist.ItemsSource = data; }
				else
				{
					UserDialogs.Instance.Alert("No Flights Found", "", "OK");
				}
			}
			else
			{
				flightlist.ItemsSource = flightlistitems;
			}


		}

		public async void Ondetaillabeltapped(object sender, EventArgs args)
		{
         
			string flightno = null;
			int detailflag = 1;
            string stops = null;
            string result = null;
			
            StackLayout label = (StackLayout)sender;
			var labelarray = label.Children;
			var reqLabel = labelarray[0];
            var redstop = labelarray[3];

            var Label3 = redstop.GetType();
			var Label1 = reqLabel.GetType();


			if (Label1 == typeof(Label))
			{
				Label label1 = (Label)reqLabel;
				flightno = label1.Text;
			}
			var reqLabel1 = labelarray[1];

			var Label2 = reqLabel1.GetType();


            // stop gettting value 
            if(Label3 ==typeof(Label)){

                Label label3 = (Label)redstop;
                stops = label3.Text;

                string[] arr = stops.Split(' ');
                result = arr[0];
               
            }

			if (Label2 == typeof(Label))
			{
				Label label2 = (Label)reqLabel1;
              
                int stopss = Convert.ToInt32(result);
                await Navigation.PushPopupAsync(new DetailsPopUp(stopss,flightno, detailflag, 0, modelproperties));
			}



		}
        public int firsttype(string value){

            string[] arr = value.Split(':');
            string frist = arr[0];
            string second = arr[1];
            int convertfirst = Convert.ToInt32(frist);
            return convertfirst;
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


                    if (modelvalue.onewayarrivaltime != null)
                    {
                        request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound + "',TripType:'" + modelvalue.TripType + "',FlightClass:'" + modelvalue.FlightClassrequest + "'},EstimateArrivalTime:'" + modelvalue.onewayarrivaltime + "',EstimateDeptTime:'" + null + "'}";
                    }
                    else
                    {
                        request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound + "',TripType:'" + modelvalue.TripType + "',FlightClass:'" + modelvalue.FlightClassrequest + "'},EstimateArrivalTime:" + modelvalue.onewayarrivaltime + ",EstimateDeptTime:'" + modelvalue.onewaydeparturetime + "'}";

                    }

                    Console.WriteLine("Request--->  " + request);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(WebserviceUrls.GETFLIGHTAVAILABILITYURL, content);
                    Console.WriteLine("Request ke bad response" + response.IsSuccessStatusCode);
                    Console.WriteLine("Request ke bad response" + response.StatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        var result = await response.Content.ReadAsStringAsync();

                        if (result != "")
                        {
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
                        flightlistitems = new List<MainModel>(placeList.FilterFlightList);





                        //for (int k = 0; k <flightlistitems.Count;k++){


                        //    TimeSpan ts;
                        //    if (!TimeSpan.TryParse(flightlistitems[k].ArrTime, out ts))
                        //    {
                        //        // throw exception or whatnot

                        //    }
                        //    TimeSpan tssecond;
                        //    if (!TimeSpan.TryParse("02:00:00", out tssecond))
                        //    {
                        //        // throw exception or whatnot

                        //    }

                        //    TimeSpan thirdtime = ts - tssecond;
                        //    int hourslist = ts.Hours;
                        //    int hourstatis = thirdtime.Hours;
                        //    if(hourslist==hourstatis){

                        //        Console.WriteLine("smae hours" + flightlistitems[k].ArrTime);

                        //    }else{
                        //        Console.WriteLine("smae hours else" + flightlistitems[k].ArrTime);
                        //    }

                        //}

                        bool spicejetfound = flightlistitems.Any(s => s.AirlineFullName == "Spice Jet");
                        bool jetfound = flightlistitems.Any(s => s.AirlineFullName == "Jet Airways");
                        bool airfound = flightlistitems.Any(s => s.AirlineFullName == "Air India");
                        bool goairfound = flightlistitems.Any(s => s.AirlineFullName == "Go Air");
                        bool indigofound = flightlistitems.Any(s => s.AirlineFullName == "Indigo");
                        bool zerostopfound = flightlistitems.Any(s => s.Stops == 0);
                        bool onestopfound = flightlistitems.Any(s => s.Stops == 1);
                        bool twostopfound = flightlistitems.Any(s => s.Stops == 2);
                        //flightlistitems = flightlistitems.Where(X => X.AirlineFullName == "Spice Jet").ToList();
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
                            vs.Add("Indigo");
                        }


                        mainFareDetails = new List<MainFareDetail>(placeList.SameFlightList);

                        for (int i = 0; i < flightlistitems.Count; i++)
                        {


                            //new code for next day
                            string dateone = flightlistitems[i].DepDate;
                            string datesecond = flightlistitems[i].ArrDate;
                          
                            //flightlistitems[i].chnageColor = "#F7EFEE";
                            if (dateone == datesecond)
                            {

                            }
                            else
                            {
                                flightlistitems[i].NextDay = "NA";
                            }


                            string arrivalDate = flightlistitems[i].ArrDate;

                            string departureDate = flightlistitems[i].DepDate;

                            TimeSpan timeone;
                            if (!TimeSpan.TryParse(flightlistitems[i].ArrTime, out timeone))
                            {
                                // throw exception or whatnot

                            }



                            TimeSpan timesecond;
                            if (!TimeSpan.TryParse(flightlistitems[i].DepTime, out timesecond))
                            {
                                // throw exception or whatnot

                            }
                            //end code ///
                            // background color code change 
                            string timedepconvert = timesecond.ToString(@"hh\:mm");
                            string deprdatetime = departureDate + ' ' + timedepconvert;
                            string timearriconvert = timeone.ToString(@"hh\:mm");
                            string arridatetime = arrivalDate + ' ' + timearriconvert;
                            //Console.WriteLine("one and two ");
                            //DateTime stdatetime = Convert.ToDateTime(deprdatetime, new CultureInfo("en-IN"));
                            //DateTime enddatetime = Convert.ToDateTime(arridatetime, new CultureInfo("en-IN"));
                            DateTime startDate = DateTime.Parse(deprdatetime, new CultureInfo("en-IN"));
                            DateTime endDate = DateTime.Parse(arridatetime, new CultureInfo("en-IN"));
                            TimeSpan difftime = endDate.Subtract(startDate);
                            //Console.WriteLine("after convert time diffrence ");
                            //string flightTime = difftime.ToString(@"hh\:mm");
                            //flightlistitems[i].FlightTime = flightTime;
                            //  code end ///
                            flightlistitems[i].FlightTime = (int)difftime.TotalHours + "hr " + difftime.Minutes + "m";


                            flightlistitems[i].space = "  ";

                            flightlistitems[i].view = "VIEW MORE";

                            flightlistitems[i].flightnamecode = flightlistitems[i].AirlineCode + " - " + flightlistitems[i].FlightNo;


                            if (flightlistitems[i].FareType == "R")
                            {
                                flightlistitems[i].FareType = "Refundable";
                            }
                            else
                            {
                                flightlistitems[i].FareType = "Non-Refundable";
                            }
                            //TimeSpan tspan = TimeSpan.FromMinutes(Convert.ToDouble(flightlistitems[i].FlightTime)); //converting minutes to timespan
                            //flightlistitems[i].FlightTime = (int)tspan.TotalHours + "hr " + tspan.Minutes + "m";
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
                                {
                                    int count = 1;

                                    for (int q = 0; q < data1.Count; q++)
                                    {
                                        bool isFound = mainFareDetails[j].samefareDetail.Intersect(data1).Any();
                                        if (!isFound)
                                        {
                                            if (count == 1)
                                            {
                                                mainFareDetails[j].samefareDetail.Add(data1[q]);
                                                count = 0;
                                            }

                                        }

                                    }
                                    mainFareDetails[j].samefareDetail.Add(fareDetails1[p]);

                                }
                                if (mainFareDetails[j].FlightNo == flightlistitems[i].FlightNo)
                                {

                                    Application.Current.Properties["myList"] = mainFareDetails[j].fltDetails;


                                    //new code 



                                
                                      
                                          
                                            
                                    //}else
                                    ///////   new code exit
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
                                                else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                }
                                                else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                }
                                                else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                }
                                                else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                }
                                                else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                {
                                                    flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                    {                                                         flightlistitems[i].BaggageIcon = "b15.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                        {                                                             flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                        {                                                             flightlistitems[i].Cheap2BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                        {                                                             flightlistitems[i].BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                        {                                                             flightlistitems[i].Cheap1BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks == null)
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";

                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Spice Saver"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
                                                    }
                                                    else if (mainFareDetails[j].samefareDetail[k].FlightRemarks.Equals("Special Fare"))
                                                    {
                                                        flightlistitems[i].Cheap1MealIcon = "nomeal.png";
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
                                                        {                                                             flightlistitems[i].BaggageIcon = "b15.png";
                                                        }
                                                        else if (mainFareDetails[j].samefareDetail[k].Baggage.Contains("35"))
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
                            flightlist.ItemsSource = flightlistitems;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        Console.WriteLine("else api response ");
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
                UserDialogs.Instance.Alert(placeList.ErrorMessage, "", "OK");
                Console.WriteLine("cathc api response exception "+ e.Message);
               
               
            }
        }
		public async void OnframeTapped(object sender, EventArgs args)
		{


			StackLayout label = (StackLayout)sender;
			label.BackgroundColor = Xamarin.Forms.Color.Gray;
			var label1 = label.Children;
			var reqLabel = label1[0];

			var theLabel = reqLabel.GetType();

			if (theLabel == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel;

				flag = 1;
				flightno = emailLabel.Text;

			}
			var reqLabel1 = label1[1];

			var theLabel1 = reqLabel1.GetType();

			if (theLabel1 == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel1;

				flag = 1;
				trackno = emailLabel.Text;
                fromt = from.Text;
                tot = To.Text;

			}
			var reqLabel2 = label1[2];
			var theLabel2 = reqLabel2.GetType();

			if (theLabel2 == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel2;

				flag = 1;
				amount = emailLabel.Text;
                fromt = from.Text;
                tot = To.Text;

			}

            await Navigation.PushAsync(new BookNowPage(modelproperties, flightno, trackno, flag, amount,fromt,tot));


		}
		public async void OnframeTapped1(object sender, EventArgs args)
		{
			StackLayout label = (StackLayout)sender;
			label.BackgroundColor = Xamarin.Forms.Color.Gray;
			var label1 = label.Children;
			var reqLabel = label1[0];

			var theLabel = reqLabel.GetType();

			if (theLabel == typeof(Label))
			{
				Label emailLabel = (Label)reqLabel;
				flag = 2;
				flightno = emailLabel.Text;
			}
			var reqLabel1 = label1[1];

			var theLabel1 = reqLabel1.GetType();

			if (theLabel1 == typeof(Label))
			{
				Label emailLabel = (Label)reqLabel1;
				flag = 2;
				trackno = emailLabel.Text;
                fromt = from.Text;
                tot = To.Text;
			}
			var reqLabel2 = label1[2];
			var theLabel2 = reqLabel2.GetType();

			if (theLabel2 == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel2;

				flag = 2;
				amount = emailLabel.Text;
                fromt = from.Text;
                tot = To.Text;

			}
            await Navigation.PushAsync(new BookNowPage(modelproperties, flightno, trackno, flag, amount,fromt,tot));
		}

		public async void OnframeTapped2(object sender, EventArgs args)
		{
			StackLayout label = (StackLayout)sender;
			label.BackgroundColor = Xamarin.Forms.Color.Gray;
			var label1 = label.Children;
			var reqLabel = label1[0];

			var theLabel = reqLabel.GetType();

			if (theLabel == typeof(Label))
			{
				Label emailLabel = (Label)reqLabel;
				flag = 0;
				flightno = emailLabel.Text;
			}
			var reqLabel1 = label1[1];

			var theLabel1 = reqLabel1.GetType();

			if (theLabel1 == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel1;

				flag = 0;
				trackno = emailLabel.Text;
                fromt = from.Text;
                tot = To.Text;

			}
			var reqLabel2 = label1[2];
			var theLabel2 = reqLabel2.GetType();

			if (theLabel2 == typeof(Label))
			{

				Label emailLabel = (Label)reqLabel2;

				flag = 0;
				amount = emailLabel.Text;
                fromt = from.Text;
                tot = To.Text;

			}
            await Navigation.PushAsync(new BookNowPage(modelproperties, flightno, trackno, flag, amount,fromt,tot));
		}

       
	}
}
    