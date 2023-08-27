using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NMeCab.Specialized;
using NMeCab;

public class Jage : MonoBehaviour
{
    // Start is called before the first frame update
    public string sentence = "かりんご";
    private string[] sep = new string[3];
    private List<string> sepa = new List<string>();
    void Start()
    {
      int num = 0;

       // 「dic/ipadicフォルダ」のパスを指定する
       var dicDir = @"Assets/Nmecab/dic/ipadic";
       var user = new[] {"origin.dic"};

       using (var tagger = MeCabIpaDicTagger.Create(dicDir,user))
       {

         var prm = new MeCabParam()
          {
              LatticeLevel = MeCabLatticeLevel.Two,
              Theta = 1f / 800f / 2f
          };

          var lattice = tagger.ParseToLattice(sentence, prm); // ラティスを取得

          // ラティスから、ベスト解を取得し処理
          foreach (var node in lattice.GetBestNodes())
          {
            Debug.Log(node.Surface);
            sepa.Add($"{node.Surface}");
            Debug.Log(sepa[num].Length);
            if(sepa[num].Length < 3){
              sepa.Remove($"{node.Surface}");
              num = num - 1;
           }
            num++;
          }



          // ラティスから、2番目と3番目のベスト解を取得し処理
          foreach (var result in lattice.GetNBestResults().Skip(1).Take(2))
          {
              foreach (var node in result)
              {
                Debug.Log(node.Surface);
                Debug.Log(node.Surface);
                sepa.Add($"{node.Surface}");
                Debug.Log(sepa[num].Length);
                if(sepa[num].Length < 3){
                  sepa.Remove($"{node.Surface}");
                  num = num - 1;
               }
                num++;
              }


          }

          Debug.Log("----------");

          // ラティスから、開始位置別の形態素を取得し処理
          for (int i = 0; i < lattice.BeginNodeList.Length - 1; i++)
          {
              for (var node = lattice.BeginNodeList[i]; node != null; node = node.BNext)
              {
                  if (node.Prob <= 0.001f) continue;


              }
          }

          for(int i = 0;i<num;i++){

            if(i +1 < sepa.Count ){
              Debug.Log("入った");
              if(sepa[i]==sepa[i+1]){
                sepa.Remove(sepa[i]);
                Debug.Log("消した");
              }
            }
          }

          for(int i = 0;i<sepa.Count;i++){
            Debug.Log(sepa[i]);
          }

          // ラティスから、最終的な累積コストのみを取得

      }
   }

  public List<string> Check(string s){
      int num = 0;

       // 「dic/ipadicフォルダ」のパスを指定する
       var dicDir = @"Assets/Nmecab/dic/ipadic";
       var user = new[] {"origin.dic"};

       using (var tagger = MeCabIpaDicTagger.Create(dicDir,user))
       {

         var prm = new MeCabParam()
          {
              LatticeLevel = MeCabLatticeLevel.Two,
              Theta = 1f / 800f / 2f
          };

          var lattice = tagger.ParseToLattice(sentence, prm); // ラティスを取得

          // ラティスから、ベスト解を取得し処理
          foreach (var node in lattice.GetBestNodes())
          {
            Debug.Log(node.Surface);
            sepa.Add($"{node.Surface}");
            Debug.Log(sepa[num].Length);
            if(sepa[num].Length < 3){
              sepa.Remove($"{node.Surface}");
              num = num - 1;
           }
            num++;
          }



          // ラティスから、2番目と3番目のベスト解を取得し処理
          foreach (var result in lattice.GetNBestResults().Skip(1).Take(2))
          {
              foreach (var node in result)
              {
                Debug.Log(node.Surface);
                Debug.Log(node.Surface);
                sepa.Add($"{node.Surface}");
                Debug.Log(sepa[num].Length);
                if(sepa[num].Length < 3){
                  sepa.Remove($"{node.Surface}");
                  num = num - 1;
               }
                num++;
              }


          }

          Debug.Log("----------");

          // ラティスから、開始位置別の形態素を取得し処理
          for (int i = 0; i < lattice.BeginNodeList.Length - 1; i++)
          {
              for (var node = lattice.BeginNodeList[i]; node != null; node = node.BNext)
              {
                  if (node.Prob <= 0.001f) continue;


              }
          }

          for(int i = 0;i<num;i++){

            if(i +1 < sepa.Count ){
              Debug.Log("入った");
              if(sepa[i]==sepa[i+1]){
                sepa.Remove(sepa[i]);
                Debug.Log("消した");
              }
            }
          }

          for(int i = 0;i<sepa.Count;i++){
            Debug.Log(sepa[i]);
          }

          return sepa;

      }

    }
    // Update is called once per frame
    void Update()
    {

    }
}