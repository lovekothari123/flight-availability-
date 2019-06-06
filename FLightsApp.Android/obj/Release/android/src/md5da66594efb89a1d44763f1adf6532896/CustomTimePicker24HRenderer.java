package md5da66594efb89a1d44763f1adf6532896;


public class CustomTimePicker24HRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.ViewRenderer_2
	implements
		mono.android.IGCUserPeer,
		android.app.TimePickerDialog.OnTimeSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimeSet:(Landroid/widget/TimePicker;II)V:GetOnTimeSet_Landroid_widget_TimePicker_IIHandler:Android.App.TimePickerDialog/IOnTimeSetListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("FLightsApp.Droid.CustomTimePicker24HRenderer, FLightsApp.Android", CustomTimePicker24HRenderer.class, __md_methods);
	}


	public CustomTimePicker24HRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomTimePicker24HRenderer.class)
			mono.android.TypeManager.Activate ("FLightsApp.Droid.CustomTimePicker24HRenderer, FLightsApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomTimePicker24HRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomTimePicker24HRenderer.class)
			mono.android.TypeManager.Activate ("FLightsApp.Droid.CustomTimePicker24HRenderer, FLightsApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomTimePicker24HRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomTimePicker24HRenderer.class)
			mono.android.TypeManager.Activate ("FLightsApp.Droid.CustomTimePicker24HRenderer, FLightsApp.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void onTimeSet (android.widget.TimePicker p0, int p1, int p2)
	{
		n_onTimeSet (p0, p1, p2);
	}

	private native void n_onTimeSet (android.widget.TimePicker p0, int p1, int p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
