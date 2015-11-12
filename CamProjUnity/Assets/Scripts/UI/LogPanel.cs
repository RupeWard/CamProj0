using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogPanel : WinWin< LogPanel>
{
//	private static readonly bool DEBUG_LOCAL = true;

	public UnityEngine.UI.Text logText;

	System.Text.StringBuilder logSB_ = new System.Text.StringBuilder();

    bool isDirty_ = false;

	private static LogPanel instance_;
	public static LogPanel Instance
	{
		get { return instance_; }
	}

	protected  override void Awake()
    {
		if (instance_ != null)
		{
			Debug.LogError( "Second LogPanel!");
		}
		instance_ = this;
		base.Awake( );

		LogManager.Instance.AddLine( "Log opened @ " + System.DateTime.Now );
	}

	public void OnDestroy()
	{
		if (instance_ == this)
		{
			instance_ = null;
		}
		else
		{
			Debug.LogError( "Destroying non-instance LogPanel" );
		}
		LogManager.Instance.lineAddedAction -= HandleLineAdded;
		LogManager.Instance.linesDeletedAction -= HandleLinesDeleted;

	}

	void Start ()
    {
		//		winLayerWin.lossOfFocusAction += HandleLossOfFocus;
		LogManager.Instance.lineAddedAction += HandleLineAdded;
		LogManager.Instance.linesDeletedAction += HandleLinesDeleted;

		DisplayAllLines( );

	}

	void Update ()
    {
        if (isDirty_)
        {
            logText.text = logSB_.ToString();
        }
	}

    private void Append(string s)
    {
        logSB_.Append(s);
        isDirty_ = true;
    }

	public void HandleLineAdded(string s)
	{
		logSB_.Append("\n").Append( s );
		isDirty_ = true;
	}

	public void HandleLinesDeleted()
	{
		logSB_.Length = 0;
		DisplayAllLines( );
	}

	public void DisplayAllLines( )
	{
		Queue<string> allLines = LogManager.Instance.AllLines;
		foreach (string s in allLines)
		{
			HandleLineAdded( s );
		}
	}

}
