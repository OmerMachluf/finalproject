using System;
using RDotNet;
using System.Text;
using System.IO;

namespace EyeDenticaService
{
    class AlgorithmRunner
    {
        public static void RunRAlgorithm()
        {
            //SetupPath(); // current process, soon to be deprecated
           /** REngine engine = REngine.GetInstance();

            // A somewhat contrived but customary Hello World:
            StringBuilder rAlgo = new StringBuilder();
             rAlgo.AppendLine("library tree()");
            rAlgo.AppendLine("x = read.csv(\"D:\\temp\\test.csv\")");
            rAlgo.AppendLine("x1 = x[sample(nrow(x), 400),]");
            rAlgo.AppendLine("set.seed(2)");
            rAlgo.AppendLine("classVector = x1$user");
            rAlgo.AppendLine("train = sample(1:nrow(x1), nrow(x1)*0.75)");
            rAlgo.AppendLine("test = -train");
            rAlgo.AppendLine("trainData = x1[train,]");
            rAlgo.AppendLine("testData = x1[test,]");
            rAlgo.AppendLine("testingTarget = classVector[test]");
            rAlgo.AppendLine("treeModel = tree(user~.,trainData)");
            rAlgo.AppendLine("treePred = predict(treeModel, testData, type =\"class\")");
            rAlgo.AppendLine("mean(treePred != testingTarget)");
            rAlgo.AppendLine("plot(treeModel)");
            rAlgo.AppendLine("text(treeModel)");
            rAlgo.AppendLine("cv_tree = cv.tree(treeModel, FUN = prune.misclass)");
            rAlgo.AppendLine("plot(cv_tree$size, cv_tree$dev, type=\"b\")");
            rAlgo.AppendLine("goodModel = prune.misclass(treeModel, best = 2)");
            rAlgo.AppendLine("plot(goodModel)");
            rAlgo.AppendLine("text(goodModel)");
            rAlgo.AppendLine("goodPred = predict(goodModel, testData, type =\"class\")");
            rAlgo.AppendLine("mean(goodPred != testingTarget)");

            SymbolicExpression a = engine.Evaluate(rAlgo.ToString()); */
            //RScriptRunner.RunFromCmd(@"C:\temp\better.R", "rscript.exe", "");

            //engine.Dispose();
        }

        //public static void SetupPath(string Rversion = "R-3.0.0")
        //{
        //    var oldPath = System.Environment.GetEnvironmentVariable("PATH");
        //    var rPath = System.Environment.Is64BitProcess ?
        //                           string.Format(@"D:\Program Files (x86)\R-3.3.0\bin\x64", Rversion) :
        //                           string.Format(@"D:\Program Files (x86)\R-3.3.0\bin\i386", Rversion);

        //    if (!Directory.Exists(rPath))
        //        throw new DirectoryNotFoundException(string.Format(" R.dll not found in : {0}", rPath));
        //    var newPath = string.Format("{0}{1}{2}", rPath,
        //                                 System.IO.Path.PathSeparator, oldPath);
        //    System.Environment.SetEnvironmentVariable("PATH", newPath);
        //}
    }
}
