//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(EnemyStats))]
//public class TestEditor : Editor
//{
//    private EnemyStats targetObject;

//    void OnEnable()
//    {
//        targetObject = (EnemyStats)this.target;
//    }

//    // Implement this function to make a custom inspector.
//    public override void OnInspectorGUI()
//    {
//        // Using Begin/End ChangeCheck is a good practice to avoid changing assets on disk that weren't edited.
//        EditorGUI.BeginChangeCheck();

//        // Use the editor auto-layout system to make your life easy
//        EditorGUILayout.BeginVertical();
//        targetObject.attaqueForce = EditorGUILayout.FloatField("CapturingTime", targetObject.capturingTime);
//        EditorGUILayout.Space();
//        EditorGUILayout.Space();
//        targetObject.attaqueForce = EditorGUILayout.FloatField("MoveSpeed", targetObject.moveSpeed);
//        targetObject.attaqueForce = EditorGUILayout.FloatField("LookRange", targetObject.lookRange);
//        targetObject.attaqueForce = EditorGUILayout.FloatField("LookSphereCastRaduis", targetObject.lookSphereCastRaduis);
//        EditorGUILayout.Space();
//        EditorGUILayout.Space();
//        targetObject.attaqueForce = EditorGUILayout.FloatField("AttackRange", targetObject.attackRange);
//        targetObject.attaqueForce = EditorGUILayout.FloatField("AttaqueRate", targetObject.attaqueRate);
//        targetObject.attaqueForce = EditorGUILayout.FloatField("AttaqueDamage", targetObject.attaqueDamage);
//        EditorGUILayout.Space();
//        EditorGUILayout.Space();
//        targetObject.IsDistanceAttack = EditorGUILayout.Toggle("IsDistanceAttack", targetObject.IsDistanceAttack);

//        // GUI.enabled enables or disables all controls until it is called again
//        GUI.enabled = targetObject.IsDistanceAttack;
//        targetObject.attaqueForce = EditorGUILayout.FloatField("AttaqueForce", targetObject.attaqueForce);
//        targetObject.errorPercentage = EditorGUILayout.IntField("ErrorPercentage", targetObject.errorPercentage);

//        // Re-enable further controls
//        GUI.enabled = true;


//        EditorGUILayout.EndVertical();

//        // If anything has changed, mark the object dirty so it's saved to disk
//        if (EditorGUI.EndChangeCheck())
//            EditorUtility.SetDirty(target);
//    }
//}