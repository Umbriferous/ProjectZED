using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneNav : MonoBehaviour {
    private void Start(){
        XRSettings.enabled = false;
    }

    public void beginCapture() {
        SceneManager.LoadScene("S_Capture");
    }

    public void beginEditor() {
        SceneManager.LoadScene("S_Editor");
    }
}
