using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform window_graph_test;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //window_graph_test = transform.Find("window_graph").GetComponent<RectTransform>();
        // CreateCircle(new Vector2(0, 0));

    }

    public void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        //  graphContainer.sizeDelta = new Vector2(anchoredPosition[0]+10, anchoredPosition[1]+10);
       // transform.position = new Vector2(anchoredPosition[0] , transform.position.y); ;

        gameObject.GetComponent<Image>().sprite = circleSprite;
        Destroy(gameObject, 13);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

       // float graphWidth = graphContainer.sizeDelta.x;
       // float graphHeight = graphContainer.sizeDelta.y;
    }
}
