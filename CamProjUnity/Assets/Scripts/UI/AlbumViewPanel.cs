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
	public UnityEngine.UI.Text saveTextureButtonText;

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
		SetAlbum( a );
	}

	public void SetAlbum( Album a )
	{
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
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
			AlbumTexture atToRemove = selectedButton_.AlbumTexture;
			if (atToRemove != null)
			{
				if (selectedButton_.AlbumTexture.IOState == AlbumTexture.EIOState.Modified)
				{
					Debug.LogWarning( "Can't delete modified" );
					LogManager.Instance.AddLine( "Can't delete modified " + selectedButton_.AlbumTexture.imageName );
				}
				else
				{
					ConfirmPanel.Data data = new ConfirmPanel.Data( );
					data.title = "Confirm delete '" + atToRemove.imageName + "'";
					data.info = "Are you sure you want to delete '" + atToRemove.imageName + "'?";
					data.yesButtonDef = new ConfirmPanel.ButtonDef( "Yes", OnDeleteConfirm );
					data.noButtonDef = new ConfirmPanel.ButtonDef( "No", null );

					WinLayerManager.Instance.CreateConfirmPanel( data );

					/*
					bool success = album_.Remove( atToRemove );
					if (success)
					{
						if (selectedButton_.AlbumTexture.IOState == AlbumTexture.EIOState.Unsaved)
						{
							Debug.Log( "Not saved so not deleting" );
							LogManager.Instance.AddLine( "Not saved so not deleting " + selectedButton_.AlbumTexture.imageName );
						}
						else
						{
							Debug.Log( "Deleting file "+selectedButton_.AlbumTexture.DebugDescribe() );
							LogManager.Instance.AddLine( "Deleting " + selectedButton_.AlbumTexture.imageName );
							AlbumManager.Instance.DeleteAlbumTexture( album_, atToRemove, HandleAlbumChanged );
						}
					}
					*/

				}
			}
		}
	}

	public void OnDeleteConfirm( )
	{
		if (selectedButton_ != null && selectedButton_.AlbumTexture != null)
		{
			AlbumTexture atToRemove = selectedButton_.AlbumTexture;

			bool success = album_.Remove( atToRemove );
			if (success)
			{
				if (selectedButton_.AlbumTexture.IOState == AlbumTexture.EIOState.Unsaved)
				{
					Debug.Log( "Not saved so not deleting" );
					LogManager.Instance.AddLine( "Not saved so not deleting " + selectedButton_.AlbumTexture.imageName );
				}
				else
				{
					Debug.Log( "Deleting file " + selectedButton_.AlbumTexture.DebugDescribe( ) );
					LogManager.Instance.AddLine( "Deleting " + selectedButton_.AlbumTexture.imageName );
					AlbumManager.Instance.DeleteAlbumTexture( album_, atToRemove, HandleAlbumChanged );
				}
				selectedButton_.Init(null );
				selectedButton_ = null;
				HandleSelectedButtonChanged( );
				HandleAlbumChanged( );
			}

		}
		else
		{
			if (selectedButton_ == null) Debug.LogError( "Null SB" );
			else if (selectedButton_.AlbumTexture == null) Debug.LogError( "Null AT" );
		}

	}

	public void OnSaveTexturePressed()
	{
		if (album_ != null)
		{
			if (selectedButton_ != null)
			{
				if (selectedButton_.AlbumTexture != null)
				{
					Debug.Log( "SaveTexture pressed " + album_.DebugDescribe( )+" "+selectedButton_.AlbumTexture.DebugDescribe() );
					AlbumManager.Instance.SaveAlbumTexture( album_ , selectedButton_.AlbumTexture, HandleAlbumChanged);
					LogManager.Instance.AddLine( "Saving " + selectedButton_.AlbumTexture.imageName );
					HandleAlbumChanged( );
				}
				else
				{
					Debug.Log( "SaveTextureAlbum pressed when texture null" );
					LogManager.Instance.AddLine( "Nothing to save");
				}
			}
			else
			{
				Debug.Log( "SaveTextureAlbum pressed when albumtexture null" );
				LogManager.Instance.AddLine( "Nothing to save" );
			}
		}
		else
		{
			Debug.Log( "SaveTextureAlbum pressed when Album null" );
			LogManager.Instance.AddLine( "Nothing to save" );
		}

	}

	public void OnSaveAlbumPressed()
	{
		if (album_ != null)
		{
			Debug.Log( "SaveAlbum pressed " + album_.DebugDescribe( ) );
			LogManager.Instance.AddLine( "SaveAlbum pressed " + album_.DebugDescribe( ) );
			AlbumManager.Instance.SaveAlbum( album_, HandleAlbumChanged);
		}
		else
		{
			Debug.Log( "SaveAlbum pressed when null" );
			LogManager.Instance.AddLine( "Nothing to save" );
		}
	}

	public void OnManagerButtonPressed()
	{
		SceneControllerTest.Instance.BringAlbumManagerToFront( );
	}


}
