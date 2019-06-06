using System;
using Android.Views.InputMethods;
using Android.Widget;
using FLightsApp.Droid;
using FLightsApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomKeyEntryRenderer))]
namespace FLightsApp.Droid
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

					// Editor Action is called when the return button is pressed  
					Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
					{
						if (entry.ReturnType != Models.ReturnType.Next)
							entry.Unfocus();

						// Call all the methods attached to custom_entry event handler Completed  
						entry.InvokeCompleted();
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
					Control.ImeOptions = Android.Views.InputMethods.ImeAction.Go;
					Control.SetImeActionLabel("Go", ImeAction.Go);
					break;
				case Models.ReturnType.Next:
					Control.ImeOptions = ImeAction.Next;
					Control.SetImeActionLabel("Next", ImeAction.Next);
					break;
				case Models.ReturnType.Send:
					Control.ImeOptions = ImeAction.Send;
					Control.SetImeActionLabel("Send", ImeAction.Send);
					break;
				case Models.ReturnType.Search:
					Control.ImeOptions = ImeAction.Search;
					Control.SetImeActionLabel("Search", ImeAction.Search);
					break;
				default:
					Control.ImeOptions = ImeAction.Done;
					Control.SetImeActionLabel("Done", ImeAction.Done);
					break;
			}
		}
	}
}
