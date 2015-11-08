using UnityEngine;
using System.Collections;

public class AlbumTexture: IDebugDescribable
{
	public Texture2D texture = null;
	public string imageName = string.Empty;

	static System.Text.StringBuilder sb_ = new System.Text.StringBuilder( );

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
