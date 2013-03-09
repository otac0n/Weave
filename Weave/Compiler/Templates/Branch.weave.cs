namespace Weave.Compiler
{
using System.IO;
using Weave.Expressions;
 partial class Templates
{
public void RenderBranch(Branch model, TextWriter writer)
{
if ( model.Expression != null)
{
writer.Write("\r\n    if (");
writer.Write(model.Expression );
writer.Write(")\r\n");
}
writer.Write("\r\n{\r\n    ");
 this.WalkElements(model.Body); writer.Write("\r\n}");
}
}
}
