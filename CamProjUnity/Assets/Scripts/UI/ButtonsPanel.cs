using UnityEngine;
using System.Collections;

public class ButtonsPanel : MonoBehaviour
{
	public GameObject outButton;
	public GameObject inButton;

	private Vector3 outPosition_;
	private Vector3 inPosition_;

	private RectTransform rectTransform_;

	private Vector2 targetPosition_;

	public void OnOutButtonClicked()
	{
		targetPosition_ = outPosition_;
		outButton.gameObject.SetActive( false );
		inButton.gameObject.SetActive( true );
	}

	public void OnInButtonClicked()
	{
		targetPosition_ = inPosition_;
		outButton.gameObject.SetActive( true );
		inButton.gameObject.SetActive( false);
	}

	public void OnAlbumButtonClicked()
	{
		SceneControllerTest.Instance.ToggleAlbumView( );
	}

	public void OnAlbumManagerButtonClicked( )
	{
		SceneControllerTest.Instance.ToggleAlbumManager( );
	}

	public void OnDeviceCamerButtonClicked()
	{
		SceneControllerTest.Instance.ToggleDeviceCameraPanel( );
	}

	public void OnLogPanelButtonClicked()
	{
		SceneControllerTest.Instance.ToggleLogPanel( );
	}

	private float tweenSpeed= 5f;


	void Update( )
	{
		Vector2 newPosition = Vector2.Lerp( rectTransform_.anchoredPosition, targetPosition_, Time.deltaTime * tweenSpeed );
		rectTransform_.anchoredPosition = newPosition;
	}


	public void OnQuitButtonClicked()
	{
#if UNITY_EDITOR
		Debug.LogWarning( "No Quit in Editor" );
#else
		if (Application.platform == RuntimePlatform.Android)
		{
			Application.Quit( );
		}
		else
		{
			Debug.LogWarning( "No Quit in "+Application.platform  );
		}
#endif
	}

	private void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
		outPosition_ = rectTransform_.anchoredPosition;
		targetPosition_ = outPosition_;
		inPosition_ = outPosition_;
		inPosition_.x = inPosition_.x + GetComponent<RectTransform>( ).GetWidth( );
		outButton.gameObject.SetActive( false );
		inButton.gameObject.SetActive( true );
	}
}
