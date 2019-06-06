using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public partial class BookNowPage : ContentPage
    {
        int listcount = 0;
        List<FlightDetail> flightDetailsitems = new List<FlightDetail>();
        List<FlightDetail> flightDetailsitems1 = new List<FlightDetail>();
        BookingDetailsData bookingDetails = new BookingDetailsData();
        AdditionalService additionalServices = new AdditionalService();
        List<BookingPassengerDetails> passengerDetails = new List<BookingPassengerDetails>();
        string trackamount = null, mainclass = null, tracknumber = null;
        string totalticketAmount = null, flightNo = null, trackvalue = null, paxno = null;
        int mealamount = 0, baggageamount = 0, totalprice = 0;
        MainModel modelproperty = new MainModel();
        BookTicketResponse bookTicketResponse = new BookTicketResponse();
        List<MainModel> flightlistitems = new List<MainModel>();
        List<MainFareDetail> mainFareDetails = new List<MainFareDetail>();
        List<FareDetail> fareDetails = new List<FareDetail>();
        int amountFinalTotal;
        string fromtext, totext;
        BaggageDetail subitemSelectedb;
        MealDetail subitemSelectedm;
        public BookNowPage(MainModel model, string flightno, string trackNo, int flag, string flightamount,string fromt,string tot)
        {
            InitializeComponent();
            flightNo = flightno;
            mainclass = model.FlightClassrequest;
            modelproperty = model;
            tracknumber = trackNo;
            this.fromtext = fromt;
            this.totext = tot;
          
            if (Application.Current.Properties.ContainsKey("id"))
            {
                bookingDetails.UserId = Convert.ToInt16(Application.Current.Properties["id"]);

            }
            if (Application.Current.Properties.ContainsKey("count"))
            {
                listcount = Convert.ToInt16(Application.Current.Properties["count"]);

            }


            GetMealDetails(tracknumber, flightamount);

          
            var gobacktap = new TapGestureRecognizer();
            gobacktap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            backarrow.GestureRecognizers.Add(gobacktap);
            from.Text = fromt;
            to.Text=tot;

        }

        public async void GetMealDetails(string trackno, string trackAmount)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                    var client = new HttpClient();
                    string[] parts = trackAmount.Split(' ');
                    string trackAmt = parts[1];
                    string usertype = modelproperty.usersType;
                    int userid = modelproperty.usersId;

                    string request = @"{TrackNo :'" + trackno + "',TotalAmount :" + trackAmt + ",UserType :'" + usertype + "',UserId :" + userid + "}";
                    Console.WriteLine("melas request" + request);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    //UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                    HttpResponseMessage response = await client.PostAsync(WebserviceUrls.MEALDETAILS, content);
                   // UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                    if (response.IsSuccessStatusCode)
                    {
                       // UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                        var result = await response.Content.ReadAsStringAsync();
                        PlaceList placeList = new PlaceList();

                        if (result != "")
                        {
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
                        if (placeList.ErrorMessage != null || placeList.ErrorMessage != "")
                        {
                            additionalServices = placeList.AdditionalServiceSelect;
                           
                            bookingDetails.lstFlightDetail = placeList.lstFlightDetail;
                            //bookingDetails.lstFlightDetail = flightDetailsitems1;
                            passengerDetails = new List<BookingPassengerDetails>(placeList.LstbookingPassengerDetails);
                            bookingDetails.LstbookingPassengerDetails = passengerDetails;
                            //bookingDetails.lstAdditionServiceDetails = additionalServices.BaggageValueLst;

                            for (int k = 0; k < bookingDetails.lstFlightDetail.Count; k++)
                            {


                                flightcode.Text = bookingDetails.lstFlightDetail[k].AirlineCode + " " + bookingDetails.lstFlightDetail[k].FlightNo;
                                if (bookingDetails.lstFlightDetail[k].AirlineCode.Contains("SG"))
                                {
                                    flightimage.Source = "SG.png";
                                }
                                else if (bookingDetails.lstFlightDetail[k].AirlineCode.Contains("AI"))
                                {
                                    flightimage.Source = "AI.png";
                                }
                                else if (bookingDetails.lstFlightDetail[k].AirlineCode.Contains("6E"))
                                {
                                    flightimage.Source = "EE.png";
                                }
                                else if (bookingDetails.lstFlightDetail[k].AirlineCode.Contains("9W"))
                                {
                                    flightimage.Source = "WW.png";
                                }
                                else if (bookingDetails.lstFlightDetail[k].AirlineCode.Contains("UK"))
                                {
                                    flightimage.Source = "UK.png";
                                }
                                else
                                {
                                    flightimage.Source = "GG.png";

                                }

                            
                             
                                string convertedarrival = changetype(bookingDetails.lstFlightDetail[k].ArrDate, bookingDetails.lstFlightDetail[k].ArrTime);
                                string converteddepart = changetype(bookingDetails.lstFlightDetail[k].DepDate, bookingDetails.lstFlightDetail[k].DepTime);
                                deptdate.Text = converteddepart;
                                arrdate.Text = convertedarrival;
                                fare.Text = Convert.ToString(bookingDetails.lstFlightDetail[0].TotalAmount);



                             
                                //TODO new Airline name change with class
                                flightclass.Text = passengerDetails[0].LstFrequentFlyer[0].AirlineName;
                              
                                //airlinename.Text = flightlistitems[i].AirlineFullName;
                                //wholetotalamount.Text = "Total Amount :" + Convert.ToString(bookingDetails.lstFlightDetail[k].TotalAmount);
                                //
                           
                            }
                            Console.WriteLine("before for ");
                            for (int j = 0; j < passengerDetails.Count; j++)
                            {
                            Console.WriteLine("inside for ");
                                if (j == 0)
                                {
                                  
                                    passengerDetails[j].PaxNo = "Passenger" + Convert.ToString(j + 1);
                                    passengerDetails[j].Title = "Mr";
                                    passengerDetails[j].Airname = passengerDetails[j].LstFrequentFlyer[0].AirlineName;
                                    //trackvalue = passengerDetails[j].TrackNo;
                                    //fromname.Text = passengerDetails[j].FirstName;
                                    //lastname.Text = passengerDetails[j].LastName;
                                    Console.WriteLine("if j=0  ");

                                }
                                if (additionalServices != null)
                                {
                                    Console.WriteLine("if additionalservie not null  ");
                                    passengerDetails[j].AdditionalServiceSelect = additionalServices;
                                    if (additionalServices.MealValueLst.Count == 0)
                                    {
                                        //UserDialogs.Instance.Alert("Meals not found", "", "OK");
                                    }
                                    else
                                    {
                                        for (int i = 0; i < passengerDetails[j].AdditionalServiceSelect.MealValueLst.Count; i++)
                                        {
                                            passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealServiceName = passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealServiceName + " - " + Convert.ToString(Math.Round(Convert.ToDecimal(passengerDetails[j].AdditionalServiceSelect.MealValueLst[i].MealAmount), 0));
                                            //passengerDetails[j].AdditionalServiceSelect.MealValueLst[j] = additionalServices.MealValueLst[i];
                                            Console.WriteLine("iside e for loop i  ");
                                        }
                                        //meals.ItemsSource = additionalServices.MealValueLst;
                                    }
                                    if (additionalServices.BaggageValueLst.Count == 0)
                                    {
                                        // UserDialogs.Instance.Alert("Baggage not found", "", "OK");
                                    }
                                    else
                                    {


                                        for (int k = 0; k < passengerDetails[j].AdditionalServiceSelect.BaggageValueLst.Count; k++)
                                        {

                                            passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageServiceName = passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageServiceName + " - " + Convert.ToString(Math.Round(Convert.ToDecimal(passengerDetails[j].AdditionalServiceSelect.BaggageValueLst[k].BaggageAmount), 0));
                                        }
                                        //baggage.ItemsSource = additionalServices.BaggageValueLst;
                                    }

                                }
                                else
                                {
                                    //UserDialogs.Instance.Alert("Additional Service not Available", "", "Ok");
                                }
                            }
                            if (listcount > 1)
                            {
                            Console.WriteLine("inside if list count 1 ");
                                for (int l = 1; l < listcount; l++)
                                {
                                    passengerDetails.Add(new BookingPassengerDetails { PaxNo = "Passenger" + Convert.ToString(l + 1), AdditionalServiceSelect = additionalServices });
                                   
                                }


                            }
                            //for (int t = 0; t < passengerDetails.Count;t++){


                            //    passengerDetails[t].Airname = passengerDetails[t].LstFrequentFlyer[t].AirlineName;

                            //}
                          

                            pasengerlist.HeightRequest = 250 * passengerDetails.Count;
                            pasengerlist.ItemsSource = passengerDetails;
                            Console.WriteLine("listPassenger--->" + passengerDetails.Count);
                            UserDialogs.Instance.HideLoading();
                           


                            //for zero position set value 
                            int passsge = passengerDetails.Count;
                            Console.WriteLine("aamir-->" + passsge);
                            string amountadd = bookingDetails.lstFlightDetail[0].TotalAmount;
                            Console.WriteLine("aamir-->amount" + amountadd);
                            int d = (int)Math.Round(Convert.ToDecimal(amountadd), 0);
                           
                            int totalcount = d * passengerDetails.Count;
                            //amountFinalTotal = totalcount;
                            totalprice = totalcount;
                            Console.WriteLine("aamir-->grand" + totalcount);
                            totalticketamount.Text = "Total Ticket Amount : "+Convert.ToString(totalcount);

                            //bookingDetails.TotalAmount = amountFinalTotal;
                          
                      
                        GetFlightDetails(flightNo, modelproperty);

                        }

                        else
                        {
                            UserDialogs.Instance.Alert(placeList.ErrorMessage, "", "OK");
                        }

                    }
                }


            }
            catch (Exception e)
            {
                UserDialogs.Instance.HideLoading();
                pasengerlist.IsVisible = true;
                dnf.IsVisible = true;

            }

        }

        public string changetype(string date,string time){


          
            string[] arr = date.Split('/');
            string dayc = arr[1].TrimStart("0".ToCharArray());
            string mont = arr[0];
            string[] monthc = { " ", "jan", "feb", "mar", "apr", "may", "jun", "july", "aug", "sep", "oct", "nov", "dec" };
            for (int i = 1; i < monthc.Length; i++)
            {


                if (dayc.Contains(Convert.ToString(i)))
                {
                    date = mont+" "+monthc[i]+" "+time;
                   
                }


            }

            return date;
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


        public async void GetFlightDetails(string flightno, MainModel modelvalue)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    //UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                    var client = new HttpClient();

                    string request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound + "',TripType:'" + modelvalue.TripType + "',FlightClass:'" + modelvalue.FlightClassrequest + "'}}";
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(WebserviceUrls.GETFLIGHTAVAILABILITYURL, content);
                    if (response.IsSuccessStatusCode)
                    {
                        //UserDialogs.Instance.HideLoading();
                        var result = await response.Content.ReadAsStringAsync();
                        PlaceList placeList = new PlaceList();
                        if (result != "")
                        {
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
                        flightlistitems = new List<MainModel>(placeList.FilterFlightList);
                        mainFareDetails = new List<MainFareDetail>(placeList.SameFlightList);
                        int c = 0;

                        for (int j = 0; j < mainFareDetails.Count; j++)
                        {
                            c = 1;
                            if (mainFareDetails[j].FlightNo == flightno && c == 1)
                            {
                                flightDetailsitems = mainFareDetails[j].fltDetails;
                                for (int i = 0; i < flightDetailsitems.Count; i++)
                                {
                                    flightDetailsitems[i].TrackNo = tracknumber;
                                    flightDetailsitems[i].TotalAmount = totalticketAmount;
                                    flightDetailsitems[i].FlightClass = flightclass.Text;
                                    flightDetailsitems[i].MainClass = mainclass;
                                   

                                    //TimeSpan tspan = TimeSpan.FromMinutes(Convert.ToDouble(flightDetailsitems[i].FlightTime)); //converting minutes to timespan
                                    //flightDetailsitems[i].FlightTime = (int)tspan.TotalHours + ":" + tspan.Minutes;
                                    string deptdateformat = null, arrdateformat = null;
                                    if (string.IsNullOrEmpty(flightDetailsitems[i].DepDate))
                                    {

                                    }
                                    else
                                    {
                                        deptdateformat = split(flightDetailsitems[i].DepDate);
                                        flightDetailsitems[i].DeptTimeDate = deptdateformat + " / " + flightDetailsitems[i].DepTime;
                                    }
                                    if (string.IsNullOrEmpty(flightDetailsitems[i].ArrDate))
                                    {

                                    }
                                    else
                                    {
                                        arrdateformat = split(flightDetailsitems[i].ArrDate);
                                        flightDetailsitems[i].ArrTimeDate = arrdateformat + " / " + flightDetailsitems[i].ArrTime;
                                    }
                                    if (!string.IsNullOrEmpty(flightDetailsitems[i].FromTerminal))
                                    {
                                        flightDetailsitems[i].FromTerminal = "Terminal " + flightDetailsitems[i].FromTerminal;

                                    }
                                    else
                                    {
                                        flightDetailsitems[i].FromTerminal = "Terminal -";

                                    }
                                    if (!string.IsNullOrEmpty(flightDetailsitems[i].ToTerminal))
                                    {
                                        flightDetailsitems[i].ToTerminal = "Terminal " + flightDetailsitems[i].ToTerminal;

                                    }
                                    else
                                    {
                                        flightDetailsitems[i].ToTerminal = "Terminal -";
                                    }
                                    if (flightDetailsitems[i].IconPath == "")
                                    {

                                    }
                                    else
                                    {
                                        if (flightDetailsitems[i].IconPath.Contains("SG"))
                                        {
                                            flightDetailsitems[i].IconPath = "SG.png";
                                        }
                                        else if (flightDetailsitems[i].IconPath.Contains("AI"))
                                        {
                                            flightDetailsitems[i].IconPath = "AI.png";
                                        }
                                        else if (flightDetailsitems[i].IconPath.Contains("6E"))
                                        {
                                            flightDetailsitems[i].IconPath = "EE.png";
                                        }
                                        else if (flightDetailsitems[i].IconPath.Contains("9W"))
                                        {
                                            flightDetailsitems[i].IconPath = "WW.png";
                                        }
                                        else if (flightDetailsitems[i].IconPath.Contains("UK"))
                                        {
                                            flightDetailsitems[i].IconPath = "UK.png";
                                        }
                                        else
                                        {
                                            flightDetailsitems[i].IconPath = "GG.png";
                                        }

                                    }
                                }


                            }
                        }
                        //bookingDetails.lstFlightDetail = flightDetailsitems;
                        string[] splits = wholetotalamount.Text.Split(':');
                        if (!string.IsNullOrEmpty(splits[1].Trim()))
                        {
                            bookingDetails.TotalAmount = Convert.ToInt16(splits[1].Trim());
                            c = 2;
                        }

                        //bookingDetails.TotalAmount = Convert.ToInt16(totalticketAmount);
                        bookingDetails.TotalAmount = totalprice;

                    }



                    else
                    {
                        UserDialogs.Instance.Alert("No flight were found that match your criteria", "", "OK");
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
                Console.WriteLine("Error Ocuured");
            }

        }

        public async  void ConfirmBooking()
        {
            if (NetworkCheck.IsInternet())
            {
                var client = new HttpClient();

                bookingDetails.FinalTotalAmount = Convert.ToDouble(totalprice);
               

               
                var json = JsonConvert.SerializeObject(bookingDetails);
                Console.WriteLine("json" + json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                HttpResponseMessage response = await client.PostAsync(WebserviceUrls.BOOKING, content);

                if (response.IsSuccessStatusCode)
                {
                    var response1 = response.Content.ReadAsStringAsync().Result;
                    PlaceList data = new PlaceList();
                    if (!string.IsNullOrEmpty(response1))
                    {
                        data = JsonConvert.DeserializeObject<PlaceList>(response1);

                    }

                    bookTicketResponse = data.BookTicketResponse;
                    string status = data.ErrorMessage;
                    if (status == "Success")
                    {
                        UserDialogs.Instance.Alert("Booking Successful", "", "Ok");
                    }
                    else
                    {
                        UserDialogs.Instance.Alert(data.ErrorMessage, "", "Ok");
                  
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
            Picker picker = ((Picker)sender);
             subitemSelectedm = picker.SelectedItem as MealDetail;
            BookingPassengerDetails bookingPassengerDetails = new BookingPassengerDetails();
            mealamount = (int)Math.Round(Convert.ToDecimal(subitemSelectedm.MealAmount), 0);
            bookingPassengerDetails.TotalAmount = mealamount;
            double valueamirprice = Convert.ToDouble(bookingDetails.lstFlightDetail[0].TotalAmount);
            int valueamirconvert = Convert.ToInt32(valueamirprice);
            mealsAmount.Text = "Total Meals Amount : " + Convert.ToString(mealamount);
            if (baggageamount == 0)
            {
                totalprice = (mealamount) + Convert.ToInt16(totalticketAmount)+(valueamirconvert);
                Console.WriteLine("totalprice on click meal if" + totalprice);
                //wholetotalamount.Text = "Total Amount : " + Convert.ToString((mealamount) + Convert.ToInt16(totalticketAmount));
                wholetotalamount.Text = "Total Amount : " + Convert.ToString(totalprice);
            }
            else
            {
                //wholetotalamount.Text = "Total Amount : " + Convert.ToString((mealamount) + (baggageamount) + Convert.ToInt16(totalticketAmount)); ;
                totalprice = (mealamount) + (baggageamount) +(valueamirconvert);
                wholetotalamount.Text = "Total Amount : " + Convert.ToString(totalprice);
                Console.WriteLine("totalprice on click meal else" + totalprice);
            }

            Console.WriteLine(bookingPassengerDetails.TotalAmount);

        }
        void Handle_BagPickerChanged(object sender, EventArgs e)
        {
            Picker picker = ((Picker)sender);
             subitemSelectedb = picker.SelectedItem as BaggageDetail;
            BookingPassengerDetails bookingPassengerDetails = new BookingPassengerDetails();
            baggageamount = (int)Math.Round(Convert.ToDecimal(subitemSelectedb.BaggageAmount), 0);

            double valueamirprice = Convert.ToDouble(bookingDetails.lstFlightDetail[0].TotalAmount);
            int valueamirconvert = Convert.ToInt32(valueamirprice);
            baggageAmount.Text = "Total Meals Amount : " + Convert.ToString(baggageamount);
            if (mealamount == 0)
            {
                totalprice = (baggageamount) + Convert.ToInt16(totalticketAmount)+(valueamirconvert);
                Console.WriteLine("totalprice on click bagage if" + totalprice);

                //wholetotalamount.Text = "Total Amount : " + Convert.ToString((baggageamount) + Convert.ToInt16(totalticketAmount) );
                wholetotalamount.Text = "Total Amount : " + Convert.ToString(totalprice);

            }
            else
            {
                totalprice = (mealamount) + (baggageamount) + Convert.ToInt16(totalticketAmount)+ (valueamirconvert);
                Console.WriteLine("totalprice on click bagage else" + totalprice);
                //wholetotalamount.Text = "Total Amount : " + Convert.ToString((mealamount) + (baggageamount) + Convert.ToInt16(totalticketAmount));

                 wholetotalamount.Text = "Total Amount : " + Convert.ToString(totalprice);
            }


        }
        void Confirm_Clicked(object sender, System.EventArgs e)
        {


           

            if (terms.Checked)
            {
                ConfirmBooking();
            }
            else { UserDialogs.Instance.Alert("Please Accept the terms and Conditions", "", "Ok"); }
        }
        async void History_Clicked(object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
            await Navigation.PushAsync(new BookingDetails(bookTicketResponse, bookingDetails.UserId));

        }
        public async void Ondetaillabeltapped(object sender, EventArgs args)
        {

            StackLayout label = (StackLayout)sender;
            var labelarray = label.Children;

            var reqLabel1 = labelarray[0];

            var Label2 = reqLabel1.GetType();

            if (Label2 == typeof(Label))
            {
                Label label1 = (Label)reqLabel1;
                paxno = label1.Text;

                Console.WriteLine("pax no gayaa" + paxno);
                GetEmployeePopupList page = new GetEmployeePopupList(paxno);                  page.OnSelected += Page_OnSelected;                 await Navigation.PushPopupAsync(page);


            }
        }
        void Page_OnSelected(object sender, SelectedItemChangedEventArgs e)         {
           





            BookingPassengerDetails datatum = (BookingPassengerDetails)(e.SelectedItem);
           
            passengerDetails.Add(datatum);
          
            pasengerlist.ItemsSource = passengerDetails;


           



        }

    }

  }
