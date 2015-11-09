using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumViewPanel : WinWin<AlbumViewPanel>
{
	//	private static readonly bool DEBUG_LOCAL = true;

	private Album album_;

	public UnityEngine.UI.Text titleText;
	public ImageButton[] imageButtons = new ImageButton[0];
	public UnityEngine.UI.RawImage rawImage;

	public UnityEngine.UI.Text previewedImageText;
	public UnityEngine.UI.Text selectedImageText;

	protected override void Awake()
	{
		base.Awake( );
		foreach (ImageButton i in imageButtons)
		{
			i.clickAction += HandleImageButtonClicked;
		}
	}

	public void Init(Album a)
	{
		album_ = a;
		album_.OnAlbumChanged += HandleAlbumChanged;

		HandleAlbumChanged( );
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
		SetButtons( );
	}

	private ImageButton selectedButton_ = null;

	public void HandleImageButtonClicked( ImageButton b )
	{
		if (b.AlbumTexture == null)
		{
			Debug.Log( "IB Clicked: NULL" );
		}
		else
		{
			Debug.Log( "IB Clicked: "+b.AlbumTexture.imageName );
			switch (b.State)
			{
				case ImageButton.EState.Empty:
					{
						if (selectedButton_ != null)
						{
							selectedButton_.DeSelect( );
							selectedButton_ = null;
							HandleSelectedButtonChanged( );
						}
						break;
					}
				case ImageButton.EState.Full:
					{
						if (selectedButton_ != null)
						{
							selectedButton_.DeSelect( );
							selectedButton_ = null;
						}
						b.Select( );
						selectedButton_ = b;
						HandleSelectedButtonChanged( );
						break;
					}
				case ImageButton.EState.Selected:
					{
						selectedButton_.DeSelect( );
						selectedButton_ = null;
						break;
					}
			}
		}
	}

	private void HandleSelectedButtonChanged()
	{
		if (selectedButton_ == null || selectedButton_.AlbumTexture == null)
		{
			selectedImageText.text = "NONE";
		}
		else
		{
			selectedImageText.text = selectedButton_.AlbumTexture.TitleString;
		}
	}

	private void SetButtons()
	{
		int numDone = 0;

		if (album_ != null)
		{
			List<AlbumTexture> albumTextures = album_.AlbumTextures;
			for (int i = 0; i<albumTextures.Count && numDone < imageButtons.Length; i++)
			{
				if (albumTextures[i] != null)
				{
					imageButtons[numDone].Init( albumTextures[i] );
					numDone++;
				}
			}
		}
		for (int i = numDone; i < imageButtons.Length; i++)
		{
			imageButtons[i].Init( null );
		}
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

	public void OnViewButtonPressed()
	{
		if (selectedButton_ != null && selectedButton_.AlbumTexture != null)
		{
			rawImage.texture = selectedButton_.AlbumTexture.texture;
			previewedImageText.text = selectedButton_.AlbumTexture.imageName;
		}
		else
		{
			rawImage.texture = null;
			previewedImageText.text = "NONE";
		}
	}

	public void OnDeleteButtonPressed()
	{
		if (selectedButton_ != null)
		{
			if (selectedButton_.AlbumTexture != null)
			{
				bool success = album_.Remove( selectedButton_.AlbumTexture );
				if (success)
				{
					HandleAlbumChanged( );
					selectedButton_ = null;
					HandleSelectedButtonChanged( );
				}

			}
		}
	}
}
