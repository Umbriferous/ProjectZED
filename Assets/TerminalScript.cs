using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : MonoBehaviour {
    float activation = 0f;

	// Use this for initialization
	void Start () {
        //transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   /* void OnCollisionEnter(Collision col) {
        print("enter " + col.gameObject.name);
        if (col.gameObject.name == "Primitive") {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
    */
    private void OnCollisionStay(Collision col) {
        //print("stay " + col.gameObject.name);
        if (col.gameObject.name == "Primitive"){
            activation += 0.01f;
            if (activation > 1f) activation = 1f;
            gameObject.GetComponent<Renderer>().material.color = new Color(1f - activation, 0f, activation);
        }
    }

    private void OnCollisionExit(Collision col) {
        if (activation < 1f) activation = 0f;
        gameObject.GetComponent<Renderer>().material.color = new Color(1f - activation, 0f, activation);
    }
}
