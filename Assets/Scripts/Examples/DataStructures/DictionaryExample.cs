using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Key Pairs, every object of the dictionary has a unique ID
/// fast loopups, damage tables, inventory system
/// </summary>
public class DictionaryExample : MonoBehaviour // key pairs  - lots of guns - name is connected to gun
{
    public Dictionary<string, int> dictionary = new Dictionary<string, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dictionary.Add("Coin", 0);
        dictionary.Add("Gem", 0);
        dictionary.Add("Music Note", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //dictionary.TryAdd();
        //.ContainsKey();
        //.TryGetValue();
    }
}
