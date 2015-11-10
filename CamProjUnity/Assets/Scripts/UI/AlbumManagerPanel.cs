﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumManagerPanel : WinWin< AlbumManagerPanel >
{
	//	private static readonly bool DEBUG_LOCAL = true;

	private Album album_;

	public UnityEngine.UI.Text titleText;
	public AlbumButton[] albumButtons = new AlbumButton[0];
//	public UnityEngine.UI.RawImage rawImage;

//	public UnityEngine.UI.Text previewedImageText;
	public UnityEngine.UI.Text selectedAlbumText;
//	public UnityEngine.UI.Text saveTextureButtonText;

	protected override void Awake()
	{
		base.Awake( );
		foreach (AlbumButton i in albumButtons)
		{
			i.clickAction += HandleAlbumButtonClicked;
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
		titleText.text = "AlbumManager: ";
	}

	private void HandleAlbumChanged()
	{
		SetTitle( );
		SetButtons( );
	}

	private AlbumButton selectedButton_ = null;

	public void HandleAlbumButtonClicked( AlbumButton b )
	{
		if (b.Album == null)
		{
			Debug.Log( "IB Clicked: NULL" );
		}
		else
		{
			Debug.Log( "IB Clicked: "+b.Album.AlbumName );
			switch (b.State)
			{
				case AlbumButton.EState.Empty:
					{
						if (selectedButton_ != null)
						{
							selectedButton_.DeSelect( );
							selectedButton_ = null;
							HandleSelectedButtonChanged( );
						}
						break;
					}
				case AlbumButton.EState.Full:
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
				case AlbumButton.EState.Selected:
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
		if (selectedButton_ == null || selectedButton_.Album== null)
		{
			selectedAlbumText.text = "NONE";
		}
		else
		{
			selectedAlbumText.text = selectedButton_.Album.TitleString;
		}
	}

	private void SetButtons()
	{
		int numDone = 0;

		/*
		if (album_ != null)
		{
			List<AlbumTexture> albumTextures = album_.AlbumTextures;
			for (int i = 0; i<albumTextures.Count && numDone < albumButtons.Length; i++)
			{
				if (albumTextures[i] != null)
				{
					albumButtons[numDone].Init( albumTextures[i] );
					numDone++;
				}
			}
		}
		*/
		for (int i = numDone; i < albumButtons.Length; i++)
		{
			albumButtons[i].Init( null );
		}
	}

	public void OnDisable( )
	{
		/*
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
		*/
	}

	public void OnEnable( )
	{
		/*
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
			album_.OnAlbumChanged += HandleAlbumChanged;
		}*/
	}

	public void OnDestroy()
	{
		/*
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
		*/
    }

	public void OnViewButtonPressed()
	{
		/*
		if (selectedButton_ != null && selectedButton_.AlbumTexture != null)
		{
			rawImage.texture = selectedButton_.AlbumTexture.texture;
			previewedImageText.text = selectedButton_.AlbumTexture.imageName;
		}
		else
		{
			rawImage.texture = null;
			previewedImageText.text = "NONE";
		}*/
	}

	public void OnDeleteButtonPressed()
	{
		/*
		if (selectedButton_ != null)
		{
			AlbumTexture atToRemove = selectedButton_.AlbumTexture;
			if (atToRemove != null)
			{
				if (selectedButton_.AlbumTexture.IOState == AlbumTexture.EIOState.Modified)
				{
					Debug.LogWarning( "Can;t delete modified" );
				}
				else
				{
					bool success = album_.Remove( atToRemove );
					if (success)
					{
						if (selectedButton_.AlbumTexture.IOState == AlbumTexture.EIOState.Unsaved)
						{
							Debug.Log( "Not saved so not deleting" );
						}
						else
						{
							Debug.Log( "Deleting file "+selectedButton_.AlbumTexture.DebugDescribe() );
							AlbumManager.Instance.DeleteAlbumTexture( album_, atToRemove, HandleAlbumChanged );
						}
					}

				}
				selectedButton_ = null;
				HandleSelectedButtonChanged( );
			}
		}
		*/
	}

	public void OnSaveTexturePressed()
	{
		/*
		if (album_ != null)
		{
			if (selectedButton_ != null)
			{
				if (selectedButton_.AlbumTexture != null)
				{
					Debug.Log( "SaveTexture pressed " + album_.DebugDescribe( )+" "+selectedButton_.AlbumTexture.DebugDescribe() );
					AlbumManager.Instance.SaveAlbumTexture( album_ , selectedButton_.AlbumTexture, HandleAlbumChanged);
					HandleAlbumChanged( );
				}
				else
				{
					Debug.Log( "SaveTextureAlbum pressed when texture null" );
				}
			}
			else
			{
				Debug.Log( "SaveTextureAlbum pressed when albumtexture null" );
			}
		}
		else
		{
			Debug.Log( "SaveTextureAlbum pressed when Album null" );
		}
		*/
	}

	public void OnSaveAlbumPressed()
	{
		/*
		if (album_ != null)
		{
			Debug.Log( "SaveAlbum pressed " + album_.DebugDescribe( ) );
			AlbumManager.Instance.SaveAlbum( album_, HandleAlbumChanged);
		}
		else
		{
			Debug.Log( "SaveAlbum pressed when null" );
		}*/
	}


}
