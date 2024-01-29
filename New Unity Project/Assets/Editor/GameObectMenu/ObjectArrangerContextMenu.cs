using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ObjectArrangerContextMenu
{
    //static ObjectArrangerContextMenu()
    //{
    //    SceneView.duringSceneGui += OnSceneGUI;
    //}

    [MenuItem("GameObject/排队", false, 10)]
    static void ArrangeObjectsContextMenu(MenuCommand menuCommand)
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected. Please select the objects you want to arrange.");
            return;
        }

        float xOffset = 0f;
        float zOffset = 0f;
        float yOffset = 0f;
        float spacing = 2f;

        foreach (var obj in selectedObjects)
        {
            obj.transform.position = new Vector3(xOffset, yOffset, zOffset);
            xOffset += spacing;

            if (xOffset >= spacing * 4)
            {
                xOffset = 0f;
                yOffset += spacing;
            }
        }

        //Debug.Log("Objects arranged vertically.");
    }

    //static void OnSceneGUI(SceneView sceneView)
    //{
    //    if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
    //    {
    //        在场景视图中右键点击时显示菜单项
    //       GameObject[] selectedObjects = Selection.gameObjects;

    //        if (selectedObjects.Length > 0)
    //        {
    //            GenericMenu menu = new GenericMenu();
    //            menu.AddItem(new GUIContent("Arrange Objects Vertically"), false, ArrangeObjectsContextMenu);
    //            menu.ShowAsContext();
    //        }
    //    }
    //}
}
