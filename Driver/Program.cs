/*
 * Program.cs
 * author: numver8638(numver8638@naver.com)
 *
 * This file is part of BuildScript.
 *
 * BuildScript is free and unencumbered software released into the public domain.
 * 
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 * 
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * For more information, please refer to <http://unlicense.org>
 */
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