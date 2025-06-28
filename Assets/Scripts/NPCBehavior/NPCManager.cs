using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace NPCBehavior
{
    public class NPCManager : MonoBehaviour
    {
        private static NPCManager _instance;
        public static NPCManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<NPCManager>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("NPCManager");
                        _instance = obj.AddComponent<NPCManager>();
                    }
                }
                return _instance;
            }
        }

        private ConcurrentDictionary<int, NpcController> npcControllers = new ConcurrentDictionary<int, NpcController>();
        private int npcIdCounter = 0;

        [Header("Spawner Settings")]
        public GameObject npcPrefab; // NPC prefab
        public Transform spawnPoint; // Spawn location
        public float spawnInterval = 3.0f; // Time interval between spawns
        public int maxNPCCount = 20; // Maximum number of NPCs

        private int currentNPCCount = 0;

        private void Start()
        {
            if (npcPrefab != null && spawnPoint != null)
            {
                StartCoroutine(SpawnNPCs());
            }
        }

        private IEnumerator SpawnNPCs()
        {
            while (true)
            {
                if (currentNPCCount < maxNPCCount)
                {
                    SpawnNPC();
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnNPC()
        {
            GameObject npcObject = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            NpcController npcController = npcObject.GetComponent<NpcController>();
            
            NPCType type = (NPCType)Random.Range(0, System.Enum.GetValues(typeof(NPCType)).Length);
            NPCStatus status = NPCStatus.Walking;
            
            // TODO: 从JSON读对应的数据生成当前的FearList
            List<NPCFearType> allFears = new List<NPCFearType>
            {
                NPCFearType.Painting,
                NPCFearType.Box,
                NPCFearType.Chair,
                NPCFearType.Lamp,
                NPCFearType.Book
            };

            NPCFearType randomFear = allFears[Random.Range(0, allFears.Count)];

            int defaultHealth = 5;
            
            npcController.Initialize(type, defaultHealth, status, randomFear); // Initialize the NPC controller
            
            if (npcController != null)
            {
                int npcId = npcIdCounter++;
                RegisterNPC(npcId, npcController);
                currentNPCCount++;
            }
        }

        public void RegisterNPC(int id, NpcController npcController)
        {
            npcControllers[id] = npcController;
        }

        public void UnregisterNPC(int id)
        {
            if (npcControllers.TryRemove(id, out _))
            {
                currentNPCCount--;
            }
        }

        public NpcController GetNPC(int id)
        {
            npcControllers.TryGetValue(id, out var npcController);
            return npcController;
        }
    }
}