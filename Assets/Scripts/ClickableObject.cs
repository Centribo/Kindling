using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

[RequireComponent (typeof (EventTrigger))]

public class ClickableObject : MonoBehaviour {

	public UnityEvent does; //What this clickable object "does"

	bool isGazedAt = false;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		UpdateClickVisuals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void SetGazedAt(bool gaze){
		isGazedAt = gaze;
		UpdateClickVisuals();
	}

	public virtual void ClickObject(){
		does.Invoke();
	}

	public virtual void UpdateClickVisuals(){
		Renderer r = GetComponent<Renderer>();
		if(r != null){
			if(isGazedAt){
				r.material.SetFloat("_isOutlined", 1);	
			} else {
				r.material.SetFloat("_isOutlined", 0);
			}
			
		}
	}
}
