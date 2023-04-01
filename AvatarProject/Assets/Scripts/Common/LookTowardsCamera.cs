using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Common
{
    public class LookTowardsCamera : MonoBehaviour
    {
        private Camera main;

        void Start()
        {
            main = Camera.main;
        }

        void LateUpdate()
        {
            transform.LookAt(transform.position + main.transform.rotation * Vector3.forward, main.transform.rotation * Vector3.up);

        }
    }
}
