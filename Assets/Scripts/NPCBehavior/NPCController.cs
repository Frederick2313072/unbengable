using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

// TODO：添加NPC animator的状态机
// TODO：添加音效触发

namespace NPCBehavior
{
    public enum NPCType
    {
        Normal, // Normal NPC
        Thief, // Thief NPC
    }

    public enum NPCStatus
    {
        Walking, // NPC is idle
        Shocked, // NPC is shocked
        Dead, // NPC is dead
    }
    
    public enum NPCFearType
    {
        Painting, 
        Box,
        Chair,
        Lamp,
        Book,
        Telephone
    }

    public class NpcController : MonoBehaviour
    {
        // NPC属性
        public NPCType npcType; // Type of the NPC
        public NPCStatus npcStatus; // Current status of the NPC
        public NPCFearType npcFearType; // Fear type of the NPC
        
        public int npcDefaultHealth = 5;
        public int npcCurrentHealth = 5; // Current health of the NPC

        public Animator npcAnimator; // Animator component for NPC animations
        
        public MusicManager musicManager; // Reference to the MusicManager for sound effects
        
        private ObjectBase _targetObject;
        
        // 步行所需代码
        public SplineContainer splineContainer; // Reference to the SplineController

        private float _currentSpeed = 0.1f; // Speed of movement along the spline

        public float walkingSpeed = 0.02f; // Speed of the NPC along the spline
        public float runningSpeed = 0.1f;

        public string splineGameObjectName = "Spline"; // Name of the GameObject containing the spline
        public string musicManagerName = "MusicManager"; // Name of the GameObject containing the MusicManager
        
        private float _progress = 0.0f; // Current progress along the spline (0 to 1)s
        
        public void Initialize(NPCType type, int defaultHealth, NPCStatus status, NPCFearType fearType)
        {
            // Initialize NPC status
            npcType = type;
            npcDefaultHealth = defaultHealth;
            npcStatus = status;
            npcFearType = fearType;
            
            // Initialize NPC health
            npcCurrentHealth = npcDefaultHealth;

            if (npcAnimator == null)
            {
                npcAnimator = GetComponent<Animator>();
            }

            if (splineContainer == null)
            {
                GameObject splineTransform = GameObject.Find(splineGameObjectName);
                if (splineTransform != null)
                {
                    splineContainer = splineTransform.GetComponent<SplineContainer>();
                }

                if (splineContainer == null)
                {
                    Debug.LogError("SplineContainer not found under the child GameObject named 'Spline'.");
                }
            }
            
            if (musicManager == null)
            {
                GameObject musicTransform = GameObject.Find(musicManagerName);
                if (musicTransform != null)
                {
                    musicManager = musicTransform.GetComponent<MusicManager>();
                }

                if (musicManager == null)
                {
                    Debug.LogError("musicManager not found under the child GameObject named 'MusicManager'.");
                }
            }
            
            musicManager.Init(); // Get the MusicManager instance
        }
        
        

        // Update is called once per frame
        void Update()
        {
            WalkingAlongSpline();
        }


        // 物体出发NPC的行为：被吓到；由ObjectBase的Trigger触发
        public void TriggerShock(float shockDuration, NPCFearType currentObjectFearType)
        {
            if (npcStatus != NPCStatus.Shocked)
            {
                npcStatus = NPCStatus.Shocked;
                StartCoroutine(HandleShock(shockDuration, currentObjectFearType));
            }
        }

        private IEnumerator HandleShock(float duration, NPCFearType currentObjectFearType)
        {
            // Reduce speed
            _currentSpeed = runningSpeed;
            Debug.Log("NPC is shocked!");
            
            if(npcFearType == currentObjectFearType)
            {
                Debug.Log("NPC is shocked by the same fear type: " + npcFearType);
                npcStatus = NPCStatus.Dead;
                Debug.Log("NPC is dead.");
                TriggerDeath();
                yield break; // Exit if NPC is dead
            }
            
            npcCurrentHealth -= 1; // Reduce health when shocked

            if (npcCurrentHealth <= 0)
            {
                npcStatus = NPCStatus.Dead;
                Debug.Log("NPC is dead.");
                TriggerDeath();
                yield break; // Exit if NPC is dead
            }

            npcAnimator.SetTrigger("Running"); // Trigger death animation
            musicManager.PlayRandomHumanHorror(); // Play horror sound effect
            
            // Wait for the shock duration
            yield return new WaitForSeconds(duration);

            // Restore speed and status
            _currentSpeed = walkingSpeed;
            npcStatus = NPCStatus.Walking;
            npcAnimator.SetTrigger("Walking"); // Trigger death animation

            Debug.Log("NPC recovered from shock.");
        }

        public void TriggerDeath()
        {
            if (npcStatus == NPCStatus.Dead)
            {
                npcStatus = NPCStatus.Dead;
                StartCoroutine(HandleDeath());
            }
        }

        private IEnumerator HandleDeath()
        {
            // Wait for the death animation to finish
            npcAnimator.SetBool("isDead", true);
            _currentSpeed = 0;
            musicManager.PlayRandomHumanDead(); // Play horror sound effect

            yield return new WaitForSeconds(1.0f); // Adjust this duration based on your animation length
            
            // Destroy the NPC GameObject
            Destroy(gameObject);
        }

        // NPC沿着样条曲线行走
        void WalkingAlongSpline()
        {
            if (splineContainer == null || splineContainer.Spline == null) return;

            _progress += _currentSpeed * Time.deltaTime; // Update progress based on speed and time
            if (_progress > 1.0f) _progress = 0.0f;


            Vector3 position = splineContainer.Spline.EvaluatePosition(_progress); // Get position on the spline
            Vector3 tangent =
                splineContainer.Spline.EvaluateTangent(_progress); // Update the GameObject's tangent for orientation

            transform.position = position; // Update the GameObject's position

            // Update rotation to align with the tangent
            if (tangent != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(tangent);
            }
        }
    }
}