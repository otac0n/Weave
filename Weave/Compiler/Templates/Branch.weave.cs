// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 1.0.0.0
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------

namespace 
#line 1 "Branch.weave"
           Weave.Compiler
#line default


{
    using System.IO;
    using 
    #line 4 "Branch.weave"
       Weave.Expressions
    #line default

;

        
 partial class     Templates

    {
        [System.CodeDom.Compiler.GeneratedCode("Weave", "1.0.0.0")]
        public void 
        #line 2 "Branch.weave"
            RenderBranch
        #line default

(
        #line 3 "Branch.weave"
       Branch
        #line default

 model, TextWriter writer, string indentation = null)
        {
            var originalIndentation = indentation = indentation ?? string.Empty;
            indentation = originalIndentation;
            var temp0 = indentation;

            #line 6 "Branch.weave"
  
    var amount = GetIndentationOffset(model.Indentation, model.Body);
    this.amountToSubtract += amount;

            #line default

            indentation = temp0;
            if (

                #line 10 "Branch.weave"
     model.Expression != null
                #line default

                )
            {
                indentation = originalIndentation;
                writer.Write(indentation);
                writer.Write("if (");
                writer.WriteLine();
                indentation = originalIndentation + "    ";
                var model0 = 
                #line 12 "Branch.weave"
                      model.Expression
                #line default

;
                
                #line 12 "Branch.weave"
           RenderCode
                #line default

(model0, writer, indentation);
                indentation = originalIndentation + "    ";
                writer.Write(indentation);
                writer.Write(")");
                writer.WriteLine();
            }
            indentation = originalIndentation;
            writer.Write(indentation);
            writer.Write("{");
            writer.WriteLine();
            indentation = originalIndentation + "    ";
            var model1 = 
            #line 16 "Branch.weave"
                    model.Body
            #line default

;
            
            #line 16 "Branch.weave"
       WalkElements
            #line default

(model1, writer, indentation);
            indentation = originalIndentation;
            writer.Write(indentation);
            writer.Write("}");
            writer.WriteLine();
            indentation = originalIndentation;
            var temp1 = indentation;

            #line 18 "Branch.weave"
  
    this.amountToSubtract -= amount;

            #line default

            indentation = temp1;
        }
    }
}