using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class OrbitBehaviour : MonoBehaviour
{
    //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    //DEFINICIÓN DE VARIABLES
    //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

    //Declaración ejes plano y ejes órbita final-----------------------------------------------------------------------------------

    protected Vector3 ejeX = Vector3.right;
    protected Vector3 ejeY = Vector3.up;
    protected Vector3 ejeZ = Vector3.forward;

    protected Vector3 xOrbit;
    protected Vector3 yOrbit;
    /// <summary>
    /// Normal al plano orbital
    /// </summary>
    protected Vector3 zOrbit;

    //Declaración parámetros órbita------------------------------------------------------------------------------------------------

    /// <summary>
    /// Parámetro gravitacional(G*M)
    /// </summary>
    public float MU = 1000f;
    /// <summary>
    /// Excentricidad de la cónica
    /// </summary>
    public float eccentricity;
    /// <summary>
    /// Movimiento medio en revoluciones por dia
    /// </summary>
    public float nMove;
    
    /// <summary>
    /// Inclinación de la órbita en grados
    /// </summary>
    public float inclination;
    /// <summary>
    /// Ascensión recta del nodo ascendente en grados
    /// </summary>
    public float ascNodeLongitude;
    /// <summary>
    /// Argumento del perigeo en grados
    /// </summary>
    public float argOfPerigeo;
    /// <summary>
    /// Anomalía media en grados
    /// </summary>
    public float meanAnomaly;

    /// <summary>
    /// Velocidad de reproduccion del movimiento de la orbita
    /// </summary>
    public float velRep = 2;
    //----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Semieje mayor de la cónica 
    /// </summary>
    public float semiMajorAxis;
    /// <summary>
    /// Semieje menor de la cónica
    /// </summary>
    protected float smi;
    /// <summary>
    /// Dirección de la línea de nodos
    /// </summary>
    protected Vector3 nodeDir;
    /// <summary>
    /// Anomalía excéntrica
    /// </summary>
    protected float eAnom;
    /// <summary>
    /// Anomalía verdadera
    /// </summary>
    protected float tAnom;
    /// <summary>
    /// Período de la órbita
    /// </summary>
    public float periodo;
    /// <summary>
    /// Parámetro de la órbita
    /// </summary>
    protected float param;
    
    /// <summary>
    /// Constante de la energía
    /// </summary>
    protected float energyParam;
    /// <summary>
    /// Ángulo de la velocidad con la horizontal local
    /// </summary>
    protected float vAngle;

    /// <summary>
    /// Centro de la órbita
    /// </summary>
    protected Vector3 centroOrbit;
    /// <summary>
    /// Apogeo de la órbita
    /// </summary>
    protected Vector3 apogeo;
    /// <summary>
    /// Perigeo de la órbita
    /// </summary>
    protected Vector3 perigeo;




    //Declaración  posición y velocidad del satélite-------------------------------------------------------------------------------

    /// <summary>
    /// Vector posición del satélite respecto a la Tierra
    /// </summary>
    public Vector3 posVec;
    /// <summary>
    /// Distancia del satélite respecto a la Tierra
    /// </summary>
    public float distancia;
    /// <summary>
    /// Vector velocidad del satélite respecto a la Tierra
    /// </summary>
    public Vector3 velVec;
    /// <summary>
    /// Velocidad del satélite respecto a la Tierra
    /// </summary>
    public float vel;




    /*private void Start()
    {

        CalculoOrbitaSat(eccentricity, nMove, inclination, ascNodeLongitude, argOfPerigeo, meanAnomaly);
        this.transform.position = transform.parent.position + posVec;
        




    }

    private void Update()
    {

        meanAnomaly += 360 * (Time.deltaTime / periodo) * velRep;
        
        CalculoOrbitaSat(eccentricity, nMove, inclination, ascNodeLongitude, argOfPerigeo, meanAnomaly);
        this.transform.position = transform.parent.position + posVec;
        



    }*/



    /// <summary>
    /// Cálculos los diversos parámetros de la órbita a partir de unos datos dados
    /// </summary>
    /// <param name="eccentricity">Excentricidad de la cónica(<1)</param>
    /// <param name="meanMotion">Movimiento medio en revoluciones/dia</param>
    /// <param name="inclination">Inclinacion en grados</param>
    /// <param name="raan">RAAN en grados</param>
    /// <param name="argPerigeo">Argumento del perigeo en grados</param>
    /// <param name="meanAnomaly">Anomalia media en grados</param>
    public void CalculoOrbitaSat(float eccentricity, float meanMotion, float inclination, float raan, float argPerigeo, float meanAnomaly)
    {

        //Asignamos las variables del métodos a la variables inicialmente designadas
        float ecc = eccentricity;
        float nMove = meanMotion * 7.2722E-05f; //Para pasarlo de rev/dia a rad/s
        
        float ascNodeLong = raan * Mathf.Deg2Rad; //Lo convertimos a radianes para operar
        float inc = inclination * Mathf.Deg2Rad; //Lo convertimos a radianes para operar
        float argPer = argPerigeo * Mathf.Deg2Rad; //Lo convertimos a radianes para operar
        float mAnom = meanAnomaly * Mathf.Deg2Rad; //Lo convertimos a radianes para operar


        
        //nMove = Mathf.Pow(MU / Mathf.Pow(sma, 3f), 0.5f);
        periodo = 2 * Mathf.PI / nMove;
        float sma = Mathf.Pow(MU / (nMove * nMove), 1 / 3f);
        semiMajorAxis = sma;
        param = sma * (1 - ecc * ecc);
        energyParam = -MU / (2 * sma);
        //Calculamos semieje menor, movimiento medio, periodo, parámetro de la órbita y constante de energía
        smi = sma * Mathf.Pow(1 - ecc * ecc, 0.5f);

        //Calculamos las anomalías excéntrica y verdadera a partir de la media
        eAnom = CalcularEAnomaly(ecc, mAnom) * Mathf.Deg2Rad;
        tAnom = 2 * Mathf.Atan(Mathf.Pow((1 + ecc) / (1 - ecc), 0.5f) * Mathf.Tan(eAnom / 2));

        //Obtenemos los ejes de la órbita
        nodeDir = RotateByAngle(ejeX.normalized, ascNodeLong, ejeZ.normalized).normalized;
        zOrbit = RotateByAngle(ejeZ.normalized, inc, nodeDir).normalized;
        xOrbit = RotateByAngle(nodeDir, argPer, zOrbit).normalized;
        yOrbit = Vector3.Cross(zOrbit, xOrbit).normalized;

        //Calculamos el centro de la órbita, perigeo y apogeo
        centroOrbit = -ecc * sma * xOrbit;
        perigeo = centroOrbit + sma * xOrbit;
        apogeo = centroOrbit - sma * xOrbit;

        //Hacemos la parametrización de las coordenadas 
        Vector3 posPlano = new Vector3(sma * Mathf.Cos(eAnom), smi * Mathf.Sin(eAnom), 0);

        //Pasamos las coordenadas al plano de la órbita
        posPlano = RotateByAngle(posPlano, ascNodeLong, ejeZ);
        posPlano = RotateByAngle(posPlano, inc, nodeDir);
        posPlano = RotateByAngle(posPlano, argPer, zOrbit);

        posVec = posPlano - ecc * sma * xOrbit;
        distancia = posVec.magnitude;

        //Obtenemos el ángulo del vector velocidad respecto al vector posicion, el vector velocidad y su magnitud
        vAngle = Mathf.Atan((1 - distancia / param) * Mathf.Tan(tAnom));
        Vector3 dirVel = posVec.normalized / Mathf.Cos(Mathf.PI / 2 - vAngle);
        vel = Mathf.Pow(MU * (2 / distancia - 1 / sma), 0.5f);
        velVec = vel * dirVel.normalized;











    }


    /// <summary>
    /// Obtiene la anomalia excéntrica a partir de la excentricidad y la anomalía media
    /// </summary>
    /// <param name="e">excentricidad</param>
    /// <param name="M">anomalia media</param>
    /// <returns></returns>
    public float CalcularEAnomaly(float e, float M)
    {
        float e0 = e * Mathf.Rad2Deg;
        float E0 = M * Mathf.Rad2Deg;
        float E1 = 0;

        int n = 5;
        for (int i = 1; i < n; i++)
        {
            E1 = E0 + (M * Mathf.Rad2Deg + e0 * Mathf.Sin(E0 * Mathf.Deg2Rad) - E0) / (1 - e * Mathf.Cos(E0 * Mathf.Deg2Rad));

            E0 = E1;
        }


        return E1;
    }


    /// <summary>
    /// Gira un vector alrededor de otro
    /// </summary>
    /// <param name="v">Vector que se gira</param>
    /// <param name="angleRad">Ángulo que se gira</param>
    /// <param name="n">Vector alrededor del que se gira</param>
    /// <returns></returns>
    public Vector3 RotateByAngle(Vector3 v, float angleRad, Vector3 n)
    {

        float cosT = Mathf.Cos(angleRad);
        float sinT = Mathf.Sin(angleRad);
        float oneMinusCos = 1f - cosT;
        // Rotation matrix:
        float a11 = oneMinusCos * n.x * n.x + cosT;
        float a12 = oneMinusCos * n.x * n.y - n.z * sinT;
        float a13 = oneMinusCos * n.x * n.z + n.y * sinT;
        float a21 = oneMinusCos * n.x * n.y + n.z * sinT;
        float a22 = oneMinusCos * n.y * n.y + cosT;
        float a23 = oneMinusCos * n.y * n.z - n.x * sinT;
        float a31 = oneMinusCos * n.x * n.z - n.y * sinT;
        float a32 = oneMinusCos * n.y * n.z + n.x * sinT;
        float a33 = oneMinusCos * n.z * n.z + cosT;
        return new Vector3(v.x * a11 + v.y * a12 + v.z * a13, v.x * a21 + v.y * a22 + v.z * a23, v.x * a31 + v.y * a32 + v.z * a33);




    }
    


    /*public void DibujarSenda()
    {
        TrailRenderer tr = this.gameObject.AddComponent<TrailRenderer>();
        tr.time = 3.0f;
        tr.startWidth = 0.1f;
        tr.endWidth = 0;
        tr.material = senda;
        tr.startColor = Color.yellow;
        tr.endColor = Color.yellow;
    }*/

    /*public void CreateRandomParameters()
    {
        ecc = Random.Range(0.1f, 0.9f);
        sma = Random.Range(3f, 6f);
        inc = Random.Range(0f, 90f);
        ascNodeLong = Random.Range(0f, 90f);
        argPer = Random.Range(0f, 90f);
        mAnom = Random.Range(0f, 360f);


    }*/



}
