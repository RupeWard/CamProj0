using UnityEngine;
using System.Collections;

public class ButtonSetButton : MonoBehaviour
{
	public UnityEngine.UI.Text buttonText;
	public UnityEngine.UI.Image image;
	public UnityEngine.UI.Button button_;

	private RectTransform rectTransform_;

	public System.Action onClickAction;

	private string defaultButtonText_;
	private ButtonSet buttonSet_;

	public void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
    }

	public void Init(string s, ButtonSet bs)
	{
		defaultButtonText_ = s;
		SetText( defaultButtonText_ );
		SetButtonSet( bs );
//		bs.AddButton( this );
		gameObject.name = s + "_Button";
	}

	public void SetText(string s)
	{
		if (string.IsNullOrEmpty( s ) )
		{
			s = defaultButtonText_;
		}
		buttonText.text = s;
	}

	public void SetColour(Color c)
	{
		image.color = c;
	}

	private void SetButtonSet(ButtonSet bs)
	{
		if (buttonSet_ != null)
		{
			Debug.LogError( "Non null BS" );
		}
		buttonSet_ = bs;
		rectTransform_.SetParent( buttonSet_.transform );
		rectTransform_.localScale = Vector3.one;
		bs.AddButton( this );
	}

	public void HandleClick( )
	{
		if (onClickAction != null)
		{
			onClickAction();
		}
	}
}
