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

	public WinLayerManager winLayerManager;

	#endregion Inspector hooks

	#region MonoBehaviour

	public void Start()
    {
		winLayerManager.InstantiateToLayer( logPanelLayerPrefab );
		winLayerManager.InstantiateToLayer( deviceCameraDisplayLayerPrefab );
		winLayerManager.InstantiateToTopLayer( buttonsLayerPrefab );
	}

	#endregion MonoBehaviour

	#region SetUp


	#endregion SetUp


}
