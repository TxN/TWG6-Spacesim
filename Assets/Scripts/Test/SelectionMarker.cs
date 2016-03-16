using UnityEngine;
using System.Collections;

public class SelectionMarker : MonoBehaviour {
    public GameObject markerFab;
    GameObject marker;
    GameObject marker2;
    RectTransform tform;
    RectTransform tform2;
    Canvas canvas;
    bool isVisible = false;
    Renderer ren;
    void Start()
    {
        canvas = GUIManager.instance.mainCanvas;
        ren = GetComponent<Renderer>();

        marker = Instantiate(markerFab) as GameObject;
        tform = marker.GetComponent<RectTransform>();
        tform.SetParent(canvas.transform);

        marker2 = Instantiate(markerFab) as GameObject;
        tform2 = marker2.GetComponent<RectTransform>();
        tform2.SetParent(canvas.transform);
        tform2.Rotate(new Vector3(0, 0, 180));
        
    }

    void Update()
    {
        if (ren != null)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            if (GeometryUtility.TestPlanesAABB(planes, ren.bounds))
            {
                isVisible = true;
                marker.SetActive(true);
                marker2.SetActive(true);
            }

            else
            {
                isVisible = false;
                marker.SetActive(false);
                marker2.SetActive(false);
            }
        }
        else
        {
            isVisible = false;
            marker.SetActive(false);
            marker2.SetActive(false);
        }



        if ((Camera.main) && (isVisible))
        {
           // Vector3 markPos = Camera.main.WorldToScreenPoint(transform.position);
            // Debug.Log(markPos.x.ToString() + " Y:" + markPos.y.ToString());
           // tform.position = markPos;
          //  Vector3 markPos = Camera.main.WorldToScreenPoint(ren.bounds.center + ren.bounds.extents);
           // Vector3 markPos2 = Camera.main.WorldToScreenPoint(ren.bounds.center - ren.bounds.extents);
            Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(ren.bounds.min.x, ren.bounds.max.y, ren.bounds.center.z));
            Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(ren.bounds.max.x, ren.bounds.min.y, ren.bounds.center.z));
     

            tform.position = origin;
            tform2.position = extent;

        }
    }

    void OnDestroy()
    {
        Destroy(marker);
        Destroy(marker2);
    }

}
