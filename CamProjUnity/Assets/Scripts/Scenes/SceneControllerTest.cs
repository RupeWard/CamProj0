using UnityEngine;
using System.Collections;

public class SceneControllerTest : SingletonSceneLifetime< SceneControllerTest >
{
	static private readonly bool DEBUG_LOCAL = false;

	#region Prefabs

	public GameObject buttonsLayerPrefab;
	public GameObject albumViewPrefab;
	public GameObject albumManagerPanelPrefab;
	public GameObject deviceCameraPanelPrefab;
	public GameObject logPanelPrefab;


	#endregion Prefabs

	#region Inspector hooks

	//	public WinLayerManager winLayerManager;

	#endregion Inspector hooks

	#region MonoBehaviour

	public void Start()
    {
		WinLayerManager.Instance.InstantiateToTopLayer( buttonsLayerPrefab );
	}

	#endregion MonoBehaviour

	#region SetUp


	#endregion SetUp

	#region AlbumManager

	AlbumManagerPanel albumManagerPanel_ = null;
	WinLayerWin albumManagerPanelWLW_ = null;

	public void ToggleAlbumManager( )
	{
		if (albumManagerPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( albumManagerPanelPrefab );
			if (wlw != null)
			{
				albumManagerPanel_ = wlw.transform.GetComponentInChildren<AlbumManagerPanel>( );
				if (albumManagerPanel_ == null)
				{
					Debug.LogError( "Couldn't find AMP in instantiated prefab" );
				}
				else
				{
					albumManagerPanelWLW_ = wlw;
					albumManagerPanel_.Init( AlbumManager.Instance.CurrentAlbum );
				}
			}
		}
		else
		{
			if (albumManagerPanelWLW_ != null)
			{
				DestroyAlbumManagerPanel( );
			}
			else
			{
				Debug.LogError( "No WLW FOR AM" );
			}
		}
	}

	public void DestroyAlbumManagerPanel( )
	{
		if (albumManagerPanelWLW_ != null && albumManagerPanel_ != null)
		{
			WinLayerManager.Instance.RemoveContentsFromLayer( albumManagerPanelWLW_ );
			GameObject.Destroy( albumManagerPanelWLW_.gameObject );
			albumManagerPanel_ = null;
			albumManagerPanelWLW_ = null;
		}
		else
		{
			Debug.LogError( "ERROR" );
		}
	}


	public void BringAlbumManagerToFront( )
	{
		if (albumManagerPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( albumManagerPanelPrefab );
			if (wlw != null)
			{
				albumManagerPanel_ = wlw.transform.GetComponentInChildren<AlbumManagerPanel>( );
				if (albumManagerPanel_ == null)
				{
					Debug.LogError( "Couldn't find AMP in instantiated prefab" );
				}
				else
				{
					albumManagerPanelWLW_ = wlw;
					albumManagerPanel_.Init( AlbumManager.Instance.CurrentAlbum );
				}
			}
		}
		else
		{
			if (albumManagerPanelWLW_ != null)
			{
				albumManagerPanel_.SetAlbum( AlbumManager.Instance.CurrentAlbum );
				albumManagerPanelWLW_.MovetoTop( );
			}
			else
			{
				Debug.LogError( "No WLW FOR AM" );
			}
		}
	}

	#endregion AlbumManager

	#region AlbumView

	AlbumViewPanel albumViewPanel_ = null;
	WinLayerWin albumViewWLW_ = null;

	public void ToggleAlbumView( )
	{
		if (albumViewPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( albumViewPrefab );
			if (wlw != null)
			{
				albumViewPanel_ = wlw.transform.GetComponentInChildren<AlbumViewPanel>( );
				if (albumViewPanel_ == null)
				{
					Debug.LogError( "Couldn't find AVP in instantiated prefab" );
				}
				else
				{
					albumViewWLW_ = wlw;
					albumViewPanel_.Init( AlbumManager.Instance.CurrentAlbum );
				}
			}
		}
		else
		{
			if (albumViewWLW_ != null)
			{
				WinLayerManager.Instance.RemoveContentsFromLayer( albumViewWLW_ );
				GameObject.Destroy( albumViewWLW_.gameObject );
				albumViewPanel_ = null;
				albumViewWLW_ = null;
			}
			else
			{
				Debug.LogError( "No WLW" );
			}
		}
	}
	public void ClearAlbumView()
	{
		if (albumViewPanel_ != null)
		{
			albumViewPanel_.SetAlbum( null );
		}
		else
		{
			Debug.LogError( "Null album view panel" );
		}
	}

	public bool IsCurrentlyViewedAlbum(Album a)
	{
		bool result = false;
		if (albumViewPanel_ != null)
		{
			if (albumViewPanel_.IsAlbum(a))
			{
				result = true;
			}
		}
		return result;
	}

	public void BringAlbumViewToFront( Album a )
	{
		AlbumManager.Instance.CurrentAlbum = a;
		if (albumViewPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( albumViewPrefab );
			if (wlw != null)
			{
				albumViewPanel_ = wlw.transform.GetComponentInChildren<AlbumViewPanel>( );
				if (albumViewPanel_ == null)
				{
					Debug.LogError( "Couldn't find AVP in instantiated prefab" );
				}
				else
				{
					albumViewWLW_ = wlw;
					albumViewPanel_.Init( AlbumManager.Instance.CurrentAlbum );
				}
			}
		}
		else
		{
			if (albumViewWLW_ != null)
			{
				albumViewPanel_.SetAlbum( AlbumManager.Instance.CurrentAlbum );
				albumViewWLW_.MovetoTop( );
			}
			else
			{
				Debug.LogError( "No WLW FOR AV" );
			}
		}
	}

