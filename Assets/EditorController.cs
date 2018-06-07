using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EditorController : MonoBehaviour {
    GameObject cube;
    List<GameObject> terminals = new List<GameObject>();
    public GameObject controllers;
    Vector3 scale;
    bool gameMode = false;
    bool setup = false;

    private void Start() {
        XRSettings.enabled = false;

        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.layer = 2;
        cube.transform.rotation = transform.rotation;
    }

    void Update() {

        if (!gameMode) {
            Ray ray0 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit0;
            if (Physics.Raycast(ray0, out hit0, 100f)) {
                Vector3 p = hit0.point;
                cube.transform.position = new Vector3(p.x, p.y + cube.transform.localScale.y / 2, p.z);
            }

            if (Input.GetMouseButtonDown(0)) {
                cube.name = "_Cube";
                cube.layer = 0;
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.layer = 2;
                cube.transform.localScale = scale;
                cube.transform.rotation = transform.rotation;
            }

            if (Input.GetMouseButtonDown(1)) {
                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit1;

                if (Physics.Raycast(ray1, out hit1, 100f)) {
                    if (hit1.collider.name == "_Cube") {
                        Destroy(hit1.collider.gameObject);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit1;

                if (Physics.Raycast(ray1, out hit1, 100f)) {
                    if (hit1.collider.name == "_Cube") {
                        Vector3 p = hit1.point;
                        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        plane.name = "_Plane";
                        plane.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        plane.transform.rotation = Quaternion.LookRotation(hit1.normal);
                        plane.transform.Rotate(90, 0, 0);
                        plane.transform.position = p + 0.001f * hit1.normal;
                        plane.GetComponent<Renderer>().material.color = Color.blue;
                        plane.tag = "interactive";
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                int num_inter = GameObject.FindGameObjectsWithTag("interactive").Length;
                if (num_inter >= 5) {
                    print("Going into the play mode");
                    gameMode = true;
                    GameObject.Find("_Mesh").SetActive(false);
                    GameObject.Find("Cube").SetActive(false);

                    List<GameObject> planes = new List<GameObject>(GameObject.FindGameObjectsWithTag("interactive"));
                    for(int i = 0; i < 5; i++) {
                        int index = Mathf.RoundToInt((planes.Count - 1) * Random.value);
                        GameObject p = planes[index];
                        GameObject t = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Terminal"));
                        t.transform.position = p.transform.position;
                        t.transform.rotation = p.transform.rotation;
                        t.transform.localScale = p.transform.localScale;
                        terminals.Add(t);
                        p.SetActive(false);
                        planes.RemoveAt(index);
                    }
                    foreach(GameObject p in planes) {
                        p.SetActive(false);
                    }
                    XRSettings.enabled = true;
                    controllers.SetActive(true);
                    transform.position = new Vector3(transform.position.x, GameObject.Find("Floor").transform.position.y, transform.position.z);

                    

                } else {
                    print("Need " + (5 - num_inter) + " more interactive surfaces");
                }
            }
        } else { //GAME MODE
            if (!setup){
                print("Trying to detect controllers...");

                while(controllers.transform.childCount != 2){
                    return;
                }

                Transform L_C = controllers.transform.GetChild(0);
                Transform R_C = controllers.transform.GetChild(1);

                while (L_C.childCount != 0){
                    L_C = L_C.GetChild(0);
                    R_C = R_C.GetChild(0);
                }
                L_C.gameObject.AddComponent<BoxCollider>();
                R_C.gameObject.AddComponent<BoxCollider>();
                Rigidbody L_R = L_C.gameObject.AddComponent<Rigidbody>();
                Rigidbody R_R = R_C.gameObject.AddComponent<Rigidbody>();
                L_R.useGravity = false;
                R_R.useGravity = false;
                L_R.constraints = RigidbodyConstraints.FreezeAll;
                R_R.constraints = RigidbodyConstraints.FreezeAll;
                setup = true;
                print("Controllers setup complete");
            }
        }
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal")/10;
        float moveVertical = Input.GetAxis("Vertical")/10;
        float moveVerticalY = Input.GetAxis("VerticalY")/10;
        float rotation = 2*Input.GetAxis("Rotate");
        float scaleChange = Input.GetAxis("Mouse ScrollWheel");
        scale = cube.transform.localScale + new Vector3(scaleChange, scaleChange, scaleChange);
        cube.transform.localScale = scale;
        cube.transform.rotation = transform.rotation;

        Vector3 movement = new Vector3(moveHorizontal, moveVerticalY, moveVertical);

        transform.Translate(movement);
        transform.Rotate(0, rotation, 0);
    }

}
