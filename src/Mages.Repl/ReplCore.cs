namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Repl.Functions;
    using System;
    using Tutorial;

    sealed class ReplCore
    {
        private readonly IInteractivity _interactivity;
        private readonly Engine _engine;

        public ReplCore(IInteractivity interactivity, Configuration configuration)
        {
            _interactivity = interactivity;
            _engine = new Engine(configuration);
            ReplFunctions.Integrate(_engine);
        }

        public void Run(String content)
        {
            Evaluate(content, false);
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
                    Evaluate(input, true);
                }
            }
            while (input != null);
        }

        private void Teardown()
        {
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

        private void Evaluate(String content, Boolean showOutput)
        {
            try
            {
                var result = _engine.Interpret(content);

                if (showOutput)
                {
                    _interactivity.Info(result);
                    _engine.Scope["ans"] = result;
                }
            }
            catch (ParseException ex)
            {
                Display(ex.Error, content);
            }
            catch (Exception ex)
            {
                _interactivity.Error(ex.Message);
            }

            _interactivity.Write(Environment.NewLine);
        }

        private void Display(ParseError error, String source)
        {
            var start = error.Start.Index - 1;
            var end = error.End.Index - 1;

            if (end == start)
            {
                end++;
            }

            var range = 80;
            var message = error.Code.GetMessage();
            var middle = (end + start) / 2;
            var ss = Math.Max(middle - range / 2, 0);
            var se = Math.Min(middle + range / 2, source.Length);
            var snippet = source.Substring(ss, se - ss);
            _interactivity.Error(snippet);
            _interactivity.Error(Environment.NewLine);
            _interactivity.Error(new String(' ', start - ss));
            _interactivity.Error(new String('^', end - start));
            _interactivity.Error(Environment.NewLine);
            _interactivity.Error("Error: ");
            _interactivity.Error(message);
        }
    }
}
