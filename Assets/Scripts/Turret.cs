using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour 
{
    public GameObject head;
    public GameObject shootDummy;

    Quaternion initRotation;
    Quaternion targetRotation;
    public float rotCoef;

    IWeapon wep;

    GameObject target;

    public LayerMask detectMask;

    void Awake()
    {
        initRotation = transform.rotation;
        wep = GetComponent<IWeapon>();
        target = PlayerStats.instance.player.gameObject;
    }

    void Update()
    {
        if (target == null)
        {
            target = PlayerStats.instance.player.gameObject;
        }

        targetRotation = initRotation;

        if (target != null)
        {
            float dist = (target.transform.position - transform.position).magnitude;
            if (dist < 150)
            {
                if (CheckVisibility(target))
                {
                    Vector3 dir = transform.position - target.transform.position;
                    targetRotation = Quaternion.LookRotation(dir);
                }
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotCoef * Time.deltaTime);

    }

    bool CheckVisibility(GameObject obj)
    {
        Vector3 startPos = transform.position;
        Vector3 dir = obj.transform.position - startPos;
        dir.Normalize();
        Ray ray = new Ray(startPos, dir);
        Debug.DrawRay(startPos, dir, Color.blue);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 150, detectMask))
        {
            if (hit.collider.gameObject == obj)
            {
                return true;
            }
        }

        return false;

    }
}
