using UnityEngine;
using UnityEditor;

namespace BlazeAISpace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HitStateBehaviour))]
    public class HitStateBehaviourInspector : Editor
    {
        SerializedProperty hitAnims,
        hitAnimT,
        hitAnimGap,

        cancelAttackOnHit,

        playAudio,
        alwaysPlayAudio,

        callOthersRadius,
        agentLayersToCall,
        showCallRadius;


        void OnEnable()
        {
            hitAnims = serializedObject.FindProperty("hitAnims");
            hitAnimT = serializedObject.FindProperty("hitAnimT");
            hitAnimGap = serializedObject.FindProperty("hitAnimGap");

            cancelAttackOnHit = serializedObject.FindProperty("cancelAttackOnHit");

            playAudio = serializedObject.FindProperty("playAudio");
            alwaysPlayAudio = serializedObject.FindProperty("alwaysPlayAudio");

            callOthersRadius = serializedObject.FindProperty("callOthersRadius");
            agentLayersToCall = serializedObject.FindProperty("agentLayersToCall");
            showCallRadius = serializedObject.FindProperty("showCallRadius");
        }


        public override void OnInspectorGUI () 
        {
            HitStateBehaviour script = (HitStateBehaviour) target;
            int spaceBetween = 10;

            EditorGUILayout.PropertyField(hitAnims);
            EditorGUILayout.PropertyField(hitAnimT);
            EditorGUILayout.PropertyField(hitAnimGap);
            EditorGUILayout.Space(spaceBetween);


            EditorGUILayout.PropertyField(cancelAttackOnHit);
            EditorGUILayout.Space(spaceBetween);


            EditorGUILayout.PropertyField(playAudio);
            if (script.playAudio) {
                EditorGUILayout.PropertyField(alwaysPlayAudio);
            }
            EditorGUILayout.Space(spaceBetween);

            
            EditorGUILayout.PropertyField(callOthersRadius);
            EditorGUILayout.PropertyField(agentLayersToCall);
            EditorGUILayout.PropertyField(showCallRadius);


            serializedObject.ApplyModifiedProperties();
        }
    }
}
