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

            
 partial class         Templates

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
                    if (

                        #line 6 "IndentationElement.weave"
     this.lastIndentation != model.Indentation
                        #line default

                        )
                {
                        indentation = originalIndentation + "    ";
                    writer.Write(indentation);
                    writer.Write("indentation = originalIndentation");
                            if (

                                #line 7 "IndentationElement.weave"
                                          model.Indentation != string.Empty
                                #line default

                                )
                        {
                            writer.Write(" + ");
                            writer.Write(
                            #line 7 "IndentationElement.weave"
                                                                                    ToLiteral(model.Indentation) 
                            #line default

);
                        }
                    writer.Write(";");
                    writer.WriteLine();
                    var temp0 = indentation;

                    #line 8 "IndentationElement.weave"
       this.lastIndentation = model.Indentation; 
                    #line default

                    indentation = temp0;
                }
                indentation = originalIndentation;
            writer.Write(indentation);
            writer.Write("writer.Write(indentation);");
            writer.WriteLine();
        }
    }
}