using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

//�ƾ� �׽�Ʈ �ڵ�
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour
{
    //��ũ��Ʈ ����
    private InventoryUI inventoryUI;
    private BaseCamp baseCampUI;
    private DwarfUI dwarfUI;
    private UIController uiController;
    private DialogueManager dialogueManager;

    public Transform cameraObj; //ī�޶� ������Ʈ
    public Transform characterBody; //�÷��̾� ��

    //�ƾ� �����Ű�� ���� �Լ�
    #region
    public Camera _camera;
    public float time = 0;
    #endregion

    //ī�޶� offset
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
        //�÷��̾�� ī�޶������ �Ÿ�
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);
        //�÷��̾�� ī�޶� ��ġ������ ���⺤��
        dir = new Vector3(0, camera_height, camera_width).normalized;
    }

    /// <summary>
    /// �ƾ� �����Ű�� ���� �Լ�
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
    //ī�޶��� ������Ʈ �浹 ��ġ ����
    private void CameraPosition()
    {
        //ī�޶� ��ġ
        Vector3 ray_target = characterBody.up * camera_height + cameraObj.forward * camera_width;
        //ī�޶󿡼� ĳ���ͷ� ���� �߻���ġ
        Physics.Raycast(new Vector3(characterBody.position.x, characterBody.position.y + 1.2f, characterBody.position.z), ray_target, out hit, camera_dist);
        //Debug.DrawRay(new Vector3(characterBody.position.x, characterBody.position.y + 1.2f, characterBody.position.z), ray_target, Color.green);

        if (hit.point != Vector3.zero && hit.collider.gameObject != characterBody.gameObject) //���� ī�޶� �浹�� ��
        {
            if (hit.collider.gameObject != characterBody.gameObject)
            {
                cameraObj.position = hit.point; //������Ʈ�� �浹�� ��ġ�� �̵�
                //ī�޶� ����
                cameraObj.Translate(dir * -1 * camera_fix);
            }
        }
        else //���� �浹���� ���� ��
        {
            cameraObj.position = characterBody.position; //�÷��̾�� �������� ����
            cameraObj.Translate(dir * camera_dist); //�� ��ġ�� �̵�
        }
    }
    //ī�޶� ȸ��
    private void CameraControl()
    {
        //ī�޶� ȸ��
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //���콺 �����¿츦 Vector2�� �Է�
        Vector3 camAngle = cameraObj.rotation.eulerAngles;

        //ī�޶� ���� ����
        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 55f); //���� 55��
        }
        else
        {
            x = Mathf.Clamp(x, 315f, 361f); //�Ʒ��� 45��
        }

        if (!uiController.activeUI && !inventoryUI.activeUI && !dwarfUI.activeUI && !dialogueManager.activeUI && !baseCampUI.activeUI)
        {
            cameraObj.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z); //ī�޶� ȸ����
        }
    }
}