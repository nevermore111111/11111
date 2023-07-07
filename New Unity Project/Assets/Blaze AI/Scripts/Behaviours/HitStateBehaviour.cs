using System.Collections.Generic;
using UnityEngine;

namespace BlazeAISpace
{
    public class HitStateBehaviour : MonoBehaviour
    {   
        [Header("HIT PROPERTIES"), Tooltip("Hit animation names and their durations. One will be chosen at random on every hit.")]
        public List<HitData> hitAnims;
        [Min(0), Tooltip("The animation transition from current animation to the hit animation.")]
        public float hitAnimT = 0.2f;
        [Min(0), Tooltip("The gap time between replaying the hit animations to avoid having the animation play on every single hit which may look bad on very fast and repitive attacks such as a machine gun.")]
        public float hitAnimGap = 0.3f;
        
        
        [Header("ATTACK OPTION"), Tooltip("If set to true will cancel the attack if got hit.")]
        public bool cancelAttackOnHit;
        
        
        [Header("AUDIO"), Tooltip("Play audio when hit. Set your audios in the audio scriptable in the General Tab in Blaze AI.")]
        public bool playAudio;
        [Tooltip("If enabled, a hit audio will always play when hit. If false, there's a 50/50 chance whether an audio will be played or not.")]
        public bool alwaysPlayAudio = true;

        
        [Header("CALL OTHERS"), Min(0), Tooltip("The radius to call other AIs when hit. You use this by calling blaze.Hit(player, true).")]
        public float callOthersRadius = 5;
        [Tooltip("The layers of the agents to call. You use this by calling blaze.Hit(player, true).")]
        public LayerMask agentLayersToCall;
        [Tooltip("Shows the call radius as a cyan wire sphere in the scene view.")]
        public bool showCallRadius;


        BlazeAI blaze;
        bool playedAudio;

        float _duration = 0;
        float _gapTimer = 0;
        float hitDuration = 0;

        [System.Serializable]
        public struct HitData {
            [Tooltip("Set the animation name of the hit.")]
            public string animName;
            [Tooltip("Set the duration of the hit state for this animation.")]
            public float duration;
        }
        

        #region UNITY METHODS

        void Start()
        {
            blaze = GetComponent<BlazeAI>();    

            // force shut if not the same state
            if (blaze.state != BlazeAI.State.hit) {
                enabled = false;
            }
        }


        void OnDisable()
        {
            ResetTimers();
            blaze.hitEnemy = null;
        }


        // show the call others radius
        void OnDrawGizmosSelected() 
        {
            if (!showCallRadius) {
                return;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, callOthersRadius);
        }


        // hit behaviour here
        void Update()
        {
            // check if a hit was registered
            if (blaze.hitRegistered) {
                blaze.hitRegistered = false;
                //这里根据hitkind来确认使用哪一种受击动画
                int chosenHitIndex = Random.Range(0, hitAnims.Count);
                hitDuration = hitAnims[chosenHitIndex].duration;
                
                
                if (_duration == 0) {
                    blaze.animManager.Play(hitAnims[chosenHitIndex].animName, hitAnimT, true);
                }
                else {
                    if (_gapTimer >= hitAnimGap) {
                        blaze.animManager.Play(hitAnims[chosenHitIndex].animName, hitAnimT, true);
                        _gapTimer = 0;
                    }
                }
                
                _duration = 0;

                
                // call others
                if (blaze.callOthersOnHit) {
                    CallOthers();
                }


                // play hit audio
                PlayAudio();
            }


            // cancel attacks
            if (cancelAttackOnHit) {
                blaze.StopAttack();
            }


            _gapTimer += Time.deltaTime;


            // hit duration timer
            _duration += Time.deltaTime;

            if (_duration >= hitDuration) {
                FinishHitState();
            }
        }

        #endregion

        #region BEHAVIOUR METHODS

        // exit hit state and turn to either alert or attack state
        void FinishHitState()   
        {
            ResetTimers();

            
            // if AI was in cover -> return to cover state
            if (blaze.hitWhileInCover && blaze.coverShooterMode) {
                blaze.SetState(BlazeAI.State.goingToCover);
                return;
            }


            if (blaze.isFleeing) {
                blaze.SetState(BlazeAI.State.attack);
                return;
            }


            // if the enemy that did the hit is passed -> set AI to go to enemy location
            if (blaze.hitEnemy) {
                // check the passed enemy isn't the same AI
                if (blaze.hitEnemy.transform.IsChildOf(transform)) {
                    blaze.ChangeState("alert");
                    return;
                }
                
                blaze.SetEnemy(blaze.hitEnemy);
                return;
            }

            
            // if an enemy is already targeted -> go to attack state
            if (blaze.enemyToAttack) {
                blaze.SetState(BlazeAI.State.attack);
                return;
            }


            // if nothing -> turn alert
            blaze.ChangeState("alert");
        }


        // play the hit audio
        void PlayAudio()
        {
            if (!playAudio) {
                return;
            }
            
            if (playedAudio) {
                return;
            }

            if (blaze.IsAudioScriptableEmpty()) {
                return;
            }

            if (!alwaysPlayAudio) {
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    return;
                }
            }

            if (blaze.PlayAudio(blaze.audioScriptable.GetAudio(AudioScriptable.AudioType.Hit))) {
                playedAudio = true;
            }
        }


        // call others
        void CallOthers()
        {
            Collider[] agentsColl = new Collider[5];
            int agentsCollNum = Physics.OverlapSphereNonAlloc(transform.position + blaze.centerPosition, callOthersRadius, agentsColl, agentLayersToCall);
        
            for (int i=0; i<agentsCollNum; i++) {
                BlazeAI script = agentsColl[i].GetComponent<BlazeAI>();

                // if caught collider is that of the same AI -> skip
                if (transform.IsChildOf(agentsColl[i].transform)) {
                    continue;
                }


                // if the caught collider is actually the current AI's enemy (AI vs AI) -> skip
                if (blaze.enemyToAttack != null) {
                    if (blaze.enemyToAttack.transform.IsChildOf(agentsColl[i].transform)) {
                        continue;
                    }
                }

                
                // if script doesn't exist -> skip
                if (script == null) {
                    continue;
                }


                // reaching this point means current item is a valid AI

                if (blaze.hitEnemy) {
                    script.SetEnemy(blaze.hitEnemy, true, true);
                    continue;
                }
                

                // if no enemy has been passed
                // make it a random point within the destination
                Vector3 randomPoint = script.RandomSpherePoint(transform.position);
                
                script.ChangeState("alert");
                script.MoveToLocation(randomPoint);
            }
        }


        // reset the timers of hit duration
        void ResetTimers()
        {
            _duration = 0;
            _gapTimer = 0;
            playedAudio = false;
        }

        #endregion
    }
}
