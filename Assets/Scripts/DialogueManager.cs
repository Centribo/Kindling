using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class DialogueLine {
	public AudioClip clip;
	public DialogueManager.Voices speaker;
}

public class DialogueManager : MonoBehaviour {

	public enum Voices {A, B};

	public CardboardAudioSource voiceA;
	public CardboardAudioSource voiceB;
	public List<Voices> Order;
	public List<AudioClip> Clips;
	public UnityEvent does;

	int clipNum = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartDialogue(){
		clipNum = 0;
		PlayClip();
	}

	void PlayClip(){
		if(Order[clipNum] == Voices.A){
			voiceA.clip = Clips[clipNum];
			voiceA.Play();	
			Invoke("AdvanceClip", Clips[clipNum].length);
		} else if(Order[clipNum] == Voices.B){
			voiceB.clip = Clips[clipNum];
			voiceB.Play();
			Invoke("AdvanceClip", Clips[clipNum].length);
		}
	}

	void AdvanceClip(){ 
		clipNum ++;
		if(clipNum >= Clips.Count){
			does.Invoke();
			return; 
		}
		PlayClip();

	}
}
