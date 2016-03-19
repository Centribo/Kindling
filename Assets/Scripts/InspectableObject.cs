using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CardboardAudioSource))]

public class InspectableObject : MonoBehaviour {

	public AudioClip pickupSound;
	public AudioClip putDownSound;

	CardboardAudioSource audioSource;

	Vector3 originalPosition;
	Quaternion originalRotation;
	Vector3 originalScale;
	Transform originalParent;

	bool isBeingInspected;


	void Awake(){
		audioSource = GetComponent<CardboardAudioSource>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleInspect(){
		if(!isBeingInspected){
			Inspect();
		} else {
			Deinspect();
		}
	}

	public void Inspect(){
		originalPosition = transform.localPosition;
		originalRotation = transform.localRotation;
		originalScale = transform.localScale;
		originalParent = transform.parent;

		audioSource.clip = pickupSound;
		audioSource.Play();

		PlayerController.Instance.InspectObject(this);
		isBeingInspected = true;
	}

	public void Deinspect(){
		transform.SetParent(originalParent);
		transform.localPosition = originalPosition;
		transform.localRotation = originalRotation;
		transform.localScale = originalScale;

		audioSource.clip = putDownSound;
		audioSource.Play();
		
		PlayerController.Instance.StopInspecting();
		isBeingInspected = false;
	}
}
