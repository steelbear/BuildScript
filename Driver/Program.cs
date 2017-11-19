using System;

using BuildScript.Parse;
using BuildScript.Util;

namespace BuildScript.Driver
{
    class Program
    {
        private const string sourceText =
@"global {
 if (buildscript.OS == 'Windows') {
  raise ""#{buildscript.OS} is not supported""
}

task Print(message) {
 PrintImpl(message)
 return true
}

project BuildScript {
 major = 1
 minor = 2
 buildNo = 123456

 target Build {
  dependsOn ':BeforeBuild'

  for (source in project.sources) {
   Print source
  }

  var loop = 10

  repeat {
   Print ""loop: #{loop}""
   loop = loop - 1
  } until (loop == 0)

  action {
   source ->
    match (source.suffix) {
     case 'cpp':
      CppCompile(source)

     default:
      raise ""#{source.suffix} is not supproted""
    }
  }
 }
}
";
        
        static int Main(string[] args)
        {
            Console.WriteLine("Hello, world!");

            var source = new SourceText(sourceText);
            var parser = new Parser(source);

            parser.ParseScript();

            Console.ReadKey();

            return 0;
        }
    }
}