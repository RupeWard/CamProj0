using UnityEngine;
using System.Collections;

public class LogControlPanel : WinControlPanel < LogPanel>
{
	#region inspector hooks


	#endregion inspector hooks

	#region private data

	LogPanel logPanel_;

	#endregion private data

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


	#region Button handlers

	#endregion Button handlers


}
