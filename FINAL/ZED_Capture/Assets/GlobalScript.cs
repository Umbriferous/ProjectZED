using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScript : MonoBehaviour {

    private static bool created = false;
    public ZEDSpatialMapping.RESOLUTION resolution_preset = ZEDSpatialMapping.RESOLUTION.MEDIUM;
    public ZEDSpatialMapping.RANGE range_preset = ZEDSpatialMapping.RANGE.MEDIUM;
    public bool isFilteringEnable = true;
    public bool isTextured = false;

    void Awake() {
        if (!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }
    public void SetResolution(int val) {
        switch (val) {
            case 0:
                resolution_preset = ZEDSpatialMapping.RESOLUTION.LOW;
                break;
            case 1:
                resolution_preset = ZEDSpatialMapping.RESOLUTION.MEDIUM;
                break;
            case 2:
                resolution_preset = ZEDSpatialMapping.RESOLUTION.HIGH;
                break;
        }
    }
    public void SetRange(int val) {
        switch (val) {
            case 0:
                range_preset = ZEDSpatialMapping.RANGE.NEAR;
                break;
            case 1:
                range_preset = ZEDSpatialMapping.RANGE.MEDIUM;
                break;
            case 2:
                range_preset = ZEDSpatialMapping.RANGE.FAR;
                break;
        }
    }
    public void SetFiltering(bool enable) {
        isFilteringEnable = enable;
    }
    public void SetTexturing(bool enable) {
        isTextured = enable;
    }




}
