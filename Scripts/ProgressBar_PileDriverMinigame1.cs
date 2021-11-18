using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar_PileDriverMinigame1 : MonoBehaviour
{
    public Image Bar;
    public float max_progress;
    public float current_progress;
    public bool isTiming = false;

    private void Update()
    {
        if (isTiming)
        {
            current_progress -= Time.deltaTime;
            if (current_progress < 0)
            {
                current_progress = 0;
                isTiming = false;

            }
            Bar.fillAmount = current_progress / max_progress;
        }
    }
}
