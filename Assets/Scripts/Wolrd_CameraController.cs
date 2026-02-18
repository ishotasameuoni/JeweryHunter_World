using UnityEngine;

public class Wolrd_CameraController : MonoBehaviour
{
    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y,
                -10);
        }
    }
}
