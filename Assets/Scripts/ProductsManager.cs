using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductsManager : MonoBehaviour {
	
	private static ProductsManager g_instance = null;
	
	public static ProductsManager Instance
	{
		get { return g_instance; }
	}
	
	private List<Product> m_products = new List<Product>();	
	
	void Awake()
	{
		g_instance = this;
	}

	// Use this for initialization
	void Start () {		
		LoadProduct("AccProduct1");
		LoadProduct("AccProduct2");
	
		LoadProduct("OutwearProduct1");
		
		LoadProduct("TopProduct1");
		LoadProduct("TopProduct2");
		LoadProduct("TopProduct3");
		
		LoadProduct("DressProduct1");
		LoadProduct("DressProduct2");
		LoadProduct("DressProduct3");
		LoadProduct("DressProduct4");
		LoadProduct("DressProduct5");
		LoadProduct("DressProduct6");
		
		LoadProduct("HandbagProduct1");
		LoadProduct("HandbagProduct2");
		LoadProduct("HandbagProduct3");
	}
	
	private void LoadProduct(string name)
	{
		m_products.Add(Resources.Load<Product>(name));
	}
	
	public List<Product> Products
	{
		get { return m_products; }
	}
	
	public Product Find(string name)
	{
		foreach( Product product in m_products )
		{
			if( product.name.Equals(name) )
			{
				return product;
			}
		}
		
		return null;
	}
}
