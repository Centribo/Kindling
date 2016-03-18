using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PasscodeController : MonoBehaviour {

	public UnityEvent onCorrectPasscode; //Called when user gets the passcode right
	public UnityEvent onIncorrectPasscode; //Called when user gets the passcode wrong 

	public int[] PASSCODE = {6, 3, 9, 2}; //The passcode to enter

	public Text passcodeText; //Link to the text to update with the user's inputs
	public AudioClip pressFX; //Sound for pressing a button once
	public AudioClip correctFX; //Sound for getting the passcode correct
	public AudioClip incorrectFX; //Sound for getting the passscode wrong
	public bool isEntered = false; //True if the passcode has been entered correctly

	int currentIndex = 0;
	int[] currentAttempt = {0, 0, 0, 0};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnterDigit(int digit){
		if(!isEntered){
			if(currentIndex == 4){
				return;	
			}
			PlayButtonSound();
			currentAttempt[currentIndex] = digit;
			currentIndex ++;
			UpdatePasscodeText();
		}
	}

	public void UpdatePasscodeText(){
		passcodeText.text = "" + currentAttempt[0] + currentAttempt[1] + currentAttempt[2] + currentAttempt[3];
	}

	public void TryPasscode(){
		if(!isEntered){
			PlayButtonSound();
			currentIndex = 0;
			for(int i = 0; i < 4; i++){
				if(currentAttempt[i] != PASSCODE[i]){
					currentAttempt[0] = 0;
					currentAttempt[1] = 0;
					currentAttempt[2] = 0;
					currentAttempt[3] = 0;
					UpdatePasscodeText();
					PlayInCorrectSound();
					onIncorrectPasscode.Invoke();
					return;
				}
			}

			//If we get here, passcode is right
			passcodeText.color = Color.green;
			PlayCorrectSound();
			onCorrectPasscode.Invoke();
			isEntered = true;
		}
	}

	public void PlayButtonSound(){
		GetComponent<CardboardAudioSource>().clip = pressFX;
		GetComponent<CardboardAudioSource>().Play();
	}

	public void PlayCorrectSound(){
		GetComponent<CardboardAudioSource>().clip = correctFX;
		GetComponent<CardboardAudioSource>().Play();
	}

	public void PlayInCorrectSound(){
		GetComponent<CardboardAudioSource>().clip = incorrectFX;
		GetComponent<CardboardAudioSource>().Play();
	}
}
