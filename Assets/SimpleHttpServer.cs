using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHttpServer : MonoBehaviour
{
    private HttpListener httpListener;
    private Thread listenerThread;
    private bool isRunning;

    [SerializeField]
    private TextMeshProUGUI text1;


    [System.Serializable]
    public class Person
    {
        public string message;
    }


    // Ana iş parçacığına veri aktarmak için kuyruk
    private ConcurrentQueue<string> requestQueue = new ConcurrentQueue<string>();

    private void Start()
    {
        StartServer("http://localhost:8080/");

    }

    private void OnTextChanged(string newText)
    {
        // Text bileşenine yeni metni ata
        text1.text = newText;
    }

    private void OnDestroy()
    {
        StopServer();
    }

    public void StartServer(string url)
    {
        httpListener = new HttpListener();
        httpListener.Prefixes.Add(url);
        isRunning = true;

        httpListener.Start();
        Debug.Log("Sunucu başlatıldı: " + url);

        listenerThread = new Thread(ListenForRequests);
        listenerThread.Start();
    }

    private void ListenForRequests()
    {
        while (isRunning)
        {
            try
            {
                HttpListenerContext context = httpListener.GetContext();
                ProcessRequest(context);
            }
            catch (Exception e)
            {
                Debug.LogError("Hata: " + e.Message);
            }
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        if (context.Request.HttpMethod == "POST")
        {
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                // JSON formatındaki veriyi oku
                string json = reader.ReadToEnd();
                Debug.Log("Gelen veri (iş parçacığı): " + json);
                // Veriyi kuyruğa ekle
                requestQueue.Enqueue(json);

                // Yanıt olarak gönderilecek metin
                string responseString = "Veri alındı!";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
        else if (context.Request.HttpMethod == "GET")
        {
            // HTML dosyasını oku ve yanıt olarak döndür
            string htmlPath = Path.Combine(Application.streamingAssetsPath, "index.html");

            if (File.Exists(htmlPath))
            {
                string htmlContent = File.ReadAllText(htmlPath);
                byte[] buffer = Encoding.UTF8.GetBytes(htmlContent);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                byte[] buffer = Encoding.UTF8.GetBytes("<html><body><h1>HTML dosyası bulunamadı!</h1></body></html>");
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }

        context.Response.OutputStream.Close();
    }



    private void Update()
    {
        // Ana iş parçacığında kuyruğu kontrol et
        while (requestQueue.TryDequeue(out string json))
        {
            Debug.Log("Gelen veri (ana iş parçacığı): " + json);

            Person person = JsonUtility.FromJson<Person>(json);
            text1.text = person.message;

            aaa();


            // Gelen JSON verisini burada işleyebilirsiniz
        }
    }

    private void aaa()
    {
        // Dosya adı ve yolu
        string filePath = Application.dataPath + "/Screenshots/screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        // Dosya yolunu oluştur
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Screenshots");

        // Ekran görüntüsünü yakala ve kaydet
        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log("Ekran görüntüsü kaydedildi: " + filePath);
    }

    public void StopServer()
    {
        isRunning = false;
        httpListener?.Stop();
        httpListener = null;

        listenerThread?.Abort();
        listenerThread = null;

        Debug.Log("Sunucu durduruldu.");
    }
}
