using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject midPointVisual, arrowPrefab, arrowSpawnPoint;
    [SerializeField] private float arrowMaxSpeed = 10;
    [SerializeField] private float searchRadius = 0.1f;

    [SerializeField] private Vector3 rotationFix = new Vector3(0, 0, 0);

    private bool arrowActivated = false;

    public void PrepareArrow()
    {
        if (arrowActivated)
        {
            midPointVisual.SetActive(true);
            return;
        }

        Collider[] nearbyObjects = Physics.OverlapSphere(midPointVisual.transform.position, searchRadius);

        foreach (var col in nearbyObjects)
        {
            if (col.CompareTag("Arrow"))
            {
                Debug.Log($"✅ СТРЕЛА НАЙДЕНА");
                arrowActivated = true;
                Destroy(col.gameObject);
                midPointVisual.SetActive(true);
                return;
            }
        }

        midPointVisual.SetActive(false);
    }

    public void ReleaseArrow(float strength)
    {
        midPointVisual.SetActive(false);

        if (arrowActivated)
        {
            Quaternion fixedRotation = arrowSpawnPoint.transform.rotation * Quaternion.Euler(rotationFix);

            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, fixedRotation);

            Rigidbody rb = arrow.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Летим по forward стрелы
                rb.AddForce(arrow.transform.forward * strength * arrowMaxSpeed, ForceMode.Impulse);

                Debug.Log("=== ВЫСТРЕЛ ===");
                Debug.Log($"Rotation: {arrow.transform.rotation.eulerAngles}");
                Debug.Log($"Forward: {arrow.transform.forward}");
            }
        }

        arrowActivated = false;
    }

    void OnDrawGizmos()
    {
        if (arrowSpawnPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.forward * 2f);
        }
    }
}
