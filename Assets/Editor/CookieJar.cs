using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CookieJar : EditorWindow
{
    private GameObject objectToDistribute;
    private int numbRows;
    private int numbColumns;
    private float startX;
    private float startY;
    private float distanceX;
    private float distanceY;

    //Assigning where we will be able to open the Cookie jar
    [MenuItem("Tools/Open Cookie Jar")]
    public static void ShowWindow()
    {
        GetWindow<CookieJar>("Open Cookie Jar");
    }

    private void OnGUI()
    {
        GUILayout.Label("Open Cookie Jar", EditorStyles.boldLabel);
        //GUI = Grafik User Interface

        EditorGUI.BeginChangeCheck();
        objectToDistribute = EditorGUILayout.ObjectField("Prefab to Distribute", objectToDistribute, typeof(GameObject), false) as GameObject;
        if (EditorGUI.EndChangeCheck())
        {
            if (objectToDistribute != null && !IsPrefab(objectToDistribute)) 
            {
                Debug.Log("Error! The object to distribute must be a prefab!");
                objectToDistribute = null;
            }
        }
        //Setting all the Variables we will need to automatically distribute the cookies 
        startX = EditorGUILayout.FloatField("Start X Position", startX);
        startY = EditorGUILayout.FloatField("Start Y Position", startY);
        numbRows = EditorGUILayout.IntField("Number of Rows", numbRows);
        numbColumns = EditorGUILayout.IntField("Number of Collums", numbColumns);
        distanceX = EditorGUILayout.FloatField("X Distance between Objects", distanceX);
        distanceY = EditorGUILayout.FloatField("Y Distance between Objects", distanceY);

        //Setting what will happen when we press the Button
        if (GUILayout.Button("Open Cookie Jar"))
        {
            OpenCookieJar();
        }
    }

    // Actually Setting what will happen when we open the Cookie Jar/ Chlicking the Button
    private void OpenCookieJar()
    {
        //Debug.Log("The Cookie Jar has been opened O.O");
        if (objectToDistribute == null)
        {
            Debug.Log("Please assign a Prefab to distribute!");
            return;
        }

        GameObject cookieParent = new GameObject("Cookies");

        //This is where the magic happens. Otherwise known as the math behind it all and not magic
        //This takes the number of rows and for each number, reduces that number by one and creats a collum, which in turn creats one cookie for each number of collums and then reduces that
        // number by one. 
        for (int row = 0; row < numbRows; row++)
        {
            for (int col = 0; col < numbColumns; col++)
            {
                Vector3 position = new Vector3(startX + col + distanceX, startY + row * distanceY, 0f);
                GameObject newObject = PrefabUtility.InstantiatePrefab(objectToDistribute as GameObject, cookieParent.transform) as GameObject;
                newObject.transform.position = position;
                newObject.name = $"Cookie-{(row * numbColumns) + col + 1:00}";
            }
        }
    }

    // Ensuring the Object is actually a prefab
    private bool IsPrefab(GameObject obj)
    {
        return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.NotAPrefab;
    }
}
