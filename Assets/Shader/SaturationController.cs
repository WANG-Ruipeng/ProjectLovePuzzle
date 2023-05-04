using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SaturationController : MonoBehaviour
{
    public float Saturation = 0f;
    private Material _material;

    void Start()
    {
        // ����һ���²���ʵ�����Ա���������ʹ�ô�Shader�Ķ�����Ӧ�ø���
        _material = new Material(Shader.Find("Custom/HSVShader"));
        //GetComponent<SpriteRenderer>().material = _material;
    }

    void Update()
    {
        // ���²��ʵı��Ͷ�ֵ
        //_material.SetFloat("_Saturation", Saturation);
    }

    public void SetSaturation(float sat)
    {
        Saturation = sat;
        if(_material != null)
        {
            _material.SetFloat("_Saturation", Saturation);
        }
        
    }

    private void OnDestroy()
    {
        // �����´����Ĳ���ʵ�����Է�ֹ�ڴ�й©
        Destroy(_material);
    }
}