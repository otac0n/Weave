// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 1.0.0.0
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------

namespace
    #line 1 "EchoTag.weave"
           Weave.Compiler
    #line default
{
    using System.IO;
    using
        #line 4 "EchoTag.weave"
       Weave.Expressions
        #line default
        ;

    partial class
    Templates
    {
        [System.CodeDom.Compiler.GeneratedCode("Weave", "1.0.0.0")]
        public void
        #line 2 "EchoTag.weave"
            RenderEchoTag
        #line default
            (
            #line 3 "EchoTag.weave"
       EchoTag
            #line default
            model, TextWriter writer, string indentation = null)
        {
            var originalIndentation = indentation = indentation ?? string.Empty;
            writer.Write(indentation);
            writer.Write("writer.Write(");
            writer.WriteLine();
            indentation = originalIndentation + "    ";
            var model0 =
                #line 7 "EchoTag.weave"
                  model.Expression
                #line default
                ;
            RenderCode(model0, writer, indentation);
            writer.Write(indentation);
            writer.Write(");");
            writer.WriteLine();
        }
    }
}