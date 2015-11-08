using UnityEngine;
using System.Collections;

public class AlbumTexture: IDebugDescribable
{
	public Texture2D texture = null;
	public string imageName = string.Empty;

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
