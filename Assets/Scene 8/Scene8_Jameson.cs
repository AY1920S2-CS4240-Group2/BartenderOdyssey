﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey 
{
    public class Scene8_Jameson : MonoBehaviour
    {
        public GameObject waypoint_Entrance;
        public GameObject waypoint_InFrontOfPlayer;
        public UnityEngine.AI.NavMeshAgent agent;
        public float rotationSpeed = 3.0f;
        public float meleeRange = 3.0f;
        public Animator anim;
        public Animator sceneAnimator;
        public GameObject player;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;
        private bool hasStarted = false;

        void Awake()
        {
            // DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            // dialogueRunner.AddCommandHandler("rotateTowards", RotateTowards);
        }

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // // Update is called once per frame
        // void Update()
        // {
        //     if(!(hasStarted)) {
        //         hasStarted = true;
        //         startDialogue();
        //     }

        //     float now = Time.realtimeSinceStartup;
        //     if (now - bounce > threshold)
        //     {
        //         bounce = Time.realtimeSinceStartup;
        //         if (Input.GetAxis(continueButton) == 1)
        //         {
        //             if (FindObjectOfType<DialogueRunner>().isDialogueRunning)
        //             {
        //                 FindObjectOfType<ExtendedDialogueUI>().MarkLineComplete();
        //             }
        //         }
        //     }
        // }

        // public void startDialogue() {
        //     if (!FindObjectOfType<DialogueRunner>().isDialogueRunning)
        //     {
        //         FindObjectOfType<DialogueRunner>().StartDialogue(this.gameObject.GetComponent<NPC>().talkToNode);
        //     }
        // }

        [YarnCommand("StandsUp")]
        public void StandsUp() {
            anim.SetTrigger("StandUp");
        }

        // [YarnCommand("defeatedAction")]
        // public void defeatedAction() {
        //     anim.SetTrigger("Defeated");
        // }

        // [YarnCommand("HandShake")]
        // public void HandShake() {
        //     anim.SetTrigger("HandShake");
        // }

        // [YarnCommand("walkToPlayer")]
        // public void walkToPlayer() {
        //     anim.SetTrigger("InjuredWalk");
        //     //agent.SetDestination(waypoint_InFrontOfPlayer.transform.position);
        // }

        // public void RotateTowards(string[] parameters, System.Action onComplete) {
        //     StartCoroutine(RotateMe(Vector3.up * -90, 0.8f, onComplete));
        // }
        // IEnumerator RotateMe(Vector3 byAngles, float inTime, System.Action onComplete) 
        // {    
        //     var fromAngle = transform.rotation;
        //     var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        //     for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
        //         transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
        //         yield return null;
        //     }
        //     onComplete();
        // }
    }
}