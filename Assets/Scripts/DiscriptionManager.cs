using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscriptionManager : MonoBehaviour
{
    [SerializeField] List<GameObject> discriptionBoards;
    [SerializeField] List<GameObject> commentBoxes;
    public IEnumerator playDiscription(int part)
    {
        CommentBox commentBox = null;
        if (part == 1)
        {
            //T���{�^���̐���
            discriptionBoards[0].SetActive(true);
            commentBoxes[0].SetActive(true);
            commentBox = commentBoxes[0].GetComponent<CommentBox>();
            yield return new WaitUntil(() => commentBox.allEnd);
            discriptionBoards[0].SetActive(false);
            commentBoxes[0].SetActive(false);
            
            //��]�{�^���̐���
            discriptionBoards[1].SetActive(true);
            commentBoxes[1].SetActive(true);
            commentBox = commentBoxes[1].GetComponent<CommentBox>();
            yield return new WaitUntil(() => commentBox.allEnd);
            discriptionBoards[1].SetActive(false);
            commentBoxes[1].SetActive(false);

            //�v���C���Ă݂悤�̕\��
            discriptionBoards[2].SetActive(true);
            commentBoxes[2].SetActive(true);
            commentBox = commentBoxes[2].GetComponent<CommentBox>();
            yield return new WaitUntil(() => commentBox.allEnd);
            discriptionBoards[2].SetActive(false);
            commentBoxes[2].SetActive(false);

        }
        else if (part == 2)
        {
            //�P�������Ă݂悤�̕\��
            discriptionBoards[3].SetActive(true);
            commentBoxes[3].SetActive(true);
            commentBox = commentBoxes[3].GetComponent<CommentBox>();
            yield return new WaitUntil(() => commentBox.allEnd);
            discriptionBoards[3].SetActive(false);
            commentBoxes[3].SetActive(false);
        }
        else if (part == 3)
        {
            //�P�������Ă݂悤�̕\��
            discriptionBoards[4].SetActive(true);
            commentBoxes[4].SetActive(true);
            commentBox = commentBoxes[4].GetComponent<CommentBox>();
            yield return new WaitUntil(() => commentBox.allEnd);
            discriptionBoards[4].SetActive(false);
            commentBoxes[4].SetActive(false);
        }
        else if (part == 4)
        {
            //�P�������Ă݂悤�̕\��
            discriptionBoards[5].SetActive(true);
            commentBoxes[5].SetActive(true);
            commentBox = commentBoxes[5].GetComponent<CommentBox>();
            yield return new WaitUntil(() => commentBox.allEnd);
            discriptionBoards[5].SetActive(false);
            commentBoxes[5].SetActive(false);
        }
        yield break;
    }
}
