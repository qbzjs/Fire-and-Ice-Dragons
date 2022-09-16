using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStone : MonoBehaviour
{
    public GameObject dragonStone;
    bool spawn;
    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        spawn = false;
    }
    
    void Update()
    {
        if (dragonStone!=null)
        {

        }
        if (dragonStone != null&&dragonStone.activeInHierarchy)   //�]�����z���A��S�X�{���A�ۦP�A�ҥH�n�ⶥ�q�P�_�A��spawn�ɤ~true�A��true�ɤ~����S��
        {
            spawn = true;
        }

        if (spawn)
        {
            if (dragonStone.GetComponent<Stronghold>().hp <= dragonStone.GetComponent<Stronghold>().maxHp * 0.001f || dragonStone == null)
            {
                    if (!gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
                    gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
               
            }
        }
    }
}
