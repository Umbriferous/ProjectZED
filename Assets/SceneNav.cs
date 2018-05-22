using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNav : MonoBehaviour {

    public void beginCapture() {
        SceneManager.LoadScene("S_Capture");
    }

    public void beginEditor() {
        SceneManager.LoadScene("S_Editor");
    }
}
