using UnityEngine;
using System.Collections;

public class HighlightableObject : MonoBehaviour {

	public float minDistance;
	public string highlightText;
	public ObjectHighlighter objectHighlighter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
		if(distance <= minDistance && objectHighlighter == null){
			objectHighlighter = PlayerController.Instance.uiController.HighlightObject(gameObject, highlightText);
		} else if (distance >= minDistance && objectHighlighter != null){
			Destroy(objectHighlighter.gameObject);
		}
	}
}
