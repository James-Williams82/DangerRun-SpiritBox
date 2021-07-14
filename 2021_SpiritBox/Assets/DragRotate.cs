using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float rotationSpeed;
    public float rotationDamping;

    private float _rotationVelocity;
    private bool _dragged;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragged = true;

        Debug.Log("dragging");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rotationVelocity = eventData.delta.x * rotationSpeed;
        transform.Rotate(Vector3.back, -_rotationVelocity, Space.Self);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragged = false;
    }

    private void Update()
    {
        if (!_dragged && !Mathf.Approximately(_rotationVelocity, 0))
        {
            float deltaVelocity = Mathf.Min(
                Mathf.Sign(_rotationVelocity) * Time.deltaTime * rotationDamping,
                Mathf.Sign(_rotationVelocity) * _rotationVelocity
            );
            _rotationVelocity -= deltaVelocity;
            transform.Rotate(Vector3.back, -_rotationVelocity, Space.Self);
        }
    }
}