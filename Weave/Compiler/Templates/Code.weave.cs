// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 1.0.0.0
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------

namespace
    #line 1 "Code.weave"
           Weave.Compiler
    #line default
{
    using System.IO;
    using
        #line 4 "Code.weave"
       Weave.Expressions
        #line default
        ;

    partial class
    Templates
    {
        [System.CodeDom.Compiler.GeneratedCode("Weave", "1.0.0.0")]
        public void
        #line 2 "Code.weave"
            RenderCode
        #line default
            (
            #line 3 "Code.weave"
       object
            #line default
            model, TextWriter writer, string indentation = null)
        {
            var originalIndentation = indentation = indentation ?? string.Empty;
            var temp0 = indentation;
            #line 6 "Code.weave"
   var span = model as SourceSpan; 
            #line default
            indentation = temp0;
            if (
                #line 7 "Code.weave"
     span != null
                #line default
                )
            {
                writer.Write(indentation);
                writer.Write("#line ");
                writer.Write(
                    #line 8 "Code.weave"
              span.Start.Line 
                    #line default
                    );
                writer.Write(" \"");
                writer.Write(
                    #line 8 "Code.weave"
                                      Path.GetFileName(span.Start.FileName) 
                    #line default
                    );
                writer.Write("\"");
                var temp1 = indentation;
                #line 8 "Code.weave"
                                                                                  writer.WriteLine(); 
                #line default
                indentation = temp1;
                writer.Write(
                    #line 8 "Code.weave"
                                                                                                            new string(' ', span.Start.Column - 1) 
                    #line default
                    );
                writer.Write(
                    #line 8 "Code.weave"
                                                                                                                                                         span.Value 
                    #line default
                    );
                writer.WriteLine();
                writer.Write(indentation);
                writer.Write("#line default");
                writer.WriteLine();
            }
            else
            {
                var temp2 = indentation;
                #line 11 "Code.weave"
       var value = model.ToString(); 
                #line default
                indentation = temp2;
                if (
                    #line 12 "Code.weave"
         !string.IsNullOrEmpty(value) 
                    #line default
                    )
                {
                    writer.Write(indentation);
                    writer.Write(
                        #line 13 "Code.weave"
            value 
                        #line default
                        );
                    writer.WriteLine();
                }
            }
        }
    }
}