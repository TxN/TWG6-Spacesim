using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ShipInfo : MonoBehaviour, IPointerDownHandler
{
    public RectTransform healthBar;
    float maxWidth;
    PlayerStats stats;


    string shipDescription = "РОК-7 Шмель\nЭтот довольно старый, снятый с вооружения в империи истребитель можно совершенно легально купить в военторге, пусть и без вооружения. Многочисленные произведенные в кустарных условиях доработки позволяют этому ветерану практически на равных бороться с современными истребителями.";

    public void OnPointerDown(PointerEventData data)
    {
        MenuGlobal global = MenuGlobal.instance;
        PlayerStats stats = PlayerStats.instance;
        global.SetDescriptionText(shipDescription + "\n \nПрочность корпуса: " + stats.health +"/" + stats.maxHealth);
    }

    void Awake()
    {
        maxWidth = healthBar.rect.width;
    }

    void Update()
    {
        if (stats == null)
        {
            stats = PlayerStats.instance;
        }

        float percent = (float) stats.health / stats.maxHealth;
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * maxWidth);
    }
}
