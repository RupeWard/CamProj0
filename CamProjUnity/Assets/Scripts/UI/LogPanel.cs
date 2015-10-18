using UnityEngine;
using System.Collections;

public class LogPanel : SingletonSceneLifetime< LogPanel >
{
    public UnityEngine.UI.Text logText;

    System.Text.StringBuilder logSB_ = new System.Text.StringBuilder();

    bool isDirty_ = false;

    protected override void PostAwake()
    {
        Append("Log started" + System.DateTime.Now);
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
}
