using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // Takip edilecek hedef (Player)
    [SerializeField] private float distance = 4f; // Kameranın oyuncudan uzaklığı
    [SerializeField] private float sensitivity = 0.3f; // Fare hassasiyeti
    [SerializeField] private float minY = -35f; // Kamera yukarı-aşağı sınırı
    [SerializeField] private float maxY = 60f;

    private PlayerControls controls; // InputActions referansı
    private Vector2 lookInput;       // Fare girdisi
    private float yaw;               // Yatay dönme
    private float pitch;             // Dikey dönme

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => lookInput = Vector2.zero;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void LateUpdate()
    {
        if (!target) return;

        // Fare girdisini kullanarak rotasyon hesapla
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Rotasyonu uygula
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Kamerayı hedefin arkasına yerleştir
        Vector3 targetPosition = target.position - transform.forward * distance;
        transform.position = targetPosition;
    }
}
