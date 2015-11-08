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

//	public WinLayerManager winLayerManager;

	#endregion Inspector hooks

	#region MonoBehaviour

	public void Start()
    {
		WinLayerManager.Instance.InstantiateToLayer( logPanelLayerPrefab );
		WinLayerManager.Instance.InstantiateToLayer( deviceCameraDisplayLayerPrefab );
		WinLayerManager.Instance.InstantiateToTopLayer( buttonsLayerPrefab );
	}

	#endregion MonoBehaviour

	#region SetUp


	#endregion SetUp


}
