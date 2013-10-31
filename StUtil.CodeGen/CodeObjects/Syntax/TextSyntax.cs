using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen.CodeObjects.Syntax
{
    [Keyword]
    public class TextSyntax : BaseSyntaxObject
    {
        public string Text { get; set; }

        public TextSyntax(string text)
        {
            this.Text = text;
        }
    }
}
