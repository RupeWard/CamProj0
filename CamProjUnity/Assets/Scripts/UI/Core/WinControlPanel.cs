using UnityEngine;
using System.Collections;

abstract public class WinControlPanel < TControlleeType >: MonoBehaviour 
{
	static private readonly bool DEBUG_LOCAL = false;
	#region inspector hooks

	public UnityEngine.UI.Text titleText;

	public ButtonSet winButtonsContainer;
	public ButtonSet funcButtonsContainer;

	public GameObject buttonSetButtonPrefab;

	private RectTransform rectTransform_;

	#endregion inspector hooks
	#region private data

	WinLayerWin win_;
	WinWin<TControlleeType> winWin_;

	TControlleeType controllee_;

	#endregion private data

	abstract public string title( );

	#region SetUp

	private void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
	}

	virtual protected void AddWindowButtons()
	{
		Debug.Log( "AddWindowButtons not implemented in " +gameObject.name);
	}


	public void Init( WinLayerWin wlw, WinWin<TControlleeType> ww)
	{
		win_ = wlw;
		winWin_ = ww;

		titleText.text = title();

		gameObject.SetActive( true );

		CreateButton( "Back", OnBackButtonPressed, winButtonsContainer );
		CreateButton( "Scale", OnScaleButtonPressed, winButtonsContainer );
		CreateButton( "Size", OnSizeButtonPressed, winButtonsContainer );
		CreateButton( "Move", OnMoveButtonPressed, winButtonsContainer );
		CreateButton( "Save", OnSaveButtonPressed, winButtonsContainer );
		CreateButton( "Close", OnCloseButtonPressed, winButtonsContainer );
		CreateButton( "Done", OnDoneButtonPressed, winButtonsContainer );

		controllee_ = wlw.GetComponent<TControlleeType>( );
		if (controllee_ == null)
		{
			controllee_ = wlw.transform.GetComponentInChildren<TControlleeType>( );
		}
		if (controllee_ == null)
		{
			Debug.LogError( "WCP: "+gameObject.name+" Failed to find controllee" );
		}
		else
		{
			PostInit( controllee_ );
		}

	}

	public abstract void PostInit ( TControlleeType controllee);

	#endregion SetUp



	#region Button handlers


	public void OnBackButtonPressed()
	{
		win_.MoveToBack( );
	}
	
	public void OnDoneButtonPressed( )
	{
		winWin_.StopStuff( );
		transform.parent.GetComponent<WinControlsLayer>( ).CloseControls( );
	}

	abstract public void OnCloseButtonPressed( );

//	abstract protected void DoDestroy( );

	public void OnSaveButtonPressed()
	{
		winWin_.SaveSizeAndPosition( );
	}

	public void OnMoveButtonPressed()
	{
		winWin_.MoveWindow( );
	}

	public void OnScaleButtonPressed( )
	{
		winWin_.ScaleWindow( );
	}

	public void OnSizeButtonPressed( )
	{
		winWin_.SizeWindow( );
	}


	#endregion Button handlers

	public ButtonSetButton CreateWinButton( string buttonText, System.Action onClickAction)
	{
		return CreateButton( buttonText, onClickAction, winButtonsContainer );
	}

	public ButtonSetButton CreateFuncButton( string buttonText, System.Action onClickAction )
	{
		return CreateButton( buttonText, onClickAction, funcButtonsContainer );
	}

	private ButtonSetButton CreateButton(string buttonText, System.Action onClickAction, ButtonSet buttonSet)
	{
		GameObject go = Instantiate( buttonSetButtonPrefab ) as GameObject;
		ButtonSetButton bsb = go.GetComponent<ButtonSetButton>( );
		if (bsb != null)
		{
			bsb.Init( buttonText, buttonSet );
			bsb.onClickAction += onClickAction;
		}
		else
		{
			Debug.LogError( "No BSB" );

		}
		float marginTotal = 
			(winButtonsContainer.NumButtons+1) * winButtonsContainer.margin 
			+ (funcButtonsContainer.NumButtons+1) * funcButtonsContainer.margin;
		float buttonsHeightTotal = rectTransform_.GetHeight( ) - titleText.GetComponent<RectTransform>().GetHeight() - marginTotal;
		int totalButtons = winButtonsContainer.NumButtons + funcButtonsContainer.NumButtons;
		float buttonHeight = buttonsHeightTotal / totalButtons;

        float winButtonsHeight = winButtonsContainer.NumButtons * buttonHeight;
		float funcButtonsHeight = funcButtonsContainer.NumButtons * buttonHeight;

		if (DEBUG_LOCAL)
		{

			Debug.Log( "WCP: wbh = " + winButtonsHeight + ", fbh = " + funcButtonsHeight );
		}

		winButtonsContainer.GetComponent<RectTransform>( ).SetHeight( winButtonsHeight + (winButtonsContainer.NumButtons+1 ) * winButtonsContainer.margin);
		funcButtonsContainer.GetComponent<RectTransform>( ).SetHeight( funcButtonsHeight + (funcButtonsContainer.NumButtons+1) * funcButtonsContainer.margin );
		return bsb;
	}

}
