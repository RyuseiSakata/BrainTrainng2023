using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentBox : MonoBehaviour
{
    private bool _isEnd = false; //�\�����̃e�L�X�g���\�����I������
    public bool allEnd = false; //�S�e�L�X�g�̕\�������I������
    [SerializeField] List<string> _textList = new List<string>();
    private int _currrentTextIndex = 0;
    private Text _text;
    Coroutine _setTextCoroutine;

    private void Start()
    {
        startText();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {

            if (_isEnd)
            {
                changeText();
            }
            else
            {
                skipText();
            }

        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);    // �^�b�`���̎擾
                                                //�^�b�`�����u��
            if (touch.phase == TouchPhase.Began)
            {
                if (_isEnd)
                {
                    changeText();
                }
                else
                {
                    skipText();
                }
            }
        }
    }

    public void startText()
    {
        _text = transform.GetChild(0).gameObject.GetComponent<Text>();

        _setTextCoroutine = StartCoroutine(setText());
    }

    private IEnumerator setText()
    {
        _isEnd = false;

        if (_textList == null || _textList.Count <= _currrentTextIndex) yield break;

        int endl = 0;
        int index = _currrentTextIndex;  //�\������e�L�X�g�̔ԍ�
        int len = _textList[index].Length;  //�\������e�L�X�g�̕�����

        //�e�L�X�g�̕����������Ȃ�
        while (endl < len)
        {
            endl++;
            _text.text = _textList[index].Substring(0, endl);
            yield return new WaitForSeconds(0.1f);

        }

        _isEnd = true;  //�e�L�X�g��\�����I����
        yield break;
    }

    private void changeText()
    {
        _currrentTextIndex++;

        if (_currrentTextIndex > _textList.Count-1)
        {
            allEnd = true;
            //Destroy(gameObject);
        }
        else
        {
            _setTextCoroutine = StartCoroutine(setText());
        }
        
    }

    private void skipText()
    {
        if (_setTextCoroutine != null)
        {
            StopCoroutine(_setTextCoroutine);
            _isEnd = true;  //�e�L�X�g��\�����I����
            _text.text = _textList[_currrentTextIndex];
        }
    }
}
