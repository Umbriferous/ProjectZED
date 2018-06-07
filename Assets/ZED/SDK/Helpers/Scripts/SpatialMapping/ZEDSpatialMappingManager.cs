//======= Copyright (c) Stereolabs Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Contols the ZEDSpatialMapping and hides its implementation
/// </summary>
[DisallowMultipleComponent]
public class ZEDSpatialMappingManager : MonoBehaviour {

    /// <summary>
    /// Current resolution, a higher resolution create more meshes
    /// </summary>
	public ZEDSpatialMapping.RESOLUTION resolution_preset = ZEDSpatialMapping.RESOLUTION.MEDIUM;

    /// <summary>
    /// The range of the spatial mapping, how far the depth is taken into account
    /// </summary>
	public ZEDSpatialMapping.RANGE range_preset = ZEDSpatialMapping.RANGE.MEDIUM;

    /// <summary>
    /// Flag if filtering is needed
    /// </summary>
    public bool isFilteringEnable = true;

    /// <summary>
    /// Falg is the textures will be created and applied
    /// </summary>
    public bool isTextured = false;

    /// <summary>
    /// Flag to save when spatial mapping is over
    /// </summary>
    public bool saveWhenOver = true;

    /// <summary>
    /// Path to save the .obj and the .area
    /// </summary>
    public string meshPath = "_TestMesh.obj";

    /// <summary>
    /// The parameters of filtering
    /// </summary>
    public sl.FILTER filterParameters;
    //filterParameters.enumValueIndex = (int) (sl.FILTER) EditorGUILayout.EnumPopup("Mesh Filtering", (sl.FILTER) filterParameters.enumValueIndex);
    /// <summary>
    /// The core of spatial mapping
    /// </summary>
    private ZEDSpatialMapping spatialMapping;
    private ZEDManager manager;

    private void Start() {
        GlobalScript globals = GameObject.Find("Globals").GetComponent<GlobalScript>();
        resolution_preset = globals.resolution_preset;
        range_preset = globals.range_preset;
        isFilteringEnable = globals.isFilteringEnable;
        isTextured = globals.isTextured;
        manager = GameObject.FindObjectOfType(typeof(ZEDManager)) as ZEDManager;
        spatialMapping = new ZEDSpatialMapping(transform, sl.ZEDCamera.GetInstance(), manager);
    }

    /// <summary>
    /// Is the spatial mapping running
    /// </summary>
    public bool IsRunning { get { return spatialMapping != null ? spatialMapping.IsRunning() : false; } }

    /// <summary>
    /// List of the submeshes processed, it is filled only when "StopSpatialMapping" is called
    /// </summary>
    public List<ZEDSpatialMapping.Chunk> ChunkList { get { return spatialMapping != null ? spatialMapping.ChunkList : null; } }

    /// <summary>
    /// Is the update thread running, the thread is stopped before the post process
    /// </summary>
    public bool IsUpdateThreadRunning { get { return spatialMapping != null ? spatialMapping.IsUpdateThreadRunning : false; } }

    /// <summary>
    /// Is the spatial mapping process is stopped
    /// </summary>
    public bool IsPaused { get { return spatialMapping != null ? spatialMapping.IsPaused : false; } }

    /// <summary>
    /// Is the texturing is running
    /// </summary>
    public bool IsTexturingRunning { get { return spatialMapping != null ? spatialMapping.IsTexturingRunning : false; } }


    private void OnEnable() {
        ZEDSpatialMapping.OnMeshReady += SpatialMappingHasStopped;
    }

    private void OnDisable() {
        ZEDSpatialMapping.OnMeshReady -= SpatialMappingHasStopped;
    }

    void SpatialMappingHasStopped() {
        print("test00");
        if (saveWhenOver) {
            print("Saving Mesh");
            if (SaveMesh(meshPath)){
                print("Saving complete");
                SceneManager.LoadScene("S_Editor");
            }
            else print("Error while saving mesh");
        }

    }

    public void StartSpatialMapping() {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        spatialMapping.StartStatialMapping(resolution_preset, range_preset, isTextured);
    }

    /// <summary>
    /// Stop the current spatial mapping, the current mapping will be processed to add a mesh collider, and events will be called
    /// </summary>
    public void StopSpatialMapping() {
        spatialMapping.StopStatialMapping();
        SpatialMappingHasStopped();
    }

    private void Update() {
        if (spatialMapping != null) {
            spatialMapping.filterParameters = filterParameters;
            spatialMapping.Update();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!IsRunning) {
                StartSpatialMapping();
                print("Starting Mapping");
            } else {
                StopSpatialMapping();
                print("Finishing Mapping");
            }
        }
    }

    private void OnApplicationQuit() {
        spatialMapping.Dispose();
    }

    /// <summary>
    /// Display the mesh
    /// </summary>
    /// <param name="state"></param>
    public void SwitchDisplayMeshState(bool state) {
        spatialMapping.SwitchDisplayMeshState(state);
    }

    /// <summary>
    /// Pause the computation of the mesh
    /// </summary>
    /// <param name="state"></param>
    public void SwitchPauseState(bool state) {
        spatialMapping.SwitchPauseState(state);
    }

    /// <summary>
    /// Save the mesh in a file. The saving will disable the spatial mapping and register and area memory.
    /// May take some time
    /// </summary>
    /// <param name="meshPath"></param>
    /// <returns></returns>
    public bool SaveMesh(string meshPath = "ZEDMeshObj.obj") {
        return spatialMapping.SaveMesh(meshPath);
    }

    /// <summary>
    /// Load the mesh from a file
    /// </summary>
    /// <param name="meshPath"></param>
    /// <returns></returns>
    public bool LoadMesh(string meshPath = "ZEDMeshObj.obj") {
        manager.gravityRotation = Quaternion.identity;

        spatialMapping.SetMeshRenderer();
        return spatialMapping.LoadMesh(meshPath);
    }

}

#if UNITY_EDITOR

#endif