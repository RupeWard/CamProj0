using UnityEngine;
using System.Collections;

public class AlbumManagerControlPanel : WinControlPanel <AlbumManagerPanel>
{
	#region inspector hooks


	#endregion inspector hooks

	#region private data

	AlbumManagerPanel albumViewPanel_;

	#endregion private data

	public override string title( )
	{
		return "Album Manager";
	}

	#region SetUp

	public override void PostInit( AlbumManagerPanel avp )
	{
		if (avp== null)
		{
			Debug.LogError( "NULL AMP" );
		}
		albumViewPanel_ = avp;
	}

	#endregion SetUp

	/*
	protected override void DoDestroy()
	{
		SceneControllerTest.Instance.DestroyAlbumManagerPanel( );
	}*/

	#region Button handlers

	override public void OnCloseButtonPressed( )
	{
		SceneControllerTest.Instance.ToggleAlbumManager( );
	}

	#endregion Button handlers


}
