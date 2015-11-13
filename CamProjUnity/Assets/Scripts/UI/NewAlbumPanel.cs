using UnityEngine;
using System.Collections;

public class NewAlbumPanel : MonoBehaviour
{
	public UnityEngine.UI.InputField nameInputField;

	public UnityEngine.UI.Text questionText;

	public System.Action<string> onNewAlbumConfirm;
	public System.Action onNewAlbumCancel;

	private string name_ = string.Empty;

	public void OnNameInputFieldChanged (string s)
	{
		s = nameInputField.text;
		if (ValidateName( s ))
		{
			name_ = s;
		}
		SetName( name_ );
	}

	private void SetName(string n)
	{
		nameInputField.text = name_;
		if (name.Length > 0)
		{
			questionText.text = "Create new album called '" + name_ + "'?";
		}
		else
		{
			questionText.text = "Enter name for new album";
		}
	}

	private bool ValidateName(string n)
	{
		bool result = true;
		if (AlbumManager.Instance.ContainsAlbum(n))
		{
			result = false;
			{
				LogManager.Instance.AddLine( "Already have album called '" + n + "'" );
			}
		}
		return result;
	}

	public void OnConfirmButtonPressed()
	{
		Debug.Log( "OnConfirmButtonPressed" );
        if (name_.Length > 0)
		{
			if (ValidateName( name_ ))
			{
				if (onNewAlbumConfirm != null)
				{
					onNewAlbumConfirm( name_ );
				}
				Close( );
			}
			else
			{
				LogManager.Instance.AddLine( "Already have album called '" + name_ + "'" );
			}
		}
		else
		{
			LogManager.Instance.AddLine( "Enter an Album name!" );
		}
	}

	public void OnCancelButtonPressed( )
	{
		if (onNewAlbumCancel!= null)
		{
			onNewAlbumCancel( );
		}
		Close( );
	}

	public void Open()
	{
		SetName( string.Empty );
		gameObject.SetActive( true );
	}
	public void Close()
	{
		gameObject.SetActive( false );
	}
}
