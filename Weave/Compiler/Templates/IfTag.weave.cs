

namespace Weave.Compiler
{
    using System.IO;
    
        using Weave.Expressions;
    

     partial class Templates
    {
        public void RenderIfTag(IfTag model, TextWriter writer)
        {
             var first = true; 
writer.Write("\r\n");




foreach (var  branch in model.Branches)
{
    

    writer.Write("\r\n    ");


    
    
    if ( !first)

{
    writer.Write("\r\n        else\r\n    ");

}
    
writer.Write("\r\n    ");
 this.WalkBranch(branch); 
writer.Write("\r\n    ");
 first = false; 
writer.Write("\r\n");

}


        }
    }
}