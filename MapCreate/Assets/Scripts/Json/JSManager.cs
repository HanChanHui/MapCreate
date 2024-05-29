using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class Maps
{
    public string names = new string("");
    public List<int> idx = new List<int>();

}



public class JSManager : MonoBehaviour
{
    public Text tx;
    Maps data = new Maps();

    private void Start() {
        //data.Add(new Map("stage1", new int[4, 4]{{0, 0, 0, 0},{0, 0, 0, 0},{0, 0, 0, 0},{0, 0, 0, 0}}));
        //data.Add(new Map("stage2", new int[4, 4]{{0, 0, 0, 0},{1, 0, 0, 0},{2, 0, 0, 0},{3, 0, 0, 0}}));
        //data.Add(new Map("stage3", new int[4, 4]{{4, 5, 6, 7},{8, 9, 10, 11},{12, 13, 14, 15},{16, 17, 18, 19}}));

        //string jdata = JsonConvert.SerializeObject(data);
        //print(jdata);
    }

    public void Save()
    {
        //data.names.Add("stage1");
        for(int i = 0; i < 12; i++)
        {

        }

        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath + "/MapStage.json", jdata);
    }

    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/MapStage.json");
        tx.text = jdata;
        //data = JsonConvert.DeserializeObject<List<Map>>(jdata);

        //print(data[0].name);
    }
}
