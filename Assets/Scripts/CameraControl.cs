using UnityEngine;

public class CameraControl : MonoBehaviour {
    public float m_DampTime = 0.02f;
    public float m_MouseEdgeBuffer = 4f;
    public float m_MaxSize= 60f;
    public float m_MinSize = 6.5f;
    public float zoomDamp = 5f;
    public Terrain terreno;

    private Camera m_Camera;
    private float m_ZoomSpeed;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;
    private Vector3 minValue;
    private Vector3 maxValue;


    private void Awake() {
        m_Camera = GetComponentInChildren<Camera>();
        m_DesiredPosition = transform.position;
    }
    private void Start() {
        CalculaScreenBuffer();

    }

    private void FixedUpdate() {
        Move();
        Zoom();
    }


    private void Move() {

        CalculateDesiredPosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private void CalculateDesiredPosition() {
        //Debug.Log(Input.mousePosition);
        if (Input.mousePosition.x < 0 + m_MouseEdgeBuffer ) {
            m_DesiredPosition += Vector3.left;
            if (m_DesiredPosition.x < minValue.x)
                m_DesiredPosition.x = minValue.x;

        }
        if (Input.mousePosition.x > Screen.width - m_MouseEdgeBuffer) {
            m_DesiredPosition += Vector3.right;
            if (m_DesiredPosition.x > maxValue.x)
                m_DesiredPosition.x = maxValue.x;

        }
        if (Input.mousePosition.y < 0 + m_MouseEdgeBuffer) {
            m_DesiredPosition+= Vector3.back;
            if (m_DesiredPosition.z < minValue.z)
                m_DesiredPosition.z = minValue.z;
        }
        if (Input.mousePosition.y > Screen.height - m_MouseEdgeBuffer) {
            m_DesiredPosition+= Vector3.forward;
            if (m_DesiredPosition.z > maxValue.z)
                m_DesiredPosition.z = maxValue.z;
        }
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        if (requiredSize != m_Camera.orthographicSize) {
            m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
            CalculaScreenBuffer();
        }
        
    }


    private float FindRequiredSize()
    {
        float size = m_Camera.orthographicSize;
        float rodinha = Input.GetAxis("Mouse ScrollWheel");
        Debug.Log("rodinha" + rodinha);
        if (!(rodinha == 0)) {
            //se a rodinha nao é 0
            size += rodinha * size*-5;
            size=Mathf.Max(size, m_MinSize);
            size = Mathf.Min(size, m_MaxSize);
            return size;
        }

        return size;
    }

    private void CalculaScreenBuffer() {
        //fazer com que isto funcione com um camera rig como no tuturial dos tanques por causa do tilt da camera
        //tamanho da camera
        Vector3 screenBufer = new Vector3(m_Camera.orthographicSize * m_Camera.aspect, 0, m_Camera.orthographicSize);
        // distancia que o cameraRig tem de ficar das beiras para a camera nao sair do mapa
        minValue = new Vector3(terreno.transform.position.x, 0, terreno.transform.position.z);
        maxValue = minValue + new Vector3(terreno.terrainData.size.x, 0, terreno.terrainData.size.z) - screenBufer;
        minValue += screenBufer;
    }
}