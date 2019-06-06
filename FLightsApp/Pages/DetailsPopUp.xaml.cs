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
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace FLightsApp.Pages
{
	public partial class DetailsPopUp : PopupPage
	{
		List<FlightDetail> flightDetailsitems = new List<FlightDetail>();
		List<MainFareDetail> mainFareDetails = new List<MainFareDetail>();
		List<MainModel> flightlistitems = new List<MainModel>();
		public DetailsPopUp(int stops,string flightNo,int flag,int flagreturn, MainModel model)
		{
			InitializeComponent();
			GetFlightDetails(stops,flightNo, flag,flagreturn, model);
		/*	if (Application.Current.Properties.ContainsKey("listOfPersons"))  
                                    {  
				flightDetailsitems =Application.Current.Properties["listOfPersons"] as List<FlightDetail>;
            }*/

		

			var closetap = new TapGestureRecognizer();
			if (Application.Current.Properties.ContainsKey("id"))
            {
                var id = Application.Current.Properties["id"];
				Console.WriteLine(id);
              
            }
            
			closetap.Tapped += async (s, e) =>
			{
				await Navigation.PopAllPopupAsync();
			};
			close.GestureRecognizers.Add(closetap);
		}


		public string split(string date)
        {
            string[] parts = date.Split('/');
            string part1 = null, part2 = null, part3 = null;
            part1 = parts[1];
            part2 = parts[0];
            part3 = parts[2];
            string actualdate = part1 + "/" + part2 + "/" + part3;
			DateTime dateTime = new DateTime();
			dateTime = Convert.ToDateTime(actualdate);
			string actualdate1 = dateTime.ToString("dd MMM");
            return actualdate1;
        }

		protected override bool OnBackgroundClicked()
		{
			CloseAllPopup();

			return false;
		}

		private async void CloseAllPopup()
		{
			await Navigation.PopAllPopupAsync();
			// await Navigation.PushPopupAsync(_thankspopup);
		}
		protected override bool OnBackButtonPressed()
		{

			return true;
		}
        public string changetype(string date)
        {



            string[] arr = date.Split('/');
            string dayc = arr[1].TrimStart("0".ToCharArray());
            string mont = arr[0];
            string[] monthc = { " ", "jan", "feb", "mar", "apr", "may", "jun", "july", "aug", "sep", "oct", "nov", "dec" };
            for (int i = 1; i < monthc.Length; i++)
            {


                if (dayc.Contains(Convert.ToString(i)))
                {
                    date = mont + " " + monthc[i];

                }


            }

            return date;
        }
		public async void GetFlightDetails(int stops,string flightno, int flag,int flagreturn, MainModel modelvalue)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                    var client = new HttpClient();
					HttpResponseMessage response = new HttpResponseMessage();
                  string request = @"{GetFlightAvailibilityRequest :{FromAirportCode :'" + modelvalue.FromOneway + "',ToAirportCode :'" + modelvalue.ToOneway + "',NoofAdult : '" + modelvalue.NoOfAdult + "',NoofChild:'" + modelvalue.NoOfChild + "',NoofInfant:'" + modelvalue.NoOfInfant + "',DepartureDate:'" + modelvalue.DateOneway + "',SpecialFare:'" + modelvalue.SpecialFare + "',ReturnDate:'" + modelvalue.ReturnDateRound + "',TripType:'" + "1" + "',FlightClass:'" + modelvalue.FlightClassrequest + "'}}";

                    Console.WriteLine("request details" + request);

                    var content = new StringContent(request, Encoding.UTF8, "application/json");
					if(flag == 1){
						
                        response	 = await client.PostAsync(WebserviceUrls.GETFLIGHTAVAILABILITYURL, content);


					}else{
						 response = await client.PostAsync(WebserviceUrls.GETROUNDTRIPAVAILABILITY, content);

					}
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        imgarrow1.IsVisible = true;
                        imgarrow.IsVisible = true;
                        var result = await response.Content.ReadAsStringAsync();
                        PlaceList placeList = new PlaceList();
                        if (result != "")
                        {
                            
                            placeList = JsonConvert.DeserializeObject<PlaceList>(result);
                        }
						if(flag ==1){
                            Console.WriteLine("detail first if");
							flightlistitems = new List<MainModel>(placeList.FilterFlightList);
                            mainFareDetails = new List<MainFareDetail>(placeList.SameFlightList);
						}else{
                            Console.WriteLine("detail second if");
							flightlistitems = new List<MainModel>(placeList.GoFilterFlightList);
                            mainFareDetails = new List<MainFareDetail>(placeList.GoSameFlightList);
						}
						if(flagreturn == 3){
                            Console.WriteLine("detail third if");
							flightlistitems = new List<MainModel>(placeList.BackFilterFlightList);
							mainFareDetails = new List<MainFareDetail>(placeList.BackSameFlightList);
						}


                        Console.WriteLine("Stoops aya " + stops);
                        
						for (int k = 0;k < flightlistitems.Count; k++)
						{
							for (int j = 0; j < mainFareDetails.Count; j++)
							{
                                if (mainFareDetails[j].FlightNo == flightno && mainFareDetails[j].Stops==stops)
								{

                                    //Console.WriteLine("Details Reposne stops" + mainFareDetails[j].Stops);
                                    //if(mainFareDetails[j].Stops==stops){
                                    //    Console.WriteLine("Detailed same stops");
                                    //}else{
                                    //    Console.WriteLine("Detailed not same stops");
                                    //}


                                   
									flightDetailsitems = mainFareDetails[j].fltDetails;


									for (int i = 0; i < flightDetailsitems.Count; i++)
                                    {
                                       
                                       
                                        TimeSpan tspan = TimeSpan.FromMinutes(Convert.ToDouble(flightDetailsitems[i].FlightTime)); //converting minutes to timespan
                                        flightDetailsitems[i].FlightTime = (int)tspan.TotalHours + ":" + tspan.Minutes;

                                        flightDetailsitems[i].WaitingTime = "Wating time : " + flightDetailsitems[i].WaitingTime;
                                        string deptdateformat = null, arrdateformat = null;
                                      
                                        if (string.IsNullOrEmpty(flightDetailsitems[i].DepDate))
                                        {
                                            
                                        }
                                        else
                                        {
                                            deptdateformat =changetype(flightDetailsitems[i].DepDate);
                                            flightDetailsitems[i].DeptTimeDate = deptdateformat + " / " + flightDetailsitems[i].DepTime;
                                          
                                        }
                                        if (string.IsNullOrEmpty(flightDetailsitems[i].ArrDate))
                                        {

                                        }
                                        else
                                        {
                                           
                                            arrdateformat = changetype(flightDetailsitems[i].ArrDate);
                                          
                                            flightDetailsitems[i].ArrTimeDate = arrdateformat + " / " + flightDetailsitems[i].ArrTime;
                                        }
                                     	if(!string.IsNullOrEmpty(flightDetailsitems[i].FromTerminal)){
											flightDetailsitems[i].FromTerminal = "Terminal " + flightDetailsitems[i].FromTerminal;
                                          

										}else{
											flightDetailsitems[i].FromTerminal = "Terminal -";
                                          

										}if(!string.IsNullOrEmpty(flightDetailsitems[i].ToTerminal)){
											flightDetailsitems[i].ToTerminal = "Terminal " + flightDetailsitems[i].ToTerminal;
                                          

										}else{
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
											if(mainFareDetails[j].fltDetails.Count>1){
												for (int l=0; l < mainFareDetails[j].fltDetails.Count;l++){
													if(l==0){
														from.Text = mainFareDetails[j].fltDetails[l].FromAirportFullName;
														stop.Text = mainFareDetails[j].fltDetails[l].ToAirportFullName;
													}else if(l==1){
														to.Text = mainFareDetails[j].fltDetails[l].ToAirportFullName;
													}else{
														
													}
												}
											}
                                        }
                                    }
                                 
                                    flightdetailslist.ItemsSource = flightDetailsitems;
									//Application.Current.Properties["myList"] = flightDetailsitems;
									int RowHeight = 200, count = flightDetailsitems.Count;
                                    mainstack.HeightRequest = RowHeight * count;
                                    flightdetailslist.HeightRequest = RowHeight * count;
								}
							}
						}

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
				Console.WriteLine("Error Ocurred");
            }

        }
	}
}
