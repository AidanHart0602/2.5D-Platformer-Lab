using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public int ScoreVal;
    [SerializeField]
    private TMP_Text _score;
    // Start is called before the first frame update
    void Start()
    {
        _score.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        _score.text = "Score: " + ScoreVal;
    }
}
