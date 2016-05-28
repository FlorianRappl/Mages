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
    }
}
