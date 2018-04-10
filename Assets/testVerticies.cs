using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testVerticies : MonoBehaviour {

    Color[] colors;
    Mesh mesh;
    Vector3[] vertices;
    Vector3[] acceptableV;
    Vector3[] acceptedV;
    List<int> toCheck;

    public Color color = new Color(1, 0, 0);
    public Color color1 = new Color(0, 0, 1);
    public float threshold = 0.5f;

    private void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            if((Vector3.up - mesh.normals[i]).magnitude < threshold) {
                colors[i] = color;
            } else {
                colors[i] = color1;
            }
        }
        mesh.colors = colors;
        /*for (int i = 0; i < mesh.triangles.Length/3; i += 1) {
            mesh.vertices[mesh.triangles[i]]
        }*/
    }

    void Update() {
        /*if (Input.GetKeyDown(KeyCode.V)) {
            
            for(int i = 0; i < vertices.Length; i++) {
                colors[i] = color;
            }
            mesh.colors = colors;
        }*/
    }
}
