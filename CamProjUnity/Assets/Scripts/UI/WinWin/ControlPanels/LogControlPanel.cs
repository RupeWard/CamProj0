using UnityEngine;
using System.Collections;

public class LogControlPanel : WinControlPanel < LogPanel>
{
	#region inspector hooks


	#endregion inspector hooks

	#region private data

	LogPanel logPanel_;

	#endregion private data

	public override string title( )
	{
		return "Logger";
	}

	#region SetUp

	public override void PostInit( LogPanel lp)
	{
		if (lp== null)
		{
			Debug.LogError( "NULL LP" );
		}
		logPanel_ = lp;
	}

	#endregion SetUp

	/*
	protected override void DoDestroy( )
	{
		SceneControllerTest.Instance.DestroyLogPanel( );
	}
	*/

	#region Button handlers

	override public void OnCloseButtonPressed( )
	{
		SceneControllerTest.Instance.ToggleLogPanel( );
	}

	#endregion Button handlers


}
