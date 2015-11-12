using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogManager : SingletonSceneLifetime<LogManager>
{
	private static bool DEBUG_LOCAL = true;

	public int maxLines = 20;
	public bool showLineNumbers = true;

	private int nextLineNumber = 0;

	private Queue<string> lines_ = new Queue<string>( );

	public Queue<string> AllLines
	{
		get { return lines_;  }
	}

	public System.Action<string> lineAddedAction;
	public System.Action linesDeletedAction;

	System.Text.StringBuilder debugsb_ = new System.Text.StringBuilder( );

	void Start()
	{
		AddLine( "Log started" + System.DateTime.Now + "\n\n" );
	}

	public void AddLine(string l)
	{
		if (DEBUG_LOCAL)
		{
			debugsb_.Length = 0;
		}
		string[] subLines = l.Split( new char[] { '\n' } );
		if (DEBUG_LOCAL)
		{
			debugsb_.Append( "LM: Adding " ).Append( subLines.Length ).Append( " lines" );
		}
		for (int i = 0; i < subLines.Length; i++)
		{
			if (showLineNumbers)
			{
				lines_.Enqueue( nextLineNumber.ToString()+" "+subLines[i] );
				nextLineNumber++;
			}
			else
			{
				lines_.Enqueue( subLines[i] );
			}
			if (lineAddedAction != null)
			{
				lineAddedAction( subLines[i] );
			}
			if (DEBUG_LOCAL)
			{
				debugsb_.Append( "\n" + i + " " + subLines[i] );
			}
		}
		int numDeleted = 0;
		while (lines_.Count > maxLines)
		{
			numDeleted++;
			lines_.Dequeue( );
		}
		if (numDeleted >0)
		{
			if (linesDeletedAction != null)
			{
				linesDeletedAction( );
			}
		}
		if (DEBUG_LOCAL)
		{
			debugsb_.Append( "\nDeleted " ).Append( numDeleted ).Append( " lines" );
			Debug.Log( debugsb_.ToString( ) );
		}
	}
}
