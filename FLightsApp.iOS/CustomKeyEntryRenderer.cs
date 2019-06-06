using System;
using FLightsApp.iOS;
using FLightsApp.Models;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomKeyEntryRenderer))]
namespace FLightsApp.iOS
{
	public class CustomKeyEntryRenderer : EntryRenderer  
    {  
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)  
        {  
            base.OnElementChanged(e);  
  
			CustomEntry entry = (CustomEntry)this.Element;  
  
            if (this.Control != null)  
            {  
                if (entry != null)  
                {  
                    SetReturnType(entry);  
  
                    Control.ShouldReturn += (UITextField tf) =>  
                    {  
                        entry.InvokeCompleted();  
                        return true;  
                    };  
                }  
            }  
        }  
  
        private void SetReturnType(CustomEntry entry)  
        {
			Models.ReturnType type = entry.ReturnType;  
  
            switch (type)  
            {  
                case Models.ReturnType.Go:  
                    Control.ReturnKeyType = UIReturnKeyType.Go;  
                    break;  
                case Models.ReturnType.Next:  
                    Control.ReturnKeyType = UIReturnKeyType.Next;  
                    break;  
                case Models.ReturnType.Send:  
                    Control.ReturnKeyType = UIReturnKeyType.Send;  
                    break;  
                case Models.ReturnType.Search:  
                    Control.ReturnKeyType = UIReturnKeyType.Search;  
                    break;  
                case Models.ReturnType.Done:  
                    Control.ReturnKeyType = UIReturnKeyType.Done;  
                    break;  
                default:  
                    Control.ReturnKeyType = UIReturnKeyType.Default;  
                    break;  
            }  
        }  
    }  
}
