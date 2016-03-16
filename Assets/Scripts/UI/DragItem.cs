using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DragItem : MonoBehaviour, IPointerDownHandler
{
    public int index;
    Canvas canvas;
    private GameObject item;
    public string itemName;
    public int itemPrice;
    public string itemTitle;
    public string itemDescription;
    Color initColor;
    Color color;

    void Start()
    {
        initColor = GetComponent<Image>().color;
    }

    public void SetItem(string name)
    {
        if (item != null)
        {
            Destroy(item);
        }
        item = Instantiate(Resources.Load<GameObject>(name)) as GameObject;
        item.transform.parent = transform;
        item.transform.localPosition = new Vector3(-10, -10, -30f);
        item.transform.localScale *= 0.7f;
        Destroy(item.GetComponent<Rigidbody>());
        Destroy(item.GetComponent<MiniMarker>());
        item.transform.rotation *= Quaternion.Euler(0, 0, -135);
        Itm itm = item.GetComponent<Itm>();
        itemName = itm.name;
        itemPrice = itm.price;
        itemTitle = itm.title;
        itemDescription = itm.desc;
    }

    public void UnsetItem()
    {
        Destroy(item);
        itemName = "";
        itemDescription = "";
        itemTitle = "";
        itemPrice = 0;
        GetComponent<Image>().color = initColor ;
    }

    public void DeselectItem()
    {
        GetComponent<Image>().color = initColor;
    }

    public void OnPointerDown(PointerEventData data)
    {
        MenuGlobal.instance.SetActiveInventoryItem(index);

        if (item != null)
        {
            Color col = initColor;
            col.a += 0.2f;
            GetComponent<Image>().color = col;
            MenuGlobal.instance.SetItemDescriptionText(itemTitle + "\n" + itemDescription + "\n" + "Цена: " + itemPrice);
        }
        else
        {
            MenuGlobal.instance.SetItemDescriptionText("");
        }

    }

}
