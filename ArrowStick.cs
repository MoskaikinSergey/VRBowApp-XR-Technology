using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowStick : MonoBehaviour
{
    private XRGrabInteractable grab;
    private Transform hand;
    private bool isSticked = false;
    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        

        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrab);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (!isSticked)
        {
            isSticked = true;
            hand = args.interactorObject.transform;

            Debug.Log("Стрела взята");
        }
    }


    void Update()
    {
        if (isSticked && hand != null)
        {
            // Привязываем стрелу к руке (следует за рукой)
            transform.position = hand.position;
            transform.rotation = hand.rotation;

            // Проверяем - взял ли что-то другое этой же рукой?
            XRDirectInteractor interactor = hand.GetComponent<XRDirectInteractor>();

            if (interactor != null)
            {
                // Считаем сколько объектов держит рука
                int heldObjects = interactor.interactablesSelected.Count;

                // Если больше 1 (стрела + что-то ещё)
                if (heldObjects > 1)
                {
                    Debug.Log("Рука взяла тетиву - прячем стрелу из руки");

                    // Скрываем стрелу (она всё ещё существует но невидима)
                    Renderer[] renderers = GetComponentsInChildren<Renderer>();
                    foreach (var r in renderers)
                    {
                        r.enabled = false;
                    }

                    // Отключаем коллайдер
                    BoxCollider col = GetComponent<BoxCollider>();
                    if (col != null) col.enabled = false;
                }
            }
        }
    }

    void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrab);
        }
    }
}
