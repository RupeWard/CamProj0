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
		winLayerManager.InstantiateToLayer( deviceCameraDisplayLayerPrefab );
		winLayerManager.InstantiateToLayer( logPanelLayerPrefab );
		winLayerManager.InstantiateToTopLayer( buttonsLayerPrefab );
	}

	#endregion MonoBehaviour

	#region SetUp


	#endregion SetUp


}
