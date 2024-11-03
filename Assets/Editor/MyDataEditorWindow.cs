using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class MyDataEditorWindow : EditorWindow
{
    [SerializeField]
    private MyData myData;

    [MenuItem("Window/My Data Editor")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<MyDataEditorWindow>();
        wnd.titleContent = new GUIContent("My Data Editor");
    }

    public void CreateGUI()
    {
        // Root element için padding
        var root = rootVisualElement;
        root.style.paddingTop = 10;
        root.style.paddingBottom = 10;
        root.style.paddingLeft = 10;
        root.style.paddingRight = 10;

        // Başlık etiketi
        var titleLabel = new Label("My Data Editor");
        titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        titleLabel.style.fontSize = 14;
        root.Add(titleLabel);



        // TextField (Text alanı)
        var textField = new TextField("Text");
        root.Add(textField);

        // IntegerField (Number alanı)
        var numberField = new IntegerField("Number");
        root.Add(numberField);

        var button1 = new Button();
        button1.text = "GameView Screenshot";
        button1.clicked += aa;
        button1.style.width = 200;
        button1.style.height = 35;
        root.Add(button1);



        // Text alanındaki değişiklikleri güncelle
        textField.RegisterValueChangedCallback(evt =>
        {
            if (myData != null)
            {
                myData.text = evt.newValue;
                EditorUtility.SetDirty(myData);
            }
        });

        // Number alanındaki değişiklikleri güncelle
        numberField.RegisterValueChangedCallback(evt =>
        {
            if (myData != null)
            {
                myData.number = evt.newValue;
                EditorUtility.SetDirty(myData);
            }
        });


    }

    private void aa()
    {
        // Dosya adı ve yolu
        string filePath = Application.dataPath + "/Screenshots/screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        myData.text = filePath;
        // Dosya yolunu oluştur
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Screenshots");

        // Ekran görüntüsünü yakala ve kaydet
        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log("Ekran görüntüsü kaydedildi: " + filePath);
    }

    private void UpdateFields(TextField textField, IntegerField numberField)
    {
        // ScriptableObject seçildiğinde alanları güncelle
        if (myData != null)
        {
            textField.value = myData.text;
            numberField.value = myData.number;
        }
        else
        {
            textField.value = "";
            numberField.value = 0;
        }
    }
}
