using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}��I��
/// </summary>
public class CharactersCollision : MonoBehaviour
{
    Animator animator;
    AnimatorStateInfo info;
    GameData_NumericalValue NumericalValue;

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    //�ƭ�
    float Hp;//�ͩR��

    public List<CharactersFloating> floating_List = new List<CharactersFloating>();//�B��/���DList

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;


        //�ƭ�
        switch(gameObject.tag)
        {
            case "Player":
                Hp = NumericalValue.playerHp;
                break;
            case "SkeletonSoldier":                
                Hp = NumericalValue.skeletonSoldierHp;
                break;
        }      
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnCollisionControl();
        OnAnimationOver();
        OnFloation();        
    }

    /// <summary>
    /// �B��
    /// </summary>
    void OnFloation()
    {
        //�B��/���D
        for (int i = 0; i < floating_List.Count; i++)
        {
            floating_List[i].OnFloating();
        }

        //�I������
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            floating_List.Clear();//�M��List
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="attacker">�����̪���</param>
    /// <param name="layer">������layer</param>
    /// <param name="damage">�y���ˮ`</param>
    /// <param name="animationName">����ʵe�W��</param>
    /// <param name="knockDirection">�����ĪG(0:���h, 1:����)</param>
    /// <param name="repel">���h�Z��</param>
    public void OnGetHit(GameObject attacker, LayerMask layer, float damage, string animationName, int knockDirection, float repel)
    {               
        
        //�P�_������H
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == LayerMask.NameToLayer("Enemy") || 
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == LayerMask.NameToLayer("Player"))
        {
            Hp -= damage;//�ͩR�ȴ��
            transform.forward = -attacker.transform.forward;//���V������

            //�P�_�����ĪG
            switch (knockDirection)
            {
                case 0://���h
                    transform.position = transform.position + attacker.transform.forward * repel * Time.deltaTime;//���h
                    break;
                case 1://����
                    floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//�B��List
                    break;
            }

            //����Ĳ�o�ʵe
            if (info.IsTag(animationName))
            {
                StartCoroutine(OnAniamtionRepeatTrigger(animationName));
                return;
            }

            //���A����(�����e�@�Ӱʵe)
            if (info.IsTag("KnockBack") && animationName == "Pain") animator.SetBool("KnockBack", false);
            if (info.IsTag("Pain") && animationName == "KnockBack") animator.SetBool("Pain", false);

            animator.SetBool(animationName, true);
        }        
    }

    /// <summary>
    /// �I������
    /// </summary>
    void OnCollisionControl()
    {
        //�g�u��V
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        //�I������
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + boxCenter, boxSize/2 * transform.localScale.x, rayDiration[i], out hit, transform.rotation, NumericalValue.boxCollisionDistance, mask))
            {
                transform.position = transform.position - rayDiration[i] * (NumericalValue.boxCollisionDistance - hit.distance);
            }
        }

        //����I��(��2�h)
        if(Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2), Quaternion.identity, mask))
        {
            transform.position = transform.position + transform.forward * 5 * Time.deltaTime;
        }

        //�a�O�I��
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            if (Physics.Raycast(transform.position + boxCenter, -transform.up, out hit, boxSize.y / 2, mask))//�a�O�I��(��2�h)
            {
                transform.position = transform.position + transform.up * (boxSize.y / 2 - 0.01f - hit.distance);
            }            
        }
        else
        {
            //���O
            transform.position = transform.position - Vector3.up * NumericalValue.gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// �ʵe����
    /// </summary>
    void OnAnimationOver()
    {
        if (info.IsTag("Pain") && info.normalizedTime >= 1) animator.SetBool("Pain", false);
        if (info.IsTag("KnockBack") && info.normalizedTime >= 1) animator.SetBool("KnockBack", false);       
    }

    /// <summary>
    /// �ʵe����Ĳ�o
    /// </summary>
    /// <param name="aniamtionName">�ʵe�W��</param>
    /// <returns></returns>
    IEnumerator OnAniamtionRepeatTrigger(string aniamtionName)
    {
        animator.SetBool(aniamtionName, false);
        yield return new WaitForSeconds(0.03f);
        animator.SetBool(aniamtionName, true);
    }
}