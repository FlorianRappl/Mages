import {
    createConnection,
    TextDocuments,
    Diagnostic,
    DiagnosticSeverity,
    ProposedFeatures,
    InitializeParams,
    InitializeResult,
    TextDocumentSyncKind,
    CompletionItem,
    CompletionItemKind,
    TextDocumentPositionParams,
    Hover,
    MarkupKind,
    DocumentSymbol,
    SymbolKind,
    DocumentSymbolParams,
    SignatureHelp,
    SignatureHelpParams
} from 'vscode-languageserver/node';

import { TextDocument } from 'vscode-languageserver-textdocument';

const connection = createConnection(ProposedFeatures.all);
const documents: TextDocuments<TextDocument> = new TextDocuments(TextDocument);

interface FunctionDoc {
    name: string;
    params: string[];
    description: string;
    returns: string;
}

const BUILTIN_FUNCTIONS: Record<string, FunctionDoc> = {
    sin: { name: 'sin', params: ['x'], description: 'Computes the sine of x.', returns: 'number' },
    cos: { name: 'cos', params: ['x'], description: 'Computes the cosine of x.', returns: 'number' },
    tan: { name: 'tan', params: ['x'], description: 'Computes the tangent of x.', returns: 'number' },
    sinh: { name: 'sinh', params: ['x'], description: 'Computes the hyperbolic sine of x.', returns: 'number' },
    cosh: { name: 'cosh', params: ['x'], description: 'Computes the hyperbolic cosine of x.', returns: 'number' },
    tanh: { name: 'tanh', params: ['x'], description: 'Computes the hyperbolic tangent of x.', returns: 'number' },
    asin: { name: 'asin', params: ['x'], description: 'Computes the arc sine of x.', returns: 'number' },
    acos: { name: 'acos', params: ['x'], description: 'Computes the arc cosine of x.', returns: 'number' },
    atan: { name: 'atan', params: ['x'], description: 'Computes the arc tangent of x.', returns: 'number' },
    atan2: { name: 'atan2', params: ['y', 'x'], description: 'Computes the four-quadrant inverse tangent of y and x.', returns: 'number' },
    exp: { name: 'exp', params: ['x'], description: 'Computes the exponential e^x.', returns: 'number' },
    log: { name: 'log', params: ['x'], description: 'Computes the natural logarithm of x.', returns: 'number' },
    sqrt: { name: 'sqrt', params: ['x'], description: 'Computes the square root of x.', returns: 'number' },
    abs: { name: 'abs', params: ['x'], description: 'Computes the absolute value of x.', returns: 'number' },
    round: { name: 'round', params: ['x'], description: 'Rounds x to the nearest integer.', returns: 'number' },
    floor: { name: 'floor', params: ['x'], description: 'Rounds x downward to nearest integer.', returns: 'number' },
    ceil: { name: 'ceil', params: ['x'], description: 'Rounds x upward to nearest integer.', returns: 'number' },
    min: { name: 'min', params: ['...args'], description: 'Returns the minimum of the provided arguments.', returns: 'number' },
    max: { name: 'max', params: ['...args'], description: 'Returns the maximum of the provided arguments.', returns: 'number' },
    clamp: { name: 'clamp', params: ['val', 'min', 'max'], description: 'Clamps val between min and max.', returns: 'number' },
    lerp: { name: 'lerp', params: ['a', 'b', 't'], description: 'Linearly interpolates between a and b with factor t.', returns: 'number' },
    clip: { name: 'clip', params: ['val', 'min', 'max'], description: 'Clips val to the interval [min, max].', returns: 'number' },
    sign: { name: 'sign', params: ['x'], description: 'Returns the sign of x (-1, 0, or 1).', returns: 'number' },
    gamma: { name: 'gamma', params: ['x'], description: 'Computes the Gamma function of x.', returns: 'number' },
    factorial: { name: 'factorial', params: ['n'], description: 'Computes the factorial of n.', returns: 'number' },
    rand: { name: 'rand', params: ['[rows]', '[cols]'], description: 'Generates a random number or matrix of random numbers between 0 and 1.', returns: 'number | matrix' },
    randi: { name: 'randi', params: ['max', '[rows]', '[cols]'], description: 'Generates a random integer or matrix of integers up to max.', returns: 'number | matrix' },
    real: { name: 'real', params: ['z'], description: 'Returns the real part of complex number z.', returns: 'number' },
    imag: { name: 'imag', params: ['z'], description: 'Returns the imaginary part of complex number z.', returns: 'number' },
    arg: { name: 'arg', params: ['z'], description: 'Returns the phase angle of complex number z.', returns: 'number' },
    conj: { name: 'conj', params: ['z'], description: 'Returns the complex conjugate of z.', returns: 'complex' },
    matrix: { name: 'matrix', params: ['rows', 'cols', '[init]'], description: 'Creates a matrix with specified dimensions and optional initializer.', returns: 'matrix' },
    eye: { name: 'eye', params: ['n'], description: 'Creates an n x n identity matrix.', returns: 'matrix' },
    zeros: { name: 'zeros', params: ['rows', '[cols]'], description: 'Creates a matrix filled with zeros.', returns: 'matrix' },
    ones: { name: 'ones', params: ['rows', '[cols]'], description: 'Creates a matrix filled with ones.', returns: 'matrix' },
    det: { name: 'det', params: ['m'], description: 'Calculates the determinant of square matrix m.', returns: 'number' },
    inv: { name: 'inv', params: ['m'], description: 'Computes the inverse of square matrix m.', returns: 'matrix' },
    trace: { name: 'trace', params: ['m'], description: 'Computes the trace of square matrix m.', returns: 'number' },
    cross: { name: 'cross', params: ['a', 'b'], description: 'Computes the cross product of two 3D vectors.', returns: 'matrix' },
    dot: { name: 'dot', params: ['a', 'b'], description: 'Computes the dot product of two vectors.', returns: 'number' },
    type: { name: 'type', params: ['obj'], description: 'Returns the type name or metadata of the given object.', returns: 'string | object' },
    keys: { name: 'keys', params: ['obj'], description: 'Returns an array with the property keys of the object.', returns: 'matrix' },
    values: { name: 'values', params: ['obj'], description: 'Returns an array with the values of the object.', returns: 'matrix' },
    json: { name: 'json', params: ['obj'], description: 'Converts an object or expression into a JSON string.', returns: 'string' },
    eval: { name: 'eval', params: ['code'], description: 'Evaluates the provided MAGES source code string.', returns: 'any' },
    import: { name: 'import', params: ['modulePath'], description: 'Imports a MAGES module or plugin file.', returns: 'any' },
    export: { name: 'export', params: ['symbol'], description: 'Exports a symbol from the current module.', returns: 'void' },
    help: { name: 'help', params: ['[topic]'], description: 'Displays help information about MAGES functions and features.', returns: 'string' }
};

