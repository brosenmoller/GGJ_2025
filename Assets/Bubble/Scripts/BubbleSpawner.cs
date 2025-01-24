using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("General")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private float bubbleSpeed;
    [SerializeField] private BubbleController bubbleController;
    [SerializeField] private List<BubbleSpawner> linkedSpawners = new();
    [SerializeField] private SplineContainer spline;

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
        spawnedBubble.Setup(bubbleSpeed, transform.forward, spline.Spline);
        spawnedBubble.OnDestroyed += DestroyBubble;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * (bubbleSpeed * Time.deltaTime * 0.01f));
    }
}