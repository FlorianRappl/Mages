namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Core.Runtime;
    using Mages.Core.Vm;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public sealed class ReplCore
    {
        private readonly IInteractivity _interactivity;
        private readonly Engine _engine;

        public ReplCore(IResolver resolver)
        {
            var currentContext = default(IExecutionContext);

            _interactivity = resolver.Interactivity;
            _engine = resolver.CreateEngine();

            _engine.OnRunning += (sender, e) =>
            {
                currentContext = e.Context;
            };

            _engine.OnStopped += (sender, e) =>
            {
                currentContext = null;
            };

            _engine.OnError += (sender, e) => 
            {
                _interactivity.Error(e.Error.Message);
            };

            _interactivity.HandleCancellation(() =>
            {
                if (currentContext is not null)
                {
                    currentContext.Stop();
                    return true;
                }

                return false;
            });
        }

        public void Run(String file)
        {
            var import = _engine.Globals["import"] as Function;

            try
            {
                import.Invoke(new[] { file });
            }
            catch (ParseException ex)
            {
                var content = File.ReadAllText(file);
                _interactivity.Display(ex.Error, content);
            }
            catch (Exception ex)
            {
                _interactivity.Error(ex.Message);
            }
        }

        public void Run()
        {
            Startup();
            Loop();
            Teardown();
        }

        public void Tutorial(ITutorialRunner tutorials)
        {
            Startup();

            _interactivity.Write("Welcome to MAGES! Press ENTER to to start the interactive tutorial ...");
            _interactivity.ReadLine();
            tutorials.RunAll(_interactivity, _engine.Scope, input => Evaluate(input, true));
            
            Teardown();
        }

        private void Loop()
        {
            var input = default(String);

            do
            {
                input = _interactivity.GetLine("SWM> ", ShowAutoComplete);

                if (input is not null && input.Trim().Length > 0)
                {
                    EvaluateCompleted(input);
                }
            }
            while (true);
        }

        private void Teardown()
        {
        }

        private void EvaluateCompleted(String input)
        {
            while (!input.IsCompleted())
            {
                var rest = _interactivity.GetLine("   > ", ShowAutoComplete);

                if (rest is null)
                    return;

                if (rest.Length == 0)
                    break;

                input = String.Concat(input, Environment.NewLine, rest);
            }

            Evaluate(input, true);
        }

        private void Evaluate(String content, Boolean showOutput)
        {
            try
            {
                var result = _engine.Interpret(content);

                if (showOutput)
                {
                    var output = default(String);

                    if (result is not null)
                    {
                        output = Stringify.This(result);
                    }

                    _interactivity.Info(output);
                    _engine.Scope["ans"] = result;
                }
            }
            catch (ParseException ex)
            {
                _interactivity.Display(ex.Error, content);
            }
            catch (Exception)
            {
            }

            _interactivity.Write(Environment.NewLine);
        }

        private Completion ShowAutoComplete(String text, Int32 position)
        {
            var autocomplete = _engine.GetCompletionAt(text, position);
            return Completion.From(autocomplete);
        }

        private void Startup()
        {
            var logo = String.Format(@"
     _____      _____    ___________________ _________
    /     \    /  _  \  /  _____/\_   _____//   _____/
   /  \ /  \  /  /_\  \/   \  ___ |    __)_ \_____  \ 
  /    Y    \/    |    \    \_\  \|        \/        \
  \____|__  /\____|__  /\______  /_______  /_______  /
          \/         \/        \/        \/        \/ 

  (c) Florian Rappl, {0}
  Version {1}
  Running on {2}

  For help type 'help()'.
", DateTime.Now.Year, _engine.Version, Environment.OSVersion.ToString());
            var logoLines = logo.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var logoLine in logoLines)
            {
                _interactivity.Info(logoLine);
                _interactivity.Write(Environment.NewLine);
            }
        }
    }
}
