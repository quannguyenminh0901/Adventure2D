using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip chooseSound;
    private RectTransform rect;
    private int currentPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePos(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePos(1);

        //
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            ChoosePos();
    }
    private void ChangePos(int _change)
    {
        currentPos += _change;

        if(_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if(currentPos < 0)
        {
            currentPos = buttons.Length - 1;
        }
        else if (currentPos > buttons.Length -1)
        {
            currentPos = 0;
        }

        //
        rect.position = new Vector3(rect.position.x, buttons[currentPos].position.y, 0);
    }

    private void ChoosePos()
    {
        SoundManager.instance.PlaySound(chooseSound);

        //
        buttons[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
