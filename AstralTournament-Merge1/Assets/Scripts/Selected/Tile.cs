using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour //Classe indicante la tile della listview nella scena dello store
{
    public Purchasable componentVehicle;
    public RawImage image;
    public Text text;
    private static Purchasable selected;
    public Button button;

    public static Purchasable Selected //restituisce il componente acquistatible selezionato
    {
        get
        {
            return selected;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(onClick);
    }

    public void onClick() //se premuto, il componenente diventa quello selezionato
    {
        selected = componentVehicle;
    }

    // Update is called once per frame
    void Update()
    {
        if(selected!=null) //se un componente è stato selezionato, viene mostrata a schermo l'immagine e la descrizione (per ora sono il nome e il colore)
        {
            Material m = selected.component.GetComponent<Renderer>().sharedMaterial;
            image.texture = m.mainTexture;
            text.text = selected.name;
        }
    }
}
