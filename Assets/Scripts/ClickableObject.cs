using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (EventTrigger))]

public class ClickableObject : MonoBehaviour {

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
		ClickableObject[] cos = GetComponents<ClickableObject>(); //Get all scripts that are/derive from ClickableObject on this GameObject
		foreach (ClickableObject co in cos){ //For each of them,
			if(co.GetType().IsSubclassOf(typeof(ClickableObject)) && co != this){ //If we're looking at a subclass of this class,
				co.ClickObject(); //Then call ClickObject() on that script
			} else {
				Debug.Log("Your object does not have a class that derives from ClickableObject that overrides ClickObject()");
			}
		}
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
