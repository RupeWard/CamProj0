using UnityEngine;
using System.Collections;

public class BusyPanel : SingletonSceneLifetime<BusyPanel>
{
	public UnityEngine.UI.Text titleText;
	public UnityEngine.UI.Text messageText;

	private static string defaultTitle = "PLEASE WAIT";

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
