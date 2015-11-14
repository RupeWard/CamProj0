using UnityEngine;
using System.Collections;

public class BusyPanel : SingletonSceneLifetime<BusyPanel>
{
	public UnityEngine.UI.Text titleText;
	public UnityEngine.UI.Text messageText;
	public UnityEngine.UI.Text versionText;

	private static string defaultTitle = "PLEASE WAIT";

	protected override void PostAwake( )
	{
		versionText.text = Version.versionNumber.ToString( );
	}

	public void Open( string m )
	{
		Open( defaultTitle, m );
	}

    public void Open(string t, string m)
	{
		if (gameObject.activeSelf)
		{
			Debug.LogError( "Busy panel already busy" );
		}
		else
		{
			titleText.text = t;
			messageText.text = m;
			gameObject.SetActive( true );
		}
	}

	public void Close()
	{
		gameObject.SetActive( false );
	}
}
