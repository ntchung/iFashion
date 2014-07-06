using UnityEngine;
using System.Collections;

public abstract class UIState : MonoBehaviour {

	public abstract void OnEnter();
	public abstract void OnUpdate();
	public abstract void OnExit();

}
