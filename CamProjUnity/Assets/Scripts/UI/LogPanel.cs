using UnityEngine;
using System.Collections;

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
	}

	void Start ()
    {
		Append( "Log started" + System.DateTime.Now + "\n\n" );
//		winLayerWin.lossOfFocusAction += HandleLossOfFocus;
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


}
