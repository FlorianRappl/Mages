namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using Mages.Repl.Tutorial;
    using System;
    using System.IO;

    sealed class ReplCore
    {
        private readonly IInteractivity _interactivity;
        private readonly Engine _engine;

        public ReplCore(IInteractivity interactivity, IEngineCreator creator)
        {
            _interactivity = interactivity;
            _engine = creator.CreateEngine();
        }

        public void Run(String file)
        {
            var import = _engine.Globals["import"] as Function;

            try
            {
                import?.Invoke(new[] { file });
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

        public void Tutorial()
        {
            Startup();

            _interactivity.IsPromptShown = false;
            _interactivity.Write("Welcome to MAGES! Press ENTER to to start the interactive tutorial ...");
            _interactivity.Read();
            _interactivity.IsPromptShown = true;
            Tutorials.RunAll(_interactivity, _engine.Scope, input => Evaluate(input, true));
            
            Teardown();
        }

        private void Loop()
        {
            var input = default(String);

            do
            {
                input = _interactivity.Read();

                if (!String.IsNullOrEmpty(input))
                {
                    EvaluateCompleted(input);
                }
            }
            while (input != null);
        }

        private void Teardown()
        {
        }

        private void EvaluateCompleted(String input)
        {
            while (!input.IsCompleted())
            {
                _interactivity.IsPromptShown = false;
                _interactivity.Info("...> ");
                var rest = _interactivity.Read();
                _interactivity.IsPromptShown = true;

                if (String.IsNullOrEmpty(rest))
                    break;

                input = String.Concat(input, Environment.NewLine, rest);
            }

            Evaluate(input, true);
        }

        private void Evaluate(String content, Boolean showOutput)
        {
            try
            {
                var result = _interactivity.Run(_engine, content);

                if (showOutput)
                {
                    _interactivity.Info(result);
                    _engine.Scope["ans"] = result;
                }
            }
            catch (ParseException ex)
            {
                _interactivity.Display(ex.Error, content);
            }
            catch (Exception ex)
            {
                _interactivity.Error(ex.Message);
            }

            _interactivity.Write(Environment.NewLine);
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
