using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour {
    public bool enable = true;

    Rigidbody body;
    Quaternion desRotation;

    public Transform checkStartPos;
    public LayerMask mask;
    GameObject selectedTarget;
    public GameObject selectionMarkerFab;
    SelectionMarker selMarker;
    public AudioClip selectTg;
    public AudioClip deselectTg;
    public AudioClip rockCollision;
    public AudioClip otherCollision;
    AudioSource audiosource;

    public float MaxFwdSpd = 100f;
    public float MaxBwdSpd = -20f;
    public float Acceleration = 3f;
    public float Decceleration = 1f;
    float yawPitchFactor = 1f;
    float rollFactor = 1.5f;

    float curSpd = 0f;
    public float trgSpd = 0f;
    PlayerStats stats;

    int activeAuxSlot = 0;

    Vector3 controlVector = Vector3.zero;
    public float mouseSensitivity = 0.1f;

    bool first = true;

    void Awake()
    {

    }

    void Init()
    {
        audiosource = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody>();
        desRotation = transform.rotation;
        SetWeaponGUIInfo();

        stats = PlayerStats.instance;
        MaxFwdSpd = stats.maxSpeed;
        Acceleration = stats.acceleration;
        yawPitchFactor = stats.yawPitchFactor;
        rollFactor = stats.rollFactor;
    }

    float horAxis;
    float vertAxis;
    float rollAxis;

    void Update()
    {
        if (first)
        {
            Init();
            first = false;
        }

        if (selectedTarget != null)
        {
            UpdateTargetInfo();
        }
        else
        {
            ClearTargetUIInfo();
        }

        Debug.DrawRay(checkStartPos.position, transform.TransformDirection(new Vector3(1, 0, 0)), Color.red);


        if (enable)
        {

            if (Input.GetKeyDown("1"))
            {
                activeAuxSlot = 0;
                SetWeaponGUIInfo();
            }

            if (Input.GetKeyDown("2"))
            {
                activeAuxSlot = 1;
                SetWeaponGUIInfo();
            }

            if (Input.GetKeyDown("3"))
            {
                activeAuxSlot = 2;
                SetWeaponGUIInfo();
            }

            if (Input.GetKeyDown("4"))
            {
                activeAuxSlot = 3;
                SetWeaponGUIInfo();
            }

            if (Input.GetMouseButton(0))
            {
               
                if (selectedTarget != null)
                {
                    IWeapon w1 = stats.weaponSlot1.GetComponent<IWeapon>();
                    if (w1 != null)
                    {
                        w1.Shoot(selectedTarget.transform);
                    }
                    IWeapon w2 = stats.weaponSlot2.GetComponent<IWeapon>();
                    if (w2 != null)
                    {
                        w2.Shoot(selectedTarget.transform);
                    }
                    
                }
                else
                {
                    IWeapon w1 = stats.weaponSlot1.GetComponent<IWeapon>();
                    if (w1 != null)
                    {
                        w1.Shoot(null);
                    }
                    IWeapon w2 = stats.weaponSlot2.GetComponent<IWeapon>();
                    if (w2 != null)
                    {
                        w2.Shoot(null);
                    }
                }
               
            }

            if (Input.GetMouseButtonDown(1))
            {
                LockOn();
            }

            if (Input.GetKey("space"))
            {
                SetWeaponGUIInfo();
                if (stats.auxSlot[activeAuxSlot] != null)
                {
                    if (selectedTarget != null)
                    {
                        stats.auxSlot[activeAuxSlot].GetComponent<IWeapon>().Shoot(selectedTarget.transform);
                    }
                    else
                    {
                        stats.auxSlot[activeAuxSlot].GetComponent<IWeapon>().Shoot(null);
                    }
                }
                
                
            }

            if (Input.GetKeyDown("r"))
            {
               
            }
        }
    }

    void UpdateTargetInfo()
    {
        
        ISelectableTarget trg = selectedTarget.GetComponent<ISelectableTarget>();
        GUIManager gui = GUIManager.instance;
        gui.trgDist.text = Mathf.CeilToInt((transform.position - selectedTarget.transform.position).magnitude * 4f).ToString() + "м";
        gui.trgName.text = trg.GetName();
        gui.trgSpd.text = (Mathf.RoundToInt(selectedTarget.GetComponent<Rigidbody>().velocity.magnitude * 4f)).ToString() + "мс";

    }

    void SetWeaponGUIInfo()
    {
        if (PlayerStats.instance.auxSlot[activeAuxSlot] != null)
        {
            GUIManager.instance.SetCurrentWeaponName(PlayerStats.instance.auxSlot[activeAuxSlot].GetComponent<IWeapon>().GetWeaponTitle());
            GUIManager.instance.SetAmmoNum(PlayerStats.instance.auxSlot[activeAuxSlot].GetComponent<IWeapon>().GetAmmoNum());
        }
        else
        {
            GUIManager.instance.SetCurrentWeaponName("Нет оружия");
        }
    }


    void LockOn()
    {
        
        RaycastHit hit;
        Vector3 dir = Camera.main.transform.TransformDirection(Vector3.forward);

        Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height / 2, Screen.width / 2, 0));
        startPos.z = Camera.main.transform.position.z;

        if (Physics.SphereCast(startPos, 0.1f, dir, out hit, 500, mask))
        {
           // Debug.Log(hit.collider.gameObject.name);
            SelectTarget(hit);
        }
        else if (Physics.SphereCast(Camera.main.transform.position, 1f, dir, out hit, 700, mask)) 
        {
            //Debug.Log(hit.collider.gameObject.name + " no2");
            SelectTarget(hit);
        } 
        else
        {
            DeselectTarget();
        }
    }

    void SelectTarget(RaycastHit h)
    {
        ISelectableTarget trg = h.collider.gameObject.GetComponent<ISelectableTarget>();

        if (trg != null)
        {
            GUIManager gui = GUIManager.instance;
            gui.trgDist.text = Mathf.CeilToInt((transform.position - h.transform.position).magnitude * 4f).ToString() + "м";
            gui.trgName.text = trg.GetName();
            gui.trgSpd.text = (h.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 4f).ToString() + "мс";

            if (selectedTarget)
            {
                selectedTarget = null;
                Destroy(selMarker);
            }
            selectedTarget = h.collider.gameObject;
            selMarker = selectedTarget.AddComponent<SelectionMarker>();
            selMarker.markerFab = selectionMarkerFab;
            audiosource.PlayOneShot(selectTg);
        }
        else
        {
            DeselectTarget();
        }
    }

    void ClearTargetUIInfo()
    {
        GUIManager gui = GUIManager.instance;
        gui.trgDist.text = "";
        gui.trgName.text = "Нет Цели";
        gui.trgSpd.text = "";
    }

    void DeselectTarget()
    {
        if (selectedTarget != null)
        {
            ClearTargetUIInfo();
        }

        selectedTarget = null;
        Destroy(selMarker);
        audiosource.PlayOneShot(deselectTg);
        
    }

	void FixedUpdate () 
    {
        if (first)
        {
            Init();
            first = false;
        }

        if (enable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            horAxis = Input.GetAxis("Mouse X");
            vertAxis = Input.GetAxis("Mouse Y");
            rollAxis = Input.GetAxis("Horizontal");
            horAxis = Mathf.Clamp(horAxis, -yawPitchFactor, yawPitchFactor);
            vertAxis = Mathf.Clamp(vertAxis, -yawPitchFactor, yawPitchFactor);

            controlVector.z += horAxis * mouseSensitivity;
            controlVector.y -= vertAxis * mouseSensitivity;
            controlVector.x = -rollAxis * rollFactor;
            
            

            desRotation *= Quaternion.Euler(new Vector3(-rollAxis * rollFactor, -vertAxis, horAxis));
            body.angularVelocity = transform.TransformDirection(controlVector);

            if (Input.GetKey("w"))
            {
                trgSpd += Acceleration * Time.deltaTime;
                trgSpd = Mathf.Clamp(trgSpd, MaxBwdSpd, MaxFwdSpd);
            }
            else if (Input.GetKey("s"))
            {
                if (trgSpd > 0)
                {
                    //trgSpd -= Acceleration * Time.deltaTime;
                    if (trgSpd > 0.3f)
                    {
                        if (trgSpd < 0.1 * MaxFwdSpd)
                        {
                            trgSpd -= Acceleration * Time.deltaTime;
                        }
                        else
                        {
                            trgSpd *= 0.98f;
                        }
                    
                    }
                    else
                    {
                        trgSpd = 0;
                    }

                }
                else
                {
                    trgSpd -= Acceleration * Time.deltaTime;
                }

                trgSpd = Mathf.Clamp(trgSpd, MaxBwdSpd, MaxFwdSpd);
            }
            else if (Input.GetKey("x"))
            {
                if (Mathf.Abs(trgSpd) > 0.3f)
                {
                    if ((trgSpd < 0.1 * MaxFwdSpd)&&(trgSpd > 0))
                    {
                        trgSpd -= Acceleration * Time.deltaTime;
                    }
                    else
                    {
                        trgSpd *= 0.98f;
                    }
                    
                }
                else
                {
                    trgSpd = 0;
                }

            }
        }

        controlVector *= 0.96f;

        body.velocity = transform.TransformDirection(new Vector3(trgSpd, 0, 0));  
        GUIManager.instance.SetSpeed(Mathf.Abs(trgSpd) / MaxFwdSpd);

	}

    void OnCollisionEnter(Collision col)
    {
        
        Vector3 colNormal = col.contacts[0].normal;
        Vector3 vel = body.velocity.normalized;
        float mul = Vector3.Cross(vel, colNormal).magnitude;

        float volume = (col.relativeVelocity.magnitude / MaxFwdSpd) + 0.25f;
        volume = Mathf.Clamp(volume, 0, 1f);

        if (col.collider.attachedRigidbody != null)
        {
            audiosource.PlayOneShot(otherCollision, volume);
            Vector3 impulse = col.collider.attachedRigidbody.mass * col.relativeVelocity;
            Vector3 ourImpulse = body.mass * body.velocity;
            if (ourImpulse.magnitude > 1)
            {
                float part = (impulse.magnitude / ourImpulse.magnitude);
                part = Mathf.Clamp(part, 0, 1);
                trgSpd -= trgSpd * part;
            }
            else
            {
                trgSpd = 0f;
            }
            
           

        }
        else
        {
            audiosource.PlayOneShot(rockCollision, volume);
            int damage = 0;
            int baseDamage = 35;
            //float colAngle = Mathf.Acos(mul) * Mathf.Rad2Deg;
            float angle = Vector3.Cross(col.relativeVelocity.normalized, body.velocity.normalized).magnitude;

            if (col.relativeVelocity.magnitude > 3f)
            {
                damage = Mathf.CeilToInt(baseDamage * angle * Mathf.Abs(trgSpd));
            }

            if (angle < 0.5f)
            {
                trgSpd -= Mathf.Clamp(0.1f + angle, 0, 1) * trgSpd;
            }
            else
            {
                trgSpd = 0f;
            }

          //  PlayerStats.instance.health -= damage;
            GetComponent<IDamageReciever>().DoDamage(damage);

           // Debug.Log(damage);
        }
        //Debug.Log(mul);
    }

}
