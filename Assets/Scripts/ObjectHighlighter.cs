using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectHighlighter : MonoBehaviour {

	public Transform transformToHighlight;
	public Text textBox;

	LineRenderer lr;

	void Awake(){
		lr = GetComponent<LineRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transformToHighlight != null){
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, transformToHighlight.position);	
		}	
	}

	public void SetObjectToHighlight(GameObject go, string name){
		transformToHighlight = go.transform;
		textBox.text = name;
	}
}
