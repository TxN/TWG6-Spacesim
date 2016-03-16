/*!
 * \mainpage Easy Bezier
 * 
 * \section Introduction
 * Easy Bezier is an extension for Unity3D that makes it easy to create and use bezier spline paths.
 * \section Usage
 * 
 * \subsection step1 Create a path
 * This extension adds a new menuitem under "GameObject-Create Other" called "BezierPath". This item is also reachable through the popup menu above the hierarchy tab. After clicking on this menuitem, a new bezierpath will be added to you scene.
 * After selecting the new BezierPath object, you can assign colors to it in the inspector. These colors are only for the visualization in the scene view. In the scene view you will see a small cube signifying the first point. After you select it, you can drag its handle and the point itself around by using the position handles. It will also show 2 buttons: "+" and "-". "+" adds a new point after the current selected point and "-" removes the current selected point.
 * 
 * By dragging around the points and handles, you can create the curve you want. For more precise control over the points, you can also edit them in the inspector.
 * 
 * \subsection step2 Create a script
 * To usage of the path is quite simple. You can query the path about a position on the path in two ways. The first way is to use GetPositionByT. This provides an interpolated position based on t where 0>=t<=points. This means that it will return the first point when t=0 and the last point when t = last point number. At t=1 it will return the second point and at values in between, the interpolated position.
 * The second way to use the path is with the function GetPositionByDistance. This returns the interpolated position based on the distance from the first point. The distance calculations are approximate but should be precise enough for real world usage.
 * \code
 * transfrom.position = path.GetPositionByT(0.5f); // get the position halfway between the first and second point
 * \endcode
 * \code
 * transfrom.position = path.GetPositionByDistance(10); // get the position 10 units along the line
 * \endcode
 * \author Sander Homan
 * \copyright Sander Homan 2012
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This the main class of the bezier splines. It holds all the points and does the interpolating.
/// </summary>
public class BezierPath : MonoBehaviour
{
    /// <summary>
    /// Editor only. The color of the points.
    /// </summary>
    public Color color = Color.yellow;
    /// <summary>
    /// Editor only. The color of the lines.
    /// </summary>
    public Color lineColor = Color.red;

    /// <summary>
    /// Stores the information about the point
    /// </summary>
    [System.Serializable]
    public class PathPoint
    {
        /// <summary>
        /// The point
        /// </summary>
        public Vector3 p1;
        /// <summary>
        /// The tangent
        /// </summary>
        public Vector3 h1;

        /// <summary>
        /// The bezier curve to the next point
        /// </summary>
        [HideInInspector]
        public Bezier bezier = new Bezier(Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero);

        /// <summary>
        /// The total distance of this bezier
        /// </summary>
        [HideInInspector]
        public float distance = 0;

        /// <summary>
        /// The distances in this bezier curve at 0.1 intervals
        /// </summary>
        [HideInInspector]
        public float[] internalDistance = new float[10];
    }

    /// <summary>
    /// The points
    /// </summary>
    public List<PathPoint> points = new List<PathPoint>();

    void Start()
    {
        // fill in the bezier curves; // calculate distance
        for (int i = 0; i < points.Count - 1; i++)
        {
            // draw line from point i to point i+1
            points[i].bezier.p1 = points[i].p1;
            points[i].bezier.h1 = points[i].h1;
            points[i].bezier.p2 = points[i + 1].p1;
            points[i].bezier.h2 = -points[i + 1].h1; // negative of this handle

            if (points[i].internalDistance == null) points[i].internalDistance = new float[10];
            // aproximate distance
            for (int t = 0; t < 10; t++)
            {
                points[i].internalDistance[t] = (points[i].bezier.GetPointAtTime((t / 10.0f) + 0.1f) - points[i].bezier.GetPointAtTime((t / 10.0f))).magnitude;
                points[i].distance += points[i].internalDistance[t];
            }
        }
    }

    /// <summary>
    /// Get a position along the path. At t=0 it will return the position of the first point. At t=i it returns the position of point i. At t=point count it will return the last point. At values in between, it returns the interpolated value.
    /// </summary>
    /// <param name="t">The t on the bezier spline</param>
    /// <returns>The interpolated postition</returns>
    public Vector3 GetPositionByT(float t)
    {
        if (t >= points.Count-1)
        {
            // clamp to end
            return points[points.Count-2].bezier.GetPointAtTime(1);
        }
        else if (t <= 0)
        {
            // clamp to begin
            return points[0].bezier.GetPointAtTime(0);
        }
        else
        {
            // position it
            int point1 = (int)t;
            return points[point1].bezier.GetPointAtTime(t - point1);
        }
    }

    /// <summary>
    /// Get a position along the path by distance. Remark: this function is more expensive to use then GetPositionByT.
    /// </summary>
    /// <param name="dist">The distance along the path</param>
    /// <returns>The interpolated distance</returns>
    public Vector3 GetPositionByDistance(float dist)
    {
        if (dist <= 0)
        {
            // clamp to begin
            return points[0].bezier.GetPointAtTime(0);
        }
        else
        {
            //Debug.Log(dist);
            // walk through point list till we hit the correct segment
            int p = 0;
            while (dist > 0 && p<points.Count-1)
            {
                dist -= points[p].distance;
                p++;
            }
            //Debug.Log(dist+":"+p);
            if (dist > 0)
            {
                // clamp to end
                return points[points.Count - 2].bezier.GetPointAtTime(1);
            }
            else
            {
                p--;
                // do the same for the internal distances
                dist += points[p].distance;
                int tBase = 0;
                while (dist > 0)
                {
                    dist -= points[p].internalDistance[tBase];
                    tBase++;
                }
                tBase--;
                //Debug.Log("tBase: " + tBase);
                dist += points[p].internalDistance[tBase];


                float t = (dist / points[p].internalDistance[tBase])/10.0f;
                t += tBase / 10.0f;
                //Debug.Log("t: " + t);
                return points[p].bezier.GetPointAtTime(t);
            }
        }
    }
}

