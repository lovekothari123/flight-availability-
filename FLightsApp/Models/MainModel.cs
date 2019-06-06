using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FLightsApp.Models
{
    public class MainModel
    {
		public int Totalcount { get; set; }
		public string FromOneway { get; set; }
		public string ToOneway { get; set; }
		public string onewayarrivaltime { get; set; }
		public string onewaydeparturetime { get; set; }
		public string roundarrivaltime { get; set; }
        public string rounddeparturetime { get; set; }
		public string roundreturnarrivaltime { get; set; }
        public string roundreturndeparturetime { get; set; }
        public int TripType { get; set; }
		public bool TripTypeVisibility => (TripType == 2);
		public int NoOfChild { get; set; }
		public string DateOneway { get; set;}
		public int NoOfInfant { get; set; }
		public string ReturnDateRound { get; set; }
		public int NoOfAdult { get; set; }
		public int SpecialFare { get; set; }
		public string FlightClass { get; set; }
		public string Cabinclass { get; set; }
		public bool booknowcorporate => (CorporateFare != "NA");
		public bool booknowcheap1 => (CheapFare1 != "NA");
		public bool booknowcheap2=> (CheapFare2 != "NA");
		public string CorporateTrackNo { get; set; }
		public string Cheap1TrackNo { get; set; }
		public string Cheap2TrackNo { get; set; }
		public string AirportCode { get; set; }
		public string AirportName { get; set; }
		public string CountryName { get; set; }
        public string usersType { get; set; }
        public int usersId { get; set; }
		public string returntype { get; set; }
		public string flightname { get; set; }
		public string fromtime { get; set; }
		public string totime { get; set; }
		public string totaltime { get; set; }
		public string stop { get; set; }
		public string Price { get; set; }
		public string title { get; set; }
		public string PlaceName { get; set; }
		public string ShortPlaceName { get; set; }
		public bool isChecked { get; set; }

		public List<string> Flightfilterlst { get; set; }
		public List<int> Stopfilterlst { get; set; }

		public string airlineFilter { get; set; }
		public string goairFilter { get; set; }
		public string indigoFilter { get; set; }
		public string spicejetFilter { get; set; }
		public string airindiaFilter { get; set; }


		public string onestopFilter { get; set; }
		public string zerostopFilter { get; set; }

		public string adult { get; set; }
		public string child { get; set; }
		public string infant { get; set; }
		public string cabinclass { get; set; }
		public string CorporateClass { get; set; }
		public string CheapFare1Class { get; set; }
		public string CheapFare2Class { get; set; }
		public string view { get; set; }
		public bool Visibility => !(view == "VIEW MORE");
		public bool Detailvisibility => (Stops != 0);
		public bool Detailvisibility1 => (Stops == 0);
		public string SrNo { get; set; }
		 public string AirlineCode { get; set; }
		 public string FlightNo { get; set; }
		 public string FromAirportCode { get; set; }
		 public string ToAirportCode { get; set; }
		 public string DepDate { get; set; }
		 public string DepTime { get; set; }
		 public string ArrDate { get; set; }
		 public string ArrTime { get; set; }
		 public string FlightClassrequest { get; set; }
		 public string FlightTime { get; set; }
		public decimal TotalAmount { get; set; }

		 public int Stops { get; set; }
	
		public string IconPath { get; set; }
		 public string FromTerminal { get; set; }
		 public string ToTerminal { get; set; }
		 public string MainClass { get; set; }
		 public string FareBasis { get; set; }
		public decimal AgencyCharge { get; set; }
		 public string FareType { get; set; }
		 public int AvailSeats { get; set; }
		 public string FlightRemarks { get; set; }
		 public string TrackNo { get; set; }
  
		public string Baggage { get; set; }
        public decimal Status { get; set; }
	    public string AirlineName { get; set; }
		public string AirlineFullName { get; set; }
        public string AirporteCode { get; set; }
		public string CorporateFare { get; set; }
		public bool handbagvisibility => (!nonhandbagvisibility);
		public bool nonhandbagvisibility => (CorporateFare == "NA");
        public string CheapFare1 { get; set; }
		public string CorporateFareType { get; set; }
		public string Cheap1FareType { get; set; }
		public string Cheap2FareType { get; set; }
        public string CheapFare2 { get; set; }


		public bool handbag1visibility => (!nonhandbag1visibility);
        public bool nonhandbag1visibility => (CheapFare1 == "NA");



		public bool handbag2visibility => (!nonhandbag2visibility);
		public bool nonhandbag2visibility => (CheapFare2 == "NA");

		public string MealIcon { get; set; }
		public string BaggageIcon { get; set; }
		public string Cheap1MealIcon { get; set; }
		public string Cheap2MealIcon { get; set; }
		public string Cheap1BaggageIcon { get; set; }
		public string Cheap2BaggageIcon { get; set; }

        public string chnageColor { get; set; }
        public string flightnamecode { get; set; }
        public string space { get; set; }
        public string NextDay { get; set; }
        public bool NextText => (NextDay == "NA");

 }


  
	public class FlightDetail{
		public string TrackNo { get; set; }
		public string TotalAmount { get; set; }
		public string FlightNo { get; set; }
		public string FromAirportFullName { get; set; }
		public string ToAirportFullName { get; set; }
		public string FromTerminal { get; set; }
		public string FromAirportCode { get; set; }
		public string ToAirportCode { get; set; }
		public string ToTerminal { get; set; }
		public string DepTime { get; set; }
		public string DepDate { get; set; }
		public string ArrTime { get; set; }
		public string DeptTimeDate { get; set; }
		public string FlightClass { get; set; }
		public string MainClass { get; set; }
		public string ArrTimeDate { get; set; }
		public string ArrDate { get; set; }
		public string SegmentSeqNo { get; set; }
		public string FlightTime { get; set; }
		public string AirlineCode { get; set; }
		public string IconPath { get; set; }
        public string AirlineName { get; set; }
        public string WaitingTime { get; set; }

        
	}
	public class BookingDetailsModel
    {
     
        public List<FlightDetail> lstFlightDetail { get; set; }
        public List<BookingPassengerDetails> LstbookingPassengerDetails { get; set; }
        public List<FareDetail> lstfareDetail { get; set; }
        public AdditionalService AdditionalServiceSelect { get; set; }
        public int NOOfPax { get; set; }
        public string ErrorMessage { get; set; }
        public string TrackNo { get; set; }
        public string VerifyTrackNo { get; set; }

    }
    public class BookingDetailsData
    {
        public List<BookingPassengerDetails> LstbookingPassengerDetails { get; set; }
        public List<FlightDetail> lstFlightDetail { get; set; }
        public List<FlightDetail> golstFlightDetail { get; set; }
        public List<FlightDetail> backlstFlightDetail { get; set; }
        public List<BaggageDetail> lstAdditionServiceDetails { get; set; }
       
        public int TotalAmount { get; set; }
        public double goFinalTotalAmount { get; set; }
        public double backFinalTotalAmount { get; set; }
        public int UserId { get; set; }
        public double FinalTotalAmount { get; set; }

    }
	public class BookTicketResponse
	{
		public string UserId { get; set; }
		public string ErrorMessage { get; set; }
		public string goErrorMessage { get; set; }
		public string backErrorMessage { get; set; }
		public bool IsNoShow { get; set; }
		public string RefNo { get; set; }
		public List<PassengerDetail> PassengerDetails {get; set;}
		public List<TicketDetail> TicketDetails { get; set; }
		public List<FlightDetail> FlightFareDetails { get; set; }

	}
	public class HistoryDetail{
		
        public string PassengerName { get; set; }
        public string RefNo { get; set; }
        public string PassengerType { get; set; }
        public string Gender { get; set; }
        public string AirlineCode { get; set; }
        public string NetAmount { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
        public string IconPath { get; set; }
		public string DepartureDate { get; set; }
		public string ArrivalDate { get; set; }
        public string FromAirportName { get; set; }
        public string ToAirportName { get; set; }
        
	}
	public class TicketDetail
    {
        public string RefNo { get; set; }
        public string TicketType { get; set; }
		public string IsDomestic { get; set; }
        public string Adult { get; set; }
		public string Child { get; set; }
		public string Infant { get; set; }
		public string Status { get; set; }
		public string TotalAmount { get; set; }
		public string TotalTicketCommissionAmount { get; set; }

    }
	public class CancelTicketRequest { 
		public string RefNo { get; set; }
		public List<PassengerDetail> Passengers { get; set; }
		public List<FlightDetail> Segments { get; set; }
		public bool IsNoShow { get; set; }
	
	}
		
	public class BookingPassengerDetails
    {

        //public int PassengerDetailsId { get; set; }
        public string Title { get; set; }
		public string PaxNo { get; set; }
		public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
		public string MerchantCode { get; set; }
		public bool GetPasssengerVisibility => !(PaxNo.Contains("1"));
		public string PassengerType { get; set; }
		public string DateOfBirth { get; set; }
		public string PassportNo { get; set; }
		public string PassportExpDate { get; set; }
		public string PassportIssuingCountry { get; set; }
		public string NationalityCountry { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
		public int TotalAmount { get; set; }
        public string Airname { get; set; }
		public AdditionalService AdditionalServiceSelect { get; set; }
		public bool additonalservicevisibility2 => (AdditionalServiceSelect == null);
	//public bool additonalservicevisibility => ((AdditionalServiceSelect.MealValueLst != null && AdditionalServiceSelect.BaggageValueLst != null) || AdditionalServiceSelect == null);
		public bool additonalservicevisibility1 => (!additonalservicevisibility2);
		public Boolean IsMainPax { get; set; }
		public List<FrequentFlyerDetail> LstFrequentFlyer { get; set; }
     // public List<Airlines> LstFrequentFlyer { get; set; }
        public string FrequentFlyerNo { get; set; }
    }
	public class Passenger
    {
        public Int32 PaxSeqNo { get; set; }
        public String Title { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PassengerType { get; set; }
        public String DateOfBirth { get; set; }
        public String FrequentFlyerNo { get; set; }
        public String PassportNo { get; set; }
        public String PassportExpDate { get; set; }
        public String PassportIssuingCountry { get; set; }
        public String NationalityCountry { get; set; }
        public String TicketNumber { get; set; }
    }
	public class FareDetail{
		public string FlightNo { get; set; }
		public string TrackNo { get; set; }
        public string IsCorporate { get; set; }
		public string FlightClass { get; set; }
		public string FlightRemarks { get; set; }
		public string Baggage { get; set; }
		public string FareType { get; set; }
		public decimal TotalAmount { get; set; }
   
	}
	public class AdditionalService
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string MealAmount { get; set; }
        public string MealServiceName { get; set; }
		public List<MealDetail> MealValueLst { get; set; }
		public List<BaggageDetail> BaggageValueLst { get; set; }
    }
	public class MealDetail
    {
		public string MealAmount { get; set; }
		public string MealType { get; set; }
		public string FromAirportCode { get; set; }
		public string ToAirportCode { get; set; }
		public string MealServiceName { get; set; }
		public string MealServiceCode { get; set; }

    }
	public class PassengerDetail{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Name { get; set; }
		public string SeqNo { get; set; }
        public string Title { get; set; }
		public string PassengerType { get; set; }
        public string Gender { get; set; }
		public string RefNo { get; set; }
		public string TotalAmount { get; set; }
		public string FromAirportCode { get; set; }
		public string ToAirportCode { get; set; }
		public string AirlineCode { get; set; }
		public string FlightClass { get; set; }
        public string FlightNo { get; set; }
		public string SegmentSeqNo { get; set; }
		public string IconPath { get; set; }
		public string Status { get; set; }
	}
	public class FrequentFlyerDetail
    {
		public string FrequentFlyerNo { get; set; }
		public string AirlineName { get; set; }


    }
	public class BaggageDetail
    {
        public string BaggageAmount { get; set; }
        public string BaggageServiceName { get; set; }
		public string BaggageServiceCode { get; set; }
		public string BaggageType { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }

    }

	public class LoginData
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserType { get; set; }
		public ErrorStatus errorStatus { get; set; }

	}
	public class ErrorStatus{
		public string Status { get; set; }
		public string Message { get; set; }
	}
	public class MainFareDetail
    {
        public string FlightNo { get; set; }
        public string IsCorporate { get; set; }
        public int Stops { get; set; }
		public List<FareDetail> samefareDetail { get; set; }
		public List<FlightDetail> fltDetails { get; set; }
    }
	public class PlaceList  
    {  
		public List<FlightDetail> lstFlightDetail { get; set; }
		public List<FlightDetail> golstFlightDetail { get; set; }
		public List<FlightDetail> backlstFlightDetail { get; set; }
		public List<MainModel> lstFlightAirport { get; set; }  
		public List<MainModel> FilterFlightList { get; set; }  
		public List<MainModel> GoFilterFlightList { get; set; }  
		public List<MainModel> BackFilterFlightList { get; set; } 
		public List<MainFareDetail> SameFlightList { get; set; } 
		public List<MainFareDetail> GoSameFlightList { get; set; } 
		public List<MainFareDetail> BackSameFlightList { get; set; } 
		public List<ErrorStatus> errorStatus { get; set; }
		public List<HistoryDetail> bookingadminLst { get; set; }
		public List<PassengerDetail> lstCancelFlightPassenger { get; set; }
		public BookTicketResponse BookTicketResponse { get; set; }
		public BookTicketResponse goBookTicketResponse { get; set; }
		public BookTicketResponse backBookTicketResponse { get; set; }
	     public 	AdditionalService AdditionalServiceSelect { get; set; }
		public List<BookingPassengerDetails> LstbookingPassengerDetails { get; set; }
        public List<BookingPassengerDetails> lstempmaster { get; set; }
		public string UserId { get; set; }
        public string ErrorMessage { get; set; }
		public string goErrorMessage { get; set; }
		public string backErrorMessage { get; set; }
    } 
}
