using System;
using System.Collections.Generic;
using System.IO;

class Solution {

    static Point posPrinc = new Point(0,0);
    static Point posBoot = new Point(0,0);
    static string letraD = "d";
    static List<CeldaDist> listaObjetos =  new List<CeldaDist>();

	static void nextMove(int posr, int posc, String [] board){
		
        posBoot = new Point(posr,posc);
              
        
        //eliminar Objeto
        int idElim = objetoEnPosicion(posBoot.X,posBoot.Y);
        if(idElim>=0)
        {
            listaObjetos.RemoveAt(idElim);
            Console.WriteLine("CLEAN");
            return;
        }
        
        List<CeldaDist> lista =  new List<CeldaDist>();
        int elemCelda=0;
        
        
        //ubica posiciones
        foreach(CeldaDist x in listaObjetos)
        {
            double dist = obtenerDistancia(posBoot,x.ubic, board.GetLength(0));
            lista.Add(new CeldaDist(elemCelda,x.ubic.X,x.ubic.Y,dist));
            elemCelda++;
        }

        
        if(lista.Count==0)
            return;
        
        //ordena lista
        lista.Sort((x, y) => {
                        int ret = x.dist.CompareTo(y.dist);
                        return ret;
                    });
        
        //posicion mas cercana
        Point PointNew = ((CeldaDist)lista[0]).ubic;
        posPrinc = new Point(PointNew.X,PointNew.Y);

        string comando = "";
            
        comando = obtenerDesplazamiento(ref board, ref posBoot, posPrinc);            
        Console.WriteLine(comando);

        }
    }
    
    static int objetoEnPosicion(int r, int c)
    {
        int idx = 0;
        foreach(CeldaDist x in listaObjetos)
        {
            if(x.ubic.X == r &&  x.ubic.Y == c)
            {
                return idx;
            }
            idx++;
                
        }
        
        return -1;
    }
    
    static string obtenerDesplazamiento(ref String [] grid, ref Point posBoot, Point posPrinc)
    {
        //0:UP, 1:RIGHT, 2:DOWN, 3:LEFT     
        double [] dist = new double [4];    

        dist[0] = obtenerDistancia(new Point(posBoot.X-1,posBoot.Y+0), posPrinc,  dist.GetLength(0));
        dist[1] = obtenerDistancia(new Point(posBoot.X+0,posBoot.Y+1), posPrinc,  dist.GetLength(0));
        dist[2] = obtenerDistancia(new Point(posBoot.X+1,posBoot.Y+0), posPrinc,  dist.GetLength(0));
        dist[3] = obtenerDistancia(new Point(posBoot.X+0,posBoot.Y-1), posPrinc,  dist.GetLength(0));
        
        int idx = obtenerIndice(dist);
        
        switch(idx) 
        {
          case 0:
            posBoot = new Point(posBoot.X-1,posBoot.Y+0);
            return "UP";
          case 1:
            posBoot = new Point(posBoot.X+0,posBoot.Y+1);
            return "RIGHT";

          case 2:
            posBoot = new Point(posBoot.X+1,posBoot.Y+0);
            return "DOWN";
          case 3:
            posBoot = new Point(posBoot.X+0,posBoot.Y-1);
            return "LEFT";
          default:
            return "";
        }
                
    }
    
    static int obtenerIndice(double [] dist){
        
        int idx=-1;
        double valor = double.MaxValue;
		
        for(int i=0;i<dist.GetLength(0);i++)
        {
            double valArreglo = dist[i];
            
            if(valArreglo<valor)
            {
                valor = valArreglo;
                idx = i;                
            }
            
        }
        
        return idx;
            
        
    }
    
    
    static double obtenerDistancia(Point posXY, Point posPrinc, int n){

        if(posXY.X<0 || posXY.X>(n))
            return double.MaxValue;
        
        return Math.Sqrt(Math.Pow(posXY.X-posPrinc.X,2)+Math.Pow(posXY.Y-posPrinc.Y,2));
        
    }
    
    
    static Point obtenerPosic(int n, String [] grid, string letra){
        
        for(int i=0;i<n;i++)
        {
            string cad = grid[i];

            for(int j=0;j<n;j++)
            {                
                if(cad.Substring(j,1)==letra)
                {
                    return new Point(i,j);
                }

            }
        }
        
        return new Point(-1,-1);
              
        
      }
    

	static void Main(String[] args) {
        String temp = Console.ReadLine();
        String[] position = temp.Split(' ');
        int[] pos = new int[2];
        String[] board = new String[5];
        for(int i=0;i<5;i++) {
            board[i] = Console.ReadLine();
        }
        for(int i=0;i<2;i++) pos[i] = Convert.ToInt32(position[i]);
    
    
        //carga objetos a limpiar
        int idLimp=0;
        for(int i=0;i<board.GetLength(0);i++)
        {
            string cad=board[i];
            for(int j=0;j<cad.Length;j++)
            {
                string elem = cad.Substring(j,1);
                
                if(elem==letraD)
                {
                    CeldaDist obj = new CeldaDist(idLimp,i, j, 0);
                    listaObjetos.Add(obj);
                    idLimp++;
                }

            }
        }

        nextMove(pos[0], pos[1], board);
    }
}

public class Point
{
    public int X;
    public int Y;
    
    public Point(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    
}


public class CeldaDist
{        
    public int caso;
    public Point ubic;
    public double dist;
        
    public CeldaDist(int caso,int x, int y, double dist)
    {
        this.caso = caso;
        ubic = new Point(x,y);
        this.dist = dist;
    }        
}
