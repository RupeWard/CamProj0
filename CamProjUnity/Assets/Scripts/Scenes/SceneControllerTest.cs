using UnityEngine;
using System.Collections;

public class SceneControllerTest : MonoBehaviour
{
	#region Prefabs

	public GameObject deviceCameraDisplayLayerPrefab;
	public GameObject logPanelLayerPrefab;
	public GameObject buttonsLayerPrefab;

	#endregion Prefabs

	#region Inspector hooks

	public RW.Win.WinLayerManager winLayerManager;

	#endregion Inspector hooks

	#region MonoBehaviour

	public void Start()
    {
		InstantiateToTopLayer( deviceCameraDisplayLayerPrefab );
		InstantiateToTopLayer( logPanelLayerPrefab );
		InstantiateToTopLayer( buttonsLayerPrefab );
	}

	#endregion MonoBehaviour

	#region SetUp

	private void InstantiateToTopLayer( GameObject prefab )
	{
		GameObject go = Instantiate( prefab ) as GameObject;
		RW.Win.WinLayerWin win = go.GetComponent<RW.Win.WinLayerWin>( );
		winLayerManager.AddToTopLayer( win );
	}

	#endregion SetUp


}
