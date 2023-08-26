using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static int score;


    public GameObject hsTable;
    public GameObject Button;
    public GameObject Quit;
    public GameObject Bground;

    private float widthFactor;
    private float heightFactor;


    public void Awake()
    {
        GameObject obj;
        Transform trans;
        Transform childTrans;

        widthFactor = Screen.width / 1920.0f;
        heightFactor = Screen.height / 1080.0f;

        HighscoreTable.textFactor = (HighscoreTable.textFactor * Screen.height) / 1080;

        // Highscore table
        Resize(hsTable);

        // Title background
        trans = hsTable.transform;
        childTrans = trans.Find("background");
        obj = childTrans.gameObject;
        Resize(obj);

        // Title background
        trans = hsTable.transform;
        childTrans = trans.Find("TitleBackground");
        obj = childTrans.gameObject;
        Resize(obj);

        // Title text
        trans = hsTable.transform;
        childTrans = trans.Find("TitleText");
        obj = childTrans.gameObject;
        Resize(obj);
        //        ResizeText(obj);
        PositionText(obj);

        // Title text
        trans = hsTable.transform;
        childTrans = trans.Find("GameScoreText");
        obj = childTrans.gameObject;
        Resize(obj);
        ResizeText(obj);

        // Highscore Entry Container
        trans = hsTable.transform;
        childTrans = trans.Find("highscoreEntryContainer");
        obj = childTrans.gameObject;
        Resize(obj);

        // Highscore Entry Container
        /*
                trans = hsTable.transform;
                childTrans = trans.Find("highscoreEntryTemplate");
                obj = childTrans.gameObject;
                Resize(obj);
        */

        // Button
        Resize(Button);

        // Button text
        trans = Button.transform;
        childTrans = trans.Find("Text");
        obj = childTrans.gameObject;
        Resize(obj);
        ResizeText(obj);

        // Quit
        Resize(Quit);

        // Button text
        trans = Quit.transform;
        childTrans = trans.Find("Text");
        obj = childTrans.gameObject;
        Resize(obj);
        ResizeText(obj);

        ResizeSpriteRendered(Bground);

    }

    public void ChangeMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Resize(GameObject gameObject)
    {
        float objectWidth;
        float objectHeight;
        RectTransform rt;

        rt = gameObject.GetComponent<RectTransform>();
        objectWidth = rt.rect.width;
        objectHeight = rt.rect.height;

        objectWidth = objectWidth * widthFactor;
        objectHeight = objectHeight * heightFactor;

        rt.sizeDelta = new Vector2(objectWidth, objectHeight);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x * widthFactor, rt.anchoredPosition.y * heightFactor);
    }

    private void ResizeText(GameObject gameObject)
    {
        Text t = gameObject.GetComponent<Text>();
        t.fontSize = (t.fontSize * Screen.width) / 1920;
    }


    private void ResizeSpriteRendered(GameObject gameObject)
    {
        float objectWidth;
        float objectHeight;
        SpriteRenderer sr;

        sr = gameObject.GetComponent<SpriteRenderer>();
        objectWidth = sr.transform.localScale.x;
        objectHeight = sr.transform.localScale.y;

        objectWidth = objectWidth * widthFactor;
        objectHeight = objectHeight * heightFactor;

        sr.transform.localScale = new Vector2(objectWidth, objectHeight);
    }

    private void PositionText(GameObject gameObject)
    {
        float objectPosx;
        float objectPosy;
        RectTransform rt;

        rt = gameObject.GetComponent<RectTransform>();
        objectPosx = rt.localPosition.x;
        objectPosy = rt.localPosition.y;

        objectPosx = objectPosx * widthFactor;
        objectPosy = objectPosy * heightFactor;

        rt.localPosition = new Vector3(objectPosx, objectPosy, 0);
    }
}
