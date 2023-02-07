using UnityEngine;

namespace LeapGame
{
    public class CursorStateChanger
    {
        public static void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        public static void Unlock()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        public static void Hide()
        {
            Cursor.visible = false;
        }
        public static void Show()
        {
            Cursor.visible = true;
        }
    }
}