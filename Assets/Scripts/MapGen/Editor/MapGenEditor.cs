using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGen))]

public class MapGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapGenerator = (MapGen)target;
        EditorGUILayout.LabelField("Map Dimensions", EditorStyles.boldLabel);
        mapGenerator.totalDimensions = EditorGUILayout.Vector2IntField("Dimensions", mapGenerator.totalDimensions);
        mapGenerator.offset = EditorGUILayout.Vector3Field("Offset", mapGenerator.offset);
        mapGenerator.spacing = EditorGUILayout.FloatField("Spacing", mapGenerator.spacing);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Tile Configuration", EditorStyles.boldLabel);
        int newSize = Mathf.Max(0, EditorGUILayout.IntField("Number of Tiles", mapGenerator.tiles.Length));
        if (newSize != mapGenerator.tiles.Length)
        {
            System.Array.Resize(ref mapGenerator.tiles, newSize);
        }

        EditorGUILayout.Space();

        for (int i = 0; i < mapGenerator.tiles.Length; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Tile {i + 1}", EditorStyles.boldLabel);

            mapGenerator.tiles[i].prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", mapGenerator.tiles[i].prefab, typeof(GameObject), false);
            int totalMapSize = mapGenerator.totalDimensions.x * mapGenerator.totalDimensions.y;
            if (mapGenerator.tiles[i].constraints == null || mapGenerator.tiles[i].constraints.Length != totalMapSize)
            {
                mapGenerator.tiles[i].constraints = new bool[totalMapSize];
            }

            EditorGUILayout.LabelField("Constraints (allowed spawn positions):");
            int width = mapGenerator.totalDimensions.x;
            int height = mapGenerator.totalDimensions.y;

            for (int y = height - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    mapGenerator.tiles[i].constraints[index] =
                        EditorGUILayout.Toggle(mapGenerator.tiles[i].constraints[index], GUILayout.Width(20));
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Map Dimensions"))
        {
            mapGenerator.getDimensions();
        }
        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateMap();
        }
        if (GUILayout.Button("Delete Map"))
        {
            mapGenerator.DeleteMap();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Generated Chunks", EditorStyles.boldLabel);

        if (mapGenerator.chunks != null && mapGenerator.chunks.Length > 0)
        {
            int width = mapGenerator.totalDimensions.x;
            int height = mapGenerator.totalDimensions.y;

            for (int y = height - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (index < mapGenerator.chunks.Length)
                    {
                        GameObject chunk = mapGenerator.chunks[index];
                        GUIContent content = chunk != null
                            ? new GUIContent(AssetPreview.GetAssetPreview(chunk), chunk.name)
                            : new GUIContent("None");

                        GUILayout.Box(content, GUILayout.Width(64), GUILayout.Height(64));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No chunks generated yet.", MessageType.Info);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
