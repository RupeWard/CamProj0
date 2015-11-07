using UnityEngine;
using System.Collections;

public class LogPanel : MonoBehaviour
{
	private static readonly bool DEBUG_LOCAL = true;

	public UnityEngine.UI.Text logText;
	public GameObject controlPanelPrefab;

	private LogControlPanel controlPanel_;

	System.Text.StringBuilder logSB_ = new System.Text.StringBuilder();

    bool isDirty_ = false;

	public WinLayerWin winLayerWin;

	private static LogPanel instance_;
	public static LogPanel Instance
	{
		get { return instance_; }
	}

	public void Awake()
    {
		if (instance_ != null)
		{
			Debug.LogError( "Second LogPanel!");
		}
		instance_ = this;
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
		winLayerWin.lossOfFocusAction += HandleLossOfFocus;
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

	public void MoveToBack( )
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "LP: MoveToBack" );
		}
		if (winLayerWin.currentLayer.LayerNum > 0 && winLayerWin.currentLayer.WinLayerManager.NumLayers > 1)
		{
			if (winLayerWin.currentLayer.IsOnTop)
			{
				HandleLossOfFocus( );
			}
			winLayerWin.MoveToBack( );
		}
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
			if (controlPanel_ == null)
			{
				GameObject go = Instantiate( controlPanelPrefab ) as GameObject;
				controlPanel_ = go.GetComponent<LogControlPanel>( );
				if (DEBUG_LOCAL)
				{
					Debug.Log( "LP: HandleClik() created control panel "+controlPanel_.gameObject.name );
				}
			}
			if (controlPanel_ == null)
			{
				Debug.LogError( "LP: Failed to make control panel" );
			}
			else
			{
				winLayerWin.WinLayerManager.SetControls( controlPanel_.GetComponent<RectTransform>( ) );
				controlPanel_.Init( this );
				if (DEBUG_LOCAL)
				{
					Debug.Log( "LP: HandleClik() opened controls" );
				}
			}
		}

	}

	public void HandleLossOfFocus( )
	{
		if (controlPanel_ != null)
		{
			controlPanel_.OnCloseButtonPressed( );
		}
	}

}
