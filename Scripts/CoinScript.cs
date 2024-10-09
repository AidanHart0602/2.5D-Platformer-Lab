using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindFirstObjectByType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _uiManager.ScoreVal += 1;
            Destroy(this.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
               
    }
}
