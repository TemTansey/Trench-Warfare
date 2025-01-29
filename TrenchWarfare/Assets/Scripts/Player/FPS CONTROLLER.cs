using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.Video;
[RequireComponent(typeof(CharacterController))]

public class FPSCONTROLLER : MonoBehaviour
{
    #region Player Variables

    [Header("Player Variables")]
    [SerializeField] private GameObject playerP;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 7.0f;
    [SerializeField] private float jumpPower = 7.0f;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 45.0f;
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] float rotationX = 0f;
    Vector3 moveDirection = Vector3.zero;
    [SerializeField] private bool canMove = true;

    [Header("General Variables")]
    CharacterController characterController;
    [SerializeField] private bool isCrouching;

    [Header("Camera Bobbing variables")]
    [SerializeField] private float bobbingSpeed = 5.0f;
    [SerializeField] private float bobbingAmount = 0.05f;
    private float bobbingTimer = 0.0f;
    private Vector3 originalCameraPosition;

    [Header("Black And White Camera variables")]
    [SerializeField] private bool isBlackAndWhiteCamera;
    [SerializeField] private GameObject blackAndWhiteFilter;
    [SerializeField] private GameObject videoPlayerHolder;


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        originalCameraPosition = playerCamera.transform.localPosition;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        

        Time.timeScale = 1f;
    }



    // Update is called once per frame
    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0.0f;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0.0f;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
        #endregion

        #region Handles Mouse Movement


        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        #region Handles Crouching

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerP.transform.localScale = new Vector3(1, 0.5f, 1);
            walkSpeed = 1.75f;
            isCrouching = true;
        }



        if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            playerP.transform.localScale = new Vector3(1, 1, 1);
            walkSpeed = 3.0f;

            isCrouching = false;

            
        }


        #endregion

        #region Camera Bobbing


        if (characterController.isGrounded && (curSpeedX != 0 || curSpeedY != 0))
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * bobbingAmount;
            playerCamera.transform.localPosition = originalCameraPosition + new Vector3(0, bobbingOffset, 0);
        }
        else
        {
            // Reset bobbing when not moving
            bobbingTimer = 0.0f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalCameraPosition, Time.deltaTime * bobbingSpeed);
        }



        #endregion

        #region Black and White Camera Filter

        if (Input.GetKeyDown(KeyCode.F) && isBlackAndWhiteCamera == false)
        {
            isBlackAndWhiteCamera = true;
            blackAndWhiteFilter.SetActive(true);
            videoPlayerHolder.SetActive(true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.F) && isBlackAndWhiteCamera == true)
        {
            isBlackAndWhiteCamera = false;
            blackAndWhiteFilter.SetActive(false);
            videoPlayerHolder.SetActive(false);
            return;
        }



        #endregion

    }
}

