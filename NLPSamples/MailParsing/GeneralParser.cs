using System;
using edu.stanford.nlp.pipeline;
using java.io;
using java.util;
using Console = System.Console;

namespace MailParsing
{
    public class GeneralParser
    {
        private readonly string modelsDir;
        private readonly string text;
        private readonly Lazy<Annotation> annotation;

        public GeneralParser(string modelsDir, string text)
        {
            this.modelsDir = modelsDir;
            this.text = text;
            this.annotation = new Lazy<Annotation>(PrepareAnnotation);
        }

        public string ParsePlace()
        {
            return string.Empty;
        }

        public string ParseType()
        {
            return string.Empty;
        }

        private Annotation PrepareAnnotation()
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

            var pipeline = new StanfordCoreNLP(props);
            var annotatedText = new Annotation(text);
            pipeline.annotate(annotatedText);

            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotatedText, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }

            return annotatedText;
        }
    }
}