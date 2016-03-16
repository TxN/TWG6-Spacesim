using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DragWeapon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    Canvas canvas;
    public int slot;
    private Vector2 pointerOffset;
    private RectTransform canvasRectTransform;
    private RectTransform panelRectTransform;
    private GameObject weapon;
    public string weaponName;
    public int weaponPrice;
    public string weaponTitle;
    public string weaponDescription;
    public bool selling = false;
    public int ammoNum = 0;
    public int maxNum;

    void Awake()
    {
       
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        canvas = MenuGlobal.instance.shopCanvas;
        if (canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;
            panelRectTransform = transform.parent as RectTransform;
        }

        if ((!selling) && (slot > 0))
        {
            if (slot == 1)
            {
                if (PlayerStats.instance.wS1Name != "")
                {
                    SetWeapon(PlayerStats.instance.wS1Name);
                    ammoNum = PlayerStats.instance.wS1Num;
                }
            }
            else if (slot == 2)
            {
                if (PlayerStats.instance.wS2Name != "")
                {
                    SetWeapon(PlayerStats.instance.wS2Name);
                    ammoNum = PlayerStats.instance.wS2Num;
                }
            }
            else if (slot > 2)
            {
                if (PlayerStats.instance.auxName[slot - 3] != "")
                {
                    SetWeapon(PlayerStats.instance.auxName[slot - 3]);
                    ammoNum = PlayerStats.instance.auxNum[slot - 3];
                }
            }
        }
        else
        {
            SetWeapon(weaponName);
        }
    }

    void Update()
    {
        if (canvas == null)
        {
            canvas = MenuGlobal.instance.shopCanvas;
            canvasRectTransform = canvas.transform as RectTransform;
            panelRectTransform = transform.parent as RectTransform;
            
        }

       // Debug.Log(EventSystem.current.);
    }

    public void SetWeapon(string name)
    {
        if (weapon != null)
        {
            Destroy(weapon);
        }
        if (name != "")
        {
            weapon = Instantiate(Resources.Load<GameObject>(name)) as GameObject;
            weapon.transform.parent = transform;
            IWeapon wep = weapon.GetComponent<IWeapon>();
            weapon.transform.localPosition = new Vector3(0, 0, -30f);
            weapon.transform.localScale = wep.GetGUIScale();
            weapon.transform.rotation = Quaternion.Euler(wep.GetGUIRotation());

            weaponName = wep.GetWeaponName();
            weaponPrice = wep.GetWeaponPrice();
            weaponTitle = wep.GetWeaponTitle();
            maxNum = wep.GetAmmoNum();
            weaponDescription = wep.GetWeaponDesc();
            if (selling)
            {
                ammoNum = wep.GetAmmoNum();
            }
            else
            {
                if (slot == 1)
                {
                    if (PlayerStats.instance.wS1Name != "")
                    {
                       
                        ammoNum = PlayerStats.instance.wS1Num;
                    }
                }
                else if (slot == 2)
                {
                    if (PlayerStats.instance.wS2Name != "")
                    {
                        
                        ammoNum = PlayerStats.instance.wS2Num;
                    }
                } else if (slot >= 3)
                {
                    ammoNum = PlayerStats.instance.auxNum[slot - 3];
                }
                
            }
        }
        else
        {
            return;
        }
        

    }

    public void OnPointerDown(PointerEventData data)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
        if (weapon != null)
        {
            MenuGlobal.instance.SetDescriptionText(weaponTitle + "\n" + weaponDescription + "\n" + "Цена: " + weaponPrice);
        }
        else
        {
            MenuGlobal.instance.SetDescriptionText("");
        }
        
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (weapon) 
        {
            weapon.transform.localPosition = new Vector3(0, 0, weapon.transform.localPosition.z);

            if (selling)
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);

                foreach (RaycastResult h in hits)
                {
                    GameObject g = h.gameObject;
                    DragWeapon wepSlot = g.GetComponent<DragWeapon>();
                    if (wepSlot != null)
                    {
                        if (!wepSlot.selling)
                        {
                            if (wepSlot.weapon != null)
                            {
                                if (MenuGlobal.instance.SellItem(slot, weaponPrice, weaponName))
                                {
                                    Destroy(weapon);
                                    weaponPrice = 0;
                                    weaponName = "";
                                    weaponTitle = "";
                                    weaponDescription = "";
                                }
                            }
                            if (MenuGlobal.instance.BuyItem(wepSlot.slot, weaponPrice, weaponName, ammoNum))
                            {
                                wepSlot.SetWeapon(weaponName);
                            }
                        }
                    }

                }
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);

                foreach (RaycastResult h in hits)
                {
                    GameObject g = h.gameObject;
                    if (g == MenuGlobal.instance.sellPanel)
                    {
                        if (MenuGlobal.instance.SellItem(slot, weaponPrice, weaponName))
                        {
                            Destroy(weapon);
                            weaponPrice = 0;
                            weaponName = "";
                            weaponTitle = "";
                            weaponDescription = "";
                            MenuGlobal.instance.SetDescriptionText("");
                        }
                    }
                    else if (g.GetComponent<DragWeapon>() != null)
                    {
                        //Debug.Log("swap");
                        DragWeapon wep = g.GetComponent<DragWeapon>();
                        if (!wep.selling)
                        {
                            MenuGlobal.instance.SwapItems(this, wep);
                        }
                        
                    }
                       
                }

            }
        }
            
    }
  
    

    public void OnDrag(PointerEventData data)
    {

        if (weapon == null)
            return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapon.transform.position = new Vector3(worldPos.x, worldPos.y, weapon.transform.position.z);

    }

}