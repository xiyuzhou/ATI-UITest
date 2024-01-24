using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MainMenuUiManager : MonoBehaviour
{
    public Transform triggerPoint;
    private int movingState = 0;

    private GameObject currentSelection;
    private GameObject movingObject;

    public float moveToObjectSpeed = 2f;  // Speed to move towards the trigger point
    public float angularSpeed = 1;

    public int CamState = 0;
    public Camera cam;
    public Animator CamAnimator;
    public bool onCamAnimation = false;

    public Transform EndPoint;
    void Start()
    {

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the mouse click hits the object
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("MenuBook"))
        {
            currentSelection = hit.collider.gameObject;
        }
        else
        {
            currentSelection = null;
        }

        if (Input.GetMouseButtonDown(0) && movingState == 0 && currentSelection != null)
        {
            //currentSelection.GetComponent<Rigidbody>().isKinematic = true;
            currentSelection.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            movingObject = currentSelection;
            movingState = 1;
            CamState = 1;
            onCamAnimation = true;
        }

        switch (movingState)
        {
            case 0:
                break;
            case 1:
                MoveObjectToFinalPoint(movingObject);
                break;
            case 2:
                MoveObjectToEndPoint(movingObject);
                break;
            case 3:
                // the book opened
                break;
        }

        switch (CamState)
        {
            case 0:
                break;
            case 1:
                if (!onCamAnimation)
                {
                    CamState = 3; break;
                }
                CamAnimator.SetInteger("CamStateChange", 1);
                break;
            case 2:
                CamAnimator.SetInteger("CamStateChange", 2);
                cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 30, 20 * Time.deltaTime);
                break;
            case 3:
                float targetFov = movingObject.GetComponent<MenuBookInfo>().camFovAdjust;
                cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFov, 100 * Time.deltaTime);
                break;

        }


    }
    void MoveObjectToFinalPoint(GameObject BookObject)
    {
        Rigidbody bookRigidbody = BookObject.GetComponent<Rigidbody>();
        Vector3 targetPosition = new Vector3(triggerPoint.position.x, BookObject.transform.position.y, triggerPoint.position.z);
        Vector3 newPosition = Vector3.MoveTowards(BookObject.transform.position, targetPosition, Time.deltaTime * moveToObjectSpeed);
        bookRigidbody.MovePosition(newPosition);

        Quaternion targetRotation = Quaternion.Euler(0f, triggerPoint.rotation.eulerAngles.y, 0f);

        bookRigidbody.MoveRotation(Quaternion.Lerp(BookObject.transform.rotation, targetRotation, Time.deltaTime * angularSpeed));
        
        if (Vector2.Distance(new Vector2(newPosition.x, newPosition.z), new Vector2(targetPosition.x, targetPosition.z)) < 0.01f)
        {
            movingState = 3;
            Debug.Log("Finish");
            // Trigger open book animation here
            bookRigidbody.isKinematic = true;
        }
        
    }
    void MoveObjectToEndPoint(GameObject BookObject)
    {
        //Trigger close book animation here
        BookObject.transform.position = Vector3.MoveTowards(BookObject.transform.position, EndPoint.position, 10 * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0f, EndPoint.rotation.eulerAngles.y, 0f);
        BookObject.transform.rotation = Quaternion.Lerp(BookObject.transform.rotation, targetRotation, Time.deltaTime * 16);

        if (Vector3.Distance(BookObject.transform.position,EndPoint.position) <= 0.01)
        {
            movingState = 0;
            var rb = BookObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            rb.isKinematic = false;
            BookObject.transform.rotation = EndPoint.rotation;
        }
    }

    void OnBookMenu(GameObject BookObject)
    {
        MenuBookInfo currentMenu = BookObject.GetComponent<MenuBookInfo>();
        var menuInfo = currentMenu.menuSelection;
    }

    public void OnBackMenu()
    {
        movingState = 2;
        CamState = 2;
    }
}
public enum MenuSelection
{
    HowToPlay,
    Settings,
    TheMakingOf,
    Play,
    NewOrLoad,
    ExitToDesktop,
}