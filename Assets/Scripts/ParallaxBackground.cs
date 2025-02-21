using Unity.Cinemachine;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float _startPosX, _startPosY, _lengthX, _lengthY;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffectX; // The speed at which the background should move relative to the camera in the x-axis
    [SerializeField] private float parallaxEffectY; // The speed at which the background should move relative to the camera in the y-axis
    [SerializeField] private bool parallaxEnabledX;
    [SerializeField] private bool parallaxEnabledY;
    [SerializeField] private float offSet = 1;
    void Start()
    {
        _startPosX = transform.position.x;
        _startPosY = transform.position.y;
        _lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        _lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
        if (parallaxEnabledX)
        {
            // Calculate the distance the background moves based on the camera movement
            float distanceX = cam.transform.position.x * parallaxEffectX;
            float movementX = cam.transform.position.x * (1 - parallaxEffectX);

            // If the background has reached the end of its length, adjust its position for infinite scrolling
            if (movementX > _startPosX + _lengthX)
            {
                _startPosX += _lengthX;
            }
            else if (movementX < _startPosX - _lengthX)
            {
                _startPosX -= _lengthX;
            }
            transform.position = new Vector3(_startPosX + distanceX, offSet, transform.position.z);
        }

        if (parallaxEnabledY)
        {
            // Calculate the distance the background moves based on the camera movement
            float distanceY = cam.transform.position.y * parallaxEffectY;
            float movementY = cam.transform.position.y * (1 - parallaxEffectY);

            // If the background has reached the end of its length, adjust its position for infinite scrolling
            if (movementY > _startPosY + _lengthY)
            {
                _startPosY += _lengthY;
            }
            else if (movementY < _startPosY - _lengthY)
            {
                _startPosY -= _lengthY;
            }

            transform.position = new Vector3(transform.position.x, (_startPosY + distanceY) + offSet, transform.position.z);
        }

    }
}
