using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIStateCloset : UIState {

	private static UIStateCloset g_instance = null;

	public GameObject BrowseView;
	public GameObject FigureViewPrefab;

	private List<WearingFigure> m_manequins = new List<WearingFigure>();
	private bool m_initialized = false;
	private bool m_needRefreshList = false;
	private bool m_freshlyEntered = true;

	public static UIStateCloset Instance
	{
		get { return g_instance; }
	}

	void Awake()
	{
		g_instance = this;
	}

	public override void OnEnter()
	{
		if( !m_initialized )
		{
			LoadPrefs();
			m_initialized = true;
		}
		
		m_needRefreshList = true;
		m_freshlyEntered = true;
	}
	
	public override void OnUpdate()
	{
		if( m_needRefreshList )
		{
			ClearBrowse();
			Browse(m_freshlyEntered);
			m_freshlyEntered = false;
		
			m_needRefreshList = false;
		}
	}
	
	public override void OnExit()
	{
	}	
	
	public void AddManequin(WearingFigure figure)
	{
		m_manequins.Add(figure);
	}
	
	private void ClearBrowse()
	{
		foreach( Transform child in BrowseView.transform )
		{
			GameObject.Destroy(child.gameObject);
		}
	}
	
	private void Browse(bool resetPosition)
	{
		float x = 0;
		float y = 0;
		float width = 310;
		
		UIScrollView scrollView = BrowseView.GetComponent<UIScrollView>();
		if( resetPosition )
		{
			scrollView.ResetPosition();
		}
		
		foreach( WearingFigure figure in m_manequins )
		{
			GameObject figView = GameObject.Instantiate(FigureViewPrefab) as GameObject;				
			
			figView.transform.parent = BrowseView.transform;
			figView.transform.localPosition = new Vector3(x, y, 0);
			figView.transform.localScale = Vector3.one;								
			
			ClosetItemController controller = figView.GetComponent<ClosetItemController>();
			figure.Fit(controller.Overlay, 800);
			controller.Reference = figure;
			
			UIEventListener.Get(controller.ButtonRemove).onClick += (obj) =>
			{
				OnRemoveClicked(obj.transform.parent.gameObject);
			};
			
			x += width;
		}
		
		if( resetPosition )
		{
			scrollView.ResetPosition();
		}
	}
	
	private void OnRemoveClicked(GameObject obj)
	{
		ClosetItemController controller = obj.GetComponent<ClosetItemController>();
		for( int i=m_manequins.Count-1; i>=0; --i )
		{
			if( m_manequins[i] == controller.Reference )
			{	
				m_manequins.RemoveAt(i);
				m_needRefreshList = true;
				break;	
			}
		}
	}
	
	public void LoadPrefs()
	{
		string lines = PlayerPrefs.GetString("Closet", "");
		string[] parts = lines.Split('\n');
		foreach( string part in parts  )
		{
			WearingFigure figure = new WearingFigure();
			figure.Import(part);
			m_manequins.Add(figure);
		}
	}
	
	public void StorePrefs()
	{
		if( !m_initialized )
		{
			return;
		}
		
		string line = "";
		foreach( WearingFigure figure in m_manequins )
		{
			if( line.Length > 0 )
			{
				line += "\n";
			}
			line += figure.Export();
		}
		PlayerPrefs.SetString("Closet", line);
	}
}
