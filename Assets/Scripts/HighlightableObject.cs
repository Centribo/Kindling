using UnityEngine;
using System.Collections;

public class HighlightableObject : MonoBehaviour {

	[Range(0, 10)]
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

	void OnDrawGizmosSelected(){
		Gizmos.color = new Color(1, 1, 1, 0.5f);
		Gizmos.DrawSphere(transform.position, minDistance);
	}
}
