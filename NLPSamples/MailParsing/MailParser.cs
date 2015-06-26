using System;
using System.IO;
using edu.stanford.nlp.pipeline;
using java.io;
using java.util;
using Console = System.Console;

namespace MailParsing
{
    public class MailParser
    {
        public ParseResult Parse(string text)
        {
            var pipeline = InitPipeline();
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }
            return new ParseResult();
        }

        private static StanfordCoreNLP InitPipeline()
        {
            const string jarRoot = @"C:\Data\Docs\NLP\stanford-corenlp-3.5.2-models\edu\stanford\nlp\models";
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit");
            props.setProperty("sutime.binders", "0");

            var currenDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(currenDir);

            return pipline;
        }

    }
}
