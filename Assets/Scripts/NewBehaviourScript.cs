using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Para ver como se hacen UIs ver 
https://www.youtube.com/watch?v=_RIsfVOqTaE
https://www.youtube.com/watch?v=EDh2DGgSN1Y
https://www.youtube.com/watch?v=TYzdhiRiKd0 // Asociar un método al evento on click de un botón
https://www.youtube.com/watch?v=JoMb2rbYEnk // Asociar un método al evento on click de un botón

*/


public class NewBehaviourScript : MonoBehaviour
{
    
    private string URLFilePath = "C:\\Users\\jorge\\OneDrive\\Documentos\\ETSIAE\\4\\PROGRAMACION_GRAFICA\\TrabajoAR\\BBDD\\3le.txt";
    public float[][] parametros;
    public OrbitBehaviour orbita;
    public GameObject[] spheres;
    public OrbitBehaviour[] orbitas;


    private void Awake()
    {
        LoadFile();
        //orbitas = new OrbitBehaviour[parametros.Length];
        
    }

    void Start()
    {
        //CreacionCalculadora();
        
        CreateSatellites();
        

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        MovimientoSatelites();

        
    }

    public void LoadFile()
    {
        SatelliteContainer satLista = new SatelliteContainer(this.URLFilePath);
        satLista.processFile();
        parametros = satLista.GettingParameters();
        
    }

    public GameObject[] CreateSatellites()
    {
        GameObject[] sats = new GameObject[parametros.Length];
        GameObject sphereToCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
        for(int i = 0; i < parametros.Length; i++)
        {
            GameObject sp = GameObject.Instantiate(sphereToCopy);
            orbita.CalculoOrbitaSat(parametros[i][3],
            parametros[i][6],
            parametros[i][1],
            parametros[i][2],
            parametros[i][4],
            parametros[i][5]);
            sp.transform.localScale = transform.parent.localScale / 5;
            sp.transform.position = transform.parent.position + orbita.posVec;
            sats[i] = sp;

        }
        GameObject.Destroy(sphereToCopy);
        spheres = sats;

        return spheres;
    }

    public void MovimientoSatelites()
    {
        for(int i = 0; i < spheres.Length;i++)
        {
            parametros[i][5] += 360 * (Time.deltaTime / (2*Mathf.PI/parametros[i][6]));
            
            orbita.CalculoOrbitaSat(parametros[i][3],
                parametros[i][6],
                parametros[i][1],
                parametros[i][2],
                parametros[i][4],
                parametros[i][5] * 1000f);
            spheres[i].transform.position = transform.parent.position + orbita.posVec;
        }
    }

    public void CreacionCalculadora() //Este lo dejo aqui pero en principio no hace falta
    {
        OrbitBehaviour[] calculadoras = new OrbitBehaviour[parametros.Length];
        OrbitBehaviour calculadorToCopy = orbita;
        for(int i = 0; i < parametros.Length; i++)
        {
            OrbitBehaviour calc = GameObject.Instantiate(calculadorToCopy);
            calculadoras[i] = calc;
        }
        orbitas = calculadoras;
        
    }
}
