using System;
using System.Collections.Generic;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.io;
using java.util;
using Console = System.Console;
using System.Linq;

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
            var sentences = annotation.Value.get(new CoreAnnotations.SentencesAnnotation().getClass()) as ArrayList;
            if (sentences == null || sentences.isEmpty())
                return string.Empty;
            var places = (from CoreMap sentence in sentences 
                          from CoreLabel token in sentence.get(new CoreAnnotations.TokensAnnotation().getClass()) as ArrayList 
                          let namedEntity = token.get(new CoreAnnotations.NamedEntityTagAnnotation().getClass()) 
                          where namedEntity.ToString() == "LOCATION" select token.originalText()).ToList();
            return places.Any() ? places[0] : string.Empty;
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
            return annotatedText;
        }
    }
}