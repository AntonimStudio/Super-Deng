using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

            /*
            RedFaces
            public bool isRedFaceTurnOn;
            public bool isRedFaceRandom;
            public int[] arrayOfRedFaces;
            public int quantityOfRedFaces;
            */

            SerializedProperty isRedFaceTurnOn = spawnTime.FindPropertyRelative("isRedFaceTurnOn");
            SerializedProperty isRedFaceRandom = spawnTime.FindPropertyRelative("isRedFaceRandom");
            SerializedProperty arrayOfRedFaces = spawnTime.FindPropertyRelative("arrayOfRedFaces");
            SerializedProperty quantityOfRedFaces = spawnTime.FindPropertyRelative("quantityOfRedFaces");

            /*
            RedWaves
            public bool isRedWaveTurnOn;
            public bool isRedWaveRandom;
            public int[] arrayOfRedWaves;
            public int quantityOfRedWaves;
            public int proximityLimitOfRedWaves;
            */

            SerializedProperty isRedWaveTurnOn = spawnTime.FindPropertyRelative("isRedWaveTurnOn");
            SerializedProperty isRedWaveRandom = spawnTime.FindPropertyRelative("isRedWaveRandom");
            SerializedProperty arrayOfRedWaves = spawnTime.FindPropertyRelative("arrayOfRedWaves");
            SerializedProperty quantityOfRedWaves = spawnTime.FindPropertyRelative("quantityOfRedWaves");
            SerializedProperty proximityLimitOfRedWaves = spawnTime.FindPropertyRelative("proximityLimitOfRedWaves");

            /*
            FallFaces
            public bool isFallFaceTurnOn;
            public bool isFallFaceRandom;
            public int[] arrayOfFallFaces;
            public int quantityOfFallFaces;
            public int proximityLimitOfFallFaces;
            public bool isResetDelay;
            public int resetDelayTime;
            */

            SerializedProperty isFallFaceTurnOn = spawnTime.FindPropertyRelative("isFallFaceTurnOn");
            SerializedProperty isFallFaceRandom = spawnTime.FindPropertyRelative("isFallFaceRandom");
            SerializedProperty arrayOfFallFaces = spawnTime.FindPropertyRelative("arrayOfFallFaces");
            SerializedProperty quantityOfFallFaces = spawnTime.FindPropertyRelative("quantityOfFallFaces");
            SerializedProperty proximityLimitOfFallFaces = spawnTime.FindPropertyRelative("proximityLimitOfFallFaces");
            SerializedProperty isResetDelay = spawnTime.FindPropertyRelative("isResetDelay");
            SerializedProperty resetDelayTime = spawnTime.FindPropertyRelative("resetDelayTime");

            /*
            ResetFallFaces
            public bool isResetFallFaceTurnOn;
            */

            SerializedProperty isResetFallFaceTurnOn = spawnTime.FindPropertyRelative("isResetFallFaceTurnOn");

            /*
            Bonuses
            public bool isBonusTurnOn;
            public bool isBonusRandom;
            public int[] arrayOfBonuses;
            public int quantityOfBonuses;
            public int proximityLimitOfBonuses;
            public bool isBonusHealth;
            public bool isBonusCombo;
            */

            SerializedProperty isBonusTurnOn = spawnTime.FindPropertyRelative("isBonusTurnOn");
            SerializedProperty isBonusRandom = spawnTime.FindPropertyRelative("isBonusRandom");
            SerializedProperty arrayOfBonuses = spawnTime.FindPropertyRelative("arrayOfBonuses");
            SerializedProperty quantityOfBonuses = spawnTime.FindPropertyRelative("quantityOfBonuses");
            SerializedProperty proximityLimitOfBonuses = spawnTime.FindPropertyRelative("proximityLimitOfBonuses");
            SerializedProperty isBonusHealth = spawnTime.FindPropertyRelative("isBonusHealth");
            SerializedProperty isBonusCombo = spawnTime.FindPropertyRelative("isBonusCombo");

            /*
            Portals
            public bool isPortalTurnOn;
            public bool isPortalRandom;
            public int[] arrayOfPortals;
            public int quantityOfPortals;
            public int proximityLimitOfPortals;
            */

            SerializedProperty isPortalTurnOn = spawnTime.FindPropertyRelative("isPortalTurnOn");
            SerializedProperty isPortalRandom = spawnTime.FindPropertyRelative("isPortalRandom");
            SerializedProperty arrayOfPortals = spawnTime.FindPropertyRelative("arrayOfPortals");
            SerializedProperty quantityOfPortals = spawnTime.FindPropertyRelative("quantityOfPortals");
            SerializedProperty proximityLimitOfPortals = spawnTime.FindPropertyRelative("proximityLimitOfPortals");

            /*
            FaceDance
            public bool isFaceDanceTurnOn;
            public bool isSetFaceDanceIncrease;
            public bool isSetFaceDanceDecrease;
            */

            SerializedProperty isFaceDanceTurnOn = spawnTime.FindPropertyRelative("isFaceDanceTurnOn");
            SerializedProperty isSetFaceDanceIncrease = spawnTime.FindPropertyRelative("isSetFaceDanceIncrease");
            SerializedProperty isSetFaceDanceDecrease = spawnTime.FindPropertyRelative("isSetFaceDanceDecrease");

            /*
            SphereDance
            public bool isSphereDanceTurnOn;
            */
            SerializedProperty isSphereDanceTurnOn = spawnTime.FindPropertyRelative("isSphereDanceTurnOn");

            /*
            CameraRotation
            public bool isCameraRotationTurnOn;
            public bool isCameraRotationClockwise;
            public float cameraRotationSpeed;
            */

            SerializedProperty isCameraRotationTurnOn = spawnTime.FindPropertyRelative("isCameraRotationTurnOn");
            SerializedProperty isCameraRotationClockwise = spawnTime.FindPropertyRelative("isCameraRotationClockwise");
            SerializedProperty cameraRotationSpeed = spawnTime.FindPropertyRelative("cameraRotationSpeed");

            /*
            RGB
            public bool isRGBTurnOn;
            public int speedRGB;
            public bool isSetRGBIncrease;
            public bool isSetRGBDecrease;
            */

            SerializedProperty isRGBTurnOn = spawnTime.FindPropertyRelative("isRGBTurnOn");
            SerializedProperty speedRGB = spawnTime.FindPropertyRelative("speedRGB");
            SerializedProperty targetValueRGB = spawnTime.FindPropertyRelative("targetValueRGB");
            SerializedProperty isSetRGBIncrease = spawnTime.FindPropertyRelative("isSetRGBIncrease");
            SerializedProperty isSetRGBDecrease = spawnTime.FindPropertyRelative("isSetRGBDecrease");

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            GUIStyle headerStyle = new(EditorStyles.label)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold
            };
            headerStyle.normal.textColor = Color.white;
            GUIStyle labelStyle = new(EditorStyles.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold
            };
            labelStyle.normal.textColor = Color.white;
            GUIStyle attentionStyle = new(EditorStyles.label)
            {
                fontSize = 10,
                fontStyle = FontStyle.Italic
            };
            attentionStyle.normal.textColor = Color.red;

            


            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Frame ¹" + i.ToString(), headerStyle);
            EditorGUILayout.PropertyField(time, new GUIContent("Time"));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Red Face Settings:", labelStyle);

            EditorGUILayout.PropertyField(isRedFaceTurnOn, new GUIContent("Is Red Face turn on?"));
            if (isRedFaceTurnOn.boolValue)
            {

                EditorGUILayout.PropertyField(isRedFaceRandom, new GUIContent("Is Red Face random?"));
                if (isRedFaceRandom.boolValue)
                {
                    EditorGUILayout.PropertyField(quantityOfRedFaces, new GUIContent("Quantity of Red Faces"));
                }
                else
                {
                    EditorGUILayout.PropertyField(arrayOfRedFaces, new GUIContent("Array of Red Faces"), true);
                }
            }


            EditorGUILayout.LabelField("Red Wave Settings:", labelStyle);
            EditorGUILayout.PropertyField(isRedWaveTurnOn, new GUIContent("Is Red Wave turn on?"));
            if (isRedWaveTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(isRedWaveRandom, new GUIContent("Is Red Wave random?"));
                if (isRedWaveRandom.boolValue)
                {
                    EditorGUILayout.PropertyField(quantityOfRedWaves, new GUIContent("Quantity of Red Waves"));
                }
                else
                {
                    EditorGUILayout.PropertyField(arrayOfRedWaves, new GUIContent("Array of Red Waves"), true);
                }
                EditorGUILayout.PropertyField(proximityLimitOfRedWaves, new GUIContent("Proximity Limit of Red Waves"));
            }

            EditorGUILayout.LabelField("Fall Face Settings:", labelStyle);

            EditorGUILayout.PropertyField(isFallFaceTurnOn, new GUIContent("Is Fall Face turn on?"));
            if (isFallFaceTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(isFallFaceRandom, new GUIContent("Is Fall Face random?"));
                if (isFallFaceRandom.boolValue)
                {
                    EditorGUILayout.PropertyField(quantityOfFallFaces, new GUIContent("Quantity of Fall Faces")); 
                }
                else
                {
                    EditorGUILayout.PropertyField(arrayOfFallFaces, new GUIContent("Array of Fall Faces"), true);
                }

                EditorGUILayout.PropertyField(proximityLimitOfFallFaces, new GUIContent("Proximity Limit of Fall Faces"));
                EditorGUILayout.PropertyField(isResetDelay, new GUIContent("Is Reset Delay?"));
                if (isResetDelay.boolValue)
                {
                    EditorGUILayout.PropertyField(resetDelayTime, new GUIContent("Reset Delay time"));
                }
                    
            }

            EditorGUILayout.LabelField("Reset Fall Face Settings:", labelStyle);

            EditorGUILayout.PropertyField(isResetFallFaceTurnOn, new GUIContent("Is Reset Fall Face turn on?"));

            EditorGUILayout.LabelField("Bonus Settings:", labelStyle);

            EditorGUILayout.PropertyField(isBonusTurnOn, new GUIContent("Is Bonus turn on?"));
            if (isBonusTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(isBonusRandom, new GUIContent("Is Bonus Random?"));
                if (isBonusRandom.boolValue)
                {
                    EditorGUILayout.PropertyField(quantityOfBonuses, new GUIContent("Quantity of Bonuses"));
                }
                else
                {
                    EditorGUILayout.PropertyField(arrayOfBonuses, new GUIContent("Array of Bonuses"), true);
                }
                
                EditorGUILayout.PropertyField(proximityLimitOfBonuses, new GUIContent("Proximity Limit of Bonuses"));
                EditorGUILayout.PropertyField(isBonusHealth, new GUIContent("Is Health Bonus?"));
                EditorGUILayout.PropertyField(isBonusCombo, new GUIContent("Is Combo Bonus?"));
                EditorGUILayout.LabelField("can not be false and false!!!", attentionStyle);
            }

            EditorGUILayout.LabelField("Portal Settings:", labelStyle);

            EditorGUILayout.PropertyField(isPortalTurnOn, new GUIContent("Is Portal turn on?"));
            if (isPortalTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(isPortalRandom, new GUIContent("Is Portal Random?"));
                if (isPortalRandom.boolValue)
                {
                    EditorGUILayout.PropertyField(quantityOfPortals, new GUIContent("Quantity of Portals"));
                }
                else
                {
                    EditorGUILayout.PropertyField(arrayOfPortals, new GUIContent("Array of Portals"), true);
                }
                EditorGUILayout.PropertyField(proximityLimitOfPortals, new GUIContent("Proximity Limit of Portals"));
            }

            EditorGUILayout.LabelField("FaceDance Settings:", labelStyle);

            EditorGUILayout.PropertyField(isFaceDanceTurnOn, new GUIContent("Is FaceDance turn on?"));
            if (isFaceDanceTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(isSetFaceDanceIncrease, new GUIContent("Is set FaceDance increase?"));
                EditorGUILayout.PropertyField(isSetFaceDanceDecrease, new GUIContent("Is set FaceDance decrease?"));
                EditorGUILayout.LabelField("can not be true and true!!!", attentionStyle);
            }


            EditorGUILayout.LabelField("SphereDance Settings:", labelStyle);

            EditorGUILayout.PropertyField(isSphereDanceTurnOn, new GUIContent("Is SphereDance turn on?"));

            EditorGUILayout.LabelField("Camera Rotation Settings:", labelStyle);

            EditorGUILayout.PropertyField(isCameraRotationTurnOn, new GUIContent("Is Camera Rotation turn on?"));

            if (isCameraRotationTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(isCameraRotationClockwise, new GUIContent("Is Camera Rotation clockwise?"));
                EditorGUILayout.PropertyField(cameraRotationSpeed, new GUIContent("Camera Rotation Speed"));
                
            }

            EditorGUILayout.LabelField("RGB Settings:", labelStyle);

            EditorGUILayout.PropertyField(isRGBTurnOn, new GUIContent("Is RGB turn on?"));
            if (isRGBTurnOn.boolValue)
            {
                EditorGUILayout.PropertyField(speedRGB, new GUIContent("Speed RGB"));
                EditorGUILayout.PropertyField(targetValueRGB, new GUIContent("Target Value SSRGB"));
                EditorGUILayout.PropertyField(isSetRGBIncrease, new GUIContent("Is set RGB increase?"));
                EditorGUILayout.PropertyField(isSetRGBDecrease, new GUIContent("Is set RGB decrease?"));
                EditorGUILayout.LabelField("can not be true and true!!!", attentionStyle);
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