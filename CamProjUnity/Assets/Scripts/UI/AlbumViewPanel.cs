using UnityEngine;
using System.Collections;

public class AlbumViewPanel : WinWin<AlbumViewPanel>
{
	//	private static readonly bool DEBUG_LOCAL = true;

	private Album album_;

	public UnityEngine.UI.Text titleText;

	public void Init(Album a)
	{
		album_ = a;
		SetTitle();
		album_.OnAlbumChanged += HandleAlbumChanged;
	}

	private void SetTitle()
	{
		if (album_ == null)
		{
			titleText.text = "No Album";
		}
		else
		{
			titleText.text = "Album: " + album_.AlbumName + " (" + album_.NumTextures + ")";
		}
	}

	private void HandleAlbumChanged()
	{
		SetTitle( );
	}

	public void OnDisable( )
	{
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
	}

	public void OnEnable( )
	{
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
			album_.OnAlbumChanged += HandleAlbumChanged;
		}
	}

	public void OnDestroy()
	{
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
    }
}
