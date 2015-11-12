using UnityEngine;
using System.Collections;

public class AlbumViewControlPanel : WinControlPanel < AlbumViewPanel>
{
	#region inspector hooks


	#endregion inspector hooks

	#region private data

	AlbumViewPanel albumViewPanel_;

	#endregion private data

	public override string title( )
	{
		return "Album";
	}

	#region SetUp

	public override void PostInit( AlbumViewPanel avp)
	{
		if (avp== null)
		{
			Debug.LogError( "NULL AVP" );
		}
		albumViewPanel_ = avp;
	}

	#endregion SetUp

	protected override void DoDestroy( )
	{
		SceneControllerTest.Instance.DestroyAlbumViewPanel( );
	}

	#region Button handlers

	#endregion Button handlers


}
