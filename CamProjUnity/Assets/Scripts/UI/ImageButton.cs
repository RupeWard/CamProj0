using UnityEngine;
using System.Collections;

public class ImageButton : MonoBehaviour
{
	public UnityEngine.UI.Text imageNameText;

	private AlbumTexture albumTexture_ = null;
	public AlbumTexture AlbumTexture
	{
		get { return albumTexture_;  }
	}

	public System.Action<ImageButton> clickAction;

	public void Init(AlbumTexture at)
	{
		albumTexture_ = at;
		SetText( );
	}

	private void SetText()
	{
		if (albumTexture_ == null)
		{
			imageNameText.text = string.Empty;
		}
		else
		{
			imageNameText.text = albumTexture_.imageName;
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
