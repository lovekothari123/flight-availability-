using System;
using Xamarin.Forms;

namespace FLightsApp.Models
{
	public class CustomEntry: Entry
	{
        // Need to overwrite default handler because we cant Invoke otherwise  
        public new event EventHandler Completed;

        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType),
            typeof(ReturnType), typeof(CustomEntry),
            ReturnType.Done, BindingMode.OneWay);

        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public void InvokeCompleted()
        {
            if (this.Completed != null)
                this.Completed.Invoke(this, null);
        }
    }
    public enum ReturnType
    {
        Go,
        Next,
        Done,
        Send,
        Search
    }
}  

