// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 2.1.0.0
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
        [System.CodeDom.Compiler.GeneratedCode("Weave", "2.1.0.0")]
        public
        void
        RenderBodyElement
            (
            #line 1 "BodyElement.weave"
       BodyElement
            #line default
            model, TextWriter writer, string indentation = null)
        {
            var __encode = new Func<object, string>(
                #line 4 "_config.weave"
        ToLiteral
                #line default
                );
            var __originalIndentation = indentation = indentation ?? string.Empty;
            indentation = __originalIndentation;
            var __model0 =
                #line 3 "BodyElement.weave"
                model.Body
                #line default
                ;
            WalkElements(__model0, writer, indentation);
        }
    }
}
