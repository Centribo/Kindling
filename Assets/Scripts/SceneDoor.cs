using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof (CardboardAudioSource))]

public class SceneDoor : InteractableObject {

	public Object scene; //The scene this door opens to
	public AudioClip doorClip; //The sound this door makes

	// Use this for initialization
	void Start () {
		GetComponent<CardboardAudioSource>().clip = doorClip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact(){
		GetComponent<CardboardAudioSource>().Play();
		Invoke("LoadScene", doorClip.length);
	}

	public void LoadScene(){
		SceneManager.LoadScene(scene.name);
	}
}
