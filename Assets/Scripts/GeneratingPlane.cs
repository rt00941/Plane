using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GeneratingPlane : MonoBehaviour {

    private float _xpos;
    private float _ypos;
    private float _zpos;
    private Vector2 _latbounds = new Vector2(24.8f,25.2f);
    private Vector2 _longbounds = new Vector2(67.2f, 68.0f);
    private Vector2 _altbounds = new Vector2(0.1f, 11.3f);
    private string previousData;
    private string ip;
    private GameObject _plane;
    [SerializeField]
    private List<GameObject> _planetypes;
    [SerializeField]
    private List<GameObject> _planelist;
    [SerializeField]
    private GameObject _world;
    private GameObject _planesholder;
    [SerializeField]
    private GameObject _map;
    [SerializeField]
    private TerrainData _terrain;

    void Start ()
    {
        ip = "10.20.3.228";
        _planesholder = new GameObject("PlanesHolder");
        _planesholder.transform.parent = _world.transform;
        _planesholder.transform.localPosition = _map.transform.localPosition;
        _planesholder.transform.localRotation = _map.transform.localRotation;
        _terrain.size = new Vector3(1, 0.2f, 1);
        StartCoroutine(ResetPositions());
    }

    void Update ()
    {
        StartCoroutine(getfromdb());
        StartCoroutine(updateDb());
        RemovePlanes();
    }

    IEnumerator ResetPositions()
    {
        WWW www = new WWW("http://" + ip + "/connectsql/reset.php"); //GET data is sent via the URL
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

        }
        else
        {
            Debug.LogWarning(www.error);
        }

        yield return new WaitForSeconds(0);

    }

    IEnumerator updateDb()
    {
        WWW www = new WWW("http://" + ip + "/connectsql/update.php"); //GET data is sent via the URL
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

        }
        else
        {
            Debug.LogWarning(www.error);
        }

        yield return new WaitForSeconds(0);

    }

    IEnumerator getfromdb()
    {
        string data = "";
        string[] s;
        WWW www = new WWW("http://" + ip + "/connectsql/process.php"); //GET data is sent via the URL
        yield return www;


        if (string.IsNullOrEmpty(www.error))
        {
            data = www.text;
        }
        else
        {
            Debug.LogWarning(www.error);
        }
        if (data != previousData)
        {
            s = data.Split('\n');
            for (int i = 0; i < s.Length/5; i++)
            {
                string planeid = s[(5 * i)];

                if (checkinlist(planeid))
                {
                    UpdatePlane(float.Parse(s[(5 * i + 1)]), float.Parse(s[(5 * i + 2)]), float.Parse(s[(5 * i + 3)]));
                }
                else
                {
                    Generate(planeid, float.Parse(s[(5 * i + 1)]), float.Parse(s[(5 * i + 2)]), float.Parse(s[(5 * i + 3)]), int.Parse(s[(5 * i + 4)]));
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
        previousData = data;
    }

    void Generate(string id, float lat, float lon, float alt, int type)
    {
        _xpos = (lat - _latbounds.x) / (_latbounds.y - _latbounds.x);
        _ypos = (alt - _altbounds.x) / (_altbounds.y - _altbounds.x);
        _zpos = (lon - _longbounds.x) / (_longbounds.y - _longbounds.x);
        float relsize = 200.0f;
        _plane = Instantiate<GameObject>(_planetypes[type], _planesholder.transform);
        _plane.GetComponent<Airplane>().setId(id);
        _plane.transform.localPosition = new Vector3(_xpos, _ypos/10, _zpos);
        _plane.transform.localRotation = new Quaternion(0, 0, 0, 0);
        _plane.transform.localScale = new Vector3(_planetypes[type].transform.localScale.x/relsize, _planetypes[type].transform.localScale.y/relsize, _planetypes[type].transform.localScale.z/relsize);
        _planelist.Add(_plane);
    }

    void UpdatePlane(float lat, float lon, float alt)
    {
        _xpos = (lat - _latbounds.x) / (_latbounds.y - _latbounds.x);
        _ypos = (alt - _altbounds.x) / (_altbounds.y - _altbounds.x);
        _zpos = (lon - _longbounds.x) / (_longbounds.y - _longbounds.x);
        Vector3 newPosition = new Vector3(_xpos, _ypos / 10, _zpos);
        Vector3 angle = (_plane.transform.localPosition - newPosition);//new Vector3(10, 10, 10);
        float angleforward = Mathf.Atan(angle.z / angle.x) * (180/Mathf.PI);
        float magvect = Mathf.Sqrt(Mathf.Pow(angle.z, 2) * Mathf.Pow(angle.z, 2));
        float angleup = Mathf.Atan(angle.y / magvect);
        Debug.Log(angleup);
        angle = new Vector3(angleup,angleforward, 0);
        _plane.transform.localPosition = newPosition;
        _plane.transform.localRotation = Quaternion.Euler(angle);
    }

    void RemovePlanes()
    {
        if (_planelist.Count > 0)
        {
            for (int i = 0; i < _planelist.Count; i++)
            {
                if (!previousData.Contains(_planelist[i].GetComponent<Airplane>().getId()))
                {
                    Destroy(_planelist[i].gameObject);
                    _planelist.Remove(_planelist[i]);
                }
            }
        }
    }

    bool checkinlist(string id)
    {
        if (_planelist.Count>0)
        {
            for (int i = 0; i < _planelist.Count; i++)
            {
                if(_planelist[i].GetComponent<Airplane>().getId() == id)
                {
                    _plane = _planelist[i];
                    return true;
                }
            }
        }
        return false;
    }

    void ClearAll()
    {
        foreach (Transform child in _planesholder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
