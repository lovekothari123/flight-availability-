using System;
using FLightsApp.iOS;
using FLightsApp.Pages;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTimePicker24H), typeof(CustomTimePicker24HRenderer))]  

namespace FLightsApp.iOS
{
	public class CustomTimePicker24HRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            var timePicker = (UIDatePicker)Control.InputView;
            timePicker.Locale = new NSLocale("no_nb");
            if (Control != null)
            {
                Control.Text = DateTime.Now.ToString("HH:mm");
            }
        }
    }
}