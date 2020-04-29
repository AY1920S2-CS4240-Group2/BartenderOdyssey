﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using Yarn.Unity;
using UnityEngine.SceneManagement;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene1_BarOwner_WayPointControl : MonoBehaviour
    {   
        public GameObject waypoint_Entrance;
        public GameObject waypoint_InFrontOfPlayer;
        public NavMeshAgent agent;
        public ThirdPersonCharacter character;
        public float rotationSpeed = 0.5f;
        public float meleeRange = 0.5f;
        public float interactionRadius = 5.0f;
        public GameObject player;
        public Animator anim;
        public Animator sceneAnimator;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;
        private string currWaypoint = "entrance";
        private bool isWalking = false;
        private bool hasStarted = false;
            
        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("waitForMove", WaitForMove);
        }
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if(!(hasStarted)) {
                hasStarted = true;
                startDialogue();
            }

            float now = Time.realtimeSinceStartup;
            if (now - bounce > threshold)
            {
                bounce = Time.realtimeSinceStartup;
                if (Input.GetAxis(continueButton) == 1)
                {
                    if (FindObjectOfType<DialogueRunner>().isDialogueRunning)
                    {
                        FindObjectOfType<ExtendedDialogueUI>().MarkLineComplete();
                    }
                }
            }
        }

        [YarnCommand("walkToPlayer")]
        public void walkToPlayer() {
                Debug.Log("walkingToPlayer");
                isWalking = true;
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", true);
                agent.SetDestination(waypoint_InFrontOfPlayer.transform.position);
                currWaypoint = "InFrontOfPlayer";
        }

        [YarnCommand("talkToPlayer")]
        public void talkToPlayer() {
            Debug.Log("talkingToPlayer");
            isWalking = false;
            anim.SetBool("isTalking", true);
            anim.SetBool("isWalking", false);
        }

        public void startDialogue() {
            if (!FindObjectOfType<DialogueRunner>().isDialogueRunning)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue(this.gameObject.GetComponent<NPC>().talkToNode);
            }
        }

        public void WaitForMove(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForMove(onComplete));
        }

        private IEnumerator DoWaitForMove(System.Action onComplete)
        {
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                //Debug.Log($"Remaining distance: {agent.remainingDistance}; Path pending: {agent.pathPending}");
                //rotate towards player
                if (IsInMeleeRangeOf(player.transform)) {
                    Debug.Log($"Rotate towards player");
                    RotateTowards(player.transform);
                }

                yield return null;
            }
            onComplete();
        }

        private bool IsInMeleeRangeOf (Transform target) {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance < meleeRange;
        }

        private void RotateTowards (Transform target) {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        [YarnCommand("loadNextLevel")]
        public void loadNextLevel() {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        IEnumerator LoadLevel(int levelIndex) {
            //play animation
            sceneAnimator.SetTrigger("Start");
            //wait
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(levelIndex + 1);
        }
    }

}

