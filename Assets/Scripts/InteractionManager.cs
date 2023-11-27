using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.04f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    private GameObject currentInteractGameObject;
    private IInteractable currentInteractable;
    public TextMeshProUGUI prompText;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if(hit.collider.gameObject != currentInteractGameObject)
                {
                    currentInteractGameObject = hit.collider.gameObject;
                    currentInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPrompText();
                }
            }else
            {
                currentInteractGameObject = null;
                currentInteractable = null;
                prompText.gameObject.SetActive(false);
            }
        }
    }

    void SetPrompText()
    {
        prompText.gameObject.SetActive(true);
        prompText.text = string.Format("<b>[E]</b> {0}", currentInteractable.GetInteractPromp());
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && currentInteractable != null)
        {
            currentInteractable.OnInteract();
            currentInteractGameObject = null;
            currentInteractable = null;
            prompText.gameObject.SetActive(false);
        }
        
    }
}

public interface IInteractable
{
    string GetInteractPromp();
    void OnInteract();
}
