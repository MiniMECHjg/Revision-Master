using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class scaler : MonoBehaviour
{
    public Canvas canvas;

    public float heightRatio()
    {
        float scaleRatioHeight = Screen.height/1920f;
        return scaleRatioHeight;
    }

    public float widthRatio()
    {
        float scaleRatioWidth = Screen.width/1080f;
        return scaleRatioWidth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float height = Screen.height;
        float width = Screen.width;
        float scaleRatioHeight = heightRatio();
        float scaleRatioWidth = widthRatio();
        if (scaleRatioHeight <= scaleRatioWidth)
        {
            canvas.scaleFactor = scaleRatioHeight;
        }
        else
        {
            canvas.scaleFactor = scaleRatioWidth;
        }

    }

    public void reset_container(GameObject overallContainer, GameObject childContainer)
    {
        RectTransform childContainerRT = (RectTransform)childContainer.transform;

        RectTransform rt = (RectTransform)overallContainer.transform; //get rect transform of the container
        float width = rt.rect.width;
        float height = rt.rect.height;

        float gapX = childContainer.GetComponent<GridLayoutGroup>().spacing.x;
        float gapY = childContainer.GetComponent<GridLayoutGroup>().spacing.y; //Gaps for math

        float cellSizeX = childContainer.GetComponent<GridLayoutGroup>().cellSize.x;
        float cellSizeY = childContainer.GetComponent<GridLayoutGroup>().cellSize.y; //Size for math

        int children = childContainer.transform.childCount; //Amount of children for math

        int numInRow = (int)Math.Floor((decimal)(width/(cellSizeX+gapX)));
        
        if(numInRow == 0)
        {
            numInRow = 1;
        }

        int rows = (int)Math.Ceiling((decimal)children/numInRow);


        childContainerRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        childContainerRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((rows*(cellSizeY+gapY)) + 50 + 50));

        childContainerRT.anchoredPosition -= new Vector2(0, ((rows*(cellSizeY+gapY)) + 50 + 50)/2);

        //50 as it is the padding at the very top
        //75 to account for the fact I have text that overlays the bottom of each statistics

    }
}
