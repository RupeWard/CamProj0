using UnityEngine;
using System.Collections;

public class LogControlPanel : WinControlPanel
{
	#region inspector hooks


	#endregion inspector hooks

	#region private data

	LogPanel logPanel_;

	#endregion private data

	#region SetUp

	public void Init( LogPanel lp)
	{
		logPanel_ = lp;
		base.Init( logPanel_.winLayerWin );
	}

	#endregion SetUp


	#region Button handlers

	#endregion Button handlers


}
