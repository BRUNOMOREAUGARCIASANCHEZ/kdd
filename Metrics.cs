/*
 * Created by SharpDevelop.
 * User: PC
 * Date: 03/04/2023
 * Time: 11:04 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace fase1
{
	/// <summary>
	/// Description of Metrics.
	/// </summary>
	public static class Metrics
	{
		public static double HEOM(String a,String b){
			if(a.Equals(b))
				return 0;
			else
				return 1;
		}
		
		public static double HEOM(double a,double b,double rango){
			return Math.Abs(a-b)/rango;	
		}
	}
}
