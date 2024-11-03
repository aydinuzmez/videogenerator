using UnityEngine;
using TMPro;

public class scene1 : MonoBehaviour
{
    public TMP_InputField inputField; // Hedef TMP_InputField
    public sss data; // ScriptableObject referansı

    private void Start()
    {
        // Oyun başladığında metni güncelle
        if (inputField != null && data != null)
        {
            inputField.text = data.inputText;
            Debug.Log($"TMP InputField güncellendi: {data.inputText}");
        }
    }
}