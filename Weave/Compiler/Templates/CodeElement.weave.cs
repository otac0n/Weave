

namespace Weave.Compiler
{
    using System.IO;
    
        using Weave.Expressions;
    

     partial class Templates
    {
        public void RenderCodeElement(CodeElement model, TextWriter writer)
        {
            writer.Write(model.Expression );
writer.Write("\r\n");

        }
    }
}