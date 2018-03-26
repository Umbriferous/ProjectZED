using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testVerticies : MonoBehaviour {

    Color[] colors;
    Mesh mesh;
    Vector3[] vertices;

    private void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
    }

    void Update() {
        int i = 0;
        while (i < vertices.Length) {
            colors[i] = new Color(1, 1, 0);
            i++;
        }
        mesh.colors = colors;
    }
}
