using System.Collections.Generic;
using UnityEngine;

namespace SkeletonEditor
{

    public class PlayerController : MonoBehaviour
    {
        public float mouseRotateSpeed = 0.3f;

        private Animator animator;
        private Quaternion initRotation;


        private int currentAnimation;
        public List<string> animations;
       

        private bool startMouseRotate;
        private Vector3 prevMousePosition;

        public static PlayerController Instance { get; private set; }

        void Awake() {
            if (Instance != null) {
                Destroy(this.gameObject);
            }
            Instance = this;
        }

        void Start() {
            animator = GetComponent<Animator>();
            initRotation = transform.rotation;

          
            animations = new List<string>()
            {
                "Attack",
                "Active",
                "Passive"   
            };
        }

        void Update() {

            if (Input.GetMouseButtonDown(1)) {
                startMouseRotate = true;
                prevMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1)) {
                startMouseRotate = false;
            }
            if (Input.GetMouseButton(1)) {
                transform.Rotate(new Vector3(0, (Input.mousePosition.x - prevMousePosition.x) * mouseRotateSpeed, 0));
                prevMousePosition = Input.mousePosition;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetTrigger("Attack");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("Active");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                animator.SetTrigger("Passive");
            }
            

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Mathf.Abs(h) > 0.001f)
                v = 0;


            if (!startMouseRotate) {
                if (h > 0.5f) {
                    transform.rotation = Quaternion.Euler(initRotation.eulerAngles + new Vector3(0, -90, 0));
                }
                if (h < -0.5f) {
                    transform.rotation = Quaternion.Euler(initRotation.eulerAngles + new Vector3(0, 90, 0));
                }
                if (v > 0.5f) {
                    transform.rotation = Quaternion.Euler(initRotation.eulerAngles + new Vector3(0, -180, 0));
                }
                if (v < -0.5f) {
                    transform.rotation = Quaternion.Euler(initRotation.eulerAngles);
                }
            }

            var speed = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            animator.SetFloat("speedv", speed);
        }
    }
}