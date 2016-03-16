using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {

    public string name;
    public string title;
    public string desc;
    public int weight;
    public int quantity;
    public int price;

    public void SetItem(string n, string t, string d, int w, int q, int p)
    {
        name = n;
        title = t;
        desc = t;
        weight = w;
        quantity = q;
        price = p;
    }

    public Item()
    {
        name = "DebugCube";
        title = "DebugItem";
        desc = "Just a placeholder";
        weight = 1;
        quantity = 1;
        price = 1;
    }

    public Item(Itm it)
    {
        name = it.name;
        title = it.title;
        desc = it.desc;
        weight = it.weight;
        quantity = it.quantity;
        price = it.price;
    }
}
