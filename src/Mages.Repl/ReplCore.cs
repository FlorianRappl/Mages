namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Repl.Functions;
    using System;

    sealed class ReplCore
    {
        private readonly IInteractivity _interactivity;
        private readonly Engine _engine;

        public ReplCore(IInteractivity interactivity)
        {
            _interactivity = interactivity;
            _engine = new Engine();
            HelpFunction.AddTo(_engine);
            ProcessObject.AddTo(_engine);
            SpawnFunction.AddTo(_engine);
        }

        public void Run()
        {
            Startup();
            Loop();
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
                    try
                    {
                        var result = _engine.Interpret(input);
                        _interactivity.Info(result);
                    }
                    catch (ParseException ex)
                    {
                        Display(ex.Error, input);
                    }
                    catch (Exception ex)
                    {
                        _interactivity.Error(ex.Message);
                    }

                    _interactivity.Write(Environment.NewLine);
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
