/*
 * Created by SharpDevelop.
 * User: PC
 * Date: 11/03/2023
 * Time: 04:06 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace fase1
{
	/// <summary>
	/// Description of repositorio.
	/// </summary>
	public class repositorio
	{
		List<dato> registro;	
		int tamNum=0;//CANTIDAD DE ATRIBUTOS NUMERICOS
		int tamNom=0;//CANTIDAD DE ATRIBUTOS NOMINALES
		bool dm=false;

		public repositorio()
		{
			this.registro=new List<dato>();
		}
		public bool DM(){
			return this.dm;
		}
		
		public void agregarRegistro(dato d){
			this.registro.Add(d);
		}
		
		public List<dato> dataset(){
			return this.registro;
		}
		
		public int totalTuplas(){
			return this.registro.Count;
		}
		
		public void setTam(){
			this.tamNom=registro[0].totalNom();
			this.tamNum=registro[0].totalNum();
		}
		public int getTamNum(){
			return this.tamNum;
		}
		public int getTamNom(){
			return this.tamNom;
		}
		public void deleteAtrNum(int x){
			foreach (dato d in registro) {
				d.deleteAtrNum(x);
			}
			this.tamNum-=1;
		}
		public void deleteAtrNom(int x){
			foreach (dato d in registro) {
				d.deleteAtrNom(x);
			}
			this.tamNom-=1;
		}
		
		public float rangoAtr(int a){
			float max=0;
			float min=0;
			List<float> val=new List<float>();
			foreach (dato d in registro) {
				val.Add(d.getAtrNum(a));
			}
			max=val.Max();
			min=val.Min();
			return max-min;
		}
		//PROMEDIO DE EL ATRIBURTO NUMERICO A
		public float promedio(int a){
			float promedio=0;
			foreach (dato d in registro) {
				promedio=promedio+d.getAtrNum(a);
			}
			return promedio/registro.Count;
		}
		//VARIANZA DEL ATRIBUTO NUMERICO A
		public float varianza(int a){
			float promedio=this.promedio(a);
			float varianza=0;
			
			foreach (dato d in registro) {
				
				varianza=varianza+(float)Math.Pow((d.getAtrNum(a)-promedio),2);
			}
			return varianza/registro.Count;
		}
		//PROMEDIO DEL ATRIBUTO NUMRICO A, DE CLASE C
		public float promedioClase(int a,String c){
			float promedio=0;
			int i=0;
			
			foreach (dato d in registro) {
				if(c.Equals(d.getClass())){
					promedio=promedio+d.getAtrNum(a);
					i++;
				}
			}
			if (i!=0) 
				promedio=promedio/i;
			return promedio;
		}
		//MODA DEL ATRIBUTO NOMINAL A, DE CLASE C
		public String modaClase(int a, String c){
			//Lista con todos los valores posibles del atributo A, con clase C
			int aux=0;
			List<int> cuenta=new List<int>();
			List<String> valores=new List<string>();
			foreach(dato d in registro){
				if(c.Equals(d.getClass())){
					if(  ( !valores.Contains(d.getAtrNom(a)) ) ) {
						valores.Add(d.getAtrNom(a));
						cuenta.Add(1);
					}else{
						aux=valores.IndexOf(d.getAtrNom(a));
						cuenta[aux]=cuenta[aux]+1;
					}
				}
				
			}
			//LAS FUNCIONES MAX Y MIN PARA LIST REGRESAN EL VALOR, NO EL INDICE
			return valores[cuenta.IndexOf(cuenta.Max())];
		}
		
		//CALCULAR CUARTILES DEL ATRIBUTO NUMERICO A
		public double q1(int a) {
			List<double> valoresA=new List<double>();
			foreach (dato d in registro) {
				valoresA.Add(d.getAtrNum(a));
			}
			valoresA.Sort();
			return valoresA[(int)Math.Round((double)(tamNum+1)/4)];
		}
		public double q3(int a) {
			List<double> valoresA=new List<double>();
			foreach (dato d in registro) {
				valoresA.Add(d.getAtrNum(a));
			}
			valoresA.Sort();
			return valoresA[(int)Math.Round((double)(3*(tamNum+1))/4)];
		}
		
		//LIMPIEZA: OULTIERS AND NULL
		public void suavizado(){
			double q3=0;
			double q1=0;
			//I RECORRE TODOS LOS "INDICES" DE LOS POSIBLES ATRIBUTOS
			for(int i=0;i<tamNum;i++){
				q1=this.q1(i);
				q3=this.q3(i);
				foreach (dato s in registro) {
					if( (s.getAtrNum(i)<q1-(1.5*(q3-q1))) || (s.getAtrNum(i)>q3+(1.5*(q3-q1))) || (s.getAtrNum(i).Equals(null)) ){
						s.setAtrNum(i,this.promedioClase(i,s.getClass()));
						
					}
				}
					
			}
			
			for(int i=0;i<tamNom;i++){
				foreach (dato s in registro) {
					if( s.getAtrNom(i).Equals(null) || s.getAtrNom(i).Equals("")|| s.getAtrNom(i).Equals(" ")|| (s.getAtrNom(i).Equals("  ")))
						s.setAtrNom(i,this.modaClase(i,s.getClass()));
				}
			}
		}
		
		
		//FUNCION QUE REGRESA TODOS LOS POSIBLES VALORES DISTINTOS DE UN ATRIBUTO NOMINAL
		public List<String> valoresPosibles(int a){
			List<String> valores=new List<string>();
			foreach(dato d in registro){
				if(  !( valores.Contains(d.getAtrNom(a)) )  ) {
					valores.Add(d.getAtrNom(a));
				}
			}
			return valores;
		}
		//FUNCIPN QUE REGRESA TODAS LAS POSIBLES CLASES EN EL REPOSITORIO
		public List<String> clasesPosibles(){
			List<String> valores=new List<string>();
			foreach(dato d in registro){
				if(  !( valores.Contains(d.getClass()) )  ) {
					valores.Add(d.getClass());
				}
			}
			return valores;
		}
		//FUNCION QUE REGRESA UNA LISTA CON TODOS LOS ELEMENTOS DE LA CLASE C
		public List<dato> listaClase(String c){
			List<dato> valores=new List<dato>();
			foreach(dato d in registro){
				if( c.Equals(d.getClass()) ) {
					valores.Add(d);
				}
			}
			return valores;
		}
		
		public void genDataMart(){
			//VECTOR DE ATRIBTOS
			List<bool> vectorAtrNum=new List<bool>();
			List<bool> vectorAtrNom=new List<bool>();
			int cont=0;
			//AQUELLOS CON VALOR TRUE SON AQUELLOS NECESARIOS PRA EL DATAMART
			float aux;
			
			for(int i=0; i<tamNom; i++){
				vectorAtrNom.Add(true);
			}
			for(int i=0; i<tamNum; i++){
				vectorAtrNum.Add(true);
			}
			
			for(int i=0; i<tamNum; i++){
				if(vectorAtrNum[i]){
					for (int j=i+1; j<tamNum;j++){
						if(vectorAtrNum[j]){
							aux=kdd.correlacionPearson(this,i,j);
							//CONSIDERANDO CERCANO A 1 COMO 0.8
							//Console.WriteLine(aux);
							if(aux>0.3){ 
								vectorAtrNum[j]=false;
								//Console.WriteLine("ATRIBUTO "+i+" ESTÁ CORRELACIONADO CON EL ATRIBUTO "+j);
							}
						} //FIN IF ATR[I]&&ATR[J] 
					} //FIN FOR J 
				}//FIN IF
			}//FIN FOR I
			
			//VER VECTOR ATRIBUTOS NUMERICOS IMPORTANTES
			//foreach (bool a in vectorAtrNum) {Console.WriteLine(a);}
			
			for(int i=0; i<tamNom;i++){
				if(vectorAtrNom[i]){
					for(int j=i+1;j<tamNom;j++){
						if(kdd.correlacionChiCuad(this,i,j)){
							vectorAtrNom[j]=false;
							//Console.WriteLine("ATRIBUTO "+i+" ESTÁ CORRELACIONADO CON EL ATRIBUTO "+j);
						}
					}
				}
			}
			//foreach (bool a in vectorAtrNom) {Console.WriteLine(a);}
			//ELIMINAR ATRIBUTOS REDUNDANTES
			//cont=vectorAtrNum.Count;
		
			while (cont<vectorAtrNum.Count) {
				if(vectorAtrNum[cont]){
					cont+=1;
				}else{
					this.deleteAtrNum(cont);
					vectorAtrNum.RemoveAt(cont);
				}
			}
			cont=0;
			while (cont<vectorAtrNom.Count) {
				if(vectorAtrNom[cont]){
					cont+=1;
				}else{
					this.deleteAtrNom(cont);
					vectorAtrNom.RemoveAt(cont);
				}
			}
			cont=0;
			this.dm=true;
			Console.WriteLine("		SE HAN DETECTADO "+ vectorAtrNum.Count+ "("+this.tamNum+") ATRIBUTOS NUMERICOS RELEVANTES");
			Console.WriteLine("		SE HAN DETECTADO "+ vectorAtrNom.Count+ "("+this.tamNom+") ATRIBUTOS NOMINALES RELEVANTES");
		}
		
	}
}
