namespace Weave.Compiler
{
using System.IO;
using System.Linq;
using Weave.Expressions;
 partial class Templates
{
public void RenderTemplate(Template model, TextWriter writer)
{

    var settings = model.Settings.ToLookup(s => s.Key, s => s.Value);
    var @namespace = settings["namespace"].Single();
    var accessibility = settings["accessibility"].SingleOrDefault() ?? string.Empty;
    var className = settings["classname"].SingleOrDefault() ?? "Templates";
    var methodName = settings["methodname"].SingleOrDefault() ?? "Render";
    var modelName = settings["model"].SingleOrDefault() ?? "dynamic";
writer.Write("\r\n\r\nnamespace ");
writer.Write(@namespace );
writer.Write("\r\n{\r\n    using System.IO;\r\n    ");
foreach (var @using in settings["using"])
{
writer.Write("\r\n        using ");
writer.Write(@using );
writer.Write(";\r\n    ");
}
writer.Write("\r\n\r\n    ");
writer.Write(accessibility );
writer.Write(" partial class ");
writer.Write(className );
writer.Write("\r\n    {\r\n        public void ");
writer.Write(methodName );
writer.Write("(");
writer.Write(modelName );
writer.Write(" model, TextWriter writer)\r\n        {\r\n            ");
 base.WalkTemplate(model); writer.Write("\r\n        }\r\n    }\r\n}");
}
}
}
