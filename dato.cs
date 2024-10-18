/*
 * Created by SharpDevelop.
 * User: PC
 * Date: 11/03/2023
 * Time: 03:29 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace fase1
{
	/// <summary>
	/// Description of dato.
	/// </summary>
	public class dato
	{
		List<float> numerico;
		List<String> nominal;
		String clase;
		
		public dato()
		{
			this.numerico=new List<float>();
			this.nominal=new List<String>();
		}
		
		public void addFloat(float x){
			this.numerico.Add(x);
		}
		public void addNom(String s){
			this.nominal.Add(s);
		}
		
		public void addClass(string c){
			this.clase=c;
		}
		
		public float getAtrNum(int x){
			return this.numerico[x];
		}
		
		public String getAtrNom(int x){
			return this.nominal[x];
		}
		
		public String getClass(){
			return this.clase;
		}
		public void setAtrNum(int a, float x){
			this.numerico[a]=x;		
		}
		public void setAtrNom(int a,String s){
			this.nominal[a]=s;
		}
		public void deleteAtrNom(int x){
			this.nominal.RemoveAt(x);
		}
		public void deleteAtrNum(int x){
			this.numerico.RemoveAt(x);
		}
		
		public String toString(){
			String r="";
			
			foreach (float f in numerico) {
				r=r+','+f.ToString();
			}
			r=r+'\n';
			foreach (String s in nominal) {
				r=r+','+s;
			} 
			
			return r;
		}
		
		public int totalNum(){
			return this.numerico.Count;
		}
		public int totalNom(){
			return this.nominal.Count;
		}
		
	}
}
