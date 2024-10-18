/*
 * Created by SharpDevelop.
 * User: PC
 * Date: 11/03/2023
 * Time: 03:28 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace fase1
{
	class Program
	{
		public static void Main(string[] args)
		{
			//VARIABLES
			repositorio DB=new repositorio();
			String archivo;
			char menu;
			int n,f;
			
			// LEER
			Console.WriteLine("--POR FAVOR INTRODUZCA LA RUTA DEL ARCHIVO CON SU EXTENSION: ");
			archivo=Console.ReadLine();
			Console.WriteLine("\nLEYENDO ARCHIVO...");
			fileReader.llenarRepositorio(archivo,DB);
			DB.setTam();
			Console.WriteLine("		SE ENCONTRARON: "+DB.totalTuplas()+" DATOS, CON: " +DB.getTamNum()+" ATRIBUTOS NUMERICOS Y " +DB.getTamNom()+" ATRIBUTOS NOMINALES");
			
			//Desde la lectura, usando la cabecera, determinamos cuales atributos son nominales y cuales numericos.
			//LIMPIAR
			DB.suavizado();
			Console.WriteLine("REPOSITORIO LIMPIADO");
			
			//INTGRACION
			Console.WriteLine("\n--DESEA REALIZAR GENERAR UN DATAMART? ( SI-S    N-NO )");
			menu=(char)Console.Read();
			if(menu=='S'||menu=='s'){
				Console.WriteLine("\nGENERANDO ANALISIS DE CORRELACION...");
				DB.genDataMart();
			}
			menu=(char)Console.Read();
			menu=(char)Console.Read();
			//IMPRIMIR DATOS (Por diversion)
			/*foreach (dato d in DB.dataset()) {
				Console.WriteLine(d.toString());
			}*/
			
			//MINERIA
			Console.WriteLine("\n--INTRODUZCA LA CANTIDAD DE VECINOS");
			n=Convert.ToInt16(Console.ReadLine());
			Console.WriteLine("--INTRODUZCA LA CANTIDAD DE PLIEGUES");
			f=Convert.ToInt16(Console.ReadLine());
			
			Console.WriteLine("--DESEA USAR MUESTREO ESTRATIFICADO O ALEATORIO? ( ESTRATIFICADO-E    ALEATORIO-A )");
			menu=(char)Console.Read();
			//7 PLIEGUES Y 50 VECINOS
			Console.WriteLine("\nMINANDO DATOS....");
			if(menu=='e'||menu=='E'){
				Console.WriteLine("PRECISION DEL MINADO:  " + kdd.kNN(DB,n,f));
			}
			if (menu=='a'||menu=='A'){	
				kdd.knnRandom(DB,n,f);
			}
			Console.Write("\nPress any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}