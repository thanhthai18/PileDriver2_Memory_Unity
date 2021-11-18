using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_PileDriverMinigame2 : MonoBehaviour
{
    public static GameController_PileDriverMinigame2 instance;

    public Camera mainCamera;
    public PileDriver_PileDriverMinigame2 pileDriverObj;
    public Button btnLeft, btnRight, btnPile;
    public bool isHoldLeft, isHoldRight;
    public Vector2 mouseCurrentPos;
    public RaycastHit2D[] hit;
    public float speed;
    public float screenRatio;
    public Text txtPower;
    public int power;
    public List<GameObject> listPrefabs = new List<GameObject>();
    public List<Transform> listPos = new List<Transform>();
    public GameObject iconObj, lightObj;
    public int CountWin = 0;
    public bool isWin, isLose, isTutorial2;
    public GameObject tutorial1, tutorial2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isTutorial2 = true;
    }

    private void Start()
    {
        SetSizeCamera();
        SetUpMap();
        screenRatio = Screen.width * 1f / Screen.height;
        speed = 10;
        power = 20;
        tutorial1.SetActive(true);
        tutorial2.SetActive(false);
        Tutorial1FX();
        txtPower.text = power.ToString();
        btnPile.onClick.AddListener(OnClickPile);
    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    void SetUpMap()
    {
        int ran;
        for (int i = 0; i < listPrefabs.Count; i++)
        {
            ran = Random.Range(0, listPos.Count);
            Instantiate(listPrefabs[i], listPos[ran].position, Quaternion.identity);
            listPos.RemoveAt(ran);
        }
    }

    void Tutorial1FX()
    {
        if (tutorial1.activeSelf)
        {
            tutorial1.transform.DOMoveX(-6.99f, 1).OnComplete(() =>
            {
                tutorial1.transform.DOMoveX(-9.59f, 1).OnComplete(() =>
                {
                    Tutorial1FX();
                });
            });
        }
    }

    void DelayEndGame()
    {
        pileDriverObj.transform.DOMoveX(25, 5);
        pileDriverObj.transform.localScale = new Vector2(-0.5f, 0.5f);
    }

    void OnClickPile()
    {
        if (tutorial2.activeSelf)
        {
            tutorial2.SetActive(false);
            tutorial2.transform.DOKill();
            btnLeft.GetComponent<CircleCollider2D>().enabled = true;
            btnRight.GetComponent<CircleCollider2D>().enabled = true;
            btnRight.enabled = true;
            btnLeft.enabled = true;
            speed = 10;
        }

        if (power > 0)
        {
            power--;
            txtPower.text = power.ToString();
            if (pileDriverObj.signal.activeSelf)
            {
                if (!pileDriverObj.isCollecting)
                {
                    pileDriverObj.Collect();
                    CountWin++;
                    if (CountWin == 6)
                    {
                        isWin = true;
                        Debug.Log("Win");
                        btnLeft.enabled = false;
                        btnPile.enabled = false;
                        btnRight.enabled = false;
                        Invoke(nameof(DelayEndGame), 2);
                    }
                }
            }
            if (power == 0 && !isWin)
            {
                isLose = true;
                Debug.Log("Lose");
                btnLeft.enabled = false;
                btnPile.enabled = false;
                btnRight.enabled = false;
            }
        }

    }

    bool isHoldMouse = false;
    private void Update()
    {
        if (!isWin && !isLose)
        {
            mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.RaycastAll(mouseCurrentPos, Vector2.zero);

            if (Input.GetMouseButtonDown(0))
            {
                isHoldMouse = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isHoldLeft = false;
                isHoldRight = false;
                isHoldMouse = false;
            }

            if (isHoldMouse)
            {
                if (hit.Length != 0)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].collider.gameObject.CompareTag("Tree"))
                        {
                            if (tutorial1.activeSelf)
                            {
                                tutorial1.SetActive(false);
                                tutorial1.transform.DOKill();
                            }
                            isHoldLeft = true;
                            isHoldRight = false;
                            pileDriverObj.transform.localScale = new Vector2(0.5f, 0.5f);
                        }
                        if (hit[i].collider.gameObject.CompareTag("People"))
                        {
                            if (tutorial1.activeSelf)
                            {
                                tutorial1.SetActive(false);
                                tutorial1.transform.DOKill();
                            }
                            isHoldRight = true;
                            isHoldLeft = false;
                            pileDriverObj.transform.localScale = new Vector2(-0.5f, 0.5f);
                        }
                    }
                }
            }

            if (isHoldLeft)
            {
                pileDriverObj.transform.Translate(Vector2.left * speed * Time.deltaTime);
                pileDriverObj.transform.position = new Vector2(Mathf.Clamp(pileDriverObj.transform.position.x, -mainCamera.orthographicSize * screenRatio + 2, mainCamera.orthographicSize * screenRatio), pileDriverObj.transform.position.y);
            }
            if (isHoldRight)
            {
                pileDriverObj.transform.Translate(Vector2.right * speed * Time.deltaTime);
                pileDriverObj.transform.position = new Vector2(Mathf.Clamp(pileDriverObj.transform.position.x, -mainCamera.orthographicSize * screenRatio, mainCamera.orthographicSize * screenRatio - 2), pileDriverObj.transform.position.y);
            }


        }
    }

}
