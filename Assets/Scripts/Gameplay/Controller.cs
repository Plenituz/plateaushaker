using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    public Rigidbody2D left;
    public Rigidbody2D right;
    public ParticleSystem jumpParticleRight;
    public ParticleSystem jumpParticleLeft;
    public float strenght = 2f;
    private SpriteRenderer srRight;
    private SpriteRenderer srLeft;
    private Vector3 offsetParticle;

    void Awake()
    {
        ControlManager.Init();
        ControlManager.GetInstance().CreateEvent("LeftClick");
        ControlManager.GetInstance().CreateEvent("RightClick");
        srRight = right.GetComponent<SpriteRenderer>();
        srLeft = left.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        float y = right.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        offsetParticle = new Vector3(0f, y);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ActionLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ActionRight();
        }


        if(left.velocity.y > 0 && !srLeft.sprite.name.Contains("up"))
            srLeft.sprite = Resources.Load<Sprite>("Sprites/" + srLeft.sprite.name[0] + "_noel_up");
        if (left.velocity.y <= 0 && !srLeft.sprite.name.Contains("down"))
            srLeft.sprite = Resources.Load<Sprite>("Sprites/" + srLeft.sprite.name[0] + "_noel_down");

        if (right.velocity.y > 0 && !srRight.sprite.name.Contains("up"))
            srRight.sprite = Resources.Load<Sprite>("Sprites/" + srRight.sprite.name[0] + "_noel_up");
        if (right.velocity.y <= 0 && !srRight.sprite.name.Contains("down"))
            srRight.sprite = Resources.Load<Sprite>("Sprites/" + srRight.sprite.name[0] + "_noel_down");
    }

    public void ClickRight()
    {
        if(enabled)
            ActionRight();
    }

    public void ClickLeft()
    {
        if(enabled)
            ActionLeft();
    }

    void ActionRight()
    {
        right.velocity = (Vector2.up * strenght);
        ControlManager.GetInstance().TriggerEvent("RightClick", null);

        jumpParticleRight.transform.position = right.transform.position - offsetParticle;
        jumpParticleRight.Play();
    }

    void ActionLeft()
    {
        left.velocity = (Vector2.up * strenght);
        ControlManager.GetInstance().TriggerEvent("LeftClick", null);

        jumpParticleLeft.transform.position = left.transform.position - offsetParticle;
        jumpParticleLeft.Play();
    }
}
