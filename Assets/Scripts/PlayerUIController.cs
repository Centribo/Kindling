using UnityEngine;
using System.Collections;

public class PlayerUIController : MonoBehaviour {

	public GameObject objectHighlighter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public ObjectHighlighter HighlightObject(Transform t, string name, Vector3 offset){
		GameObject highlighterGO = (GameObject) Instantiate(objectHighlighter);
		//t.SetParent(transform);

		RectTransform rt = highlighterGO.GetComponent<RectTransform>();
		rt.anchoredPosition3D = new Vector3(0, 0.15f, 0);
		rt.localEulerAngles = Vector3.zero;

		ObjectHighlighter oh = highlighterGO.GetComponent<ObjectHighlighter>();
		oh.SetObjectToHighlight(t.position + offset, name);

		return oh;
	}
}
