/*
 * Created by SharpDevelop.
 * User: LYCJ
 * Date: 27/10/2007
 * Time: 21:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace AlertDialogWindow.Toolkit
{
	
	
	/// <summary>
	/// Description of DependencyProterty.
	/// </summary>
	internal class HtmlTextBlockDependencyProperty<AnyType>
	{
		private static List<HtmlTextBlockDependencyProperty<AnyType>> dependencyPropertyList = new List<HtmlTextBlockDependencyProperty<AnyType>>();				
		private string name;
		private object propertyValue;
		private Type propertyType;
		private Type ownerType;
		private PropertyMetadata typeMetadata;
		private ValidateValueCallback validateValueCallback;
		
		public Type PropertyType { get { return propertyType; } }		
		internal object PropertyValue
		{
			get { return propertyValue; }
			set { propertyValue = value; CallValidateValueCallback(); }
		}
			
		
		
		
		private HtmlTextBlockDependencyProperty(string aName, Type aPropertyType, 
		                           Type anOwnerType, PropertyMetadata aTypeMetadata,
		                           ValidateValueCallback aValidateValueCallback)
		{
			name = aName;
			propertyType = aPropertyType;
			ownerType = anOwnerType;
			typeMetadata = aTypeMetadata;
			validateValueCallback = aValidateValueCallback;
		}
			
		public static HtmlTextBlockDependencyProperty<AnyType> Register(string name, Type propertyType, 
		                                          Type ownerType, PropertyMetadata typeMetadata,
		                                          ValidateValueCallback validateValueCallback)
		{
			HtmlTextBlockDependencyProperty<AnyType> retVal = new HtmlTextBlockDependencyProperty<AnyType>(name, propertyType, ownerType, 
			                                                   typeMetadata, validateValueCallback);
			
			
			
			
			dependencyPropertyList.Add(retVal);
			return retVal;
		}
		
		public static void SetValue(HtmlTextBlockDependencyProperty<AnyType> property, object propertyValue)
		{			
			property.PropertyValue = propertyValue;
		}
		
		public static object GetValue(HtmlTextBlockDependencyProperty<AnyType> property)
		{
			return property.PropertyValue;
		}
		
		
		private void CallValidateValueCallback()
		{
			if (validateValueCallback != null)
				validateValueCallback(this, new ValidateValueEventArgs());
		}
	}
	

	public class ValidateValueEventArgs : EventArgs
	{
		
	}
	
	public delegate void ValidateValueCallback(Object sender, ValidateValueEventArgs e);	
}

		
