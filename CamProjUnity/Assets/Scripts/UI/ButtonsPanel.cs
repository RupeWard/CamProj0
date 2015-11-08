using UnityEngine;
using System.Collections;

public class ButtonsPanel : MonoBehaviour
{
	public GameObject outButton;

	private Vector3 outPosition_;
	private Vector3 inPosition_;

	private RectTransform rectTransform_;

	public void OnOutButtonClicked()
	{
		rectTransform_.anchoredPosition = outPosition_;
		outButton.gameObject.SetActive( false );
	}

	public void OnInButtonClicked()
	{
		rectTransform_.anchoredPosition = inPosition_;
		outButton.gameObject.SetActive( true );
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
		inPosition_ = outPosition_;
		inPosition_.x = inPosition_.x + GetComponent<RectTransform>( ).GetWidth( );
		outButton.gameObject.SetActive( false );
	}
}
