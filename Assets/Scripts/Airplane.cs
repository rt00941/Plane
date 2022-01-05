using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

    private string _id;
    [SerializeField]
    private int _type;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setId(string id)
    {
        _id = id;
    }

    public string getId()
    {
        return _id;
    }
}
