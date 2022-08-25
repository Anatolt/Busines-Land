using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Crowd))]
public class CrowdEditor : Editor
{
    protected Crowd Crowd => target as Crowd;

    private void OnSceneGUI()
    {
        foreach (var wayNode in Crowd.WayNodes)
        {
            Handles.color = Color.white;

            wayNode.SetPosition(Handles.PositionHandle(wayNode.Position, Quaternion.identity));
        }

        for (int i = 0; i < Crowd.WayNodes.Count; i++)
        {
            var p1 = Crowd.WayNodes[i].Position;
            var p2 = Crowd.WayNodes[(i + 1) % Crowd.WayNodes.Count].Position;

            Handles.DrawLine(p1, p2);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add"))
        {
            Crowd.AddNewNode();

            EditorUtility.SetDirty(this);
        }

        if (GUILayout.Button("Fix WayNodes Y position"))
        {
            var neededPositionY = Crowd.transform.position.y;

            foreach (var wayNode in Crowd.WayNodes)
            {
                var nodePosition = wayNode.Position;
                wayNode.SetPosition(new Vector3(nodePosition.x, neededPositionY, nodePosition.z), true);
            }

            EditorUtility.SetDirty(this);
        }
    }
}