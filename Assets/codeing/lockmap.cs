using UnityEngine;

public class lockmap : MonoBehaviour
{
    [SerializeField] Transform plr;
    [SerializeField] Transform map;
    [SerializeField] float halfMapscale = 0.5f;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        var plrPos = plr.position;
        plrPos.x = ((int)plrPos.x / halfMapscale) * halfMapscale;
        plrPos.y = ((int)plrPos.y / halfMapscale) * halfMapscale;

        map.position = plrPos;
    }
}
