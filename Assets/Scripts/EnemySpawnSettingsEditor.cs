using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemySpawnSettings))]
public class EnemySpawnSettingsEditor : Editor
{
    SerializedProperty spawnTimes;

    void OnEnable()
    {
        spawnTimes = serializedObject.FindProperty("spawnTimes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < spawnTimes.arraySize; i++)
        {
            SerializedProperty spawnTime = spawnTimes.GetArrayElementAtIndex(i);

            SerializedProperty time = spawnTime.FindPropertyRelative("time");
            SerializedProperty isRandom = spawnTime.FindPropertyRelative("isRandom");
            SerializedProperty gameObjects = spawnTime.FindPropertyRelative("gameObjects");
            SerializedProperty colvo = spawnTime.FindPropertyRelative("colvo");

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(time, new GUIContent("Time")); // Изменение существующего свойства
            EditorGUILayout.PropertyField(isRandom, new GUIContent("Is Random"));

            if (isRandom.boolValue)
            {
                EditorGUILayout.PropertyField(colvo, new GUIContent("Quantity")); // Изменение существующего свойства
            }
            else
            {
                EditorGUILayout.PropertyField(gameObjects, new GUIContent("Game Objects"), true); // Изменение существующего свойства
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Spawn Time"))
        {
            spawnTimes.InsertArrayElementAtIndex(spawnTimes.arraySize);
        }

        if (GUILayout.Button("Remove Last Spawn Time"))
        {
            if (spawnTimes.arraySize > 0)
            {
                spawnTimes.DeleteArrayElementAtIndex(spawnTimes.arraySize - 1);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}