const KEYWORDS = [
    { label: 'var', detail: 'Variable declaration', doc: 'Declares a local or module variable.' },
    { label: 'let', detail: 'Variable declaration', doc: 'Declares a scoped variable.' },
    { label: 'const', detail: 'Constant declaration', doc: 'Declares a read-only constant.' },
    { label: 'if', detail: 'Conditional statement', doc: 'Executes a block if condition is true: `if (cond) { ... } else { ... }`' },
    { label: 'else', detail: 'Conditional branch', doc: 'Alternative branch for an `if` statement.' },
    { label: 'while', detail: 'Loop statement', doc: 'Executes body while condition holds: `while (cond) { ... }`' },
    { label: 'for', detail: 'Loop statement', doc: 'For loop: `for (init; cond; post) { ... }`' },
    { label: 'return', detail: 'Return statement', doc: 'Returns a value from a function: `return expr;`' },
    { label: 'break', detail: 'Break statement', doc: 'Breaks out of the enclosing loop or match.' },
    { label: 'continue', detail: 'Continue statement', doc: 'Continues with the next iteration of the loop.' },
    { label: 'match', detail: 'Pattern match statement', doc: 'Matches value against cases: `match(val) { ... }`' },
    { label: 'new', detail: 'Object creation', doc: 'Instantiates a new object: `new { key: val }`' },
    { label: 'await', detail: 'Await expression', doc: 'Awaits the completion of an asynchronous task/future.' },
    { label: 'delete', detail: 'Delete property', doc: 'Deletes a member property from an object.' }
];

