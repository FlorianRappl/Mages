namespace Mages.Repl
{
    using System;

    struct Handler
    {
        public readonly ConsoleKeyInfo KeyInfo;
        public readonly Action KeyHandler;
        public readonly Boolean ResetCompletion;

        public Handler(ConsoleKey key, Action h, Boolean resetCompletion = true)
        {
            KeyInfo = new ConsoleKeyInfo((Char)0, key, false, false, false);
            KeyHandler = h;
            ResetCompletion = resetCompletion;
        }

        public Handler(ConsoleKey key, ConsoleModifiers mod, Action h, Boolean resetCompletion = true)
        {
            var shift = mod.HasFlag(ConsoleModifiers.Shift);
            var alt = mod.HasFlag(ConsoleModifiers.Alt);
            var ctrl = mod.HasFlag(ConsoleModifiers.Control);
            KeyInfo = new ConsoleKeyInfo((Char)0, key, shift, alt, ctrl);
            KeyHandler = h;
            ResetCompletion = resetCompletion;
        }

        public Handler(Char c, Action h, Boolean resetCompletion = true)
        {
            // Use the "Zoom" as a flag that we only have a character.
            KeyInfo = new ConsoleKeyInfo(c, ConsoleKey.Zoom, false, false, false);
            KeyHandler = h;
            ResetCompletion = resetCompletion;
        }

        public Handler(ConsoleKeyInfo cki, Action h, Boolean resetCompletion = true)
        {
            KeyInfo = cki;
            KeyHandler = h;
            ResetCompletion = resetCompletion;
        }

        public static Handler Control(Char c, Action h, Boolean resetCompletion = true)
        {
            return new Handler((Char)(c - 'A' + 1), h, resetCompletion);
        }

        public static Handler Alt(Char c, ConsoleKey k, Action h)
        {
            var keyInfo = new ConsoleKeyInfo((Char)c, k, false, true, false);
            return new Handler(keyInfo, h);
        }
    }
}
