using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject arrowPrefab; // Префаб стрелы

    private XRGrabInteractable grabInteractable;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Запоминаем исходную позицию
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Подписываемся на событие захвата
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        // Создаём новую стрелу НА ТОМ ЖЕ МЕСТЕ
        GameObject newArrow = Instantiate(arrowPrefab, originalPosition, originalRotation);

        // Копируем компонент ArrowSpawner на новую стрелу
        ArrowSpawner spawner = newArrow.GetComponent<ArrowSpawner>();
        if (spawner == null)
        {
            spawner = newArrow.AddComponent<ArrowSpawner>();
            spawner.arrowPrefab = arrowPrefab;
        }

        // Включаем гравитацию
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        Debug.Log("Взята стрела, создана новая на том же месте");

        // Текущая стрела становится обычной (без спавнера)
        // Удаляем скрипт со взятой стрелы
        Destroy(this);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }
}
