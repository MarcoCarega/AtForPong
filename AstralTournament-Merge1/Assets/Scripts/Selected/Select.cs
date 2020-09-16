using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    private int current;

    public Button left;
    public Button right;
    public RawImage choice;
    public string key;
    private bool first;

    private static Dictionary<string, VehicleComponent> set;

    public List<VehicleComponent> images;

    private VehicleComponent selected;

    public static bool changed;

    private TMP_Text nameSelected;

    public VehicleComponent getSelected()
    {
        return selected;
    }

    // Start is called before the first frame update
    void Start()
    {
        nameSelected = GameObject.Find("Selectors/" + name + "/Panel/Desc").GetComponent<TMP_Text>();
        first = false;
        set = new Dictionary<string, VehicleComponent>();
        changed= false;
        current = 0;
        left.onClick.AddListener(onLeft);
        right.onClick.AddListener(onRight);
        selected = images[current];
    }

    private void onLeft()
    {
        changed = true;
        current = (current - 1);
        if (current == -1) current = images.Count - 1;
        setComponents();
    }

    private void onRight()
    {
        changed = true;
        current = (current + 1) % images.Count;
        setComponents();
    }

    private void setComponents()
    {
        selected = images[current];
        if (!set.ContainsKey(key))
            set.Add(key, selected);
        else set[key] = selected;
    }
        // Update is called once per frame
    void Update()
    {
        //print(images[current]);
        //changed = true;
        nameSelected.text = selected.name;
        //choice.texture = images[current].GetComponent<Renderer>().sharedMaterial.mainTexture;
        
        if (!set.ContainsKey(key)) set.Add(key, selected);
        else set[key] = selected;
      
    }

    public static Dictionary<string, VehicleComponent> getSet()
    {
        return set;
    }

    
}
