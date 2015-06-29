using System;
using System.IO;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.time;
using edu.stanford.nlp.util;
using java.io;
using java.util;
using Console = System.Console;

namespace MailParsing
{
    public class MailParser
    {
        private const string modelsDir = @"C:\Data\Docs\NLP\stanford-corenlp-3.5.2-models\edu\stanford\nlp\models\";
        
        public ParseResult Parse(string text, DateTime currentDate)
        {
//            ParseWholePipeline(text);
            return new ParseResult
                {
                    DateAndTime = ParseTimePipeline(text, currentDate)
                };
        }

        private static DateInfo ParseTimePipeline(string text, DateTime date)
        {
            var pipeline = SetupTimePipline();
            var annotation = new Annotation(text);
            annotation.set(new CoreAnnotations.DocDateAnnotation().getClass(), date.ToString("yyyy-MM-dd"));
            pipeline.annotate(annotation);
           
            var timeAnnotations = annotation.get(new TimeAnnotations.TimexAnnotations().getClass()) as ArrayList;
            if (timeAnnotations == null || timeAnnotations.isEmpty())
                return new DateInfo();
            var cm = (CoreMap)timeAnnotations.get(0);
            var timeExpression = cm.get(new TimeExpression.Annotation().getClass()) as TimeExpression;
            return new DateInfo
                {
                    Literal = cm.ToString(),
                    Expression = timeExpression.getTemporal().ToString()
                };
        }

        private static void ParseWholePipeline(string text)
        {
            var pipeline = SetupGeneralPipeline();
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }
        }

        private static AnnotationPipeline SetupTimePipline()
        {
            var pipeline = new AnnotationPipeline();
            pipeline.addAnnotator(new TokenizerAnnotator(false));
            pipeline.addAnnotator(new WordsToSentencesAnnotator(false));
            var tagger = new MaxentTagger(modelsDir + @"pos-tagger\english-bidirectional\english-bidirectional-distsim.tagger");
            pipeline.addAnnotator(new POSTaggerAnnotator(tagger));
            const string sutimeRules = modelsDir + @"\sutime\defs.sutime.txt,"
                                       + modelsDir + @"\sutime\english.holidays.sutime.txt,"
                                       + modelsDir + @"\sutime\english.sutime.txt";
            var props = new Properties();
            props.setProperty("sutime.rules", sutimeRules);
            props.setProperty("sutime.binders", "0");
            pipeline.addAnnotator(new TimeAnnotator("sutime", props));
            return pipeline;
        }

        private static StanfordCoreNLP SetupGeneralPipeline()
        {
            var props = new Properties();
            props.put("pos.model", modelsDir + "pos-tagger/english-left3words/english-left3words-distsim.tagger");
            props.put("ner.model", modelsDir + "ner/english.conll.4class.distsim.crf.ser.gz");
            props.put("parse.model", modelsDir + "lexparser/englishPCFG.ser.gz");
            props.put("sutime.rules", modelsDir + "sutime/defs.sutime.txt, " + modelsDir + "sutime/english.sutime.txt");
            props.put("dcoref.demonym", modelsDir + "dcoref/demonyms.txt");
            props.put("dcoref.states", modelsDir + "dcoref/state-abbreviations.txt");
            props.put("dcoref.animate", modelsDir + "dcoref/animate.unigrams.txt");
            props.put("dcoref.inanimate", modelsDir + "dcoref/inanimate.unigrams.txt");
            props.put("dcoref.big.gender.number", modelsDir + "dcoref/gender.data.gz");
            props.put("dcoref.countries", modelsDir + "dcoref/countries");
            props.put("dcoref.states.provinces", modelsDir + "dcoref/statesandprovinces");
            props.put("dcoref.singleton.model", modelsDir + "dcoref/singleton.predictor.ser");
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
            props.setProperty("sutime.binders", "0");
            props.setProperty("ner.useSUTime", "0"); 

            return new StanfordCoreNLP(props);
        }

    }
}
