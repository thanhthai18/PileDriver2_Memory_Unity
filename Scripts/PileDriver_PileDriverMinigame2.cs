using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriver_PileDriverMinigame2 : MonoBehaviour
{
    public GameObject signal;
    public GameObject collectItem;
    public bool isCollecting = false;

    private void Start()
    {
        signal = transform.GetChild(0).gameObject;
        signal.SetActive(false);
    }

    void SignalFadeFX()
    {
        signal.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
        {
            signal.GetComponent<SpriteRenderer>().DOFade(1, 0.5f).OnComplete(() =>
            {
                if (signal.activeSelf)
                {
                    SignalFadeFX();
                }
            });
        });
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            if (!signal.activeSelf)
            {
                if (GameController_PileDriverMinigame2.instance.isTutorial2)
                {
                    GameController_PileDriverMinigame2.instance.speed = 0;
                    GameController_PileDriverMinigame2.instance.isTutorial2 = false;
                    GameController_PileDriverMinigame2.instance.btnLeft.GetComponent<CircleCollider2D>().enabled = false;
                    GameController_PileDriverMinigame2.instance.btnRight.GetComponent<CircleCollider2D>().enabled = false;
                    GameController_PileDriverMinigame2.instance.btnRight.enabled = false;
                    GameController_PileDriverMinigame2.instance.btnLeft.enabled = false;
                    GameController_PileDriverMinigame2.instance.tutorial2.SetActive(true);
                    GameController_PileDriverMinigame2.instance.tutorial2.transform.DOScale(2, 1).SetLoops(-1);
                }
                signal.SetActive(true);
                SignalFadeFX();
                if (collectItem == null)
                {
                    collectItem = collision.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            signal.SetActive(false);
            collectItem = null;
        }
    }

    int CheckItem()
    {
        int index = -1;
        for (int i = 0; i < GameController_PileDriverMinigame2.instance.listPrefabs.Count; i++)
        {
            if ((GameController_PileDriverMinigame2.instance.listPrefabs[i].name +"(Clone)").Equals(collectItem.name))
            {
                index = i;          
            }              
        }
        return index;
    }

    public void Collect()
    {
        isCollecting = true;
        int index = CheckItem();
        if (index != -1)
        {
            GameObject tmpObj = collectItem;
            tmpObj.GetComponent<SpriteRenderer>().sortingOrder = 10;
            Invoke(nameof(DelayLightFX), 0.5f);
            tmpObj.GetComponent<BoxCollider2D>().enabled = false;
            tmpObj.transform.DOScale(2, 0.5f);
            tmpObj.transform.DOMove(new Vector2(0, 0), 1f).OnComplete(() =>
            {                
                isCollecting = false;
                tmpObj.transform.DOMove(GameController_PileDriverMinigame2.instance.iconObj.transform.GetChild(index).GetChild(0).transform.position, 0.7f).SetEase(Ease.InOutQuart);
                tmpObj.transform.DOScale(0.65f, 0.3f).OnComplete(() =>
                {
                });
            });
        }
    }

    void DelayLightFX()
    {
        GameController_PileDriverMinigame2.instance.lightObj.transform.DOScale(new Vector3(2, 2, 2), 2).OnComplete(() =>
        {
            GameController_PileDriverMinigame2.instance.lightObj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        });
        for (int i = 0; i < GameController_PileDriverMinigame2.instance.lightObj.transform.childCount; i++)
        {
            GameObject tmpLight = GameController_PileDriverMinigame2.instance.lightObj.transform.GetChild(i).gameObject;
            tmpLight.GetComponent<SpriteRenderer>().DOFade(1, 0.5f).OnComplete(() =>
            {
                tmpLight.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
            });
        }
    }

}
