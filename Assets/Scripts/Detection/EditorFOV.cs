using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(FieldOfView))]
public class EnemyFOVEditor : Editor
{
    // Yes I practically copypasted this from this link https://www.youtube.com/watch?v=j1-OyLo77ss. Will try to apply for other stuff if need be 
    private void OnSceneGUI()
    {
        FieldOfView enemyEditor = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemyEditor.transform.position, Vector3.up, Vector3.forward, 360, enemyEditor.range);

        Vector3 viewAngleLeft = DirFromForward(enemyEditor.transform.eulerAngles.y, -enemyEditor.angle / 2);
        Vector3 viewAngleRight = DirFromForward(enemyEditor.transform.eulerAngles.y, enemyEditor.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(enemyEditor.transform.position, enemyEditor.transform.position + viewAngleLeft * enemyEditor.range);
        Handles.DrawLine(enemyEditor.transform.position, enemyEditor.transform.position + viewAngleRight * enemyEditor.range);
    }

    private Vector3 DirFromForward(float eulerY, float angle)
    {
        angle += eulerY;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
