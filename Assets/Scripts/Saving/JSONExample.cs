using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class JSONExample : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SampleData sample = new SampleData();
        sample.name = "Steve";
        sample.score = 10.0f;

        string data = JsonUtility.ToJson(sample); // creates JSON version of our data
        Debug.Log(data);

        // deserialize JSON to data
        string JSON = "{\n\t\"name\": \"Alice\",\n\t\"score\": 90.34\n}";
        SampleData sample2 = JsonUtility.FromJson<SampleData>(JSON);
        Debug.Log($"Deserialized {sample2.name} - Score : {sample2.score}");

        string path = Application.persistentDataPath + "/savefile.json";

        File.WriteAllText(path, JSON);
    }
}

// JSON is lightweight, easy way to store data - use when you have lots of data
// leaderboard, multiplayer, etc
// JSON is universal and good for sending data to other languages, including Blender etc


