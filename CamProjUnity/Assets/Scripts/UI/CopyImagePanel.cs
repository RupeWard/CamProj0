using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyImagePanel : MonoBehaviour
{
	//	private static readonly bool DEBUG_LOCAL = true;

	private Album fromAlbum_;
	private Album toAlbum_;
	private AlbumTexture albumTexture_;

	public UnityEngine.UI.Text titleText;
	public AlbumButton[] albumButtons = new AlbumButton[0];

	public UnityEngine.UI.Text questionText;

	void Start()
	{	
		foreach (AlbumButton i in albumButtons)
		{
			i.clickAction += HandleAlbumButtonClicked;
		}
	}

	protected void OnDestroy( )
	{
	}

	public void Init(Album a, AlbumTexture at)
	{
		SetAlbumTexture( a, at );
	}

	public void Open( Album a, AlbumTexture at )
	{
		Init( a, at );
		gameObject.SetActive( true );
	}

	public void SetAlbumTexture( Album a, AlbumTexture at )
	{
		toAlbum_ = null;
		fromAlbum_ = a;
		albumTexture_ = at;
		SetTitle( );
		SetQuestion( );
//		fromAlbum_.OnAlbumChanged += HandleAlbumsChanged;
	}

	private void SetTitle()
	{
		if (albumTexture_ == null)
		{
			titleText.text = "Nothing to Copy";
		}
		else
		{
			titleText.text = "Copy" + albumTexture_.imageName+"?";
		}
	}

	private void SetQuestion( )
	{
		if (albumTexture_ == null)
		{
			questionText.text = "";
		}
		else
		{
			if (toAlbum_ == null)
			{
				questionText.text = "Copy " + albumTexture_.imageName + " from " + fromAlbum_.AlbumName + " to where?";
			}
			else
			{
				questionText.text = "Copy " + albumTexture_.imageName +" from "+fromAlbum_.AlbumName+" to "+toAlbum_.AlbumName+ "?";
			}
		}
	}

	/*
	private void HandleAlbumsChanged()
	{
		Debug.Log( "AMP: HandleAlbumsChanged" );
		SetTitle( );
		SetButtons( );
	}*/

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
						HandleSelectedButtonChanged( );
						break;
					}
			}
		}
	}

	private void HandleSelectedButtonChanged()
	{
		if (selectedButton_ == null)
		{
			toAlbum_ = null;
		}
		else
		{
			toAlbum_ = selectedButton_.Album;
		}
		SetQuestion( );
		SetButtons( );
	}

	private void SetButtons()
	{
		int numDone = 0;

		List<Album> albums = AlbumManager.Instance.Albums;
		if (albums != null)
		{
			for (int i = 0; i < albums.Count && numDone < albumButtons.Length; i++)
			{
				if (albums[i] != fromAlbum_)
				{
					albumButtons[numDone].Init( albums[i] );
					numDone++;
				}
			}
		}
		for (int i = numDone; i < albumButtons.Length; i++)
		{
			albumButtons[i].Init( null );
		}
	}

	public void OnConfirmButtonPressed()
	{
		if (albumTexture_ == null)
		{
			LogManager.Instance.AddLine( "No pic to copy" );
		}
		else if (fromAlbum_ == null)
		{
			LogManager.Instance.AddLine( "No from album for copy" );
		}
		else if (toAlbum_ == null)
		{
			//			LogManager.Instance.AddLine( "No to album for copy" );
			Debug.LogWarning( "No to album for copy" );
		}
		else
		{
			Debug.Log( "COPY " + albumTexture_.imageName + " from " + fromAlbum_.AlbumName + " to " + toAlbum_.AlbumName );
		}
	}

	public void OnCancelButtonPressed()
	{
		this.gameObject.SetActive( false );
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
	 


}
