using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour 
{
    [Serializable]
    public class SpawnElement
    {
        [field: SerializeField] public float SpawnDelay { get; private set; }
        [field: SerializeField] public BubbleController BubbleController { get; private set; }
    }

    [Header("General")]
    [SerializeField] private List<SpawnElement> spawnElements = new();
    [SerializeField] private List<BubbleSpawner> linkedSpawners = new();

    [Header("Spline")]
    [SerializeField] private BubbleController.Config config;

    private readonly List<BubbleController> activeBubbleControllers = new();

    public bool AreAllBubblesDestroyed => activeBubbleControllers.Count <= 0;

    private IEnumerator Start() 
    {
        yield return ParticleManager.WaitUntillExists;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            for (int i = 0; i < spawnElements.Count; i++)
            {
                yield return new WaitForSeconds(spawnElements[i].SpawnDelay);
                SpawnBubble(spawnElements[i].BubbleController);
            }
            
            yield return new WaitUntil(() => AreAllBubblesDestroyed && linkedSpawners.All(x => x.AreAllBubblesDestroyed));
        }
    }

    private void DestroyBubble(BubbleController bubble)
    {
        activeBubbleControllers.Remove(bubble);
        Destroy(bubble.gameObject);
    }

    private void SpawnBubble(BubbleController bubble)
    {
        BubbleController spawnedBubble = Instantiate(bubble, transform.position, Quaternion.identity);
        spawnedBubble.Setup(config);
        spawnedBubble.OnDestroyed += DestroyBubble;
        activeBubbleControllers.Add(spawnedBubble);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.6f);
    }
}