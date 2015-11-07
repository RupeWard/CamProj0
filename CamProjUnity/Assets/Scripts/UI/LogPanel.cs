using UnityEngine;
using System.Collections;

public class LogPanel : SingletonSceneLifetime< LogPanel >
{
	private static readonly bool DEBUG_LOCAL = true;

	public UnityEngine.UI.Text logText;

    System.Text.StringBuilder logSB_ = new System.Text.StringBuilder();

    bool isDirty_ = false;

	public WinLayerWin winLayerWin;

	protected override void PostAwake()
    {
        Append("Log started" + System.DateTime.Now+"\n\n");
    }

    void Start ()
    {
	    
	}
	
	void Update ()
    {
        if (isDirty_)
        {
            logText.text = logSB_.ToString();
        }
	}

    public void Append(string s)
    {
        logSB_.Append(s);
        isDirty_ = true;
    }

	public void HandleClick( )
	{
		if (winLayerWin.MovetoTop( ))
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "LP: HandleClik() moved to top" );
			}
		}
		else
		{
		}
	}
}
