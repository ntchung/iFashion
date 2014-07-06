using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WearingFigure {
	
	private List<Product> m_items = new List<Product>();

	public void Add(Product product)
	{
		for( int i=m_items.Count-1; i>=0; --i )
		{
			if( m_items[i].FittingPosition == product.FittingPosition )
			{
				m_items.RemoveAt(i);
				break;
			}
		}
		
		m_items.Add(product);
	}
	
	public bool Remove(Product product)
	{
		for( int i=m_items.Count-1; i>=0; --i )
		{
			if( m_items[i] == product )
			{
				m_items.RemoveAt(i);				
				return true;
			}
		}
		
		return false;
	}
	
	public void Fit(GameObject view, int viewHeight)	
	{
		int depth = 2;		
		foreach( Product product in m_items )
		{
			Fit(view, depth, product, viewHeight);
			++depth;
		}
	}
	
	public void Clear(GameObject view)	
	{
		foreach( Transform child in view.transform )
		{
			GameObject.Destroy(child.gameObject);
		}
	}
	
	private static void Fit(GameObject view, int depth, Product product, int viewHeight)
	{
		GameObject prod = GameObject.Instantiate(product.gameObject) as GameObject;
		prod.transform.parent = view.transform;
		prod.transform.localPosition = Vector3.zero;
		prod.transform.localScale = Vector3.one;
		
		UI2DSprite spr = prod.GetComponent<UI2DSprite>();
		spr.depth = depth;
		spr.keepAspectRatio = UIWidget.AspectRatioSource.Free;
		spr.height = viewHeight;		
		spr.width = viewHeight >> 1;
		spr.sprite2D = product.WearingSprite;
	}
	
	public WearingFigure Clone()
	{
		WearingFigure temp = new WearingFigure();
		temp.m_items.AddRange(m_items);
		return temp;
	}
	
	public string Export()
	{
		string line = "";
		foreach( Product product in m_items )
		{
			if( line.Length > 0 )
			{
				line += "\t";
			}
			line += product.name;
		}
		return line;
	}
	
	public void Import(string line)
	{
		string[] parts = line.Split('\t');
		foreach( string part in parts )
		{
			Product product = ProductsManager.Instance.Find(part);
			if( product != null )
			{
				m_items.Add(product);
			}
		}
	}
	
	public bool IsWearing(Product product)
	{
		foreach( Product item in m_items )
		{
			if( item.name.Equals(product.name) )
			{
				return true;
			}
		}
		
		return false;
	}
}
