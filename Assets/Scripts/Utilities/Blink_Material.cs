
using UnityEngine;

public class Blink_Material : MonoBehaviour
{
    public Color startColor = Color.yellow;
    public Color endcolor = Color.grey;
    [Range(0, 10)]
    public float speed = 2.5f;

    Renderer _ren;

    private void Awake()
    {
        _ren = GetComponent<Renderer>();

    }
    private void Update()
    {
        _ren.material.color = Color.Lerp(startColor, endcolor, Mathf.PingPong(Time.time * speed, 1));
    }
}
