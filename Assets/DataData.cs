using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataData : MonoBehaviour
{
    [SerializeField]
    MyData sss;
    public TextMeshProUGUI text1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text1.text = sss.text;
    }
}
