using System;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.time;
using edu.stanford.nlp.util;
using java.util;

namespace MailParsing
{
    public class DateTimeParser
    {
        private readonly string modelsDir;

        public DateTimeParser(string modelsDir)
        {
            this.modelsDir = modelsDir;
        }

        public DateTimeInfo Parse(string text, DateTime currentDate)
        {
            var annotation = PrepareAnnotation(text, currentDate);
            var timeAnnotations = annotation.get(new TimeAnnotations.TimexAnnotations().getClass()) as ArrayList;
            if (timeAnnotations == null || timeAnnotations.isEmpty())
                return new DateTimeInfo();
            var cm = (CoreMap)timeAnnotations.get(0);
            var timeExpression = cm.get(new TimeExpression.Annotation().getClass()) as TimeExpression;
            return new DateTimeInfo
            {
                Literal = cm.ToString(),
                Expression = timeExpression.getTemporal().ToString()
            };
        }

        private Annotation PrepareAnnotation(string text, DateTime currentDate)
        {
            var pipeline = new AnnotationPipeline();
            pipeline.addAnnotator(new TokenizerAnnotator(false));
            pipeline.addAnnotator(new WordsToSentencesAnnotator(false));
            var tagger = new MaxentTagger(modelsDir + @"pos-tagger\english-bidirectional\english-bidirectional-distsim.tagger");
            pipeline.addAnnotator(new POSTaggerAnnotator(tagger));
            var sutimeRules = modelsDir + @"\sutime\defs.sutime.txt,"
                                       + modelsDir + @"\sutime\english.holidays.sutime.txt,"
                                       + modelsDir + @"\sutime\english.sutime.txt";
            var props = new Properties();
            props.setProperty("sutime.rules", sutimeRules);
            props.setProperty("sutime.binders", "0");
            pipeline.addAnnotator(new TimeAnnotator("sutime", props));

            var annotation = new Annotation(text);
            annotation.set(new CoreAnnotations.DocDateAnnotation().getClass(), currentDate.ToString("yyyy-MM-dd"));
            pipeline.annotate(annotation);
            return annotation;
        }
    }
}