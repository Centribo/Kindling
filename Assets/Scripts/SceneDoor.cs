using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof (CardboardAudioSource))]

public class SceneDoor : MonoBehaviour {

	public AudioClip doorClip; //The sound this door makes
	public string sceneToLoad; //The name of scene this door opens to

	// Use this for initialization
	void Start () {
		GetComponent<CardboardAudioSource>().clip = doorClip;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OpenDoor(){
		GetComponent<CardboardAudioSource>().Play();
		Invoke("LoadScene", doorClip.length);
	}

	public void LoadScene(){
		
		SceneManager.LoadScene(sceneToLoad);
	}
}