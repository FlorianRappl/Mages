namespace Mages.Repl.Tutorial
{
    using Core.Runtime;
    using System;
    using System.Collections.Generic;

    static class Tutorials
    {
        public static void RunAll(IInteractivity interactivity, Scope scope, Action<String> evaluate)
        {
            var snippets = GetAllTutorials();
            interactivity.Write(Environment.NewLine);

            for (var i = 0; i < snippets.Count; i++)
            {
                var snippet = snippets[i];
                WriteTitle(interactivity, i, snippet);
                WriteExample(interactivity, evaluate, snippet);
                WriteTask(interactivity, snippet);
                var success = TryToLearn(interactivity, scope, evaluate, snippet);

                if (success)
                {
                    interactivity.Write("Great!");
                    interactivity.Write(Environment.NewLine);
                }
                else
                {
                    WriteSolution(interactivity, evaluate, snippet);
                }
            }
        }

        private static void WriteSolution(IInteractivity interactivity, Action<String> evaluate, ITutorialSnippet snippet)
        {
            interactivity.Write("Solution:");
            interactivity.Write(Environment.NewLine);
            interactivity.WritePrompt();
            interactivity.Write(snippet.Solution);
            interactivity.Write(Environment.NewLine);
            evaluate.Invoke(snippet.Solution);
            interactivity.Write(Environment.NewLine);
        }

        private static Boolean TryToLearn(IInteractivity interactivity, Scope scope, Action<String> evaluate, ITutorialSnippet snippet)
        {
            var hints = snippet.Hints.GetEnumerator();
            var success = true;

            do
            {
                if (!success)
                {
                    interactivity.Info(hints.Current);
                    interactivity.Write(Environment.NewLine);
                }

                var input = interactivity.Read();
                evaluate.Invoke(input);
                success = snippet.Check(scope);
            }
            while (!success && hints.MoveNext());
            return success;
        }

        private static void WriteTask(IInteractivity interactivity, ITutorialSnippet snippet)
        {
            interactivity.Write("Your task: ");
            interactivity.Write(snippet.Task);
            interactivity.Write(Environment.NewLine);
        }

        private static void WriteExample(IInteractivity interactivity, Action<string> evaluate, ITutorialSnippet snippet)
        {
            interactivity.Write("Example:");
            interactivity.Write(Environment.NewLine);
            interactivity.WritePrompt();
            interactivity.Write(snippet.ExampleCommand);
            interactivity.Write(Environment.NewLine);
            evaluate.Invoke(snippet.ExampleCommand);
        }

        private static void WriteTitle(IInteractivity interactivity, Int32 index, ITutorialSnippet snippet)
        {
            var nr = index + 1;
            interactivity.Write("#" + nr + " " + snippet.Title);
            interactivity.Write(Environment.NewLine);
            interactivity.Write(Environment.NewLine);
            interactivity.Write(snippet.Description);
            interactivity.Write(Environment.NewLine);
            interactivity.Write(Environment.NewLine);
        }

        private static IList<ITutorialSnippet> GetAllTutorials()
        {
            return new List<ITutorialSnippet>
            {
                new StandardSnippet<Double>((ans, scope) => ans == 2.0 * Math.PI)
                {
                    Description = "Simple arithmetic expressions are simple with Mages. Take for instance an addition. You only need to enter the two operands and the plus operator.",
                    ExampleCommand = "2 + 3",
                    Hints = new[] {
                        "We need to enter three parts: Number, operator, and identifier.",
                        "The number is 2, the operator is *.",
                        "PI is given by typing `pi`"
                    },
                    Solution = "2 * pi",
                    Task = "Compute two times PI, where PI is a mathematical constant, representing the ratio of a circle's circumference to its diameter. PI is given by typing pi.",
                    Title = "Simple Arithmetic"
                }
            };
        }
    }
}
