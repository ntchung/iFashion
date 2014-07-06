using UnityEngine;
using System.Collections;

public class NavigationBarController : MonoBehaviour {

	public UIToggle ToggleShop;
	public UIToggle ToggleCloset;
	public UIToggle ToggleBag;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( ToggleShop.value )
		{
			UIStateManager.Instance.ChangeToStateShop();
		}
		else if( ToggleCloset.value )
		{
			UIStateManager.Instance.ChangeToStateCloset();
		}
		else		
		{
			UIStateManager.Instance.ChangeToStateBag();
		}
	}
}
