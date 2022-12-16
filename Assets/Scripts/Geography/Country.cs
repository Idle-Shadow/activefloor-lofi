using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Country
{
    public string code { get; set; }
    public string name { get; set; }
    public string capital { get; set; }
    public string region { get; set; }
    public string flag { get; set; }
    public int difficulty { get; set; }

    public Sprite GetMap()
    {
        return Resources.Load<Sprite>($"Geography/Maps/{code}");
    }

    public Sprite GetFlag()
    {
        return Resources.Load<Sprite>($"Geography/Flags/{code}");
    }
}
