using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
    const int STAGE_0 = 1;//user has to click right
    const int STAGE_1 = 2;//user has to click left
    const int STAGE_2 = 3;//user has to click both together
    const int STAGE_3 = 4; //the planck fell and user has to lift it to a point
    //after that go in game
    int stage = 1;

    private GameObject left;
    private GameObject right;
    private GameObject plate;
    private GameObject fakePlate;
    private float lastClickLeft = -10f;
    private float lastClickRight = -10f;

    public Transform rightDude;
    public Transform leftDude;
    public GameObject reset;

	void Start () {
        right = Instantiate(Resources.Load<GameObject>("ClickHere"));
        right.GetComponent<Trans>().SetPos(new Vector2(0.52f, 0.25f));
        ControlManager.GetInstance().SubscribeToEvent("RightClick", OnClickRight);
        ControlManager.GetInstance().SubscribeToEvent("LeftClick", OnClickLeft);
	}

    void OnClickRight(object o)
    {
        switch (stage)
        {
            case STAGE_0:
                //get ride of click right and spawn click left
                right.GetComponent<Trans>().SetPos(new Vector2(-0.52f, 0.15f));
                left = right;
                right = null;
                stage++;
                break;
            case STAGE_2:
                //check if the last click on the other side is recent, if so go next 
                lastClickRight = Time.time;
                if (Time.time - lastClickLeft < 0.5f)
                    StartCoroutine(Stage2GotoNext());
                break;
            case STAGE_3:
                //check if planck is hogh enough
                if (plate.transform.position.y >= fakePlate.transform.position.y)
                    End();
                break;
        }
    }

    void OnClickLeft(object o)
    {
        switch (stage)
        {
            case STAGE_1:
                //get ride of click left and wait a bitand spawn right AND left
                Destroy(left);
                stage = -1;
                StartCoroutine(WaitStage1());
                break;
            case STAGE_2:
                //check if the last click on the other side is recent, if so go next 
                //and spawn plankc
                lastClickLeft = Time.time;
                if (Time.time - lastClickRight < 0.5f)
                    StartCoroutine(Stage2GotoNext());
                break;
            case STAGE_3:
                //check if planck is hogh enough
                if (plate.transform.position.y >= fakePlate.transform.position.y)
                    End();
                break;
        }
    }

    IEnumerator WaitStage1()
    {
        yield return new WaitForSeconds(2.3f);
        right = Instantiate(Resources.Load<GameObject>("ClickHere"));
        left = Instantiate(Resources.Load<GameObject>("CLickHere"));
        right.GetComponent<Trans>().SetPos(new Vector2(0.6f, -0.3f));
        left.GetComponent<Trans>().SetPos(new Vector2(-0.62f, 0.5f));
        stage = STAGE_2;
    }

    IEnumerator Stage2GotoNext()
    {
        stage = -1;
        Destroy(left);
        Destroy(right);
        yield return new WaitForSeconds(0.5f);

        float minY = P.pocP(-0.1f, Side.H);
        while (leftDude.position.y > minY || rightDude.position.y > minY)
        {
            yield return new WaitForEndOfFrame();
        }

        reset.SetActive(true);

        plate = Instantiate(Resources.Load<GameObject>("Plate"));
        Trans plTrans = plate.GetComponent<Trans>();
        plTrans.SetPos(new Vector2(0f, 0.42f));
        plTrans.ForceUpdatePos();

        reset.GetComponent<Reset>().plate = plate;

        fakePlate = Instantiate(Resources.Load<GameObject>("FakePlate"));
        //TODO mettre un reset button
        stage = STAGE_3;
    }

    public void Reset()
    {
        plate.GetComponent<Trans>().SetPos(new Vector2(0f, 0.42f));
        plate.transform.rotation = Quaternion.identity;
        leftDude.GetComponent<Trans>().SetPos(new Vector2(-0.52f, -0.17f));
        rightDude.GetComponent<Trans>().SetPos(new Vector2(0.52f, -0.17f));
    }

    void End()
    {
        ControlManager.Reset();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Startup");
    }
}
