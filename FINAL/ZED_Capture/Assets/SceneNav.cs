using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneNav : MonoBehaviour {

    public void beginCapture() {
        SceneManager.LoadScene("CaptureScene");
    }

    public void beginEditor() {
        //SceneManager.LoadScene("S_Editor");
    }
}
