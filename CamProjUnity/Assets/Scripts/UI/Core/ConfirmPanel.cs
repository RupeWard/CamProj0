using UnityEngine;
using System.Collections;

public class ConfirmPanel : MonoBehaviour 
{
	static private readonly bool DEBUG_LOCAL = false;

	#region inspector hooks

	public UnityEngine.UI.Text titleText;
	public UnityEngine.UI.Text infoText;

	public System.Action okButtonAction;
	public System.Action yesButtonAction;
	public System.Action noButtonAction;

	public System.Action closeAction;

	public ButtonSet buttonsContainer;

	public GameObject buttonSetButtonPrefab;

	private RectTransform rectTransform_;

	private ButtonSetButton okButton_;
	private ButtonSetButton yesButton_;
	private ButtonSetButton noButton_;

	#endregion inspector hooks
	#region private data

	public class ButtonDef
	{
		public System.Action clickAction;
		public string str;
		public ButtonDef( string s, System.Action a)
		{
			clickAction = a;
			str = s;
		}
		private ButtonDef( ) { }
	}

	public class Data
	{
		public string title = string.Empty;
		public string info = string.Empty;
		public ButtonDef okButtonDef;
		public ButtonDef yesButtonDef;
		public ButtonDef noButtonDef;
	}

	#endregion private data

	#region SetUp

	private void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
	}

	public void Init( Data d )
	{
		titleText.text = d.title;
		infoText.text = d.info;

		SetOkButton( d.okButtonDef );
		SetYesButton( d.yesButtonDef );
		SetNoButton( d.noButtonDef );
	}

	public void Init( string title, string info )
	{
		titleText.text = title;
		infoText.text = info;
		gameObject.SetActive( true );
	}

	private void DoClose()
	{
		if (closeAction != null)
		{
			closeAction( );
		}
		WinLayerManager.Instance.CloseOverlays( );
	}

	public void SetOkButton(ButtonDef def)
	{
		if (def==null || string.IsNullOrEmpty( def.str ))
		{
			if (okButton_ != null)
			{
				GameObject.Destroy( okButton_ );
				okButton_ = null;
			}
		}
		else
		{
			if (okButton_ == null)
			{
				okButton_ = CreateButton( def.str, HandleOkButtonClicked, buttonsContainer );
			}
			okButtonAction += def.clickAction;
		}
	}

	public void SetYesButton( ButtonDef def)
	{
		if (def == null || string.IsNullOrEmpty( def.str ))
		{
			if (yesButton_ != null)
			{
				GameObject.Destroy( yesButton_ );
				yesButton_ = null;
			}
		}
		else
		{
			if (yesButton_ == null)
			{
				yesButton_ = CreateButton( def.str, HandleYesButtonClicked, buttonsContainer );
			}
			yesButtonAction += def.clickAction;
		}
	}

	public void SetNoButton( ButtonDef def)
	{
		if (def == null || string.IsNullOrEmpty( def.str ))
		{
			if (noButton_ != null)
			{
				GameObject.Destroy( noButton_ );
				noButton_ = null;
			}
		}
		else
		{
			if (noButton_ == null)
			{
				noButton_ = CreateButton( def.str, HandleNoButtonClicked, buttonsContainer );
			}
			noButtonAction += def.clickAction;
		}
	}
	#endregion SetUp



	#region Button handlers

	public void HandleOkButtonClicked()
	{
		if (okButtonAction != null)
		{
			okButtonAction( );
		}
		DoClose( );
	}

	public void HandleYesButtonClicked( )
	{
		if (yesButtonAction != null)
		{
			yesButtonAction( );
		}
		DoClose( );
	}

	public void HandleNoButtonClicked( )
	{
		if (noButtonAction != null)
		{
			noButtonAction( );
		}
		DoClose( );
	}


	#endregion Button handlers


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
			(buttonsContainer.NumButtons+1) * buttonsContainer.margin;
		float buttonsHeightTotal = rectTransform_.GetHeight( ) - marginTotal;
		int totalButtons = buttonsContainer.NumButtons;
		float buttonHeight = buttonsHeightTotal / totalButtons;

        float buttonsHeight = buttonsContainer.NumButtons * buttonHeight;

		if (DEBUG_LOCAL)
		{
			Debug.Log( "WCP: bh = " + buttonsHeight );
		}

		buttonsContainer.GetComponent<RectTransform>( ).SetHeight( buttonsHeight + (buttonsContainer.NumButtons+1 ) * buttonsContainer.margin);
		return bsb;
	}

}