const CONSTANTS = [
    { label: 'true', detail: 'Boolean true', doc: 'The boolean value true.' },
    { label: 'false', detail: 'Boolean false', doc: 'The boolean value false.' },
    { label: 'null', detail: 'Null value', doc: 'Represents the intentional absence of any value.' },
    { label: 'undefined', detail: 'Undefined value', doc: 'Represents an uninitialized or missing value.' },
    { label: 'PI', detail: 'Mathematical constant π', doc: 'Ratio of circumference of a circle to its diameter (~3.14159265).' },
    { label: 'E', detail: 'Euler\'s number e', doc: 'Base of the natural logarithm (~2.71828182).' },
    { label: 'Infinity', detail: 'Infinity', doc: 'Represents mathematical infinity.' },
    { label: 'NaN', detail: 'Not a Number', doc: 'Represents an invalid or unrepresentable numerical result.' },
    { label: 'I', detail: 'Imaginary unit i', doc: 'The imaginary unit satisfying i² = -1.' }
];

connection.onInitialize((_params: InitializeParams): InitializeResult => {
    return {
        capabilities: {
            textDocumentSync: TextDocumentSyncKind.Incremental,
            completionProvider: {
                resolveProvider: false,
                triggerCharacters: ['.', '(', ',']
            },
            hoverProvider: true,
            documentSymbolProvider: true,
            signatureHelpProvider: {
                triggerCharacters: ['(', ',']
            }
        }
    };
});

documents.onDidChangeContent(change => {
    validateTextDocument(change.document);
});

async function validateTextDocument(textDocument: TextDocument): Promise<void> {
    const text = textDocument.getText();
    const diagnostics: Diagnostic[] = [];

    const lines = text.split(/\r?\n/);

    // Simple robust syntax validator
    const stack: { char: string; line: number; col: number }[] = [];
    let inString: string | null = null;
    let inBlockComment = false;

    for (let lineIdx = 0; lineIdx < lines.length; lineIdx++) {
        const line = lines[lineIdx];

        for (let colIdx = 0; colIdx < line.length; colIdx++) {
            const ch = line[colIdx];
            const nextCh = colIdx + 1 < line.length ? line[colIdx + 1] : '';

            if (inBlockComment) {
                if (ch === '*' && nextCh === '/') {
                    inBlockComment = false;
                    colIdx++;
                }
                continue;
            }

            if (inString !== null) {
                if (ch === '\\') {
                    colIdx++; // Skip escaped char
                } else if (ch === inString) {
                    inString = null;
                }
                continue;
            }

            if (ch === '/' && nextCh === '/') {
                break; // Line comment
            }

            if (ch === '/' && nextCh === '*') {
                inBlockComment = true;
                colIdx++;
                continue;
            }

            if (ch === '"' || ch === '\'' || ch === '`') {
                inString = ch;
                continue;
            }

            if (ch === '{' || ch === '(' || ch === '[') {
                stack.push({ char: ch, line: lineIdx, col: colIdx });
            } else if (ch === '}' || ch === ')' || ch === ']') {
                const expected: Record<string, string> = { '}': '{', ')': '(', ']': '[' };
                const match = expected[ch];
                if (stack.length === 0 || stack[stack.length - 1].char !== match) {
                    diagnostics.push({
                        severity: DiagnosticSeverity.Error,
                        range: {
                            start: { line: lineIdx, character: colIdx },
                            end: { line: lineIdx, character: colIdx + 1 }
                        },
                        message: `Unexpected closing '${ch}'`,
                        source: 'mages'
                    });
                } else {
                    stack.pop();
                }
            }
        }

        if (inString !== null && inString !== '`') {
            diagnostics.push({
                severity: DiagnosticSeverity.Error,
                range: {
                    start: { line: lineIdx, character: line.length - 1 },
                    end: { line: lineIdx, character: line.length }
                },
                message: `Unterminated string literal`,
                source: 'mages'
            });
            inString = null;
        }
    }

    while (stack.length > 0) {
        const unclosed = stack.pop()!;
        diagnostics.push({
            severity: DiagnosticSeverity.Error,
            range: {
                start: { line: unclosed.line, character: unclosed.col },
                end: { line: unclosed.line, character: unclosed.col + 1 }
            },
            message: `Unclosed bracket '${unclosed.char}'`,
            source: 'mages'
        });
    }

    if (inBlockComment) {
        diagnostics.push({
            severity: DiagnosticSeverity.Error,
            range: {
                start: { line: lines.length - 1, character: 0 },
                end: { line: lines.length - 1, character: 1 }
            },
            message: 'Unclosed block comment',
            source: 'mages'
        });
    }

    connection.sendDiagnostics({ uri: textDocument.uri, diagnostics });
}

