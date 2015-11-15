using UnityEngine;
using System.Collections;

public class AlbumTexture: IDebugDescribable
{
	public enum EIOState
	{
		Unsaved,
		Modified,
		Saved
	};

	private EIOState ioState_ = EIOState.Unsaved;
	public EIOState IOState
	{
		get { return ioState_;  }
	}
	public void HandleSaved()
	{
		ioState_ = EIOState.Saved;	
	}
	public void HandleModified( )
	{
		ioState_ = EIOState.Modified;
	}

	public Texture2D texture = null;
	public string imageName = string.Empty;

	static System.Text.StringBuilder sb_ = new System.Text.StringBuilder( );

	public AlbumTexture CloneNameless()
	{
		AlbumTexture newAt = new AlbumTexture( );

		Texture2D newTexture = new Texture2D( texture.width, texture.height );
		newTexture.SetPixels( texture.GetPixels( ) );
		newTexture.Apply( );
		newAt.texture = newTexture;
		return newAt;
	}

    public string TitleString
	{
		get
		{
			sb_.Length = 0;
			sb_.Append( "'" ).Append( imageName ).Append( "' " );
			if (texture == null)
			{
				sb_.Append( "null" );
			}
			else
			{
				sb_.Append( texture.width ).Append( "x" ).Append( texture.height );
			}
			switch (ioState_)
			{
				case EIOState.Modified:
					{
						sb_.Append( " M" );
						break;
					}
				case EIOState.Saved:
					{
						sb_.Append( " S" );
						break;
					}
				case EIOState.Unsaved:
					{
						sb_.Append( " x" );
						break;
					}

			}
			return sb_.ToString( );
		}
	}

	public void DebugDescribe(System.Text.StringBuilder sb)
	{
		sb.Append( "[ATexture: '" ).Append( imageName ).Append( "' " );
		if (texture == null)
		{
			sb.Append( "null" );
		}
		else
		{
			sb.Append( texture.width ).Append( "x" ).Append( texture.height );
		}
		sb.Append( "]" );
	}
}
