using UnityEngine;
using System.Collections;

public class AlbumDefn
{
	private string albumName_;
	
	public string AlbumName
	{
		get { return albumName_;  }
	}

	public AlbumDefn(string n)
	{
		albumName_ = n;
	}

	
}
