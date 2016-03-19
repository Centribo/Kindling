using UnityEngine;
using System.Collections;

public class InspectableObject : MonoBehaviour {

	Vector3 originalPosition;
	Quaternion originalRotation;
	Vector3 originalScale;
	Transform originalParent;

	bool isBeingInspected;

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

		PlayerController.Instance.InspectObject(this);
		isBeingInspected = true;
	}

	public void Deinspect(){
		transform.SetParent(originalParent);
		transform.localPosition = originalPosition;
		transform.localRotation = originalRotation;
		transform.localScale = originalScale;
		
		PlayerController.Instance.StopInspecting();
		isBeingInspected = false;
	}
}
