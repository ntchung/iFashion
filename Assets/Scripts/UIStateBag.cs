using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIStateBag : UIState {

	private static UIStateBag g_instance = null;

	public GameObject BrowseView;
	public GameObject BagItemPrefab;
	public GameObject FittingView;
	
	private List<Product> m_shoppingBag = new List<Product>();
	private List<BagItemController> m_controllers = new List<BagItemController> ();
	private WearingFigure m_manequin = new WearingFigure();
	private bool m_needRefreshList = false;	
	private bool m_needRefreshFitting = false;	
	private bool m_initialized = false;
	private bool m_isFreshlyEntered = true;

	public static UIStateBag Instance
	{
		get { return g_instance; }
	}

	void Awake()
	{
		g_instance = this;		
		m_controllers.Clear ();
	}

	public override void OnEnter()
	{
		if( !m_initialized )
		{
			LoadPrefs();
			m_initialized = true;
		}
	
		m_needRefreshList = true;
		m_needRefreshFitting = true;
		m_isFreshlyEntered = true;
	}
	
	public override void OnUpdate()
	{
		if( m_needRefreshList )
		{
			m_needRefreshList = false;
			
			StartCoroutine(RefreshList());
		}
		
		if( m_needRefreshFitting )
		{
			m_manequin.Clear(FittingView);
			m_manequin.Fit(FittingView, 700);			
			m_needRefreshFitting = false;
		}
	}
	
	private IEnumerator RefreshList()
	{
		ClearBrowse();
		
		yield return new WaitForEndOfFrame();
		Browse(m_isFreshlyEntered);
		m_isFreshlyEntered = false;
	}
	
	public override void OnExit()
	{
		ClearBrowse();
	}
	
	public void AddProduct(Product product)	
	{
		foreach( Product prd in m_shoppingBag )
		{
			if( prd.name.Equals(product.name) )
			{
				return;
			}
		}
		
		m_shoppingBag.Add(product);		
	}
	
	public void RemoveProduct(Product product)
	{
		m_shoppingBag.Remove(product);
	}
	
	private void OnRemoveClicked(GameObject obj)
	{
		BagItemController detailsController = obj.GetComponent<BagItemController>();
		RemoveProduct(detailsController.ProductReference);
	
		m_needRefreshFitting = m_manequin.Remove(detailsController.ProductReference);	
		m_needRefreshList = true;
	}
	
	private void OnTryOnClicked(GameObject obj)
	{
		BagItemController detailsController = obj.GetComponent<BagItemController>();
		m_manequin.Add(detailsController.ProductReference);	
		m_needRefreshFitting = true;		
		
		RefreshWearingButtons();
	}
	
	private void OnUnTryClicked(GameObject obj)
	{
		BagItemController detailsController = obj.GetComponent<BagItemController>();
		m_manequin.Remove(detailsController.ProductReference);	
		m_needRefreshFitting = true;		
		
		RefreshWearingButtons();
	}
	
	private void Browse(bool resetPosition)
	{
		float x = 0;
		float y = 0;
		float height = 210;
				
		UIScrollView scrollView = BrowseView.GetComponent<UIScrollView>();

		m_controllers.Clear ();
		
		foreach( Product product in m_shoppingBag )
		{
			GameObject details = GameObject.Instantiate(BagItemPrefab) as GameObject;				
			
			details.transform.parent = BrowseView.transform;
			details.transform.localPosition = new Vector3(x, y, 0);
			details.transform.localScale = Vector3.one;								
			
			GameObject prod = GameObject.Instantiate(product.gameObject) as GameObject;
			prod.transform.parent = details.transform;
			prod.transform.localPosition = Vector3.zero;
			prod.transform.localScale = Vector3.one;
			
			UI2DSprite spr = prod.GetComponent<UI2DSprite>();
			spr.depth = 8;
			spr.height = 100;
			
			BagItemController detailsController = details.GetComponent<BagItemController>();
			detailsController.PriceLabel.text = product.Price;
			detailsController.TitleLabel.text = product.Title;
			detailsController.ProductReference = product;

			m_controllers.Add(detailsController);
			
			UIEventListener.Get(detailsController.RemoveButton).onClick += (obj) =>
			{
				OnRemoveClicked(obj.transform.parent.gameObject);
			};
			
			UIEventListener.Get(detailsController.UntryButton).onClick += (obj) =>
			{
				OnUnTryClicked(obj.transform.parent.gameObject);
			};
			
			UIEventListener.Get(detailsController.TryOnButton).onClick += (obj) =>
			{
				OnTryOnClicked(obj.transform.parent.gameObject);
			};
			
			y += height;
		}

		RefreshWearingButtons ();
		
		if( resetPosition )
		{
			scrollView.ResetPosition();
		}
	}

	private void RefreshWearingButtons()
	{
		foreach (BagItemController detailsController in m_controllers) 
		{
			if (m_manequin.IsWearing (detailsController.ProductReference)) {
				detailsController.TryOnButton.SetActive (false);
				detailsController.UntryButton.SetActive (true);
			} else {
				detailsController.TryOnButton.SetActive (true);
				detailsController.UntryButton.SetActive (false);
			}
		}
	}

	private void ClearBrowse()
	{		
		foreach( Transform child in BrowseView.transform )
		{
			GameObject.Destroy(child.gameObject);
		}
	}
	
	public void AddToCloset()
	{
		UIStateCloset.Instance.AddManequin(m_manequin.Clone());
	}
	
	public void LoadPrefs()
	{
		string list = PlayerPrefs.GetString("ShoppingBag", "");
		string[] listItems = list.Split('\n');
		
		foreach( string name in listItems )
		{
			Product product = ProductsManager.Instance.Find(name);
			if( product != null )
			{
				m_shoppingBag.Add(product);
			}
		}
		
		m_manequin.Import(PlayerPrefs.GetString("ShoppingBagManequin", ""));
	}
	
	public void StorePrefs()
	{
		if( !m_initialized )
		{
			return;
		}
	
		string list = "";
		foreach(Product product in m_shoppingBag )
		{		
			if( list.Length > 0 )
			{
				list += "\n";
			}
			list += product.name;
		}
		PlayerPrefs.SetString("ShoppingBag", list);
		PlayerPrefs.SetString("ShoppingBagManequin", m_manequin.Export());
	}
}
