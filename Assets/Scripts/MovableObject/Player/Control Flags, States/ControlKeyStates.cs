using UnityEngine;

namespace UnityChanRPG
{
    // true라면 키가 눌린 상태, false라면 눌리지 않은 상태
    static class ControlKeyStates
    {
        static public bool RunningButtonClicked()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        static public bool ArrowButtonClicked()
        {
            return (Input.GetAxis("Horizontal") != 0 | Input.GetAxis("Vertical") != 0);
        }

        static public bool KickAttackButtonClicked()
        {
            return Input.GetButtonDown("KickAttack");
        }

        static public bool DashAttackButtonClicked()
        {
            return 
                Input.GetButtonDown("KickAttack") &&
                Input.GetKey(KeyCode.LeftShift);
        }

    }
}
