using UnityEngine;
using System.Collections;

public class UIStateManager : MonoBehaviour {

	private static UIStateManager g_instance = null;
	
	void Awake()
	{
		g_instance = this;		
	}
	
	public static UIStateManager Instance
	{
		get { return g_instance; }
	}

	public UIState Shop;
	public UIState Closet;
	public UIState Bag;
	
	private UIState m_currentState;
	private UIState m_nextState;

	// Use this for initialization
	void Start () {
		Shop.gameObject.SetActive(true);
		Closet.gameObject.SetActive(false);
		Bag.gameObject.SetActive(false);
	
		m_currentState = Shop;
		m_currentState.OnEnter();
		
		m_nextState = m_currentState;		
	}
	
	// Update is called once per frame
	void Update () {
		if( m_currentState != m_nextState )
		{
			m_currentState.OnExit();
			m_currentState.gameObject.SetActive(false);
			
			m_nextState.OnEnter();
			m_nextState.gameObject.SetActive(true);
			m_currentState = m_nextState;
		}
		
		m_currentState.OnUpdate();
	}
	
	public void ChangeToStateShop()
	{
		m_nextState = Shop;
	}
	
	public void ChangeToStateCloset()
	{
		m_nextState = Closet;
	}
	
	public void ChangeToStateBag()
	{
		m_nextState = Bag;
	}
	
	void OnApplicationPause()
	{
		StorePrefs();		
	}	
	
	void OnApplicationQuit()
	{
		StorePrefs();
	}
	
	private void StorePrefs()
	{
		if( UIStateBag.Instance != null )
		{
			UIStateBag.Instance.StorePrefs();
		}
		if( UIStateCloset.Instance != null )
		{
			UIStateCloset.Instance.StorePrefs();
		}
		PlayerPrefs.Save();
	}
}
