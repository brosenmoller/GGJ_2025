using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("General")]
    [SerializeField] private bool useInitialSpawnDelay;
    [SerializeField] private float initialSpawnDelay;
    [SerializeField] private List<SpawnElement> spawnElements = new();
    [SerializeField] private List<BubbleSpawner> linkedSpawners = new();

    [Header("Spline")]
    [SerializeField] private BubbleController.Config config;

    private readonly List<BubbleInstance> instances = new();
    private bool isFirstSpawn;

    private void OnEnable()
    {
        isFirstSpawn = true;
    }

    private IEnumerator Start() 
    {
        yield return ParticleManager.WaitUntillExists;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        if (spawnElements.Count <= 0) { yield break; }

        bool areAllBubblesDestroyed = true;
        while (true)
        {
            for (int i = 0; i < spawnElements.Count; i++)
            {
                SpawnElement spawnElement = spawnElements[i];
                float delay = spawnElement.SpawnDelay;
                if (isFirstSpawn && useInitialSpawnDelay) { delay = initialSpawnDelay; }

                yield return new WaitForSeconds(delay);

                if (spawnElement.CanSpawnWhenActive || areAllBubblesDestroyed)
                {
                    BubbleInstance instance = new(spawnElement, config);
                    instance.SpawnBubble();
                    instance.OnDestroy += () => instances.Remove(instance);
                    instances.Add(instance);
                }

                isFirstSpawn = false;
            }

            areAllBubblesDestroyed = AreAllBubblesDestroyed();
        }
    }

    private bool AreAllBubblesDestroyed(bool recursive = false)
    {
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i].spawnElement.CanSpawnWhenActive) { continue; }
            if (instances[i].IsActive) { return false; }
        }

        if (recursive) { return true; }

        for (int i = 0; i < linkedSpawners.Count; i++)
        {
            if (!linkedSpawners[i].AreAllBubblesDestroyed(true)) { return false; }
        }

        return true;
    }

    [Serializable]
    public class SpawnElement
    {
        [field: SerializeField] public float SpawnDelay { get; private set; }
        [field: SerializeField] public BubbleController BubbleController { get; private set; }
        [field: SerializeField] public bool CanSpawnWhenActive { get; private set; }
    }

    public class BubbleInstance
    {
        public bool IsActive => instance != null;
        
        private BubbleController instance;

        public event Action OnDestroy;

        public readonly SpawnElement spawnElement;
        public readonly BubbleController.Config config;

        public BubbleInstance(SpawnElement spawnElement, BubbleController.Config config)
        {
            this.spawnElement = spawnElement;
            this.config = config;
        }

        public void SpawnBubble()
        {
            DestroyBubble();
            instance = Instantiate(spawnElement.BubbleController, config.Spline.EvaluatePosition(0), Quaternion.identity);
            instance.Setup(config);
            instance.OnDestroyed += DestroyBubble;
        }

        private void DestroyBubble()
        {
            if (IsActive)
            {
                OnDestroy?.Invoke();   
            }
        }
    }
}