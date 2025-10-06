using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField]
    PathManager pathManager;

    [SerializeField]
    List<WayPoint> thePath;
    List<int> toDelete;

    WayPoint selectedPoint = null;
    bool doRepaint = true;

    private void OnSceneGUI()
    {
        thePath = pathManager.GetPath();
        DrawPath(thePath);
    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }

    override public void OnInspectorGUI()
    {
        this.serializedObject.Update();
        thePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        //Button for adding a point to the path
        if(GUILayout.Button("Add Point to Path"))
        {
            pathManager.CreateAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if(thePath != null && thePath.Count > 0)
        {
            for(int i = 0; i < thePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                WayPoint p = thePath[i];

                Color c = GUI.color;
                if(selectedPoint == p) GUI.color = Color.green;

                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if(EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                //the delete button
                if(GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i); //add the index to out delete list
                }

                GUI.color = c;
                EditorGUILayout.EndHorizontal();

            }
        }

        if(toDelete.Count > 0)
        {
            foreach (int i in toDelete)
                thePath.RemoveAt(i);
            toDelete.Clear();
        }
    }

    public void DrawPath(List<WayPoint> path)
    {
        if(path != null)
        {
            int current = 0;
            foreach (WayPoint wp in path)
            {
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                WayPoint wpnext = path[next];

                DrawPathLine(wp, wpnext);

                current += 1;
            }
        }
        if(doRepaint) Repaint();
    }

    public void DrawPathLine(WayPoint p1, WayPoint p2)
    {
        //draw a line between current and next point
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    public bool DrawPoint(WayPoint p)
    {
        bool isChanged = false;

        if(selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldpos = p.GetPos();
            Vector3 newPos = Handles.PositionHandle(oldpos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newPos);

            Handles.SphereHandleCap(-1, newPos, Quaternion.identity, 0.25f * handleSize, EventType.Repaint);
            if(EditorGUI.EndChangeCheck())
            {
                p.SetPos(newPos);
            }

            Handles.color = c;

        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }
        return isChanged;
    }

}
