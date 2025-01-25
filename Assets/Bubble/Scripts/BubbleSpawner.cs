using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("General")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private BubbleController bubbleController;
    [SerializeField] private List<BubbleSpawner> linkedSpawners = new();

    [Header("Spline")]
    [SerializeField] private BubbleController.Config config;

    public bool IsBubbleDestroyed { get; private set; } = true;
    private BubbleController spawnedBubble;

    private void Awake() 
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnBubble();
            yield return new WaitUntil(() => IsBubbleDestroyed && linkedSpawners.All(x => x.IsBubbleDestroyed));
        }
    }

    private void DestroyBubble()
    {
        IsBubbleDestroyed = true;
        Destroy(spawnedBubble.gameObject);
    }

    private void SpawnBubble()
    {
        IsBubbleDestroyed = false;
        spawnedBubble = Instantiate(bubbleController, transform.position, Quaternion.identity);
        spawnedBubble.Setup(config);
        spawnedBubble.OnDestroyed += DestroyBubble;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.6f);
    }
}