// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 1.0.0.0
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------

namespace
    #line 1 "_config.weave"
           Weave.Compiler
    #line default
{
    using System;
    using System.IO;
    using
        #line 2 "_config.weave"
       Weave.Expressions
        #line default
        ;

    partial class
    Templates
    {
        [System.CodeDom.Compiler.GeneratedCode("Weave", "1.0.0.0")]
        public void
        RenderCodeElement
            (
            #line 1 "CodeElement.weave"
       CodeElement
            #line default
            model, TextWriter writer, string indentation = null)
        {
            var __originalIndentation = indentation = indentation ?? string.Empty;
            var __temp0 = indentation;
            #line 3 "CodeElement.weave"
   var temp = this.CreateVariable("temp"); 
            #line default
            indentation = __temp0;
            writer.Write(indentation);
            writer.Write("var ");
            writer.Write(
                #line 4 "CodeElement.weave"
        temp 
                #line default
                );
            writer.Write(" = indentation;");
            writer.WriteLine();
            var __model0 =
                #line 5 "CodeElement.weave"
              model.Expression
                #line default
                ;
            RenderCode(__model0, writer, indentation);
            writer.Write(indentation);
            writer.Write("indentation = ");
            writer.Write(
                #line 6 "CodeElement.weave"
                  temp 
                #line default
                );
            writer.Write(";");
            writer.WriteLine();
        }
    }
}