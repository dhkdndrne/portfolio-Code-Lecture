using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float orthographSize;
    private float targetOrthographSize;

    private void Start()
    {
        //시네머신 렌즈의 orthograpghSize로 설정
        orthographSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographSize = orthographSize;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //방향벡터 구하기
        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthographSize += -Input.mouseScrollDelta.y * zoomAmount;  //targetOrthographSize를 마우스 휠 * 줌양

        float minOrthographicSize = 10; //최소 크기
        float maxOrthographicSize = 30; //최대 크기

        // targetOrthographSize의 사이즈를 최소와 최대 사이값으로 조정
        targetOrthographSize = Mathf.Clamp(targetOrthographSize, minOrthographicSize, maxOrthographicSize);


        //줌 되는 속도 부드럽게
        float zoomSpeed = 5f;
        orthographSize = Mathf.Lerp(orthographSize, targetOrthographSize, zoomSpeed * Time.deltaTime);

        //시네머신 렌즈의 orthographSize 를 변경
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographSize;
    }
}
