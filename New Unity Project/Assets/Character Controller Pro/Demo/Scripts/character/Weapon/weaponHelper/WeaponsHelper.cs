using UnityEngine;

public class WeaponsHelper : MonoBehaviour
{
    public Transform character; // 可选项字符

    private Vector3 localPosition;
    private Vector3 localForward;

    public void ShowDebugLogs()
    {
        if (character != null)
        {
            // 将物体的 forward 方向转换到 character 坐标系下
            localForward = character.InverseTransformDirection(transform.forward);

            Debug.Log("Local Forward in Character Coordinates: " + localForward);
        }
        else
        {
            Debug.LogError("Character transform is not assigned.");
        }
    }
    
}
