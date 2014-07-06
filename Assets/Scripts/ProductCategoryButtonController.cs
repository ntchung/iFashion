using UnityEngine;
using System.Collections;

public class ProductCategoryButtonController : MonoBehaviour {

	private UIToggle m_masterToggle;
	
	public ECategory Category;

	void Awake()
	{	
		m_masterToggle = this.GetComponent<UIToggle>();
	}

	void Update()
	{
		if( m_masterToggle.value )
		{	
			UIStateShop.Instance.ChangeCategory(Category);
		}
	}
}
