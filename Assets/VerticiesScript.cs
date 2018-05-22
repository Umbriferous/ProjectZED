using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticiesScript : MonoBehaviour {

    Color[] colors;
    Vector3[] vertices;
    int[] triangles;
    Vector3[] normals;
    HashSet<int>[] vertexConnections;

    public Color hColor = new Color(1, 0, 0);
    public Color vColor = new Color(0, 1, 0);
    public Color defColor = new Color(0, 0, 1);
    public float hSensitivity = 0.75f;
    public float vSensitivity = 0.75f;
    public float surfaceSensitivity = 0.05f;

    List<int> hTriangles = new List<int>(); //holds triangle indicies (#of a triangle) div by 3
    List<int> hVerts = new List<int>(); //holds indicies of horizontal Vertices
    List<int> vVerts = new List<int>(); //holds indicies of vertical Vertices
    List<HashSet<int>> hGroups = new List<HashSet<int>>(); //holds indicies of horizontal Vertices

    private void Start() {
        vertices = GetComponent<MeshFilter>().mesh.vertices;
        triangles = GetComponent<MeshFilter>().mesh.triangles;
        normals = GetComponent<MeshFilter>().mesh.normals;
        colors = new Color[vertices.Length];
        vertexConnections = new HashSet<int>[vertices.Length];
        int num_triangles = triangles.Length / 3;


        print("Breaking the mesh into horizontal and vertical verticies");
        for (int v = 0; v < vertices.Length; v++) {
            vertexConnections[v] = new HashSet<int>();
            if (Vector3.Dot(Vector3.up, normals[v]) > hSensitivity) {
                hVerts.Add(v);
                colors[v] = hColor;
            } else if (Mathf.Abs(Vector3.Dot(Vector3.up, normals[v])) < 1 - vSensitivity) {
                vVerts.Add(v);
                colors[v] = vColor;
            } else {
                colors[v] = defColor;
            }
        }
        print("Done in " + Time.realtimeSinceStartup + "s");
        float t0 = Time.realtimeSinceStartup;



        print("Finding immediate vertex connections");
        for (int t = 0; t < num_triangles; t++) {
            for (int v = 0; v < 3; v++) {
                int v_index = triangles[3 * t + v];
                vertexConnections[v_index].Add(triangles[3 * t + (v + 1) % 3]); // for each vertex of each triangle, add 2 other vertices to its connections set 
                vertexConnections[v_index].Add(triangles[3 * t + (v + 2) % 3]);
            }
        }
        print("Done in " + (Time.realtimeSinceStartup - t0) + "s");
        t0 = Time.realtimeSinceStartup;



        print("Grouping horizontal vertices");
        while(hVerts.Count != 0) {
            int v = hVerts[0];
            hVerts.RemoveAt(0);
            HashSet<int> group = new HashSet<int>();
            group.Add(v);
            Group(v, group);
            if (group.Count > 1) hGroups.Add(group);
        }
        print("Done in " + (Time.realtimeSinceStartup - t0) + "s");
        t0 = Time.realtimeSinceStartup;



        print("Grouping close horizontal groups");
        List<float> heights = new List<float>();
        hGroups.Sort(delegate (HashSet<int> x, HashSet<int> y) {
            return x.Count.CompareTo(y.Count);
        });

        foreach (HashSet<int> g in hGroups) {
            float height = 0;
            foreach (int v in g) {
                height += vertices[v].y;
            }
            heights.Add(height / g.Count);
        }
        print(hGroups.Count + " original horizontal groups");

        for (int g = 0; g < hGroups.Count; g++) {
            for (int g2 = hGroups.Count - 1; g2 > g; g2--) {
                if (Mathf.Abs(heights[g] - heights[g2]) < surfaceSensitivity) {
                    foreach (int elem in hGroups[g]) {
                        hGroups[g2].Add(elem);
                    }
                    hGroups.RemoveAt(g);
                    heights.RemoveAt(g);
                }
            }
        }
        print(hGroups.Count + " final horizontal groups");
        print("Done in " + (Time.realtimeSinceStartup - t0) + "s");
        t0 = Time.realtimeSinceStartup;



        print("Coloring " + hGroups.Count + " different horizontal groups");
        for (int i = 0; i < hGroups.Count; i++) {
            Color c = new Color((float)(i + 1) / hGroups.Count, (float)(i + 1) / hGroups.Count, (float)(i + 1) / hGroups.Count);
            foreach (int v in hGroups[i]) {
                colors[v] = c;
            }
        }
        GetComponent<MeshFilter>().mesh.colors = colors;
        print("Done in " + (Time.realtimeSinceStartup - t0) + "s");
        t0 = Time.realtimeSinceStartup;



        print("Creating the floor plane");
        float max_x = float.MinValue;
        float max_z = float.MinValue;
        float min_x = float.MaxValue;
        float min_z = float.MaxValue;
        float h = 0;
        foreach (int v in hGroups[hGroups.Count - 1]) {
            if (vertices[v].x > max_x) max_x = vertices[v].x;
            if (vertices[v].x < min_x) min_x = vertices[v].x;
            if (vertices[v].z > max_z) max_z = vertices[v].z;
            if (vertices[v].z < min_z) min_z = vertices[v].z;
            h += vertices[v].y;
        }
        float avg_y = h / hGroups[hGroups.Count - 1].Count;
        float x_len = max_x - min_x;
        float z_len = max_z - min_z;

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(min_x + x_len/2, avg_y, min_z + z_len/2);
        plane.transform.localScale = new Vector3(x_len/10, 1, z_len/10);
        print("Done in " + (Time.realtimeSinceStartup - t0) + "s");


    }



    private void Group(int v, HashSet<int> g) {
        foreach (int connection in vertexConnections[v]) {
            if (hVerts.Contains(connection)) {
                g.Add(connection);
                hVerts.Remove(connection);
                Group(connection, g);
            }
        }
    }
}

