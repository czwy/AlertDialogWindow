using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace AlertDialogWindow.Toolkit
{
    /// <summary>
    /// An Item entry, can store a string and an object (Pie)
    /// </summary>
    internal class HtmlTextBlockPropertyItemType 
    {
    	public string key;
        public string value;
        public object attachment;
        public HtmlTextBlockPropertyItemType(string aKey, string aValue)
        {
        	key = aKey;
            value = aValue;
        }
        public HtmlTextBlockPropertyItemType(string aKey, object anAttachment)
        {   
        	key = aKey;
            attachment = anAttachment;
        }
        public HtmlTextBlockPropertyItemType(string aKey, string aValue, object anAttachment)
        {
        	key = aKey;
            value = aValue;
            attachment = anAttachment;
        }
    }
    /// <summary>
    /// Variable List that use "Key" to store PropertyItemType
    /// </summary>
    internal class PropertyList : ListDictionary
    {
        public bool createIfNotExist = true;

        /// <summary>
        /// Get an item from the list
        /// </summary>
        private HtmlTextBlockPropertyItemType getPropertyInfo(string aKey)
        {
            if (Contains(aKey))
            {
                foreach (DictionaryEntry de in this)
                {
                    if ((string)de.Key == aKey)
                    {
                        return (HtmlTextBlockPropertyItemType)de.Value;
                    }
                }
            }

            HtmlTextBlockPropertyItemType retVal = new HtmlTextBlockPropertyItemType(aKey, string.Empty);
            if (createIfNotExist) { Add(aKey, retVal); }
            return retVal;
        }
        /// <summary>
        /// Get an item from the list
        /// </summary>
        private HtmlTextBlockPropertyItemType getPropertyInfo(Int32 anId)
        {            
            IDictionaryEnumerator Enum = GetEnumerator();
            if (Count >= anId)
            {
                for (int i = 0; i <= anId; i++) { Enum.MoveNext(); }
                return (HtmlTextBlockPropertyItemType)Enum.Value;
            }
            return new HtmlTextBlockPropertyItemType(anId.ToString(),string.Empty);
        }
        /// <summary>
        /// Set or Add an item to the list
        /// </summary>
        private void setPropertyInfo(string aKey, HtmlTextBlockPropertyItemType aValue)
        {
            VerifyType(aValue);
            if (Contains(aKey))
            {
                this.Remove(aKey);
            }
            Add(aKey, aValue);
        }
        /// <summary>
        /// Check item before add.
        /// </summary>        
        private void VerifyType(object value)
        {
            if (!(value is HtmlTextBlockPropertyItemType))
            { throw new ArgumentException("Invalid Type."); }
        }
        /// <summary>
        /// Add a new PropertyInfo
        /// </summary>
        public void Add(string aKey, string aValue)
        {
            Add(aKey, new HtmlTextBlockPropertyItemType(aKey, aValue));
        }
        /// <summary>
        /// Add a new PropertyInfo
        /// </summary>
        public void Add(string aKey, string aValue, object anAttachment)
        {
            Add(aKey, new HtmlTextBlockPropertyItemType(aKey, aValue,anAttachment));
        }
        /// <summary>
        /// Retrieve a PropertyItem using a key
        /// </summary>
        public HtmlTextBlockPropertyItemType this[String aKey]
        {
            get
            {
                return getPropertyInfo(aKey);
            }
            set
            {
                setPropertyInfo(aKey, value);
            }
        }
        /// <summary>
        /// Retrieve a PropertyItem using an id
        /// </summary>
        public HtmlTextBlockPropertyItemType this[Int32 anId]
        {
            get
            {
                return getPropertyInfo(anId);
            }

        }
        
        public string Html()
        {
        	string retVal = string.Empty;
        	for (Int32 i = 0; i < this.Count; ++i)
        	{
        		HtmlTextBlockPropertyItemType item = this[i];
        		retVal += new StringBuilder().Append(" ").Append(item.key).Append("=\"").Append(item.value).Append("\"").ToString();
        	}
        		
        	return retVal;
        }
        
        
        public PropertyList Clone()
        {
        	PropertyList retVal = new PropertyList();
        	foreach (HtmlTextBlockPropertyItemType item in this)
        		retVal.Add(item.key, item.value);
        	return retVal;
        }
        
		public override string ToString()
		{
			string retVal = string.Empty;
			for (int i = 0; i < Count; i++)
				retVal += string.Format(" {0}=\"{1}\"; ", this[i].key, this[i].value);
			if (retVal.Length<1)
				return " ";
			else return retVal;
		}
        
    
        public static void DebugUnit()
        {
            PropertyList list = new PropertyList();
            list["abc"].value = "abcd";
            list["bcd"].value = "bcde";
            list["cde"].value = "cdef";
            list.Remove("abc");

            Debug.Assert((list["bcd"].value == "bcde"), "PropertyList Failed.");
            Debug.Assert((list["abc"].value == ""), "PropertyList Failed.");
        }


        public static PropertyList FromString(string s)
        {
            return HtmlTextBlockUtils.ExtravtVariables(s);
        }

        

    }
}
