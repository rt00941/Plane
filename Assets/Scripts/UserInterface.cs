using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    [SerializeField]
    private Button _rotate, _zoomin, _zoomout;
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private TerrainData _terrain;

    // Use this for initialization
    void Start () {
        _rotate.onClick.AddListener(RotateWorld);
        _zoomin.onClick.AddListener(ZoominWorld);
        _zoomout.onClick.AddListener(ZoomoutWorld);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void RotateWorld()
    {
        transform.LookAt(_target.transform.position);
        transform.RotateAround(_target.transform.localPosition, Vector3.up, 90.0f * Time.deltaTime);
    }

    void ZoominWorld()
    {
        //transform.LookAt(_target.transform.position);
        //transform.RotateAround(_target.transform.localPosition, Vector3.up, 90.0f * Time.deltaTime);
        Debug.Log("Zoom");
        _target.transform.localScale = Vector3.Scale(_target.transform.localScale, new Vector3(1.1f, 1.1f, 1.1f));
        _terrain.size = Vector3.Scale(_terrain.size, new Vector3(1.1f, 1.1f, 1.1f));
        transform.LookAt(_target.transform.position);
    }

    void ZoomoutWorld()
    {
        //transform.LookAt(_target.transform.position);
        //transform.RotateAround(_target.transform.localPosition, Vector3.up, 90.0f * Time.deltaTime);
        Debug.Log("Zoom");
        _target.transform.localScale = Vector3.Scale(_target.transform.localScale, new Vector3(0.9f, 0.9f, 0.9f));
        _terrain.size = Vector3.Scale(_terrain.size, new Vector3(0.9f, 0.9f, 0.9f));
        transform.LookAt(_target.transform.position);
    }
}
