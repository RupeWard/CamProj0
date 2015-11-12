using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonSet : MonoBehaviour
{
	private UnityEngine.UI.VerticalLayoutGroup vlg_;
	private UnityEngine.UI.HorizontalLayoutGroup hlg_;

	public int margin = 5;

	private void Awake()
	{
		vlg_ = GetComponent<UnityEngine.UI.VerticalLayoutGroup>( );
		hlg_ = GetComponent<UnityEngine.UI.HorizontalLayoutGroup>( );
		if (vlg_ != null)
		{
			vlg_.padding = new RectOffset( margin, margin, margin, margin );
		}
	}

	public int NumButtons
	{
		get { return buttons_.Count; }
	}

	private List<ButtonSetButton> buttons_ = new List<ButtonSetButton>( );

	public void AddButton(ButtonSetButton b)
	{
		buttons_.Add( b );
	}
}
