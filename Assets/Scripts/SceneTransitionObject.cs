using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class SceneTransitionObject : MonoBehaviour {

	public UnityEvent does;
	public string sceneToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FadeOut(){
		PlayerController.Instance.FadeOut();
	}

	public void FadeOutDelay(float delay){
		Invoke("FadeOut", delay);
	}

	public void InvokeMe(){
		does.Invoke();
	}

	public void LoadSceneDelay(float delay){
		Invoke("LoadScene", delay);
	}

	public void LoadScene(){
		GameManager.Instance.SetSceneToLoad(sceneToLoad);
		GameManager.Instance.LoadScene(0);
	}
}
