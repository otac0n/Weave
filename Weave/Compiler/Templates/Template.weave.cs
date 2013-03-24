// -----------------------------------------------------------------------
// <auto-generated>
//   This code was generated by Weave 1.0.0.0
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------

namespace 
#line 1 "Template.weave"
           Weave.Compiler
#line default


{
    using System.IO;
        using 
#line 4 "Template.weave"
       System.Linq
#line default

;
        using 
#line 5 "Template.weave"
       System.Reflection
#line default

;
        using 
#line 6 "Template.weave"
       Weave.Expressions
#line default

;

        
 partial class     Templates

    {
        [System.CodeDom.Compiler.GeneratedCode("Weave", "1.0.0.0")]
        public void 
#line 2 "Template.weave"
            RenderTemplate
#line default

(
#line 3 "Template.weave"
       Template
#line default

 model, TextWriter writer, string indentation = null)
        {
            indentation = indentation ?? string.Empty;
            var temp0 = indentation;

            #line 8 "Template.weave"
  
    var settings = model.Settings.ToLookup(s => s.Key.Value, s => (object)s.Value);
    var @namespace = settings["namespace"].Single();
    var accessibility = settings["accessibility"].SingleOrDefault() ?? string.Empty;
    var className = settings["classname"].SingleOrDefault() ?? "Templates";
    var methodName = settings["methodname"].SingleOrDefault() ?? "Render";
    var modelName = settings["model"].SingleOrDefault() ?? "dynamic";
    var assemblyName = Assembly.GetExecutingAssembly().GetName();

            #line default

            indentation = temp0;
            writer.Write(indentation);
            writer.Write("// -----------------------------------------------------------------------");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("// <auto-generated>");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("//   This code was generated by ");
            writer.Write(
            #line 19 "Template.weave"
                                    assemblyName.Name 
            #line default

);
            writer.Write(" ");
            writer.Write(
            #line 19 "Template.weave"
                                                             assemblyName.Version 
            #line default

);
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("//");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("//   Changes to this file may cause incorrect behavior and will be lost if");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("//   the code is regenerated.");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("// </auto-generated>");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("// -----------------------------------------------------------------------");
            writer.WriteLine();
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("namespace ");
            var model0 = 
            #line 26 "Template.weave"
                        @namespace
            #line default

;
            
            #line 26 "Template.weave"
             RenderCode
            #line default

(model0, writer, indentation);
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("{");
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("    ");
            writer.Write("using System.IO;");
            writer.WriteLine();


            foreach (var 
            #line 29 "Template.weave"
           @using in settings["using"]
            #line default

)
            {

                writer.Write(indentation);
                    writer.Write("        ");
                writer.Write("using ");
                var model1 = 
                #line 30 "Template.weave"
                            @using
                #line default

;
                
                #line 30 "Template.weave"
                 RenderCode
                #line default

(model1, writer, indentation);
                writer.Write(";");
                writer.WriteLine();
            }

            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("    ");
            var model2 = 
            #line 33 "Template.weave"
                  accessibility
            #line default

;
            
            #line 33 "Template.weave"
       RenderCode
            #line default

(model2, writer, indentation);
            writer.Write(" partial class ");
            var model3 = 
            #line 33 "Template.weave"
                                                              className
            #line default

;
            
            #line 33 "Template.weave"
                                                   RenderCode
            #line default

(model3, writer, indentation);
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("    ");
            writer.Write("{");
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("        ");
            writer.Write("[System.CodeDom.Compiler.GeneratedCode(\"");
            writer.Write(
            #line 35 "Template.weave"
                                                    assemblyName.Name 
            #line default

);
            writer.Write("\", \"");
            writer.Write(
            #line 35 "Template.weave"
                                                                                assemblyName.Version 
            #line default

);
            writer.Write("\")]");
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("        ");
            writer.Write("public void ");
            var model4 = 
            #line 36 "Template.weave"
                                  methodName
            #line default

;
            
            #line 36 "Template.weave"
                       RenderCode
            #line default

(model4, writer, indentation);
            writer.Write("(");
            var model5 = 
            #line 36 "Template.weave"
                                                             modelName
            #line default

;
            
            #line 36 "Template.weave"
                                                  RenderCode
            #line default

(model5, writer, indentation);
            writer.Write(" model, TextWriter writer, string indentation = null)");
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("        ");
            writer.Write("{");
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("            ");
            writer.Write("indentation = indentation ?? string.Empty;");
            writer.WriteLine();
            var model6 = 
            #line 39 "Template.weave"
                                model
            #line default

;
            
            #line 39 "Template.weave"
               BaseWalkTemplate
            #line default

(model6, writer, indentation + "            ");
            writer.Write(indentation);
                writer.Write("        ");
            writer.Write("}");
            writer.WriteLine();
            writer.Write(indentation);
                writer.Write("    ");
            writer.Write("}");
            writer.WriteLine();
            writer.Write(indentation);
            writer.Write("}");
        }
    }
}