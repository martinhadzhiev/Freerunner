using UnityEngine;

[ExecuteInEditMode]
public class CurveController : MonoBehaviour
{
    public Transform CurveOrigin;

    [Range(-500f, 500f)]
    [SerializeField]
    float x = 0f;

    [Range(-500f, 500f)]
    [SerializeField]
    float y = 0f;

    private Vector2 bendAmount = Vector2.zero;


    private int bendAmountId;
    private int bendOriginId;

    void Start()
    {
        bendAmountId = Shader.PropertyToID(Constants.CurveShaderBendAmountPropertyName);
        bendOriginId = Shader.PropertyToID(Constants.CurveShaderBendOriginPropertyName);
    }

    void Update()
    {
        bendAmount.x = x;
        bendAmount.y = y;

        Shader.SetGlobalVector(bendAmountId, bendAmount);
        Shader.SetGlobalVector(bendOriginId, CurveOrigin.position);
    }
}