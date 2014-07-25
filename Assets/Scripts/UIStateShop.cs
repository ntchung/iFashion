using UnityEngine;
using System.Collections;

public class UIStateShop : UIState {

	private static UIStateShop g_instance = null;
	
	void Awake()
	{
		g_instance = this;
		
		m_currentCategory = ECategory.Top;
		m_nextCategory = ECategory.Outwear;
	}
	
	public static UIStateShop Instance
	{
		get { return g_instance; }
	}	
	
	private ECategory m_currentCategory;
	private ECategory m_nextCategory;
	
	public GameObject BrowseView;
	public GameObject ItemDetailsPrefab;
	public GameObject ZoomView;
	public GameObject ZoomWindow;
	
	private GameObject m_zoomedProduct;
	private Vector3 m_startingPosition;

	public override void OnEnter()
	{
		UIScrollView scrollView = BrowseView.GetComponent<UIScrollView>();
		m_startingPosition = scrollView.panel.cachedTransform.localPosition;
	}
	
	public override void OnUpdate()
	{
		if( m_nextCategory != m_currentCategory )
		{
			m_currentCategory = m_nextCategory;
			StartCoroutine(SwitchCategory());			
		}
	}
	
	private IEnumerator SwitchCategory()
	{		
		ClearBrowse();
		
		yield return new WaitForEndOfFrame();
		Browse(m_nextCategory);		
	}
	
	public override void OnExit()
	{
	}	
	
	public void ChangeCategory(ECategory category)
	{
		m_nextCategory = category;
	}

	private void Browse(ECategory category)	
	{
		float x = 205;
		float y = 200;
		float height = 310;
		
		UIScrollView scrollView = BrowseView.GetComponent<UIScrollView>();
		foreach( Product product in ProductsManager.Instance.Products )
		{
			if( product.Category == category )
			{
				GameObject details = GameObject.Instantiate(ItemDetailsPrefab) as GameObject;				
				
				details.transform.parent = BrowseView.transform;
				details.transform.localPosition = new Vector3(x, y, 0);
				details.transform.localScale = Vector3.one;								
				
				GameObject prod = GameObject.Instantiate(product.gameObject) as GameObject;
				prod.transform.parent = details.transform;
				prod.transform.localPosition = new Vector3(0, 30, 0);
				prod.transform.localScale = Vector3.one;
				
				ItemDetailsController detailsController = details.GetComponent<ItemDetailsController>();
				detailsController.PriceLabel.text = product.Price;
				detailsController.TitleLabel.text = product.Title;
				detailsController.ProductObject = prod;
				detailsController.ProductReference = product;
				
				UIEventListener.Get(detailsController.ZoomButton).onClick += (obj) =>
				{
					OnZoomClicked(obj.transform.parent.gameObject);
				};
				
				UIEventListener.Get(detailsController.AddCartButton).onClick += (obj) =>
				{
					OnAddCartClicked(obj.transform.parent.gameObject);
				};
				
				y += height;
			}
		}
		
		scrollView.ResetPosition();
	}
	
	private void ClearBrowse()
	{
		foreach( Transform child in BrowseView.transform )
		{
			GameObject.Destroy(child.gameObject);
		}
	}
	
	private void OnZoomClicked(GameObject obj)
	{
		ZoomView.SetActive(true);
		
		ItemDetailsController detailsController = obj.GetComponent<ItemDetailsController>();
		
		GameObject prod = GameObject.Instantiate(detailsController.ProductObject) as GameObject;
		m_zoomedProduct = prod;
		
		prod.transform.parent = ZoomWindow.transform;
		prod.transform.localPosition = new Vector3(0, 0, 0);
		prod.transform.localScale = Vector3.one;
		
		Product prd = prod.GetComponent<Product>();
		prd.Sprite2D.depth = 25;
		prd.Sprite2D.height = 600;
	}
	
	private void OnAddCartClicked(GameObject obj)
	{
		ItemDetailsController detailsController = obj.GetComponent<ItemDetailsController>();
		UIStateBag.Instance.AddProduct(detailsController.ProductReference);
	}
	
	public void ExitZoom()
	{
		GameObject.Destroy(m_zoomedProduct);
		ZoomView.SetActive(false);		
	}
}
