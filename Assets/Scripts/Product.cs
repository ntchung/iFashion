using UnityEngine;
using System.Collections;

public class Product : MonoBehaviour {

	public ECategory Category;
	public EFittingPosition FittingPosition;
	public string Price;
	public string Title;
	
	public UI2DSprite Sprite2D;	
}

public enum EFittingPosition
{
	Top = 1,
	Bottom = 2,
	TopAndBottom = 3,
	HandBag = 4,
	Necklace = 8,	
}

public enum ECategory
{
	Outwear = 0,
	Dess,
	Top,
	Handbags,
	Accessories,
}
