using UnityEngine;
using UnityEditor;
using System.Collections;

public class InteractableObject : ClickableObject {

	public Vector3 interactLocation; //Where the player moves to before the object is used

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ClickObject(){ //If the player clicks on this object
		PlayerController.Instance.InteractWithObject(this); //Tell the player object to move to use then interact with us
	}

	public virtual void Interact(){
		InteractableObject[] ios = GetComponents<InteractableObject>(); //Get all scripts that are/derive from InteractableObject on this GameObject
		foreach (InteractableObject io in ios){ //For each of them,
			if(io.GetType().IsSubclassOf(typeof(InteractableObject)) && io != this){ //If we're looking at a subclass of this class,
				io.Interact(); //Then call Interact() on that script
			} else {
				//Debug.Log("Your object does not have a class that derives from InteractableObject that overrides Interact()");
				//Do nothing
			}
		}
	}
}

//For drawing UI:
[CustomEditor(typeof(InteractableObject))]
public class testedi : Editor {
	void OnSceneGUI(){
		InteractableObject io = target as InteractableObject;
		if(io == null){
			return;
		} else {
			Handles.DrawDottedLine(io.transform.position, io.transform.position + io.interactLocation, 4.0f);
		}
	}
}

