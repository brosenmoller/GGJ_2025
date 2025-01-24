using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("General")]
    [SerializeField] private float bubbleSpeed;
    [SerializeField] private BubbleController bubbleController;
    [SerializeField] private float timeBetweeSpawns;

    private void Awake() {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine() {
        while (true) {
            yield return new WaitForSeconds(timeBetweeSpawns);
            BubbleController spawnedBubble = Instantiate(bubbleController, transform.position, Quaternion.identity);
            spawnedBubble.Setup(bubbleSpeed, transform.forward);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * bubbleSpeed);
    }
}