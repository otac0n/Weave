// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 1.0.0.0
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------

namespace 
#line 1 "IndentationElement.weave"
           Weave.Compiler
#line default


{
    using System.IO;
    using 
    #line 4 "IndentationElement.weave"
       Weave.Expressions
    #line default

;

        
 partial class     Templates

    {
        [System.CodeDom.Compiler.GeneratedCode("Weave", "1.0.0.0")]
        public void 
        #line 2 "IndentationElement.weave"
            RenderIndentationElement
        #line default

(
        #line 3 "IndentationElement.weave"
       IndentationElement
        #line default

 model, TextWriter writer, string indentation = null)
        {
            var originalIndentation = indentation = indentation ?? string.Empty;
            indentation = originalIndentation;
            writer.Write(indentation);
            writer.Write("writer.Write(indentation);");
            writer.WriteLine();
        }
    }
}