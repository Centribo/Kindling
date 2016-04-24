using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectHighlighter : MonoBehaviour {

	public Vector3 highlightPos;
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
		if(highlightPos != null){
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, highlightPos);	
		}	
	}

	public void SetObjectToHighlight(Vector3 p, string name){
		highlightPos = p;
		textBox.text = name;
	}
}
