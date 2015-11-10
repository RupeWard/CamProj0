using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumButton : MonoBehaviour
{
	public UnityEngine.UI.Text albumNameText;
	private UnityEngine.UI.Image image_;

	private Album album_;
	public Album Album
	{
		get { return album_;  }
	}

	static private Dictionary<EState, Color> stateColours = new Dictionary<EState, Color>( )
	{
		{ EState.Empty,  Color.gray},
		{ EState.Full, Color.white },
		{ EState.Selected, Color.green }
	};

	public enum EState
	{
		Empty,
		Full,
		Selected
	}

	private EState state_ = EState.Empty;
	public EState State
	{
		get { return state_; }
	}

	public System.Action<AlbumButton> clickAction;

	public void Awake()
	{
		image_ = GetComponent<UnityEngine.UI.Image>( );
	}


	public void Init(Album at)
	{
		album_ = at;
		SetText( );
		if (album_ == null)
		{
			state_ = EState.Empty;
		}
		else
		{
			state_ = EState.Full;
		}
		HandleStateChange( );
	}

	private void HandleStateChange( )
	{
		image_.color = stateColours[state_];
	}

	public bool Select()
	{
		bool result =false;
		switch (state_)
		{
			case EState.Empty:
				{
					Debug.LogWarning( "Select called when empty" );
					break;
				}
			case EState.Full:
				{
					state_ = EState.Selected;
					HandleStateChange( );
					result = true;
					break;
				}
			case EState.Selected:
				{
					Debug.LogWarning( "Select called when already selected" );
					state_ = EState.Selected;
					HandleStateChange( );
					result = true;
					break;
				}
		}
		return result;
	}

	public bool DeSelect( )
	{
		bool result = false;
		switch (state_)
		{
			case EState.Empty:
				{
					Debug.LogWarning( "DeSelect called when empty" );
					break;
				}
			case EState.Full:
				{
					
					Debug.LogWarning( "DeSelect called when already deselected" );
					state_ = EState.Full;
					HandleStateChange( );
					result = true;
					break;
				}
			case EState.Selected:
				{
					state_ = EState.Full;
					HandleStateChange( );
					result = true;
					break;
				}
		}
		return result;
	}


	System.Text.StringBuilder sb = new System.Text.StringBuilder( );

	private void SetText()
	{
		if (album_ == null)
		{
			albumNameText.text = string.Empty;
		}
		else
		{
			sb.Length = 0;
			sb.Append( album_.AlbumName );
			albumNameText.text = sb.ToString();
		}
	}

	public void OnClick()
	{
		if (clickAction != null)
		{
			clickAction( this);
		}
	}
}
