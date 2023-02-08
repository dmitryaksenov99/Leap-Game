using Mirror;
using System;
using UnityEngine;

namespace LeapGame
{
    public class PlayerMover : NetworkBehaviour
    {
        [SerializeField] CharacterController characterController;
        [SerializeField] Transform playerCamera;

        [SerializeField] float leapDistance = 5f;
        [SerializeField] float leapTime = 0.2f;
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float mouseSensitivity = 2f;
        [SerializeField] float minCameraAngle = -15;
        [SerializeField] float maxCameraAngle = 90f;

        private Vector3 oldPosition;
        private float velocity;
        private float xRotation;
        private float leapVertical, leapHorizontal;
        private float leapTimer;
        private bool isLeap;
        private bool canMove;

        public override void OnStartClient()
        {
            if (isLocalPlayer == false)
            {
                DisableCamera();
                return;
            }
        }

        private void FixedUpdate()
        {
            velocity = Vector3.Distance(transform.position, oldPosition) / Time.fixedDeltaTime;
            oldPosition = transform.position;
        }

        private void Update()
        {
            if (isLocalPlayer == false)
                return;

            if (canMove == false)
                return;

            if (Input.GetMouseButtonDown(0) || isLeap)
            {
                isLeap = true;

                leapTimer += Time.deltaTime;

                leapVertical = RoundValue(Input.GetAxis("Vertical"), 0);
                leapHorizontal = RoundValue(Input.GetAxis("Horizontal"), 0);

                float leapSpeed = leapDistance / leapTime;

                Vector3 movement = transform.forward * leapVertical + transform.right * leapHorizontal;
                characterController.Move(movement * leapSpeed * Time.deltaTime);

                if (leapTimer >= leapTime)
                {
                    leapTimer = 0f;
                    isLeap = false;
                }
            }
            else
            {
                float vertical = Input.GetAxis("Vertical");
                float horizontal = Input.GetAxis("Horizontal");

                Vector3 movement = transform.forward * vertical + transform.right * horizontal;
                characterController.Move(movement * moveSpeed * Time.deltaTime);

                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, minCameraAngle, maxCameraAngle);

                playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
                transform.Rotate(Vector3.up * mouseX * 3);
            }

            characterController.Move(Physics.gravity);
        }

        public void ResetTransform(Vector3 position, Quaternion rotation)
        {
            characterController.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            characterController.enabled = true;
        }

        private void DisableCamera()
        {
            playerCamera.gameObject.SetActive(false);
        }

        private float RoundValue(float value, int decimalpoint)
        {
            double result = Math.Round(value, decimalpoint);

            if (result < value)
            {
                result += Math.Pow(10, -decimalpoint);
            }

            return (float)result;
        }

        public float Velocity { get => velocity; }
        public bool IsLeap { get => isLeap; }
        public bool CanMove { get => canMove; set => canMove = value; }

        public CharacterController CharacterController { get => characterController; }
    }
}