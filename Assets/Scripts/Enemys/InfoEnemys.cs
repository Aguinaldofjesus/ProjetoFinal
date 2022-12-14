using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoEnemys : MonoBehaviour
{
    private FlyrobAI FlyrobAI;

    private Icebob Icebob;


    //Dano do Inimigo
    public int DanoInimigo;

    //GetComponent<FlyrobAI>().Dano();

    //Vida Inimigo
    public int VidaInimigo;


    // Start is called before the first frame update
    void Start()
    {
       
        FlyrobAI = GetComponent<FlyrobAI>();

        Icebob = GetComponent<Icebob>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TiroBasic"))
        {
            LevarDano(other.gameObject.GetComponent<Tiro>().DanoTiro);
            Destroy(other.gameObject);
            VerificaInimigo();
        }
    }



    void LevarDano(int Dano)
    {
        
        VidaInimigo -= Dano;

        if (VidaInimigo <= 0)
        {
            VidaInimigo = 0;

        }
        
        if(VidaInimigo == 0)
        {
            //Esse Objeto
            Destroy(this.gameObject);
        }
    }


    void VerificaInimigo()
    {
        if (FlyrobAI != null)
        {
            GetComponent<FlyrobAI>().Dano();
        }

        if(Icebob != null)
        {
            GetComponent<Icebob>().Dano();
        }
    }




}
