using UnityEngine;
using BlazeAISpace;
using BlazeAIDemo;


public class DeathDemoScript : MonoBehaviour
{
    public BlazeAI blazeAI;
    HitStateBehaviour hitState;
    BlazeAIDemo.Health blazeHealth;

    
    void Start()
    {
        hitState = blazeAI.transform.GetComponent<HitStateBehaviour>();
        blazeHealth = blazeAI.GetComponent<BlazeAIDemo.Health>();
    }


    void Update()
    {
        // hit the AI
        if (Input.GetKeyDown(KeyCode.E)) {
            blazeAI.Hit();

            if (blazeHealth.currentHealth > 0) {
                blazeHealth.currentHealth -= 10;
            }

            if (blazeHealth.currentHealth <= 0) {
                blazeAI.Death();
            }
        }


        // return alive
        if (Input.GetKeyDown(KeyCode.R)) {
            blazeHealth.currentHealth = blazeHealth.maxHealth;
            blazeAI.ChangeState("normal");
        }
    }
}
