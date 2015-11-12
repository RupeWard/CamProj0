﻿using UnityEngine;
using System.Collections;

abstract public class WinControlPanel < TControlleeType >: MonoBehaviour 
{
	#region inspector hooks

	public UnityEngine.UI.Text titleText;

	public ButtonSet winButtonsContainer;
	public ButtonSet funcButtonsContainer;

	public GameObject buttonSetButtonPrefab;

	#endregion inspector hooks
	#region private data

	WinLayerWin win_;
	WinWin<TControlleeType> winWin_;

	TControlleeType controllee_;

	#endregion private data

	abstract public string title( );

	#region SetUp

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
		CreateButton( "Close", OnCloseButtonPressed, winButtonsContainer );

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

	public void OnCloseButtonPressed( )
	{
		winWin_.StopStuff( );
		transform.parent.GetComponent<WinControlsLayer>( ).CloseControls( );
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
		return bsb;
	}

}
