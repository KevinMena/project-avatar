using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.UI
{
    public class HealthBar : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private MaterialPropertyBlock _matProp;
        private Camera _mainCamera;
        private Health _health;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _health = GetComponentInParent<Health>();
            _matProp = new MaterialPropertyBlock();
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if(_health.Current < _health.Maximum)
            {
                _meshRenderer.enabled = true;
                AlignCamera();
                UpdateGraphic();
            }
            else
                _meshRenderer.enabled = false;
        }

        private void UpdateGraphic()
        {
            // Get the property of the shader and change it
            _meshRenderer.GetPropertyBlock(_matProp);
            _matProp.SetFloat("_Fill", _health.Current / _health.Maximum);
            _meshRenderer.SetPropertyBlock(_matProp);
        }

        private void AlignCamera()
        {
            if(_mainCamera is not null)
            {
                Vector3 forward = transform.position - _mainCamera.transform.position;
                forward.Normalize();
                Vector3 up = Vector3.Cross(forward, _mainCamera.transform.right);
                transform.rotation = Quaternion.LookRotation(forward, up);
            }
        }
    }
}
