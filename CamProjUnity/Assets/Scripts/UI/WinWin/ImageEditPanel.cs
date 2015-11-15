using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageEditPanel : WinWin<ImageEditPanel>
{
//	private static readonly bool DEBUG_LOCAL = true;

	private AlbumTexture albumTexture_;

	public UnityEngine.UI.Text titleText;
	public UnityEngine.UI.RawImage rawImage;

//	public CopyImagePanel copyImagePanel;

	protected override void Awake()
	{
		base.Awake( );
	}

	protected override void Start()
	{
		base.Start( );
	//	AlbumManager.Instance.currentAlbumChangeActon += SetAlbum;
//		copyImagePanel.Close( );
	}

	public void Init(AlbumTexture  at)
	{
		SetAlbumTexture( at );
	}

	public void SetAlbumTexture( AlbumTexture at )
	{
		if (albumTexture_ != null)
		{
//			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
		albumTexture_ = at;
		if (albumTexture_ != null)
		{
//			album_.OnAlbumChanged += HandleAlbumChanged;
		}

		HandleAlbumTextureChanged( );
	}

	private void SetTitle()
	{
		if (albumTexture_ == null || albumTexture_.texture == null)
		{
			titleText.text = "No Pic";
		}
		else
		{
			titleText.text = albumTexture_.imageName+ " (" + albumTexture_.texture.width+"x"+albumTexture_.texture.height + ")";
		}
	}

	private void HandleAlbumTextureChanged()
	{
		SetTitle( );
		SetPreviewedImage( );
	}


	public void OnDisable( )
	{
		/*
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
		*/
		RemovePreviewedImage( );
	}

	public void OnEnable( )
	{
		/*
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
			album_.OnAlbumChanged += HandleAlbumChanged;
		}*/
		SetPreviewedImage( );
	}

	private void SetPreviewedImage()
	{
		if (albumTexture_ != null && albumTexture_.texture != null)
		{
			rawImage.texture = albumTexture_.texture;
		}
		else
		{
			rawImage.texture = null;
		}
	}
	private void RemovePreviewedImage()
	{
		rawImage.texture = null;
	}

	public void OnDestroy()
	{
		/*
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumChanged;
		}
		AlbumManager.Instance.currentAlbumChangeActon -= SetAlbum;
		*/
	}

	/*
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

	public void OnSaveTexturePressed()
	{
		if (AlbumManager.Instance.IOInProgress)
		{
			LogManager.Instance.AddLine( "Wait for previous IO to finish" );
		}
		else
		{
			if (album_ != null)
			{
				if (selectedButton_ != null)
				{
					AlbumTexture atToSave = selectedButton_.AlbumTexture;
					if (atToSave != null)
					{
						AlbumManager.Instance.IOInProgress = true;

						Debug.Log( "SaveTexture pressed " + album_.DebugDescribe( ) + " " + atToSave.DebugDescribe( ) );
						ConfirmPanel.Data data = new ConfirmPanel.Data( );
						data.title = "Confirm save '" + atToSave.imageName + "'";
						data.info = "Are you sure you want to save '" + atToSave.imageName + "'?";
						data.yesButtonDef = new ConfirmPanel.ButtonDef( "Yes", OnSaveConfirmed );
						data.noButtonDef = new ConfirmPanel.ButtonDef( "No", OnIOCancelled );

						WinLayerManager.Instance.CreateConfirmPanel( data );
					}
					else
					{
						Debug.Log( "SaveTextureAlbum pressed when texture null" );
						LogManager.Instance.AddLine( "Nothing to save" );
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

	}

	private void OnTextureSaveFinished()
	{
		AlbumManager.Instance.IOInProgress = false;
		BusyPanel.Instance.Close( );
	}

	private void OnSaveConfirmed()
	{
		if (selectedButton_ != null && selectedButton_.AlbumTexture != null)
		{
			Debug.Log( "OnSaveConfirmed: "+selectedButton_.AlbumTexture.imageName );
			AlbumTexture atToSave = selectedButton_.AlbumTexture;

			AlbumManager.Instance.SaveAlbumTexture( album_, selectedButton_.AlbumTexture, OnTextureSaveFinished);
			LogManager.Instance.AddLine( "Saving " + selectedButton_.AlbumTexture.imageName );
			Debug.Log( "Saving " + selectedButton_.AlbumTexture.imageName );
			HandleAlbumChanged( );

		}
		else
		{
			if (selectedButton_ == null) Debug.LogError( "Null SB" );
			else if (selectedButton_.AlbumTexture == null) Debug.LogError( "Null AT" );
		}
	}

	public void OnIOCancelled()
	{
		AlbumManager.Instance.IOInProgress = false;
	}
	*/
}
