namespace Mages.Repl
{
    using System;

    struct Handler
    {
        public ConsoleKeyInfo CKI;
        public Action KeyHandler;
        public Boolean ResetCompletion;

        public Handler(ConsoleKey key, Action h, Boolean resetCompletion = true)
        {
            CKI = new ConsoleKeyInfo((char)0, key, false, false, false);
            KeyHandler = h;
            ResetCompletion = resetCompletion;
        }

        public Handler(Char c, Action h, Boolean resetCompletion = true)
        {
            KeyHandler = h;
            // Use the "Zoom" as a flag that we only have a character.
            CKI = new ConsoleKeyInfo(c, ConsoleKey.Zoom, false, false, false);
            ResetCompletion = resetCompletion;
        }

        public Handler(ConsoleKeyInfo cki, Action h, Boolean resetCompletion = true)
        {
            CKI = cki;
            KeyHandler = h;
            ResetCompletion = resetCompletion;
        }

        public static Handler Control(char c, Action h, bool resetCompletion = true)
        {
            return new Handler((char)(c - 'A' + 1), h, resetCompletion);
        }

        public static Handler Alt(char c, ConsoleKey k, Action h)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo((char)c, k, false, true, false);
            return new Handler(cki, h);
        }
    }
}
