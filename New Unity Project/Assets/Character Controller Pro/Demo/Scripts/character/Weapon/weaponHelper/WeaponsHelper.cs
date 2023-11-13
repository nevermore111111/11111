using UnityEngine;

public class WeaponsHelper : MonoBehaviour
{
    public Transform character; // ��ѡ���ַ�

    private Vector3 localPosition;
    private Vector3 localForward;

    public void ShowDebugLogs()
    {
        if (character != null)
        {
            // ������� forward ����ת���� character ����ϵ��
            localForward = character.InverseTransformDirection(transform.forward);

            Debug.Log("Local Forward in Character Coordinates: " + localForward);
        }
        else
        {
            Debug.LogError("Character transform is not assigned.");
        }
    }
    
}
