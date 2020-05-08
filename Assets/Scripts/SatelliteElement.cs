using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class SatelliteElement{
    private string _name;
    public string SatName{
        get{return this._name;}
        set{this._name = value;}
    }
    private float _satelliteCatalogNumber;
    public float SatelliteCatalogNumber
    {
        get { return this._satelliteCatalogNumber; }
        set { this._satelliteCatalogNumber = value; }
    }

    private float _inclination;
    public float Inclination
    {
        get { return this._inclination; }
        set { this._inclination = value; }
    }
    private float _rightAscension;
    public float RightAscension
    {
        get { return this._rightAscension; }
        set { this._rightAscension = value; }
    }
    private float _eccentricity;
    public float Eccentricity
    {
        get { return this._eccentricity; }
        set { this._eccentricity = value; }
    }
    private float _argumentOfPerigee;
    public float ArgumentOfPerigee
    {
        get { return this._argumentOfPerigee; }
        set { this._argumentOfPerigee = value; }
    }
    private float _meanAnomaly;
    public float MeanAnomaly
    {
        get { return this._meanAnomaly; }
        set { this._meanAnomaly = value; }
    }
    private float _meanMotion;
    public float MeanMotion
    {
        get { return this._meanMotion; }
        set { this._meanMotion = value; }
    }
    private float _revolutionNumber;
    public float RevolutionNumber
    {
        get { return this._revolutionNumber; }
        set { this._revolutionNumber = value; }
    }
    private float _checkSum;
    public float CheckSum
    {
        get { return this._checkSum; }
        set { this._checkSum = value; }
    }
      
    /*    
    Field	Columns	Content	Example
    1	01–01	Line number	2
    2	03–07	Satellite Catalog number	25544
    3	09–16	Inclination (degrees)	51.6416
    4	18–25	Right Ascension of the Ascending Node (degrees)	247.4627
    5	27–33	Eccentricity (decimal point assumed)	0006703
    6	35–42	Argument of Perigee (degrees)	130.5360
    7	44–51	Mean Anomaly (degrees)	325.0288
    8	53–63	Mean Motion (revolutions per day)	15.72125391
    9	64–68	Revolution number at epoch (revolutions)	56353
    10	69–69	Checksum (modulo 10)	7
    */

    

    public void ProcessFirstLine(string line)
    {
        String[] separator = { " "}; 
        Int32 count = 2; 
        string[] lineArr = line.Split(separator,count, StringSplitOptions.None);
        this.SatName = lineArr[1];
    }
    public void ProcessSecondLine(string line)
    {
        
        
        this.SatelliteCatalogNumber = Convert.ToSingle(line.Substring(2, 6));      
        this.Inclination = Convert.ToSingle(line.Substring(8, 8))/10000f;
        this.RightAscension = Convert.ToSingle(line.Substring(17, 8))/10000f;
        this.Eccentricity = Convert.ToSingle(line.Substring(26, 7))/1000000f;
        this.ArgumentOfPerigee = Convert.ToSingle(line.Substring(34, 8))/10000f;
        this.MeanAnomaly = Convert.ToSingle(line.Substring(43, 8))/10000f;
        this.MeanMotion = Convert.ToSingle(line.Substring(52, 11))/100000000F;
        this.RevolutionNumber = Convert.ToSingle(line.Substring(63, 5));
        this.CheckSum = Convert.ToSingle(line.Substring(68, 1));
        
    }

    public float[] Agrupar()
    {
        float[] parameters = new float[] {
                this.SatelliteCatalogNumber,
                this.Inclination,
                this.RightAscension,
                this.Eccentricity,
                this.ArgumentOfPerigee,
                this.MeanAnomaly,
                this.MeanMotion,
                this.RevolutionNumber,
                this.CheckSum };
        return parameters;
    }
    
}