connection.onCompletion((textDocumentPosition: TextDocumentPositionParams): CompletionItem[] => {
    const document = documents.get(textDocumentPosition.textDocument.uri);
    if (!document) {
        return [];
    }

    const items: CompletionItem[] = [];

    // Add keywords
    for (const kw of KEYWORDS) {
        items.push({
            label: kw.label,
            kind: CompletionItemKind.Keyword,
            detail: kw.detail,
            documentation: kw.doc
        });
    }

    // Add constants
    for (const c of CONSTANTS) {
        items.push({
            label: c.label,
            kind: CompletionItemKind.Constant,
            detail: c.detail,
            documentation: c.doc
        });
    }

    // Add built-in functions
    for (const [name, fn] of Object.entries(BUILTIN_FUNCTIONS)) {
        items.push({
            label: name,
            kind: CompletionItemKind.Function,
            detail: `(function) ${fn.name}(${fn.params.join(', ')}): ${fn.returns}`,
            documentation: fn.description
        });
    }

    // Extract user symbols from document
    const text = document.getText();
    const identifierRegex = /\b([a-zA-Z_][a-zA-Z0-9_]*)\s*(=|=>)/g;
    let match;
    const seen = new Set<string>();

    while ((match = identifierRegex.exec(text)) !== null) {
        const name = match[1];
        if (!seen.has(name) && !BUILTIN_FUNCTIONS[name] && !KEYWORDS.some(k => k.label === name)) {
            seen.add(name);
            const isFunc = match[2] === '=>';
            items.push({
                label: name,
                kind: isFunc ? CompletionItemKind.Function : CompletionItemKind.Variable,
                detail: isFunc ? `(user function) ${name}` : `(user variable) ${name}`
            });
        }
    }

    return items;
});

connection.onHover((params: TextDocumentPositionParams): Hover | null => {
    const document = documents.get(params.textDocument.uri);
    if (!document) {
        return null;
    }

    const offset = document.offsetAt(params.position);
    const text = document.getText();

    let start = offset;
    while (start > 0 && /[a-zA-Z0-9_]/.test(text[start - 1])) {
        start--;
    }

    let end = offset;
    while (end < text.length && /[a-zA-Z0-9_]/.test(text[end])) {
        end++;
    }

    if (start === end) {
        return null;
    }

    const word = text.slice(start, end);

    if (BUILTIN_FUNCTIONS[word]) {
        const fn = BUILTIN_FUNCTIONS[word];
        return {
            contents: {
                kind: MarkupKind.Markdown,
                value: `\`\`\`mages\n${fn.name}(${fn.params.join(', ')}): ${fn.returns}\n\`\`\`\n\n${fn.description}`
            }
        };
    }

    const kw = KEYWORDS.find(k => k.label === word);
    if (kw) {
        return {
            contents: {
                kind: MarkupKind.Markdown,
                value: `**${kw.label}** — ${kw.detail}\n\n${kw.doc}`
            }
        };
    }

    const c = CONSTANTS.find(k => k.label === word);
    if (c) {
        return {
            contents: {
                kind: MarkupKind.Markdown,
                value: `**${c.label}** — ${c.detail}\n\n${c.doc}`
            }
        };
    }

    return null;
});

