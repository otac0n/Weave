namespace Weave.Compiler
{
using System.IO;
using Weave.Expressions;
 partial class Templates
{
public void RenderEachTag(EachTag model, TextWriter writer)
{

    var flag = this.CreateVariable("_flag");
writer.Write("\r\n\r\n");
if (model.NoneBody != null)
{
writer.Write("\r\n    bool ");
writer.Write(flag );
writer.Write(";\r\n");
}
writer.Write("\r\n\r\nforeach (");
writer.Write(model.Expression );
writer.Write(")\r\n{\r\n    ");
if (model.NoneBody != null)
{
writer.Write("\r\n        ");
writer.Write(flag );
writer.Write(" = true;\r\n    ");
}
writer.Write("\r\n\r\n    ");
 this.WalkElements(model.Body); writer.Write("\r\n}\r\n\r\n");
if (model.NoneBody != null)
{
writer.Write("\r\n    if (!");
writer.Write(flag );
writer.Write(")\r\n    {\r\n        ");
 this.WalkElements(model.NoneBody); writer.Write("\r\n    }\r\n");
}
}
}
}
