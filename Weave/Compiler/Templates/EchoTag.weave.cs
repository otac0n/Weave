namespace Weave.Compiler
{
using System.IO;
using Weave.Expressions;
 partial class Templates
{
public void RenderEchoTag(EchoTag model, TextWriter writer)
{
writer.Write("writer.Write(");
writer.Write(model.Expression );
writer.Write(");");
}
}
}
