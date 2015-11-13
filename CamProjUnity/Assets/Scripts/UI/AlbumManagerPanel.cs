using UnityEngine;
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

	private void Start()
	{
		AlbumManager.Instance.allAlbumsLoadedAction += HandleAlbumsChanged;
		foreach (AlbumButton i in albumButtons)
		{
			i.clickAction += HandleAlbumButtonClicked;
		}
		newAlbumPanel.onNewAlbumCancel += OnNewAlbumCancelled;
		newAlbumPanel.onNewAlbumConfirm += OnNewAlbumConfirmed;

		newAlbumPanel.Close( );
	}

	protected void OnDestroy( )
	{
		AlbumManager.Instance.allAlbumsLoadedAction -= HandleAlbumsChanged;
	}

	public void Init(Album a)
	{
		SetAlbum( a );
	}

	public void SetAlbum( Album a )
	{
		if (album_ != null)
		{
			album_.OnAlbumChanged -= HandleAlbumsChanged;
		}
		album_ = a;
		album_.OnAlbumChanged += HandleAlbumsChanged;
		HandleAlbumsChanged( );
	}

	private void SetTitle()
	{
		titleText.text = "AlbumManager: ";
	}

	private void HandleAlbumsChanged()
	{
		Debug.Log( "AMP: HandleAlbumsChanged" );
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

		List<Album> albums = AlbumManager.Instance.Albums;
		if (albums != null)
		{
			for (int i = 0; i < albums.Count && numDone < albumButtons.Length; i++)
			{
				albumButtons[numDone].Init( albums[i] );
				numDone++;
			}
		}
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

	public void OnViewButtonPressed()
	{
		if (selectedButton_ != null && selectedButton_.Album != null)
		{
			SceneControllerTest.Instance.BringAlbumViewToFront( selectedButton_.Album );
		}
		else
		{
			if (selectedButton_ == null)
			{
				Debug.LogWarning( "No SB" ); 
            }
			else
			{
				Debug.LogWarning( "No Album" );
			}
		}
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

	public void OnNewAlbumClicked()
	{
		if (AlbumManager.Instance.IOInProgress)
		{
			LogManager.Instance.AddLine( "Wait for previous IO to finish" );
		}
		else
		{
			AlbumManager.Instance.IOInProgress = true;

			bool allSaved = true;
			if (AlbumManager.Instance.CurrentAlbum != null && AlbumManager.Instance.CurrentAlbum.HasUnsavedChanges())
			{
				allSaved = false;
			}
			if (!allSaved)
			{
				ConfirmPanel.Data data = new ConfirmPanel.Data( );
				data.title = "Unsaved changes";
				data.info = "Current Album '" + AlbumManager.Instance.CurrentAlbum.AlbumName + "' has unsaved changes."
					+"\nAre you sure you want to create a new album?'";
				data.yesButtonDef = new ConfirmPanel.ButtonDef( "Yes", OpenNewAlbumPanel);
				data.noButtonDef = new ConfirmPanel.ButtonDef( "No", OnIOCancelled );

				WinLayerManager.Instance.CreateConfirmPanel( data );

			}
			else
			{
				OpenNewAlbumPanel( );
			}
		}
		// check for unsaved changes
		// open confirm panel if unsaved changes otherwise straight to open panel


	}

	private void OnIOCancelled()
	{
		AlbumManager.Instance.IOInProgress = false;
	}
	public NewAlbumPanel newAlbumPanel;

	private void OpenNewAlbumPanel()
	{
		newAlbumPanel.Open( );
	}

	private void OnNewAlbumConfirmed(string s)
	{
		if (s.Length == 0)
		{
			Debug.LogError( "Empty album name" );
		}
		else
		{
			AlbumManager.Instance.CreateNewCurrentAlbum( s );
		}
		AlbumManager.Instance.IOInProgress = false;
	}

	private void OnNewAlbumCancelled( )
	{
		AlbumManager.Instance.IOInProgress = false;
	}

}
