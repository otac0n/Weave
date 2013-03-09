namespace Weave.Compiler
{
using System.IO;
using Weave.Expressions;
 partial class Templates
{
public void RenderTextElement(TextElement model, TextWriter writer)
{
writer.Write("writer.Write(");
writer.Write(ToLiteral(model.Value) );
writer.Write(");");
}
}
}
