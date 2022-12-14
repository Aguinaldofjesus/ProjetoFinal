using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCharacter : MonoBehaviour
{
    public GameObject MyPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Seguir();
    }

    void Seguir()
    {
        Vector3 destino = new Vector3(MyPlayer.transform.position.x, MyPlayer.transform.position.y,transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, destino, 0.1f);
    }
}
