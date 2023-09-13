using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaurosu: MonoBehaviour
{

    // �Đ��A�j���[�V������Resources�t�H���_���̃T�u�p�X
    [SerializeField]
    public Object[] AnimationList;
    int num;

    // �Đ��A�j���[�V�����w��p 
    private enum AnimationPattern : int
    {
        Wait = 33,      // �ҋ@
        Attack = 1,     // �U�� 
        Run = 24,       // ���� 
        Count
    }

    // �L�����N�^�[�Ǘ��p 
    private GameObject m_goCharacter = null;
    private GameObject m_goCharPos = null;
    private Vector3 m_vecCharacterPos;      // �L�����N�^�[�ʒu 
    private Vector3 m_vecCharacterScale;    // �L�����N�^�[�X�P�[�� 
    private GameObject gameObject;

    // �����X�e�b�v�p 
    private enum Step : int
    {
        Init = 0,   // ������ 
        Title,      // �^�C�g�� 
        Wait,       // �ҋ@ 
        Move,       // �ړ� 
        Attack,     // �U��
        End
    }

    // �����X�e�b�v�Ǘ��p 
    private Step m_Step = Step.Init;

    // �ėp
    // ���낢��g���܂킷�p�ϐ�
    private int m_Count = 0;
    private bool m_SW = true;

    // Use this for initialization
    void Start()
    {

        // �L�����N�^�[�p�����[�^�֘A��ݒ� 

        // ���W�ݒ� 
        m_vecCharacterPos.x = 14.5f;
        m_vecCharacterPos.y = 10f;
        m_vecCharacterPos.z = 0.0f;

        // �X�P�[���ݒ� 
        m_vecCharacterScale.x = 0.0022f;
        m_vecCharacterScale.y = 0.0022f;
        m_vecCharacterScale.z = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) == true)
        {
            //StartCoroutine("AStart");
            //gameObject.SetActive(false);
        }

    }
    public void SA()
    {
        StartCoroutine("AStart");
        //meObject.SetActive(false);
    }

    IEnumerator AStart()
    {
        Destroy(GameObject.Find("Comipo_mino"));
        AnimationStart();
        AnimationChange(AnimationPattern.Attack);
        num++;
        m_goCharacter = null;

        yield return new WaitForSeconds(1f);
        Destroy(GameObject.Find("Comipo_mino"));
        AnimationStart();
        AnimationChange(AnimationPattern.Attack);

        num++;
        m_goCharacter = null;



    }
    // �A�j���[�V�����J�n 
    private void AnimationStart()
    {
        Object resourceObject;
        Script_SpriteStudio6_Root scriptRoot = null;    // SpriteStudio Anime �𑀍삷�邽�߂̃N���X
        int listLength = AnimationList.Length;

        // ���łɃA�j���[�V���������� or ���\�[�X�ݒ薳���ꍇ��return
        if (m_goCharacter != null || listLength < 1)
            return;

        // �Đ����郊�\�[�X�������X�g����擾���čĐ�����
        if (num % 2 == 0)
        {
            resourceObject = AnimationList[0];
            Debug.Log("1");
        }
        else
        {
            resourceObject = AnimationList[1];
            Debug.Log("2");
        }
        if (resourceObject != null)
        {
            // �A�j���[�V���������̉�
            m_goCharacter = Instantiate(resourceObject, Vector3.zero, Quaternion.identity) as GameObject;
            if (m_goCharacter != null)
            {
                scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(m_goCharacter);
                if (scriptRoot != null)
                {
                    // ���W�ݒ肷�邽�߂�GameObject�쐬
                    m_goCharPos = new GameObject();
                    if (m_goCharPos == null)
                    {
                        // �쐬�ł��Ȃ��P�[�X�Ή� 
                        Destroy(m_goCharacter);
                        m_goCharacter = null;
                    }
                    else
                    {
                        // Object���ύX 
                        m_goCharPos.name = "Comipo_mino";

                        // ���W�ݒ� 
                        m_goCharacter.transform.parent = m_goCharPos.transform;

                        // �����̎q�Ɉړ����č��W��ݒ�
                        m_goCharPos.transform.parent = this.transform;
                        m_goCharPos.transform.localPosition = m_vecCharacterPos;
                        m_goCharPos.transform.localRotation = Quaternion.identity;
                        m_goCharPos.transform.localScale = m_vecCharacterScale;

                        //�A�j���[�V�����Đ�
                        AnimationChange(AnimationPattern.Wait);
                    }
                }
            }
        }
    }

    // �A�j���[�V���� �Đ�/�ύX 
    private void AnimationChange(AnimationPattern pattern)
    {
        Script_SpriteStudio6_Root scriptRoot = null;    // SpriteStudio Anime �𑀍삷�邽�߂̃N���X
        int iTimesPlaey = 0;

        if (m_goCharacter == null)
            return;

        scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(m_goCharacter);
        if (scriptRoot != null)
        {
            switch (pattern)
            {
                case AnimationPattern.Wait:
                    iTimesPlaey = 0;    // ���[�v�Đ� 
                    break;
                case AnimationPattern.Attack:
                    iTimesPlaey = 1;    // 1�񂾂��Đ� 
                    break;
                case AnimationPattern.Run:
                    iTimesPlaey = 0;    // ���[�v�Đ� 
                    break;
                default:
                    break;
            }
            scriptRoot.AnimationPlay(-1, (int)pattern, iTimesPlaey);
        }
    }

    // �A�j���[�V�������Đ�������~��(�G���[��)���擾���܂�
    private bool IsAnimationPlay()
    {
        bool ret = false;

        Script_SpriteStudio6_Root scriptRoot = null;    // SpriteStudio Anime �𑀍삷�邽�߂̃N���X

        if (m_goCharacter != null)
        {
            scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(m_goCharacter);
            if (scriptRoot != null)
            {
                // �Đ��񐔂��擾���āA�v���C�I�����𔻒f���܂�
                int Remain = scriptRoot.PlayTimesGetRemain(0);
                if (Remain >= 0)
                    ret = true;
            }
        }

        return ret;
    }

}