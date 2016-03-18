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

	public ObjectHighlighter HighlightObject(GameObject obj, string name){
		GameObject highlighterGO = (GameObject) Instantiate(objectHighlighter);
		highlighterGO.transform.parent = transform;

		RectTransform rt = highlighterGO.GetComponent<RectTransform>();
		rt.anchoredPosition3D = new Vector3(0, 0.15f, 0);
		rt.localEulerAngles = Vector3.zero;

		ObjectHighlighter oh = highlighterGO.GetComponent<ObjectHighlighter>();
		oh.SetObjectToHighlight(obj, name);

		return oh;
	}
}
