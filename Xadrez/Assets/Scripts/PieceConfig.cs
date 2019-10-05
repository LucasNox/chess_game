using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceConfig : MonoBehaviour
{
    public enum Color { blue, red };

    public Color piece_color;
    [SerializeField] PieceMove[] move;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
