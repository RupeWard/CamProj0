using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageButton : MonoBehaviour
{
	public UnityEngine.UI.Text imageNameText;
	private UnityEngine.UI.Image image_;
	private AlbumTexture albumTexture_ = null;
	public AlbumTexture AlbumTexture
	{
		get { return albumTexture_;  }
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

	public System.Action<ImageButton> clickAction;

	public void Awake()
	{
		image_ = GetComponent<UnityEngine.UI.Image>( );
	}

	public void Init(AlbumTexture at)
	{
		albumTexture_ = at;
		SetText( );
		if (albumTexture_ == null)
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
		if (albumTexture_ == null)
		{
			imageNameText.text = string.Empty;
		}
		else
		{
			sb.Length = 0;
			if (albumTexture_.IOState == AlbumTexture.EIOState.Modified)
			{
				sb.Append( "(" );
			}
			else if (albumTexture_.IOState == AlbumTexture.EIOState.Saved)
			{
				sb.Append( "[" );
			}
			sb.Append( albumTexture_.imageName );
			if (albumTexture_.IOState == AlbumTexture.EIOState.Modified)
			{
				sb.Append( ")" );
			}
			else if (albumTexture_.IOState == AlbumTexture.EIOState.Saved)
			{
				sb.Append( "]" );
			}
			imageNameText.text = sb.ToString();
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
