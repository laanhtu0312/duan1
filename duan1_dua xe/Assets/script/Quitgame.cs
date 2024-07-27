using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Hàm này được gọi khi nút được nhấn
    public void OnQuitButtonPressed()
    {
        // Thoát ứng dụng
#if UNITY_EDITOR
        // Nếu đang chạy trong Editor, dừng chơi
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Nếu đang chạy trong build, thoát ứng dụng
        Application.Quit();
#endif
    }
}