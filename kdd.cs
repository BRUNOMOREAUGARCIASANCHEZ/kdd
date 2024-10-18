/*
 * Created by SharpDevelop.
 * User: PC
 * Date: 11/03/2023
 * Time: 07:54 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace fase1
{
	/// <summary>
	/// Aca iran todas las funciones para los distintos pasos del proceso KDD
	/// </summary>
	public static class kdd
	{
			
								//(dataset a aevaluar, posicion del atributo 1, pos atributo 2)
		public static float correlacionPearson(repositorio bd,int a1, int a2){
			float pX=bd.promedio(a1);
			float pY=bd.promedio(a2);
			float varX=bd.varianza(a1);
			float varY=bd.varianza(a2);
			float r=0;
			
			foreach (dato d in bd.dataset()) {
				//r=r+((d.getAtrNum(a1)*d.getAtrNum(a2))-(bd.dataset().Count*pX*pY));
				r=r+(d.getAtrNum(a1)*d.getAtrNum(a2));
			}
			r=r-(bd.dataset().Count*pX*pY);
			return r/(varX*varY*bd.dataset().Count);
			
		}
								
		public static bool correlacionChiCuad(repositorio bd, int a1, int a2){
			List<String> A = bd.valoresPosibles(a1);
			List<String> B = bd.valoresPosibles(a2);
			List<dato> dataset = bd.dataset();
			int countA=0;
			int countB=0;
			int e;
			int n=bd.totalTuplas();
			int o=0;
			float chi=0;

			foreach (String a in A) {
				foreach (String b in B) {
					foreach (dato d in dataset) {
						if( a.Equals(d.getAtrNom(a1)) ){
							countA++;
						}
						if( b.Equals(d.getAtrNom(a2)) ){
							countB++;
						}
						if( ( a.Equals(d.getAtrNom(a1)) ) && ( b.Equals(d.getAtrNom(a2)) ) )
							o++;
					}
					e=(countA*countB)/n;
					chi=chi+(float)( Math.Pow(o-e,2)/e );
					
					countA=0;
					countB=0;
					o=0;
						
				}
			}
			//degree of freedom (A.Count-1)*(B.Count-1)
			//alpha=0.001
			//SE CONSIDERAN LA SIGNIFICACNCIA COMO 1-LV
			//Console.WriteLine("densidad-------------------------"+ChiSquared.InvCDF(2,1-0.05));
			//VERDADERO RECHAZAMOS LA HIPOTESIS "SON INDEPENDIENTES", ES DECIR ESTÁN CORRELACIONAODS
			return (chi>ChiSquared.InvCDF((A.Count-1)*(B.Count-1),1-0.001));
		}
		
		//fold DEBE SER MENOR QUE EL TOTAL DE TUPLAS Y DE PREFERENCIA MENOR QUE LA CANTIDAD DE ELEMENTOS DE CLASE MINIMA
		//1<fold<|CLASE MINIMA|
		//K NO PUEDE SER MAYOR QUE LA CANTIDAD DE DATOS EN CADA DOBLES
		public static float kNN(repositorio bd,int k, int fold){
			int tuplasFold=(int)Math.Round((float)(bd.totalTuplas()/fold));  //CANTIDAD DE TUPLAS POR CADA DOBLES
			int tam=0;
			int count=0;
			int aciertos=0;
			float distancia=0;
			List<List<dato>> subC=new List<List<dato>>(); //LISTA CON TODOS LOS DOBLECES
			float precision=0;
			int auxiliarXD=0;
			
			for(int i=0; i<fold;i++){
				subC.Add(new List<dato>());
			}
			Console.WriteLine("		SE GENERARON "+ subC.Count+" PLIEGUES, CON UN TAMAÑO APROXIMADO DE "+tuplasFold+" ELEMENTOS");
			List <String> clases=bd.clasesPosibles();
			Console.WriteLine("		SE DETECTARON: "+clases.Count+" CLASES");
			//REP ES DE DONDE SE VAN A IR TOMANDO LOS DATOS
			List<List<dato>> rep=new List<List<dato>>();
			List<dato> aux=new List<dato>();
			
			foreach (String c in clases) {
				aux=bd.listaClase(c);
				rep.Add(aux);
			}
			//CADA ELEMENTO DE REP ES UNA LISTA CON UNA CLASE EN ESPECIFICO
			do{
				aux=rep[0];
				tam=aux.Count;
				
				for(int i=0; i<tam; i++){
					subC[i%fold].Add(aux[0]);
					aux.RemoveAt(0);
				}
				
				rep.RemoveAt(0); //REP QUEDA VACIA AL FINAL DEL PROCEDIMEINTO
			}while(rep.Count!=0);
			//subC CONTIENE CADA UNO DE LOS DOBLECES
			for(int i=0;i<subC.Count;i++){
				aux=subC[i];
				Console.WriteLine("ANALIZANDO PLIEGUE "+(i+1)+" DE "+ aux.Count +" ELEMENTOS");
				List<dato> knn=new List<dato>();
				List<float> distancias=new List<float>();
				auxiliarXD=0;
				foreach (dato d1 in aux) {
					knn.Clear();
					distancias.Clear();
					foreach (List<dato> pliegue in subC) {
						//Console.WriteLine(subC.Count);
						if(!pliegue.Equals(subC[i])){
							foreach (dato d2 in pliegue) {
								//CALCULAR DISTANCIA
								//ATR NUM
								for(int j=0; j<bd.getTamNum();j++){
									distancia=distancia+(Math.Abs(d1.getAtrNum(j)-d2.getAtrNum(j))/bd.rangoAtr(j));
								}
								//ATR NOM
								for(int j=0; j<bd.getTamNom(); j++){
									if(d1.getAtrNom(j).Equals(d2.getAtrNom(j)))
										distancia=distancia+1;
								}
								if(knn.Count<k){
									knn.Add(d2);
									distancias.Add(distancia);
								}else{
									if(distancia<distancias.Max()){
										knn[distancias.IndexOf(distancias.Max())]=d2;
										distancias[distancias.IndexOf(distancias.Max())]=distancia;
									}
								}
								//Console.WriteLine(distancia); //VER DISTANCIAS
								distancia=0;
							}
						}else{
							Console.WriteLine(auxiliarXD++);
						}
						
						
					}
					
					//COMPARAR LA CLASE DE d1 CON LOS KNN
					count=count+1;
					//Console.WriteLine("CLASE PROBABLE: " +kdd.claseProbable(knn));
					//Console.WriteLine("CLASE REAL: " +d1.getClass());
					if(d1.getClass().Equals(kdd.claseProbable(knn)))
						aciertos=aciertos+1;
					
				}//SIGUIENTE DATO DEL CONJUNTO DE PRUEBA
				
				precision=precision+(float)((decimal)aciertos/(decimal)aux.Count); //SIN EL CASTEO A DECIMAL, LA DIVISION RESULTA 0
				//Console.WriteLine("ACIERTOS: "+aciertos);
				//Console.WriteLine("TOTAL DE EXPERIMENTOS "+aux.Count);
				
			//	Console.WriteLine("PRECISION DEL PLIEGUE: "+((decimal)aciertos/(decimal)aux.Count));
				aciertos=0;
			}
			
			//Console.WriteLine(subC.Count);
			return precision/subC.Count;
		}
		
		
		public static String claseProbable(List<dato> lista){
			int aux=0;
			List<int> cuenta=new List<int>();
			List<String> valores=new List<string>();
			foreach(dato d in lista){
				if(  ( !valores.Contains(d.getClass()) ) ) {
						valores.Add(d.getClass());
						cuenta.Add(1);
					}else{
						aux=valores.IndexOf(d.getClass());
						cuenta[aux]=cuenta[aux]+1;
					}
				
			}
			//LAS FUNCIONES MAX Y MIN PARA LIST REGRESAN EL VALOR, NO EL INDICE
			return valores[cuenta.IndexOf(cuenta.Max())];
		}
		
		public static float knnRandom(repositorio bd,int k, int fold){
			int tuplasFold=(int)Math.Round((float)(bd.totalTuplas()/fold));  //CANTIDAD DE TUPLAS POR CADA DOBLES
			int tam=0;
			int count=0;
			int aciertos=0;
			float distancia=0;
			List<List<int>> subC=new List<List<int>>(); //LISTA CON TODOS LOS DOBLECES
			float precision=0;
			Random r=new Random();
			int auxR=0;
			List<int> aux;
			for(int i=0; i<fold;i++){
				subC.Add(new List<int>());	//CADA MIEMBRO DE subC ES UNA LISTA DE INDICES
			}
			Console.WriteLine("		SE GENERARON "+ subC.Count+" PLIEGUES, CON UN TAMAÑO APROXIMADO DE "+tuplasFold+" ELEMENTOS");
			List <String> clases=bd.clasesPosibles();	
			Console.WriteLine("		SE DETECTARON: "+clases.Count+" CLASES");
			//REP ES DE DONDE SE VAN A IR TOMANDO LOS DATOS
			List<dato> rep=bd.dataset();
			
			List<int> indices=new List<int>();
			
			for (int i = 0; i < rep.Count; i++) {
				indices.Add(i+1);	//LOS INDICES VAN DE 1 A "CANTIDAD DE DATOS"
			}
			
			do{
				foreach (List<int> f in subC) {	//A CADA DOBLEZ SE LE AGEGA UN INDICE DISTINTO 
					if(indices.Count>0){
						auxR=r.Next(0,indices.Count);
						f.Add(indices[auxR]);
						indices.RemoveAt(auxR);
					}
				}
			}while (indices.Count!=0); 
			
			//subC CONTIENE CADA UNO DE LOS DOBLECES
			for(int i=0;i<subC.Count;i++){
				aux=subC[i];
				Console.WriteLine("ANALIZANDO PLIEGUE "+(i+1)+" DE "+ aux.Count +" ELEMENTOS");
				List<dato> knn=new List<dato>();
				List<float> distancias=new List<float>();
				
				foreach (int ind1 in aux) {
					dato d1=rep[ind1-1];
					knn.Clear();
					distancias.Clear();
					foreach (List<int> pliegue in subC) {
						//Console.WriteLine(subC.Count);
						if(!pliegue.Equals(subC[i])){
							foreach (int ind2 in pliegue) {
								dato d2=rep[ind2-1];
								//CALCULAR DISTANCIA
								//ATR NUM
								for(int j=0; j<bd.getTamNum();j++){
									distancia=distancia+(Math.Abs(d1.getAtrNum(j)-d2.getAtrNum(j))/bd.rangoAtr(j));
								}
								//ATR NOM
								for(int j=0; j<bd.getTamNom(); j++){
									if(d1.getAtrNom(j).Equals(d2.getAtrNom(j)))
										distancia=distancia+1;
								}
								if(knn.Count<k){
									knn.Add(d2);
									distancias.Add(distancia);
								}else{
									if(distancia<distancias.Max()){
										knn[distancias.IndexOf(distancias.Max())]=d2;
										distancias[distancias.IndexOf(distancias.Max())]=distancia;
									}
								}
								//Console.WriteLine(distancia); //VER DISTANCIAS
								distancia=0;
							}
						}else{
							//Console.WriteLine("HOLI");
						}
						
						
					}
					
					//COMPARAR LA CLASE DE d1 CON LOS KNN
					count=count+1;
					//Console.WriteLine("CLASE PROBABLE: " +kdd.claseProbable(knn));
					//Console.WriteLine("CLASE REAL: " +d1.getClass());
					if(d1.getClass().Equals(kdd.claseProbable(knn)))
						aciertos=aciertos+1;
					
				}//SIGUIENTE DATO DEL CONJUNTO DE PRUEBA
				
				precision=precision+(float)((decimal)aciertos/(decimal)aux.Count); //SIN EL CASTEO A DECIMAL, LA DIVISION RESULTA 0
				//Console.WriteLine("ACIERTOS: "+aciertos);
				//Console.WriteLine("TOTAL DE EXPERIMENTOS "+aux.Count);
				
			//	Console.WriteLine("PRECISION DEL PLIEGUE: "+((decimal)aciertos/(decimal)aux.Count));
				aciertos=0;
			}
			
			
			Console.WriteLine("PRECICSION NORMAL: "+precision/subC.Count);
			if(!bd.DM()){
				bd.genDataMart();
				precision=0;
				aciertos=0;
				
				for(int i=0;i<subC.Count;i++){
					aux=subC[i];
					Console.WriteLine("ANALIZANDO PLIEGUE "+(i+1)+" DE "+ aux.Count +" ELEMENTOS");
					List<dato> knn=new List<dato>();
					List<float> distancias=new List<float>();
					
					foreach (int ind1 in aux) {
						dato d1=rep[ind1-1];
						knn.Clear();
						distancias.Clear();
						foreach (List<int> pliegue in subC) {
							//Console.WriteLine(subC.Count);
							if(!pliegue.Equals(subC[i])){
								foreach (int ind2 in pliegue) {
									dato d2=rep[ind2-1];
									//CALCULAR DISTANCIA
									//ATR NUM
									for(int j=0; j<bd.getTamNum();j++){
										distancia=distancia+(Math.Abs(d1.getAtrNum(j)-d2.getAtrNum(j))/bd.rangoAtr(j));
									}
									//ATR NOM
									for(int j=0; j<bd.getTamNom(); j++){
										if(d1.getAtrNom(j).Equals(d2.getAtrNom(j)))
											distancia=distancia+1;
									}
									if(knn.Count<k){
										knn.Add(d2);
										distancias.Add(distancia);
									}else{
										if(distancia<distancias.Max()){
											knn[distancias.IndexOf(distancias.Max())]=d2;
											distancias[distancias.IndexOf(distancias.Max())]=distancia;
										}
									}
									//Console.WriteLine(distancia); //VER DISTANCIAS
									distancia=0;
								}
							}else{
								//Console.WriteLine("HOLI");
							}
							
							
						}
						
						//COMPARAR LA CLASE DE d1 CON LOS KNN
						count=count+1;
						//Console.WriteLine("CLASE PROBABLE: " +kdd.claseProbable(knn));
						//Console.WriteLine("CLASE REAL: " +d1.getClass());
						if(d1.getClass().Equals(kdd.claseProbable(knn)))
							aciertos=aciertos+1;
						
					}//SIGUIENTE DATO DEL CONJUNTO DE PRUEBA
					
					precision=precision+(float)((decimal)aciertos/(decimal)aux.Count); //SIN EL CASTEO A DECIMAL, LA DIVISION RESULTA 0
					//Console.WriteLine("ACIERTOS: "+aciertos);
					//Console.WriteLine("TOTAL DE EXPERIMENTOS "+aux.Count);
					
				//	Console.WriteLine("PRECISION DEL PLIEGUE: "+((decimal)aciertos/(decimal)aux.Count));
					aciertos=0;
				}
				//Console.WriteLine(subC.Count);
				Console.WriteLine("PRECICSION datamart "+precision/subC.Count);	
			}
			
			
			
			return precision/subC.Count;
		}
		
		
		
	}
}
