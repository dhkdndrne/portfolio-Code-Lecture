using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraPanZoom : MonoBehaviour
{
    Vector3 firstTouch;
    Camera myCamera;
    bool isZoom;

    [SerializeField] float zoomOutMin;
    [SerializeField] float zoomOutMax;
    [SerializeField] Tilemap myTileMap;
    [SerializeField] TilemapRenderer tileMapRenderer;
    float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Start()
    {
        myCamera = GetComponent<Camera>();

        tileMapRenderer = GameObject.Find("BG").GetComponent<TilemapRenderer>();
        myTileMap = GameObject.Find("BG").GetComponent<Tilemap>();
        myTileMap.CompressBounds();

        mapMinX = tileMapRenderer.bounds.min.x;
        mapMaxX = tileMapRenderer.bounds.max.x;

        mapMinY = tileMapRenderer.bounds.min.y;
        mapMaxY = tileMapRenderer.bounds.max.y;
    }

    private void LateUpdate()
    {
        TouchEvent();
    }

    void TouchEvent()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!EventSystem.current.currentSelectedGameObject && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !GameManager.Instance.magicController.IsMagicSelected)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    firstTouch = myCamera.ScreenToWorldPoint(Input.mousePosition);
                    isZoom = false;
                }

                if (Input.touchCount.Equals(2))
                {
                    isZoom = true;
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    // deltaposition은 전 프레임에서의 터치 위치와 이번 프레임에서 터치위치의 차이
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // magnitude = 벡터의 길이 반환
                    float preMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - preMagnitude;

                    Zoom(difference * 0.1f);
                }
                else if (touch.phase == TouchPhase.Moved && !isZoom)
                {
                    Vector3 direction = firstTouch - myCamera.ScreenToWorldPoint(Input.mousePosition);
                    myCamera.transform.position = ClampCamera(myCamera.transform.position + direction);

                }
            }
        }
#endif

#if UNITY_EDITOR

        if (!EventSystem.current.currentSelectedGameObject && !EventSystem.current.IsPointerOverGameObject()&& !GameManager.Instance.magicController.IsMagicSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstTouch = myCamera.ScreenToWorldPoint(Input.mousePosition);
                isZoom = false;
            }

            if (Input.touchCount.Equals(2))
            {
                isZoom = true;
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // deltaposition은 전 프레임에서의 터치 위치와 이번 프레임에서 터치위치의 차이
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // magnitude = 벡터의 길이 반환
                float preMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - preMagnitude;

                Zoom(difference * 0.1f);
            }
            else if (Input.GetMouseButton(0) && !isZoom)
            {
                Vector3 direction = firstTouch - myCamera.ScreenToWorldPoint(Input.mousePosition);
                myCamera.transform.position = ClampCamera(myCamera.transform.position + direction);

            }
        }
#endif
    }

    Vector3 ClampCamera(Vector3 _TargetPos)
    {
        float camHeight = myCamera.orthographicSize;
        float camWidth = myCamera.orthographicSize * myCamera.aspect;

        float minX = mapMinX + camWidth + .005f;
        float maxX = mapMaxX - camWidth - .005f;
        float minY = mapMinY + camHeight + .005f;
        float maxY = mapMaxY - camHeight - .005f;

        float newX = Mathf.Clamp(_TargetPos.x, minX, maxX);
        float newY = Mathf.Clamp(_TargetPos.y, minY, maxY);

        //return new Vector3(newX, newY, _TargetPos.z);
        return new Vector3(newX, newY, -1);
    }

    void Zoom(float _Increment)
    {
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - _Increment, zoomOutMin, zoomOutMax);
        myCamera.transform.position = ClampCamera(myCamera.transform.position);
    }
}