connection.onDocumentSymbol((params: DocumentSymbolParams): DocumentSymbol[] => {
    const document = documents.get(params.textDocument.uri);
    if (!document) {
        return [];
    }

    const symbols: DocumentSymbol[] = [];
    const text = document.getText();
    const lines = text.split(/\r?\n/);

    for (let i = 0; i < lines.length; i++) {
        const line = lines[i];

        // Match function assignment: name = (...) => or name = arg =>
        const fnMatch = /^\s*([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(\(.*?\)|[a-zA-Z_][a-zA-Z0-9_]*)\s*=>/.exec(line);
        if (fnMatch) {
            const name = fnMatch[1];
            symbols.push({
                name,
                detail: fnMatch[2] + ' => ...',
                kind: SymbolKind.Function,
                range: {
                    start: { line: i, character: 0 },
                    end: { line: i, character: line.length }
                },
                selectionRange: {
                    start: { line: i, character: line.indexOf(name) },
                    end: { line: i, character: line.indexOf(name) + name.length }
                }
            });
            continue;
        }

        // Match variable assignment: var x = ... or x = ...
        const varMatch = /^\s*(?:var\s+)?([a-zA-Z_][a-zA-Z0-9_]*)\s*=/.exec(line);
        if (varMatch) {
            const name = varMatch[1];
            symbols.push({
                name,
                detail: 'variable',
                kind: SymbolKind.Variable,
                range: {
                    start: { line: i, character: 0 },
                    end: { line: i, character: line.length }
                },
                selectionRange: {
                    start: { line: i, character: line.indexOf(name) },
                    end: { line: i, character: line.indexOf(name) + name.length }
                }
            });
        }
    }

    return symbols;
});

connection.onSignatureHelp((params: SignatureHelpParams): SignatureHelp | null => {
    const document = documents.get(params.textDocument.uri);
    if (!document) {
        return null;
    }

    const offset = document.offsetAt(params.position);
    const text = document.getText().slice(0, offset);

    let depth = 0;
    let argIndex = 0;
    let callStart = -1;

    for (let i = text.length - 1; i >= 0; i--) {
        const ch = text[i];
        if (ch === ')') {
            depth++;
        } else if (ch === '(') {
            if (depth > 0) {
                depth--;
            } else {
                callStart = i;
                break;
            }
        } else if (ch === ',' && depth === 0) {
            argIndex++;
        }
    }

    if (callStart === -1) {
        return null;
    }

    let funcNameEnd = callStart;
    while (funcNameEnd > 0 && /\s/.test(text[funcNameEnd - 1])) {
        funcNameEnd--;
    }
    let funcNameStart = funcNameEnd;
    while (funcNameStart > 0 && /[a-zA-Z0-9_]/.test(text[funcNameStart - 1])) {
        funcNameStart--;
    }

    const funcName = text.slice(funcNameStart, funcNameEnd);
    const fn = BUILTIN_FUNCTIONS[funcName];
    if (!fn) {
        return null;
    }

    return {
        signatures: [
            {
                label: `${fn.name}(${fn.params.join(', ')}): ${fn.returns}`,
                documentation: fn.description,
                parameters: fn.params.map(p => ({
                    label: p,
                    documentation: `Parameter ${p}`
                }))
            }
        ],
        activeSignature: 0,
        activeParameter: Math.min(argIndex, fn.params.length - 1)
    };
});

documents.listen(connection);
connection.listen();
