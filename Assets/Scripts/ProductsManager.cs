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
		LoadProduct("TopProduct1");
	}
	
	private void LoadProduct(string name)
	{
		m_products.Add(Resources.Load<Product>(name));
	}
	
	public List<Product> Products
	{
		get { return m_products; }
	}
}
