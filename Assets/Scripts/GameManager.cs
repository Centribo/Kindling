using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static GameManager Instance { //Singleton pattern instance
		get { //Getter
			if(instance == null){ //If its null,
				instance = (GameManager)FindObjectOfType(typeof(GameManager)); //Find it
			}
			return instance; //Return it
		}
	}

	public enum States{ DisplayingLogos, Playing, LoadingScene }
	public States state;
	public Image logoHolder;
	public List<Sprite> logos;

	int logoIndex = -1;
	int fadeState = 1; //0 = Not fading, -1 = fading out, 1 = fading in, 2 = Switch to next logo

	void Awake(){
		DontDestroyOnLoad(transform.gameObject); //Don't destroy us on loading new scenes
	}

	// Use this for initialization
	void Start () {
		state = States.DisplayingLogos;
		SwapToNextLogo();
		fadeState = 1;
	}
	
	// Update is called once per frame
	void Update () {
		switch(state){
			case States.DisplayingLogos:
				HandleLogoFades();
			break;
			case States.LoadingScene:
				if(shouldMoveOnSpawn){
					GameObject spawnPoint = GameObject.Find("SpawnPoint");
					if(spawnPoint != null){
						PlayerController.Instance.transform.localPosition = spawnPoint.transform.localPosition;
						PlayerController.Instance.transform.localRotation = spawnPoint.transform.localRotation;
						shouldMoveOnSpawn = false;
					}
				} else {
					PlayerController.Instance.FadeIn(2);
					Cardboard.SDK.Recenter();
					state = States.Playing;
				}
			break;
		}
	}

	string sceneToLoad;
	bool shouldMoveOnSpawn = true;

	public void SetSceneToLoad(string sceneName){
		sceneToLoad = sceneName;
	}

	public void SetMoveOnSpawn(bool move){
		shouldMoveOnSpawn = move;
	}

	public void LoadScene(float delay){
		Invoke("LoadScene", delay);
	}

	public void LoadScene(){
		state = States.LoadingScene;
		SceneManager.LoadScene(sceneToLoad);
	}

	public void FadeLogoIn(){
		fadeState = 1;
	}

	public void FadeLogoOut(){
		fadeState = -1;
	}

	void SwapToNextLogo(){
		logoIndex ++;
		if(logoIndex < logos.Count){
			logoHolder.sprite = logos[logoIndex];
			FadeLogoIn();
		} else {
			PlayerController.Instance.FadeIn();
		}
	}

	void HandleLogoFades(){
		Color originalColor = logoHolder.color; //Get our current color
		float fadeTolerance = 0.01f; //When to just snap to fully transparent or opaque
		float fadeRate = 2;
		if(fadeState == 1){ //If we're fading in
			originalColor.a = 1; //Set our target color to be opaque
			logoHolder.color = Color.Lerp(logoHolder.color, originalColor, fadeRate * Time.deltaTime); 
			if(logoHolder.color.a >= 1 - fadeTolerance){ //If we're within tolerance
				logoHolder.color = originalColor; //Set it to be fully opaque
				Invoke("FadeLogoOut", 2);
				fadeState = 0; //We're not longer fading
			}
		} else if (fadeState == -1){ //Similar to above
			originalColor.a = 0;
			logoHolder.color = Color.Lerp(logoHolder.color, originalColor, fadeRate * Time.deltaTime);
			if(logoHolder.color.a <= fadeTolerance){
				logoHolder.color = originalColor;
				fadeState = 0;
				Invoke("SwapToNextLogo", 1);
			}
		}
	}



}
