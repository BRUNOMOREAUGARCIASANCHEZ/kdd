/*
 * Created by SharpDevelop.
 * User: PC
 * Date: 11/03/2023
 * Time: 03:31 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;


namespace fase1
{
	/// <summary>
	/// Description of fileReader.
	/// </summary>
	public static class fileReader
	{
		
		
		
		public static void llenarRepositorio(String nombreArchivo,repositorio bd){
			String line;
			dato tupla;
			String [] arreglo,cabecera;
			
			try
			{
			    StreamReader sr = new StreamReader(nombreArchivo);  
			    line = sr.ReadLine();
			    //AQUI IRIA LA LECTURA DE ENCABEZADO{}
			    cabecera=line.Split(',');
			   	
			    
			    //RECOGER LOS DATOS
			    while ((line=sr.ReadLine()) != null)
			    {
			    	//Console.WriteLine(line);
			    	arreglo=line.Split(',');
			    	tupla=new dato();
			    	for(int i=0; i<arreglo.Length-1;i++){
			    		String s=arreglo[i];
			    		
			    		if(cabecera[i].Equals("0")){
			    			try{
			    				tupla.addFloat((float)Convert.ToDouble(s));	
			    			}catch(Exception e){
			    				Console.WriteLine("ERROR AL INGRESAR VALOR");
			    			}
			    		}else{
			    			//COMPROBAR QUE ES UNO DE LOS 14 VALORES POSIBLES (?}
			    			try {
			    				if( ( (int)Convert.ToInt16(s) < (int)Convert.ToInt16(cabecera[i])) && ((int)Convert.ToInt16(s)>=0) ){
				    				tupla.addNom(s);
				    			}else{
				    				tupla.addNom(" ");
				    				Console.WriteLine("SE ENCNTRO ANOMALIA STRING");
				    			}	
			    			} catch (Exception e) {
			    				tupla.addNom(" ");
			    				Console.WriteLine("SE ENCNTRE ANOMALIA STRING (CATCH)");
			    			}
			    			
			    		}
			    	}
			    	
			    	tupla.addClass(arreglo[arreglo.Length-1]);
					
			    	bd.agregarRegistro(tupla);
			    	//Console.WriteLine(tupla.toString());
			        //Console.WriteLine(line); 
			    }
			    //close the file
			    sr.Close();
			    //Console.ReadLine();
			    
			}
			catch(Exception e)
			{
			    Console.WriteLine("Exception: " + e.Message);
			}
			finally
			{
			    //Console.WriteLine("Executing finally block.");
			}
		}
	
		public static void llenarRepositorio2(String nombreArchivo,repositorio bd){
			String line;
			dato tupla;
			try
			{
			    StreamReader sr = new StreamReader(nombreArchivo);  
			    line = sr.ReadLine();
			    //AQUI IRIA LA LECTURA DE ENCABEZADO{}
			    while (line != null)
			    {
			    	//Console.WriteLine(line);
			    	String [] arreglo=line.Split(',');
			    	tupla=new dato();
			    	
			    	for(int i=0; i<arreglo.Length-1;i++){
			    		String s=arreglo[i];
			    		try{
			    			tupla.addFloat((float)Convert.ToDouble(s));
			    		}catch(System.FormatException e){
			    			//Console.WriteLine(e.ToString());
			    			tupla.addNom(s);
			    		}
			    	}
			    	
			    	tupla.addClass(arreglo[arreglo.Length-1]);
					
			    	bd.agregarRegistro(tupla);
			    	//Console.WriteLine(tupla.toString());
			        //Console.WriteLine(line);
			        line = sr.ReadLine();
			    }
			    //close the file
			    sr.Close();
			    //Console.ReadLine();
			    
			}
			catch(Exception e)
			{
			    Console.WriteLine("Exception: " + e.Message);
			}
			finally
			{
			    //Console.WriteLine("Executing finally block.");
			}
		}
		
	}
	
}
