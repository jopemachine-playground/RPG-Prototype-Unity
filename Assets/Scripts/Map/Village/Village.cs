using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : Scene
{
    // 유니티에서 셋팅
    public GameObject EntryPointFromDungeon;
    public GameObject EntryPointFromHouse;
    public Camera cam;

    private void Start()
    {
        base.PlayerInit();
        base.ScreenCoverInit();
        playerControl.MoveSpeed = 10;
        playerControl.JumpPower = 0;
        playerControl.CameraChange(cam.transform);

        switch (previousScene)
        {
            case "MyHouse":
                Goto(EntryPointFromHouse.transform.position);
                break;
            case "Dungeon":
                Goto(EntryPointFromDungeon.transform.position);
                break;
            default:
                player.transform.position = EntryPointFromDungeon.transform.position;
                break;
        }
    }

    #region Handle A Button Clicked Events At UI Emerging Points
    // 아래의 메서드 네임은 UI Emerging Points 내 자식 컴포넌트들의 이름과 동일해야 함

    public void GotoDungeon()
    {

    }

    public void GotoMyHouse()
    {
        MoveScene("MyHouse");
    }

    public void Shop()
    {

    }

    #endregion



}

