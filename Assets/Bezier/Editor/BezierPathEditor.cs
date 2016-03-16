using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierPath))]
public class BezierPathEditor : Editor
{
    public BezierPath.PathPoint selectedPoint;

    private Vector3 pointPos;
    private Vector3 handlePos;

    [MenuItem("GameObject/Create Other/Bezier Path")]
    public static void CreateBezierPath()
    {
        GameObject go = new GameObject("BezierPath");
        BezierPath path = go.AddComponent<BezierPath>();
        BezierPath.PathPoint p = new BezierPath.PathPoint();
        path.points.Add(p);
    }

    void addPoint(BezierPath path, BezierPath.PathPoint point)
    {
        Undo.RegisterUndo(path, "create bezier point");
        // if lsat point, add new. if middle point, add point inbetween
        if (path.points.Count - 1 == path.points.IndexOf(point))
        {
            // add at end
            BezierPath.PathPoint p = new BezierPath.PathPoint();
            if (point.h1.magnitude < Mathf.Epsilon)
                p.p1 = point.p1 + (Vector3.right) * 2;
            else
                p.p1 = point.p1 + (point.h1).normalized * 2;
            p.h1 = point.h1;
            path.points.Add(p);
            selectedPoint = p;
        }
        else
        {
            // add in middle
            BezierPath.PathPoint point2 = path.points[path.points.IndexOf(point)+1];
            Bezier b = new Bezier(point.p1, point.h1, -point2.h1, point2.p1);
            BezierPath.PathPoint p = new BezierPath.PathPoint();
            p.p1 = b.GetPointAtTime(0.5f);
            p.h1 = b.GetPointAtTime(0.6f) - p.p1;
            path.points.Insert(path.points.IndexOf(point) + 1, p);
        }
    }

    void deletePoint(BezierPath path, BezierPath.PathPoint point)
    {
        Undo.RegisterUndo(path, "delete bezier point");
        if (path.points.Count > 1)
            path.points.Remove(point);

        if (selectedPoint == point)
            selectedPoint = path.points[0];
    }

    void OnSceneGUI()
    {
        BezierPath path = (BezierPath)target;

        if (Event.current.type == EventType.mouseDown)
        {
            // record positions
            pointPos = selectedPoint.p1;
            handlePos = selectedPoint.h1;
        }
        if (Event.current.type == EventType.mouseUp)
        {
            if (selectedPoint.p1 != pointPos || selectedPoint.h1 != handlePos)
            {
                // put them back, register undo
                Vector3 newPos = selectedPoint.p1;
                Vector3 newHandle = selectedPoint.h1;
                selectedPoint.p1 = pointPos;
                selectedPoint.h1 = handlePos;
                Undo.RegisterUndo(path, "moving point");
                selectedPoint.p1 = newPos;
                selectedPoint.h1 = newHandle;
                EditorUtility.SetDirty(path);
            }
        }
        bool needCreatePoint = false;
        bool needDeletePoint = false;

        if (selectedPoint != null && Event.current.type != EventType.repaint)
        {
            // add + and - buttons
            Handles.BeginGUI();
            Vector2 screenpos = HandleUtility.WorldToGUIPoint(selectedPoint.p1);
            // inverse y
            //screenpos.y = Screen.height - screenpos.y;
            if (GUI.Button(new Rect(screenpos.x - 10 + 20, screenpos.y - 20, 20, 20), "+"))
                needCreatePoint = true;
            if (GUI.Button(new Rect(screenpos.x - 10 - 20, screenpos.y - 20, 20, 20), "-"))
                needDeletePoint = true;
            Handles.EndGUI();
        }
        

        // disable transform tool if selected
        if (Selection.activeGameObject == path.gameObject)
        {
            Tools.current = Tool.None;
            if (selectedPoint == null && path.points.Count > 0)
                selectedPoint = path.points[0];
        }
        
        // draw the handles for the points
        foreach (BezierPath.PathPoint point in path.points)
        {
            if (point == selectedPoint)
            {
                //if (GUI.changed)
                //    EditorUtility.SetDirty(target);
                point.p1 = Handles.PositionHandle(point.p1, Quaternion.identity);

                point.h1 = Handles.PositionHandle(point.h1 + point.p1, Quaternion.identity) - point.p1;
                Handles.CubeCap(0, point.p1, Quaternion.identity, 1);
                Handles.CubeCap(0, point.h1 + point.p1, Quaternion.identity, 0.5f);
            }
            else
            {
                // draw button
                float size = 1;//HandleUtility.GetHandleSize(point.p1) / 3;
                if (Handles.Button(point.p1, Quaternion.identity, 1, size, Handles.CubeCap))
                {
                    selectedPoint = point;
                }
            }
        }

        Handles.color = path.lineColor;
        // draw the lines
        for (int i = 0; i < path.points.Count - 1; i++)
        {
            // draw line from point i to point i+1
            path.points[i].bezier.p1 = path.points[i].p1;
            path.points[i].bezier.h1 = path.points[i].h1;
            path.points[i].bezier.p2 = path.points[i + 1].p1;
            path.points[i].bezier.h2 = -path.points[i + 1].h1; // negative of this handle

            Vector3 oldPos = path.points[i].bezier.GetPointAtTime(0);
            for (float t = 0; t < 1.01; t += 0.1f)
            {
                Vector3 newPos = path.points[i].bezier.GetPointAtTime(t);
                Handles.DrawLine(oldPos, newPos);
                oldPos = newPos;
            }
        }

        if (selectedPoint != null && Event.current.type == EventType.repaint)
        {
            // add + and - buttons
            Handles.BeginGUI();
            Vector2 screenpos = HandleUtility.WorldToGUIPoint(selectedPoint.p1);
            // inverse y
            //screenpos.y = Screen.height - screenpos.y;
            if (GUI.Button(new Rect(screenpos.x - 10 + 20, screenpos.y - 20, 20, 20), "+"))
                needCreatePoint = true;
            if (GUI.Button(new Rect(screenpos.x - 10 - 20, screenpos.y - 20, 20, 20), "-"))
                needDeletePoint = true;
            Handles.EndGUI();
        }

        if (needCreatePoint)
            addPoint(path,selectedPoint);

        if (needDeletePoint)
            deletePoint(path, selectedPoint);

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
    static void drawPathGizmos(BezierPath path, GizmoType gizmoType)
    {
        // don't draw if selected
        if (Selection.activeGameObject == path.gameObject) return;

        foreach (BezierPath.PathPoint p in path.points)
        {
            // draw point
            Gizmos.color = path.color;
            Gizmos.DrawCube(p.p1, Vector3.one);

            //draw handle
            //Gizmos.color = Color.blue;
            //Gizmos.DrawCube(p.p1 + p.h1, Vector3.one / 2);
        }

        Gizmos.color = path.lineColor;

        for (int i = 0; i < path.points.Count - 1; i++)
        {
            // draw line from point i to point i+1
            path.points[i].bezier.p1 = path.points[i].p1;
            path.points[i].bezier.h1 = path.points[i].h1;
            path.points[i].bezier.p2 = path.points[i + 1].p1;
            path.points[i].bezier.h2 = -path.points[i + 1].h1; // negative of this handle

            Vector3 oldPos = path.points[i].bezier.GetPointAtTime(0);
            for (float t = 0; t < 1.01; t += 0.1f)
            {
                Vector3 newPos = path.points[i].bezier.GetPointAtTime(t);
                Gizmos.DrawLine(oldPos, newPos);
                oldPos = newPos;
            }
        }
    }
}