	public void DestroyAlbumViewPanel( )
	{
		if (albumViewWLW_ != null && albumViewPanel_ != null)
		{
			WinLayerManager.Instance.RemoveContentsFromLayer( albumViewWLW_ );
			GameObject.Destroy( albumViewWLW_.gameObject );
			albumViewPanel_ = null;
			albumViewWLW_ = null;
		}
		else
		{
			Debug.LogError( "ERROR" );
		}
	}

	#endregion AlbumView

	#region Device

	DeviceCameraDisplay deviceCameraPanel_ = null;
	WinLayerWin deviceCameraPanelWLW_ = null;

	public void ToggleDeviceCameraPanel( )
	{
		if (deviceCameraPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( deviceCameraPanelPrefab );
			if (wlw != null)
			{
				deviceCameraPanel_ = wlw.transform.GetComponentInChildren<DeviceCameraDisplay>( );
				if (deviceCameraPanel_ == null)
				{
					Debug.LogError( "Couldn't find DCP in instantiated prefab" );
				}
				else
				{
					if (DEBUG_LOCAL)
					{
						Debug.LogWarning( "SCT created DCD" );
					}
					deviceCameraPanelWLW_ = wlw;
					//					deviceCameraPanel_.Init( CurrentAlbum );
				}
			}
		}
		else
		{
			if (deviceCameraPanelWLW_ != null)
			{
				WinLayerManager.Instance.RemoveContentsFromLayer( deviceCameraPanelWLW_ );
				GameObject.Destroy( deviceCameraPanelWLW_.gameObject );
				deviceCameraPanel_ = null;
				deviceCameraPanelWLW_ = null;
				if (DEBUG_LOCAL)
				{
					Debug.LogWarning( "SCT closed DCD" );
				}
			}
			else
			{
				Debug.LogError( "No WLW" );
			}
		}
	}

	public void BringDeviceCameraPanelToFront( )
	{
		if (deviceCameraPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( deviceCameraPanelPrefab );
			if (wlw != null)
			{
				deviceCameraPanel_ = wlw.transform.GetComponentInChildren<DeviceCameraDisplay>( );
				if (deviceCameraPanel_ == null)
				{
					Debug.LogError( "Couldn't find DCP in instantiated prefab" );
				}
				else
				{
					deviceCameraPanelWLW_ = wlw;
					//					deviceCameraPanel_.Init( CurrentAlbum );
				}
			}
		}
		else
		{
			if (deviceCameraPanelWLW_ != null)
			{
				//				deviceCameraPanel_.SetAlbum( CurrentAlbum );
				deviceCameraPanelWLW_.MovetoTop( );
			}
			else
			{
				Debug.LogError( "No WLW FOR DCP" );
			}
		}
	}

	public void DestroyDeviceCameraPanel( )
	{
		if (deviceCameraPanelWLW_ != null && deviceCameraPanel_ != null)
		{
			WinLayerManager.Instance.RemoveContentsFromLayer( deviceCameraPanelWLW_ );
			GameObject.Destroy( deviceCameraPanelWLW_.gameObject );
			deviceCameraPanel_ = null;
			deviceCameraPanelWLW_ = null;
		}
		else
		{
			Debug.LogError( "ERROR" );
		}
	}
	#endregion Device

	#region Log

	LogPanel logPanel_ = null;
	WinLayerWin logPanelWLW_ = null;

	public void ToggleLogPanel( )
	{
		if (logPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( logPanelPrefab );
			if (wlw != null)
			{
				logPanel_ = wlw.transform.GetComponentInChildren<LogPanel>( );
				if (logPanel_ == null)
				{
					Debug.LogError( "Couldn't find LP in instantiated prefab" );
				}
				else
				{
					logPanelWLW_ = wlw;
					//					deviceCameraPanel_.Init( CurrentAlbum );
				}
			}
		}
		else
		{
			if (logPanelWLW_ != null)
			{
				WinLayerManager.Instance.RemoveContentsFromLayer( logPanelWLW_ );
				GameObject.Destroy( logPanelWLW_.gameObject );
				logPanel_ = null;
				logPanelWLW_ = null;
			}
			else
			{
				Debug.LogError( "No WLW" );
			}
		}
	}

	public void BringLogPanelToFront( )
	{
		if (logPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( logPanelPrefab );
			if (wlw != null)
			{
				logPanel_ = wlw.transform.GetComponentInChildren<LogPanel>( );
				if (logPanel_ == null)
				{
					Debug.LogError( "Couldn't find LP in instantiated prefab" );
				}
				else
				{
					logPanelWLW_ = wlw;
					//					deviceCameraPanel_.Init( CurrentAlbum );
				}
			}
		}
		else
		{
			if (logPanelWLW_ != null)
			{
				//				deviceCameraPanel_.SetAlbum( CurrentAlbum );
				logPanelWLW_.MovetoTop( );
			}
			else
			{
				Debug.LogError( "No WLW FOR LP" );
			}
		}
	}

	public void DestroyLogPanel( )
	{
		if (logPanelWLW_ != null && logPanel_ != null)
		{
			WinLayerManager.Instance.RemoveContentsFromLayer( logPanelWLW_ );
			GameObject.Destroy( logPanelWLW_.gameObject );
			logPanel_ = null;
			logPanelWLW_ = null;
		}
		else
		{
			Debug.LogError( "ERROR" );
		}
	}
	#endregion Log


}
