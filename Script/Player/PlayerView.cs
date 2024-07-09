using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

//컷씬 테스트 코드
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour
{
    //스크립트 참조
    private InventoryUI inventoryUI;
    private BaseCamp baseCampUI;
    private DwarfUI dwarfUI;
    private UIController uiController;
    private DialogueManager dialogueManager;

    public Transform cameraObj; //카메라 오브젝트
    public Transform characterBody; //플레이어 몸

    //컷씬 적용시키기 위한 함수
    #region
    public Camera _camera;
    public float time = 0;
    #endregion

    //카메라 offset
    [SerializeField]
    private float camera_dist = 0f;
    private float camera_width = -5.5f;
    private float camera_height = 1.8f;
    private float camera_fix = 0.5f;

    Vector3 dir;

    RaycastHit hit;

    private void Awake()
    {
        cameraObj = transform;

        dialogueManager = FindObjectOfType<DialogueManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        baseCampUI = FindObjectOfType<BaseCamp>();
        dwarfUI = FindObjectOfType<DwarfUI>();
        uiController = FindObjectOfType<UIController>();
        characterBody = FindObjectOfType<Player>().transform;
        characterBody.GetComponent<Player>().playerView = this;
    }
    void Start()
    {
        //플레이어에서 카메라까지의 거리
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);
        //플레이어에서 카메라 위치까지의 방향벡터
        dir = new Vector3(0, camera_height, camera_width).normalized;
    }

    /// <summary>
    /// 컷씬 적용시키기 위한 함수
    /// </summary>
    void Update()
    {
        if (!_camera.enabled)
        {
            time += Time.deltaTime;

            if (time > 25.0f || Input.GetKeyDown(KeyCode.T))
                _camera.enabled = true;     
        }
    }

    void LateUpdate()
    {
        CameraControl();
        CameraPosition();
    }
    //카메라의 오브젝트 충돌 위치 조정
    private void CameraPosition()
    {
        //카메라 위치
        Vector3 ray_target = characterBody.up * camera_height + cameraObj.forward * camera_width;
        //카메라에서 캐릭터로 레이 발사위치
        Physics.Raycast(new Vector3(characterBody.position.x, characterBody.position.y + 1.2f, characterBody.position.z), ray_target, out hit, camera_dist);
        //Debug.DrawRay(new Vector3(characterBody.position.x, characterBody.position.y + 1.2f, characterBody.position.z), ray_target, Color.green);

        if (hit.point != Vector3.zero && hit.collider.gameObject != characterBody.gameObject) //벽과 카메라가 충돌할 때
        {
            if (hit.collider.gameObject != characterBody.gameObject)
            {
                cameraObj.position = hit.point; //오브젝트와 충돌한 위치로 이동
                //카메라 보정
                cameraObj.Translate(dir * -1 * camera_fix);
            }
        }
        else //벽과 충돌하지 않을 때
        {
            cameraObj.position = characterBody.position; //플레이어와 일정간격 유지
            cameraObj.Translate(dir * camera_dist); //이 위치로 이동
        }
    }
    //카메라 회전
    private void CameraControl()
    {
        //카메라 회전
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //마우스 상하좌우를 Vector2에 입력
        Vector3 camAngle = cameraObj.rotation.eulerAngles;

        //카메라 각도 제한
        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 55f); //위로 55도
        }
        else
        {
            x = Mathf.Clamp(x, 315f, 361f); //아래로 45도
        }

        if (!uiController.activeUI && !inventoryUI.activeUI && !dwarfUI.activeUI && !dialogueManager.activeUI && !baseCampUI.activeUI)
        {
            cameraObj.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z); //카메라 회전값
        }
    }
}