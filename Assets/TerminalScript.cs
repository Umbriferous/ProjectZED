using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : MonoBehaviour {
    float activation = 0f;

    void OnCollisionEnter(Collision col) {

    }
   
    private void OnCollisionStay(Collision col) {
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
