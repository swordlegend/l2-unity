using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Dictionary<string, Camera> cameras = new Dictionary<string, Camera>();

    private Camera _activeCamera;

    private static CameraManager _instance;
    public static CameraManager Instance { get { return _instance; } }

    void Awake() {
        if (_instance == null) {
            _instance = this;
        } else if (_instance != this) {
            Destroy(this);
        }
    }

    private void Start() {
        cameras.Add("Login", GameObject.Find("Login").GetComponent<Camera>());
        cameras.Add("CharSelect", GameObject.Find("CharSelect").GetComponent<Camera>());
        cameras.Add("Dark Elf", GameObject.Find("DarkElf").GetComponent<Camera>());
        cameras.Add("Orc", GameObject.Find("Orc").GetComponent<Camera>());
        cameras.Add("Dwarf", GameObject.Find("Dwarf").GetComponent<Camera>());
        cameras.Add("Elf", GameObject.Find("Elf").GetComponent<Camera>());
        cameras.Add("Human", GameObject.Find("Human").GetComponent<Camera>());

        DisableCameras();
        SwitchCamera("Login");
    }

    public void SwitchCamera(string camera) {
        DisableMainCamera();

        if (cameras.TryGetValue(camera, out Camera obj)) {
            Debug.Log(camera + " camera enabled.");
            obj.enabled = true;
            _activeCamera = obj;
        }
    }

    public void DisableCameras() {
        foreach (var cam in cameras.Values) {
            cam.enabled = false;
        }
    }

    public void DisableMainCamera() {
        if (Camera.main != null) {
            Debug.Log("Disabling camera " + Camera.main.transform);
            Camera.main.enabled = false;
        } else if(_activeCamera != null) {
            _activeCamera.enabled = false;
        }
    }
}
