/*
 * Created by SharpDevelop.
 * User: LYCJ
 * Date: 19/10/2007
 * Time: 2:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace AlertDialogWindow.Toolkit
{
    /// <summary>
    /// Represent owner of all HtmlTag.
    /// </summary>
    internal class HtmlTagTree : HtmlTagNode
	{
		public HtmlTagTree() : base(null, null)
		{
			isRoot = true;
			tag = new HtmlTag("master",string.Empty);
		}
		
		public override bool CanAdd(HtmlTag aTag)
		{
			return true;
		}		
		
		static string printNode(Int32 level, HtmlTagNode node)
		{
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            for (int i = 0; i < level; ++i)
                sb.Append("  ");

            sb.Append(node.ToString()).Append('\r').Append('\n');

            foreach (HtmlTagNode subnode in node)
                sb.Append(HtmlTagTree.printNode(level + 1, subnode));

            return sb.ToString();
		}
		
		public override string ToString()
		{
            StringBuilder sb = new StringBuilder();
            foreach (HtmlTagNode subnode in this)
                sb.Append(HtmlTagTree.printNode(0, subnode));
			
			return sb.ToString();
		}
	}
}
