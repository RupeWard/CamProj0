using UnityEngine;
using System.Collections;

public class ImageEditControlPanel : WinControlPanel <ImageEditPanel>
{
	#region inspector hooks


	#endregion inspector hooks

	#region private data

	ImageEditPanel imageEditPanel_;

	#endregion private data

	public override string title( )
	{
		return "Edit";
	}

	#region SetUp

	public override void PostInit( ImageEditPanel avp )
	{
		if (avp== null)
		{
			Debug.LogError( "NULL AVP" );
		}
		imageEditPanel_ = avp;
	}

	#endregion SetUp
	/*
	protected override void DoDestroy( )
	{
		SceneControllerTest.Instance.DestroyAlbumViewPanel( );
	}
	*/

	#region Button handlers

	override public void OnCloseButtonPressed( )
	{
		SceneControllerTest.Instance.CloseImageEditPanel( );
	}

	#endregion Button handlers


}
